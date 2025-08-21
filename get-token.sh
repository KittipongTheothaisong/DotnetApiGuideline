#!/bin/bash

# Quick token getter for testing
# Usage: ./get-token.sh [username] [password]

KEYCLOAK_URL="http://localhost:8080"
REALM_NAME="dotnet-api-guideline"
CLIENT_ID="dotnet-api-client"
ADMIN_USER="admin"
ADMIN_PASSWORD="admin123"

USERNAME="${1:-testuser}"
PASSWORD="${2:-password123}"
 
ADMIN_TOKEN=$(curl -s -X POST "$KEYCLOAK_URL/realms/master/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=admin-cli" \
  -d "username=$ADMIN_USER" \
  -d "password=$ADMIN_PASSWORD" | jq -r '.access_token')

if [ "$ADMIN_TOKEN" = "null" ] || [ -z "$ADMIN_TOKEN" ]; then
    echo "❌ Failed to get admin token"
    exit 1
fi

# Get client secret
CLIENT_UUID=$(curl -s -X GET "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients?clientId=$CLIENT_ID" \
  -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.[0].id')

CLIENT_SECRET=$(curl -s -X GET "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients/$CLIENT_UUID/client-secret" \
  -H "Authorization: Bearer $ADMIN_TOKEN" | jq -r '.value')

# Get user token
TOKEN_RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/realms/$REALM_NAME/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=$CLIENT_ID" \
  -d "client_secret=$CLIENT_SECRET" \
  -d "username=$USERNAME" \
  -d "password=$PASSWORD")

ACCESS_TOKEN=$(echo $TOKEN_RESPONSE | jq -r '.access_token')
EXPIRES_IN=$(echo $TOKEN_RESPONSE | jq -r '.expires_in')

if [ "$ACCESS_TOKEN" = "null" ] || [ -z "$ACCESS_TOKEN" ]; then
    echo "❌ Failed to get access token"
    echo "Response: $TOKEN_RESPONSE"
    exit 1
fi

echo $ACCESS_TOKEN