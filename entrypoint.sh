#!/bin/bash

CS="server=$DB_HOST;uid=root;pwd=$DB_PASSWORD;database=ChoixSejour;"

echo "Connection string : "
echo $CS

echo "Replace Connection string in appsettings.json"
find . -type f -name "appsettings.json" -print0 | xargs -0 sed -i -e "s|{cs}|$CS|g"

echo "sleep 30 seconds..."
sleep 30

echo "Choix Sejour is running..."
dotnet ChoixSejour.dll
