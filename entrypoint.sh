#!/bin/bash

set -e
run_cmd="dotnet ChoixSejour.dll"

CS="server=$DBHOST;uid=root;pwd=Coucou123!;database=ChoixSejour;"

echo $CS

find . -type f -name "appsettings.json" -print0 | xargs -0 sed -i -e "s|{cs}|$CS|g"

sleep 30

exec $run_cmd
