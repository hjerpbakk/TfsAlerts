#! /bin/bash
set -e

export tag=

docker-compose stop tfs-alerts
docker-compose rm --force