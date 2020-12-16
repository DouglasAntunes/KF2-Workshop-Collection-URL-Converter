# KF2 Workshop Collection URL Converter

[![Tests](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/workflows/Tests/badge.svg)](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/actions?query=workflow%3ATests+branch%3Amaster)
[![Coverage Status](https://coveralls.io/repos/github/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/badge.svg?branch=master)](https://coveralls.io/github/DouglasAntunes/KF2-Workshop-Collection-URL-Converter?branch=master)

KF2 Workshop Collection URL Converter is a tool to help server owners to keep updated custom content(maps, items) on a server using Steam Workshop Collections. (Like [this](https://steamcommunity.com/sharedfiles/filedetails/?id=882417829 "Example Collection") Example Collection)

The use of Steam Workshop Collection facilitate users to download all content before joining the server.

## LICENSE

MIT License, see [here](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/blob/master/LICENSE "MIT License").

## Download

Go to [Releases Page](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/releases "Releases Page").

## How to Use

### Windows

1. Download the last version of [KF2 Workshop Collection URL Converter](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/releases "Releases Page") for win64.

2. Extract the files on a folder.

3. Open PowerShell or Command Prompt on the folder.

4. Type ```KF2WorkshopUrlConverter -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829``` changing the url link with the desired collection (http or https).

5. See the results.

- You can export this list do a file using the ```-o path\to\file``` parameter.
Example: ```KF2WorkshopUrlConverter -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829 -o maps.txt```

- For more help, type ```KF2WorkshopUrlConverter -help```

### Linux / MacOS

1. Download the last version of [KF2 Workshop Collection URL Converter](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/releases "Releases Page") for your respective OS.

2. Extract the files on a folder.

3. Open Terminal on the folder.

4. Type ```chmod +x ./KF2WorkshopUrlConverter``` to grant execution rights.

5. Type ```./KF2WorkshopUrlConverter -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829``` changing the url link with the desired collection (http or https).

6. See the results.

- You can export this list do a file using the ```-o path\to\file``` parameter.
Example: ```./KF2WorkshopUrlConverter -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829 -o maps.txt```

- For more help, type ```./KF2WorkshopUrlConverter -help```

## How to Create a Collection

Check this [guide](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/blob/master/HowToCreateACollection.md "Guide").
