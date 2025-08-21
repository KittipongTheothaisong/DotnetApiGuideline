#!/bin/bash

echo "=========================================="
echo "🚀 Keycloak Complete Setup Script"
echo "=========================================="

# Configuration variables
KEYCLOAK_URL="http://localhost:8080"
ADMIN_USER="admin"
ADMIN_PASSWORD="admin123"
REALM_NAME="dotnet-api-guideline"
CLIENT_ID="dotnet-api-client"
TEST_USER="testuser"
TEST_PASSWORD="password123"

echo "📋 Configuration:"
echo "  Keycloak URL: $KEYCLOAK_URL"
echo "  Realm: $REALM_NAME"
echo "  Client: $CLIENT_ID"
echo "  Test User: $TEST_USER"
echo "  (Client secret will be generated automatically)"
echo ""

# Step 1: Wait for Keycloak to be ready
echo "⏳ Step 1: Waiting for Keycloak to be ready..."
until curl -s "$KEYCLOAK_URL" > /dev/null; do
    echo "   Waiting for Keycloak..."
    sleep 2
done
echo "✅ Keycloak is ready!"

# Step 2: Get admin token
echo "🔑 Step 2: Getting admin token..."
ADMIN_TOKEN=$(curl -s -X POST "$KEYCLOAK_URL/realms/master/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=admin-cli" \
  -d "username=$ADMIN_USER" \
  -d "password=$ADMIN_PASSWORD" | jq -r '.access_token')

if [ "$ADMIN_TOKEN" = "null" ] || [ -z "$ADMIN_TOKEN" ]; then
    echo "❌ Failed to get admin token. Check Keycloak credentials."
    exit 1
fi
echo "✅ Admin token obtained"

# Step 3: Create Realm
echo "🏠 Step 3: Creating realm '$REALM_NAME'..."
curl -s -X POST "$KEYCLOAK_URL/admin/realms" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"realm\": \"$REALM_NAME\",
    \"enabled\": true,
    \"displayName\": \"DotNet API Guideline Realm\"
  }" > /dev/null

echo "✅ Realm created"

# Step 4: Create Client
echo "👤 Step 4: Creating client '$CLIENT_ID'..."
CLIENT_RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"clientId\": \"$CLIENT_ID\",
    \"enabled\": true,
    \"clientAuthenticatorType\": \"client-secret\",
    \"standardFlowEnabled\": true,
    \"directAccessGrantsEnabled\": true,
    \"serviceAccountsEnabled\": true,
    \"publicClient\": false,
    \"protocol\": \"openid-connect\",
    \"redirectUris\": [\"http://localhost:5249/*\"],
    \"webOrigins\": [\"http://localhost:5249\"],
    \"attributes\": {
      \"access.token.lifespan\": \"300\"
    }
  }")

echo "✅ Client created"

# Step 4b: Get the client UUID and retrieve the secret
echo "🔑 Step 4b: Getting client secret..."
CLIENT_UUID=$(curl -s -X GET "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients?clientId=$CLIENT_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[0].id')

CLIENT_SECRET=$(curl -s -X GET "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients/$CLIENT_UUID/client-secret" \
  -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.value')

echo "✅ Client secret retrieved: ${CLIENT_SECRET:0:20}..."

# Step 5: Create Test User
echo "👥 Step 5: Creating test user '$TEST_USER'..."
USER_RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/admin/realms/$REALM_NAME/users" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"username\": \"$TEST_USER\",
    \"enabled\": true,
    \"emailVerified\": true,
    \"firstName\": \"Test\",
    \"lastName\": \"User\",
    \"email\": \"test@example.com\"
  }")

echo "✅ Test user created"

# Step 5b: Set user password
echo "🔒 Step 5b: Setting user password..."
USER_ID=$(curl -s -X GET "$KEYCLOAK_URL/admin/realms/$REALM_NAME/users?username=$TEST_USER" \
  -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[0].id')

curl -s -X PUT "$KEYCLOAK_URL/admin/realms/$REALM_NAME/users/$USER_ID/reset-password" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"type\": \"password\",
    \"value\": \"$TEST_PASSWORD\",
    \"temporary\": false
  }" > /dev/null

echo "✅ User password set"

# Step 6: Update appsettings.json
echo "⚙️  Step 6: Updating appsettings.json..."

# Use jq to update only the Keycloak section
jq --arg authority "$KEYCLOAK_URL/realms/$REALM_NAME" \
   --arg audience "account" \
   --arg metadata "$KEYCLOAK_URL/realms/$REALM_NAME/.well-known/openid_configuration" \
   '.Keycloak.Authority = $authority | 
    .Keycloak.Audience = $audience | 
    .Keycloak.MetadataAddress = $metadata' \
   appsettings.json > appsettings.json.tmp && mv appsettings.json.tmp appsettings.json

echo "✅ appsettings.json updated"

# Step 7: Get test access token
echo "🎫 Step 7: Getting test access token..."
TOKEN_RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/realms/$REALM_NAME/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=$CLIENT_ID" \
  -d "client_secret=$CLIENT_SECRET" \
  -d "username=$TEST_USER" \
  -d "password=$TEST_PASSWORD")

ACCESS_TOKEN=$(echo $TOKEN_RESPONSE | jq -r '.access_token')
REFRESH_TOKEN=$(echo $TOKEN_RESPONSE | jq -r '.refresh_token')
EXPIRES_IN=$(echo $TOKEN_RESPONSE | jq -r '.expires_in')

if [ "$ACCESS_TOKEN" = "null" ] || [ -z "$ACCESS_TOKEN" ]; then
    echo "❌ Failed to get access token"
    echo "🔍 Debugging information:"
    echo "   Client ID: $CLIENT_ID"
    echo "   Client Secret: ${CLIENT_SECRET:0:10}..."
    echo "   User: $TEST_USER"
    echo "   Token response: $TOKEN_RESPONSE"
    echo ""
    echo "💡 Troubleshooting steps:"
    echo "1. Check if Keycloak is fully started"
    echo "2. Verify the realm was created correctly"
    echo "3. Check if the user exists and is enabled"
    echo "4. Ensure the client allows direct access grants"
else
    echo "✅ Access token obtained (expires in ${EXPIRES_IN}s)"
fi

echo ""
echo "=========================================="
echo "🎉 Setup Complete!"
echo "=========================================="
echo ""
echo "📋 Configuration Summary:"
echo "  Keycloak URL: $KEYCLOAK_URL"
echo "  Realm: $REALM_NAME"
echo "  Client ID: $CLIENT_ID"
echo "  Client Secret: $CLIENT_SECRET"
echo "  Test User: $TEST_USER"
echo "  Test Password: $TEST_PASSWORD"
echo ""
echo "🚀 Next Steps:"
echo "1. Restart your .NET API"
echo "2. Test authentication with your preferred method"
echo "3. Use Keycloak admin console for further configuration"
echo ""
echo "🌐 Access Points:"
echo "  Keycloak Admin: $KEYCLOAK_URL (admin/admin123)"
echo "  API Swagger: http://localhost:5249 (after starting API)"
echo ""
echo "🎫 Ready-to-Use Access Token:"
echo "=========================================="
if [ "$ACCESS_TOKEN" != "null" ] && [ ! -z "$ACCESS_TOKEN" ]; then
    echo $ACCESS_TOKEN
else
    echo "❌ No access token available"
    echo "💡 You can get one manually using:"
    echo "   POST $KEYCLOAK_URL/realms/$REALM_NAME/protocol/openid-connect/token"
    echo "   Body: grant_type=password&client_id=$CLIENT_ID&client_secret=$CLIENT_SECRET&username=$TEST_USER&password=$TEST_PASSWORD"
fi
echo "=========================================="
