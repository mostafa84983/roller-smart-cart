#!/bin/bash

HOST="smartcart-db"
PORT=1433
RETRIES=20
DELAY=3

echo "Waiting for SQL Server to be reachable at $HOST:$PORT..."

for ((i=1;i<=RETRIES;i++)); do
    /bin/bash -c "</dev/tcp/$HOST/$PORT" &>/dev/null
    if [ $? -eq 0 ]; then
        echo "SQL Server is accepting TCP connections."
        break
    fi
    echo "Attempt $i: SQL Server is still unavailable, retrying in $DELAY seconds..."
    sleep "$DELAY"
done

echo "Starting application..."
exec "$@"
