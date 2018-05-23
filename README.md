# KF2 Workshop Collection URL Converter

KF2 Workshop Collection URL Converter is a tool to help server owners to keep updated custom content(maps, items) on a server using Steam Workshop Collections. (Like [this](https://steamcommunity.com/sharedfiles/filedetails/?id=882417829 "Example Collection") Example Collection)

The use of Steam Workshop Collection facilitate users to download all content before joining the server.

## LICENSE

MIT License, see [here](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/blob/master/LICENSE "MIT License").

## Download

Go to [Releases Page](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/releases "Releases Page").

## How to Use

If you don't have the .NET Core Runtime, Download [Here](https://www.microsoft.com/net/download/core#/runtime "Download .NET Core")

1. Download the last version of [KF2 Workshop Collection URL Converter](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/releases "Releases Page").

2. Extract the files on a folder.

3. Open Terminal, PowerShell or Command Prompt on the folder.

4. Type ```dotnet KF2WorkshopUrlConverter.dll -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829``` changing the url link with the desired collection (http or https).

5. See the results.

- You can export this list do a file using the ```-o path\to\file``` parameter.
Example: ```dotnet KF2WorkshopUrlConverter.dll -u http://steamcommunity.com/sharedfiles/filedetails/?id=882417829 -o maps.txt```

For more help, type ```dotnet KF2WorkshopUrlConverter.dll -help```

## How to Create a Collection

Check this [guide](https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter/blob/master/HowToCreateACollection.md "Guide").
