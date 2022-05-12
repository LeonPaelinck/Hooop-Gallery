#!/bin/bash

set -o errexit  # abort on nonzero exitstatus
set -o nounset  # abort on unbound variable
set -o pipefail # don't hide some errors in pipes

# Version: 1.0

printf "\nApplicatie (container) stoppen...\n"

if docker stop app_web-server_1 > /dev/null 2>&1
then
printf "\nApplicatie is gestopt\n"
else
printf "\nApplicatie is niet gestopt\n"
fi

printf "\nContainer verwijderen...\n"

if docker rm app_web-server_1 > /dev/null 2>&1
then
printf "\nContainer is verwijderd\n"
else
printf "\nContainer is niet verwijderd\n"
fi

printf "\nApplicatie image verwijderen...\n"

if docker rmi app_web-server > /dev/null 2>&1
then
printf "\nApplicatie image is verwijderd\n"
else
printf "\nApplicatie image is niet verwijderd\n"
fi