#!/bin/bash

set -euo pipefail

# Paths
SRC_FILE="../../../SPW.Infrastructure/infra/terraform/rds/scripts/database-script.sql"
DEST_DIR="."
DOCKER_COMPOSE_FILE="docker-compose-local.yaml"
COPIED_FILE="${DEST_DIR}/database-script.sql"

# Copy the file
echo "Copying the file..."
if cp "$SRC_FILE" "$DEST_DIR"; then
    echo "File copied successfully."
else
    echo "Error copying the file. Please check the paths."
    exit 1
fi

# Run docker-compose
echo "Running docker-compose..."
if docker-compose -f "$DOCKER_COMPOSE_FILE" up -d; then
    echo "Docker-compose executed successfully."
else
    echo "Error running docker-compose. Please check the file and the environment."
    exit 1
fi

echo "Wait docker-compose..."
sleep 10

# Delete the file
echo "Deleting the copied file..."
if rm -f "$COPIED_FILE"; then
    echo "File deleted successfully."
else
    echo "Error deleting the file. Please check permissions."
    exit 1
fi