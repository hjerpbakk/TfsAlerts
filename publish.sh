#! /bin/bash
set -e

VERSION=$(cat Hjerpbakk.Tfs.Alerts/wwwroot/VERSION.txt)

./build.sh
docker tag dips/tfs-alerts:latest vt-optimus-solr02:5000/dips/tfs-alerts:$VERSION
docker push vt-optimus-solr02:5000/dips/tfs-alerts:$VERSION
