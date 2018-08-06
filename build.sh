#! /bin/bash
set -e
docker build --pull --build-arg SlackAPIToken -t dips/tfs-alerts .