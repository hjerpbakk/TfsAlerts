#! /bin/bash
set -e

./down.sh
./build.sh

docker-compose up -d