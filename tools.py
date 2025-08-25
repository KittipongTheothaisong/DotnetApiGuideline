#!/usr/bin/env python3
"""
DotNet API Guideline Project Tools
==================================

This script combines all project management scripts into a single Python tool.
It provides commands for managing Docker services, Keycloak setup, and project utilities.

Usage:
    python3 tools.py <command> [options]

Commands:
    setup-keycloak      - Complete Keycloak setup with realm, client, and test user
    reset-environment   - Reset entire environment (WARNING: destroys all data)
    restart-services    - Quick restart of all Docker services
    get-token           - Get access token for testing [username] [password]
    status             - Show status of all services
    help               - Show this help message

Author: Combined from shell scripts for DotNet API Guideline project
"""

import argparse
import json
import os
import subprocess
import sys
import time
import urllib.request
import urllib.parse
import urllib.error
from typing import Dict, Any, Optional, Tuple


class Colors:
    """ANSI color codes for terminal output"""
    RESET = '\033[0m'
    BOLD = '\033[1m'
    RED = '\033[91m'
    GREEN = '\033[92m'
    YELLOW = '\033[93m'
    BLUE = '\033[94m'
    MAGENTA = '\033[95m'
    CYAN = '\033[96m'


class ProjectTools:
    """Main class for managing project tools"""

    def __init__(self):
        self.keycloak_url = "http://localhost:8080"
        self.admin_user = "admin"
        self.admin_password = "admin123"
        self.realm_name = "dotnet-api-guideline"
        self.client_id = "dotnet-api-client"
        self.test_user = "testuser"
        self.test_password = "password123"

        # Service health check configs
        self.services = {
            'sqlserver': {
                'container': 'sqlserver',
                'check_cmd': [
                    'docker', 'exec', 'sqlserver',
                    '/opt/mssql-tools18/bin/sqlcmd', '-S', 'localhost',
                    '-U', 'sa', '-P', 'YourPassword123!', '-C', '-Q', 'SELECT 1'
                ],
                'port': 1433
            },
            'mongodb': {
                'container': 'mongodb',
                'check_cmd': [
                    'docker', 'exec', 'mongodb',
                    'mongosh', '--eval', 'db.adminCommand("ping")', '--quiet'
                ],
                'port': 27017
            },
            'postgres': {
                'container': 'postgres',
                'check_cmd': [
                    'docker', 'exec', 'postgres',
                    'pg_isready', '-U', 'keycloak'
                ],
                'port': 5432
            },
            'keycloak': {
                'container': 'keycloak',
                'check_cmd': None,  # Uses HTTP check
                'port': 8080
            }
        }

    def print_header(self, title: str, symbol: str = "🚀"):
        """Print a formatted header"""
        print(f"\n{Colors.CYAN}{'='*50}")
        print(f"{symbol} {title}")
        print(f"{'='*50}{Colors.RESET}\n")

    def print_step(self, step: str, message: str):
        """Print a formatted step message"""
        print(f"{Colors.BLUE}{step}: {message}...{Colors.RESET}")

    def print_success(self, message: str):
        """Print a success message"""
        print(f"{Colors.GREEN}✅ {message}{Colors.RESET}")

    def print_error(self, message: str):
        """Print an error message"""
        print(f"{Colors.RED}❌ {message}{Colors.RESET}")

    def print_warning(self, message: str):
        """Print a warning message"""
        print(f"{Colors.YELLOW}⚠️  {message}{Colors.RESET}")

    def run_command(self, cmd: list, capture_output: bool = True, check: bool = True) -> subprocess.CompletedProcess:
        """Run a shell command and return the result"""
        try:
            if capture_output:
                result = subprocess.run(
                    cmd, capture_output=True, text=True, check=check)
            else:
                result = subprocess.run(cmd, check=check)
            return result
        except subprocess.CalledProcessError as e:
            if capture_output:
                print(f"Command failed: {' '.join(cmd)}")
                print(f"Error: {e.stderr}")
            raise

    def http_request(self, url: str, data: Dict[str, Any] = None, headers: Dict[str, str] = None, method: str = 'GET') -> Tuple[int, Dict[str, Any]]:
        """Make an HTTP request and return status code and parsed JSON response"""
        try:
            if headers is None:
                headers = {}

            if data is not None:
                if method == 'POST' and 'Content-Type' not in headers:
                    headers['Content-Type'] = 'application/json'
                data = json.dumps(data).encode(
                    'utf-8') if isinstance(data, dict) else data

            req = urllib.request.Request(
                url, data=data, headers=headers, method=method)

            with urllib.request.urlopen(req) as response:
                response_data = response.read().decode('utf-8')
                try:
                    return response.status, json.loads(response_data)
                except json.JSONDecodeError:
                    return response.status, {'raw': response_data}

        except urllib.error.HTTPError as e:
            try:
                error_data = json.loads(e.read().decode('utf-8'))
                return e.code, error_data
            except:
                return e.code, {'error': str(e)}
        except Exception as e:
            return 0, {'error': str(e)}

    def wait_for_service(self, service_name: str, timeout: int = 300) -> bool:
        """Wait for a service to become ready"""
        print(f"   Waiting for {service_name}...")
        start_time = time.time()

        service_config = self.services.get(service_name)
        if not service_config:
            self.print_error(f"Unknown service: {service_name}")
            return False

        while time.time() - start_time < timeout:
            try:
                if service_name == 'keycloak':
                    # Special handling for Keycloak HTTP check
                    status, _ = self.http_request(self.keycloak_url)
                    if status == 200:
                        return True
                else:
                    # Use docker exec command check
                    result = self.run_command(
                        service_config['check_cmd'], capture_output=True, check=False)
                    if result.returncode == 0:
                        return True

                time.sleep(2)
            except Exception:
                time.sleep(2)
                continue

        return False

    def get_admin_token(self) -> Optional[str]:
        """Get Keycloak admin token"""
        data = {
            'grant_type': 'password',
            'client_id': 'admin-cli',
            'username': self.admin_user,
            'password': self.admin_password
        }

        url = f"{self.keycloak_url}/realms/master/protocol/openid-connect/token"
        headers = {'Content-Type': 'application/x-www-form-urlencoded'}
        encoded_data = urllib.parse.urlencode(data).encode('utf-8')

        status, response = self.http_request(
            url, encoded_data, headers, 'POST')

        if status == 200 and 'access_token' in response:
            return response['access_token']

        return None

    def setup_keycloak(self):
        """Complete Keycloak setup"""
        self.print_header("Keycloak Complete Setup Script", "🚀")

        print(f"{Colors.CYAN}📋 Configuration:")
        print(f"  Keycloak URL: {self.keycloak_url}")
        print(f"  Realm: {self.realm_name}")
        print(f"  Client: {self.client_id}")
        print(f"  Test User: {self.test_user}")
        print(
            f"  (Client secret will be generated automatically){Colors.RESET}\n")

        # Step 1: Wait for Keycloak
        self.print_step("⏳ Step 1", "Waiting for Keycloak to be ready")
        if not self.wait_for_service('keycloak'):
            self.print_error("Keycloak is not responding")
            return False
        self.print_success("Keycloak is ready!")

        # Step 2: Get admin token
        self.print_step("🔑 Step 2", "Getting admin token")
        admin_token = self.get_admin_token()
        if not admin_token:
            self.print_error(
                "Failed to get admin token. Check Keycloak credentials.")
            return False
        self.print_success("Admin token obtained")

        # Step 3: Create Realm
        self.print_step("🏠 Step 3", f"Creating realm '{self.realm_name}'")
        realm_data = {
            "realm": self.realm_name,
            "enabled": True,
            "displayName": "DotNet API Guideline Realm"
        }

        url = f"{self.keycloak_url}/admin/realms"
        headers = {
            'Authorization': f'Bearer {admin_token}',
            'Content-Type': 'application/json'
        }

        status, _ = self.http_request(url, realm_data, headers, 'POST')
        self.print_success("Realm created")

        # Step 4: Create Client
        self.print_step("👤 Step 4", f"Creating client '{self.client_id}'")
        client_data = {
            "clientId": self.client_id,
            "enabled": True,
            "clientAuthenticatorType": "client-secret",
            "standardFlowEnabled": True,
            "directAccessGrantsEnabled": True,
            "serviceAccountsEnabled": True,
            "publicClient": False,
            "protocol": "openid-connect",
            "redirectUris": ["http://localhost:5249/*"],
            "webOrigins": ["http://localhost:5249"],
            "attributes": {
                "access.token.lifespan": "300000000"
            }
        }

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/clients"
        status, _ = self.http_request(url, client_data, headers, 'POST')
        self.print_success("Client created")

        # Step 4b: Get client secret
        self.print_step("🔑 Step 4b", "Getting client secret")

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/clients?clientId={self.client_id}"
        status, response = self.http_request(url, headers=headers)
        client_uuid = response[0]['id']

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/clients/{client_uuid}/client-secret"
        status, response = self.http_request(url, headers=headers)
        client_secret = response['value']

        self.print_success(f"Client secret retrieved: {client_secret[:20]}...")

        # Step 5: Create Test User
        self.print_step("👥 Step 5", f"Creating test user '{self.test_user}'")
        user_data = {
            "username": self.test_user,
            "enabled": True,
            "emailVerified": True,
            "firstName": "Test",
            "lastName": "User",
            "email": "test@example.com"
        }

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/users"
        status, _ = self.http_request(url, user_data, headers, 'POST')
        self.print_success("Test user created")

        # Step 5b: Set user password
        self.print_step("🔒 Step 5b", "Setting user password")

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/users?username={self.test_user}"
        status, response = self.http_request(url, headers=headers)
        user_id = response[0]['id']

        password_data = {
            "type": "password",
            "value": self.test_password,
            "temporary": False
        }

        url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/users/{user_id}/reset-password"
        status, _ = self.http_request(url, password_data, headers, 'PUT')
        self.print_success("User password set")

        # Step 6: Update appsettings.json
        self.print_step("⚙️ Step 6", "Updating appsettings.json")
        try:
            with open('appsettings.json', 'r') as f:
                settings = json.load(f)

            if 'Keycloak' not in settings:
                settings['Keycloak'] = {}

            settings['Keycloak']['Authority'] = f"{self.keycloak_url}/realms/{self.realm_name}"
            settings['Keycloak']['Audience'] = "account"
            settings['Keycloak']['MetadataAddress'] = f"{self.keycloak_url}/realms/{self.realm_name}/.well-known/openid_configuration"

            with open('appsettings.json', 'w') as f:
                json.dump(settings, f, indent=2)

            self.print_success("appsettings.json updated")
        except Exception as e:
            self.print_error(f"Failed to update appsettings.json: {e}")

        # Step 7: Get test access token
        self.print_step("🎫 Step 7", "Getting test access token")
        access_token = self.get_user_token(
            self.test_user, self.test_password, client_secret)

        if access_token:
            self.print_success("Access token obtained")
        else:
            self.print_error("Failed to get access token")

        # Summary
        self.print_header("Setup Complete!", "🎉")
        print(f"{Colors.CYAN}📋 Configuration Summary:")
        print(f"  Keycloak URL: {self.keycloak_url}")
        print(f"  Realm: {self.realm_name}")
        print(f"  Client ID: {self.client_id}")
        print(f"  Client Secret: {client_secret}")
        print(f"  Test User: {self.test_user}")
        print(f"  Test Password: {self.test_password}")
        print(f"")
        print(f"🚀 Next Steps:")
        print(f"1. Restart your .NET API")
        print(f"2. Test authentication with your preferred method")
        print(f"3. Use Keycloak admin console for further configuration")
        print(f"")
        print(f"🌐 Access Points:")
        print(f"  Keycloak Admin: {self.keycloak_url} (admin/admin123)")
        print(f"  API Swagger: http://localhost:5249 (after starting API)")
        print(f"")
        print(f"🎫 Ready-to-Use Access Token:")
        print(f"{'='*50}")
        if access_token:
            print(access_token)
        else:
            print("❌ No access token available")
            print(f"💡 You can get one manually using the get-token command")
        print(f"{'='*50}{Colors.RESET}")

        return True

    def get_user_token(self, username: str, password: str, client_secret: str = None) -> Optional[str]:
        """Get user access token"""
        if not client_secret:
            # Get client secret first
            admin_token = self.get_admin_token()
            if not admin_token:
                return None

            headers = {'Authorization': f'Bearer {admin_token}'}
            url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/clients?clientId={self.client_id}"
            status, response = self.http_request(url, headers=headers)

            if status != 200 or not response:
                return None

            client_uuid = response[0]['id']
            url = f"{self.keycloak_url}/admin/realms/{self.realm_name}/clients/{client_uuid}/client-secret"
            status, response = self.http_request(url, headers=headers)

            if status != 200:
                return None

            client_secret = response['value']

        # Get user token
        data = {
            'grant_type': 'password',
            'client_id': self.client_id,
            'client_secret': client_secret,
            'username': username,
            'password': password
        }

        url = f"{self.keycloak_url}/realms/{self.realm_name}/protocol/openid-connect/token"
        headers = {'Content-Type': 'application/x-www-form-urlencoded'}
        encoded_data = urllib.parse.urlencode(data).encode('utf-8')

        status, response = self.http_request(
            url, encoded_data, headers, 'POST')

        if status == 200 and 'access_token' in response:
            return response['access_token']

        return None

    def reset_environment(self):
        """Reset entire environment"""
        self.print_header("Environment Reset Script", "🧹")
        print("This script will:")
        print("  1. Stop and remove all containers")
        print("  2. Remove all Docker volumes (DATA WILL BE LOST)")
        print("  3. Clean up Docker networks")
        print("  4. Reset .NET database migrations")
        print("  5. Restart all services")
        print("  6. Recreate Keycloak configuration")
        print("")

        # Ask for confirmation
        try:
            response = input(
                f"{Colors.YELLOW}⚠️  This will PERMANENTLY DELETE all data. Continue? (y/N): {Colors.RESET}")
            if response.lower() not in ['y', 'yes']:
                self.print_error("Reset cancelled")
                return False
        except KeyboardInterrupt:
            print("\n")
            self.print_error("Reset cancelled")
            return False

        print("\n🚀 Starting environment reset...")

        # Step 1: Stop containers
        self.print_step("📦 Step 1", "Stopping and removing containers")
        try:
            self.run_command(['docker-compose', 'down', '--volumes',
                             '--remove-orphans'], capture_output=False)
            self.print_success("Containers stopped and removed")
        except Exception as e:
            self.print_error(f"Failed to stop containers: {e}")

        # Step 2: Remove volumes
        self.print_step("💾 Step 2", "Removing Docker volumes")
        volumes = [
            'dotnetapiguideline_sqlserver_data',
            'dotnetapiguideline_mongodb_data',
            'dotnetapiguideline_mongodb_config',
            'dotnetapiguideline_postgres_data'
        ]

        for volume in volumes:
            try:
                self.run_command(
                    ['docker', 'volume', 'rm', '-f', volume], capture_output=True, check=False)
            except:
                pass

        self.print_success("Docker volumes removed")

        # Step 3: Clean networks
        self.print_step("🌐 Step 3", "Cleaning up Docker networks")
        try:
            self.run_command(['docker', 'network', 'prune',
                             '-f'], capture_output=True)
            self.print_success("Docker networks cleaned")
        except Exception as e:
            self.print_warning(f"Network cleanup had issues: {e}")

        # Step 4: Clean images
        self.print_step("🗂️ Step 4", "Cleaning up Docker images")
        try:
            self.run_command(['docker', 'image', 'prune',
                             '-f'], capture_output=True)
            self.print_success("Dangling images removed")
        except Exception as e:
            self.print_warning(f"Image cleanup had issues: {e}")

        # Step 5: Clean .NET artifacts
        self.print_step("🧹 Step 5", "Cleaning .NET build artifacts")
        try:
            self.run_command(['dotnet', 'clean'], capture_output=True)
            # Remove bin and obj directories
            for dir_name in ['bin', 'obj']:
                if os.path.exists(dir_name):
                    self.run_command(['rm', '-rf', dir_name],
                                     capture_output=True)
            self.print_success("Build artifacts cleaned")
        except Exception as e:
            self.print_warning(f"Build cleanup had issues: {e}")

        # Step 6: Start services
        self.print_step("🚀 Step 6", "Starting Docker services")
        try:
            self.run_command(['docker-compose', 'up', '-d'],
                             capture_output=False)
            self.print_success("Docker services started")
        except Exception as e:
            self.print_error(f"Failed to start services: {e}")
            return False

        # Step 7: Wait for services
        self.print_step("⏳ Step 7", "Waiting for services to be ready")

        for service_name in ['sqlserver', 'mongodb', 'postgres', 'keycloak']:
            if self.wait_for_service(service_name):
                self.print_success(f"{service_name.title()} is ready")
            else:
                self.print_error(f"{service_name.title()} failed to start")

        # Step 8: EF migrations
        self.print_step(
            "🗃️ Step 8", "Creating and applying database migrations")
        try:
            self.run_command(['dotnet', 'ef', 'database', 'drop',
                             '-f', '--no-build'], capture_output=True, check=False)
            self.run_command(['dotnet', 'ef', 'migrations', 'add',
                             'InitialCreate', '--force'], capture_output=True, check=False)
            self.run_command(['dotnet', 'ef', 'database', 'update',
                             '--no-build'], capture_output=True, check=False)
            self.print_success("Database migrations applied")
        except Exception as e:
            self.print_warning(f"Migration issues (may be normal): {e}")

        # Step 9: Setup Keycloak
        self.print_step("🔐 Step 9", "Setting up Keycloak configuration")
        if self.setup_keycloak():
            self.print_success("Keycloak configured")
        else:
            self.print_warning("Keycloak setup had issues")

        # Final status
        self.print_header("Environment Reset Complete!", "🎉")
        self.show_status()
        print(f"{Colors.CYAN}🎯 Environment is ready for development!{Colors.RESET}")

        return True

    def restart_services(self):
        """Quick restart of services"""
        self.print_header("Quick Restart Script", "🔄")
        print("This script will:")
        print("  1. Stop all containers")
        print("  2. Restart all services")
        print("  3. Wait for services to be ready")
        print("")
        print("📝 Note: Data will be preserved")
        print("")

        # Step 1: Stop containers
        self.print_step("📦 Step 1", "Stopping containers")
        try:
            self.run_command(['docker-compose', 'down'], capture_output=False)
            self.print_success("Containers stopped")
        except Exception as e:
            self.print_error(f"Failed to stop containers: {e}")

        # Step 2: Start services
        self.print_step("🚀 Step 2", "Starting Docker services")
        try:
            self.run_command(['docker-compose', 'up', '-d'],
                             capture_output=False)
            self.print_success("Docker services started")
        except Exception as e:
            self.print_error(f"Failed to start services: {e}")
            return False

        # Step 3: Wait for services
        self.print_step("⏳ Step 3", "Waiting for services to be ready")

        for service_name in ['sqlserver', 'mongodb', 'postgres', 'keycloak']:
            if self.wait_for_service(service_name):
                self.print_success(f"{service_name.title()} is ready")
            else:
                self.print_error(f"{service_name.title()} failed to start")

        self.print_header("Quick Restart Complete!", "🎉")
        self.show_status()
        print(f"{Colors.CYAN}🎯 All services are ready!{Colors.RESET}")

        return True

    def get_token_command(self, username: str = None, password: str = None):
        """Get access token for testing"""
        username = username or self.test_user
        password = password or self.test_password

        print(f"Getting token for user: {username}")

        token = self.get_user_token(username, password)
        if token:
            print(token)
            return token
        else:
            self.print_error("Failed to get access token")
            return None

    def show_status(self):
        """Show status of all services"""
        print(f"{Colors.CYAN}🌐 Services Status:")
        print(f"  SQL Server:    http://localhost:1433")
        print(f"  MongoDB:       http://localhost:27017")
        print(f"  Mongo Express: http://localhost:8081")
        print(f"  PostgreSQL:    http://localhost:5432")
        print(f"  Keycloak:      http://localhost:8080")
        print("")

        # Show container status
        print("📦 Container Status:")
        try:
            result = self.run_command(
                ['docker-compose', 'ps'], capture_output=True)
            print(result.stdout)
        except Exception as e:
            self.print_error(f"Failed to get container status: {e}")

        print(f"{Colors.RESET}")

    def show_help(self):
        """Show help message"""
        print(__doc__)


def main():
    """Main entry point"""
    parser = argparse.ArgumentParser(
        description='DotNet API Guideline Project Tools',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  python3 tools.py setup-keycloak
  python3 tools.py reset-environment
  python3 tools.py restart-services
  python3 tools.py get-token testuser password123
  python3 tools.py status
        """
    )

    parser.add_argument('command',
                        choices=['setup-keycloak', 'reset-environment',
                                 'restart-services', 'get-token', 'status', 'help'],
                        help='Command to execute')
    parser.add_argument('username', nargs='?',
                        help='Username for get-token command')
    parser.add_argument('password', nargs='?',
                        help='Password for get-token command')

    if len(sys.argv) == 1:
        parser.print_help()
        return

    args = parser.parse_args()
    tools = ProjectTools()

    try:
        if args.command == 'setup-keycloak':
            tools.setup_keycloak()
        elif args.command == 'reset-environment':
            tools.reset_environment()
        elif args.command == 'restart-services':
            tools.restart_services()
        elif args.command == 'get-token':
            tools.get_token_command(args.username, args.password)
        elif args.command == 'status':
            tools.show_status()
        elif args.command == 'help':
            tools.show_help()
    except KeyboardInterrupt:
        print(f"\n{Colors.YELLOW}Operation cancelled by user{Colors.RESET}")
        sys.exit(1)
    except Exception as e:
        print(f"{Colors.RED}Error: {e}{Colors.RESET}")
        sys.exit(1)


if __name__ == '__main__':
    main()
