#!/bin/bash

set -o errexit  # abort on nonzero exitstatus
set -o nounset  # abort on unbound variable
set -o pipefail # don't hide some errors in pipes

# Version: 1.0

printf "\nremove-script-testomgeving.sh downloaden..."
curl -# https://raw.githubusercontent.com/HoGentTIN/devops-project-ops-2122-h4-ops/main/remove-script-testomgeving.sh?token=AOSTVSFBDTNQFYOV2LYGHLTBTRE3E > remove-script-testomgeving.sh
cd ./src/App/
printf "\ndocker-compose.yml downloaden...\n"
curl -# https://raw.githubusercontent.com/HoGentTIN/devops-project-ops-2122-h4-ops/main/testomgeving/docker-compose.yml?token=ALK4HXQBBKRD2R6YQYPL3SLBTQSEI > docker-compose.yml
cd ./Project3H04/
printf "\nDockerfile downloaden...\n"
curl -# https://raw.githubusercontent.com/HoGentTIN/devops-project-ops-2122-h4-ops/main/testomgeving/Dockerfile?token=ALK4HXSK7LS52HAV2PRHQATBTQR2G > Dockerfile
cd ../../../

printf "\nControleren of Docker geinstalleerd is...\n"

if docker --version > /dev/null 2>&1
then
printf "\nDocker is geinstalleerd\n"
else
printf "\nDocker is niet geinstalleerd\n"
fi

printf "\nControleren of Docker Compose geinstalleerd is...\n"

if docker-compose --version > /dev/null 2>&1
then
printf "\nDocker Compose is geinstalleerd\n"
else
printf "\nDocker Compose is niet geinstalleerd\n"
fi

printf "\nControleren of Docker Engine draait...\n"

if docker info > /dev/null 2>&1
then
printf "\nDocker Engine draait\n"
else
printf "\nDocker Engine draait niet\n"
fi

printf "\nControleren of de bestanden van de testomgeving op de correcte locaties zitten...\n"

if [ -f "./src/App/docker-compose.yml" ]; then
    if [ -f "./src/App/Project3H04/Dockerfile" ]; then
    printf "\nAlle bestanden zitten op de correcte locatie\n"
    else
    printf "\nDockerfile zit niet bij de .sln file"
    fi
else 
printf "\ndocker-compose.yml zit niet in de App folder"
fi

#printf "\nHttps developer certificate genereren...\n"
#dotnet dev-certs https --trust

printf "\nTestomgeving builden...\n"

cd ./src/App
if docker-compose build > /dev/null 2>&1
then
printf "\nTestomgeving is succesvol gebuild\n"
else
printf "\nTestomgeving is niet gebuild\n"
fi

printf "\nTestomgeving opzetten in een container...\n"

if docker-compose up -d > /dev/null 2>&1
then
printf "\nTestomgeving is succesvol aan het runnen in een container\n"
else
printf "\nTestomgeving is niet aan het runnen in een container\n"
fi

printf "\nDe applicatie in je webbrowser openen...\n"

if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    xdg-open http://localhost:5000/ # Linux
elif [[ "$OSTYPE" == "darwin"* ]]; then
    open http://localhost:5000/ # Mac OSX
elif [[ "$OSTYPE" == "msys" ]]; then
    start http://localhost:5000/ # Windows
fi
