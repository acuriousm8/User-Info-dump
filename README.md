# User-Info-dump
A simple c# GUI meant to abuse the net user command to scrape information. It was made to assist me in security consulting work for my high school to figure out what accounts had the desired permissions I wanted. It takes advantage of the fact that the net user command provides a list of accounts on the network and a list of permissions for each account and that this command cannot be rate-limited or restricted.
## Features
* Get a list of every account on the computer
* Get a list of every account on the domain
* Get the list of user permissions for every account on the computer
* Get the list of user permissions for every account on the domain
* Save any desired information to a text file for later parsing
## Notes
* It is very slow without multithreading... I lost the version with multithreading. It wouldn't be difficult to add back, but I no longer have access to a network to test it with.
* Speed also depends on how fast the server can fulfill requests, not just how many you can do at once.
