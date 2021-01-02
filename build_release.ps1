$NETFRAMEWORK = "net5.0"

function clean {
    param([string] $ProjectFolder)
    Remove-Item -Path $ProjectFolder\bin\ -Recurse
    Remove-Item -Path $ProjectFolder\obj\ -Recurse
}

function dotnetPublish {
    param([string]$Project, [string]$Runtime)
    dotnet publish $Project -c Release -f $NETFRAMEWORK --self-contained true -r $Runtime -v q /p:PublishSingleFile=true /p:PublishTrimmed=true /p:IncludeAllContentForSelfExtract=true
}

function compressAndRemove {
    param([string] $File, [string] $ZipName)
    Compress-Archive -Path $File -DestinationPath $ZipName
    Remove-Item -Path $File
}


#MAIN

$version = Read-Host "Please enter the version string (ex: 1.0)"

$projectPath = Get-Item -Path '.\KF2 Workshop URL Converter\KF2WorkshopUrlConverter.Core'
$releaseFolderPath = Get-Item -Path .\releaseZips

if(($releaseFolderPath).Exists) {
    Write-Host 'The Release folder named "'($releaseFolderPath).Name '" already exists, removing files inside folder...' -Separator '' -ForegroundColor Red
    Remove-Item $releaseFolderPath\*.*
} else {
    Write-Host 'The Release folder named "' ($releaseFolderPath).Name '" not exists, creating folder...' -Separator '' -ForegroundColor DarkRed
    New-Item -Path .\releaseZips\ -ItemType Directory
}

Write-Host "Building win-x64..." -ForegroundColor DarkGreen

clean -ProjectFolder $projectPath
dotnetPublish -Project $projectPath -Runtime "win-x64"

Move-Item -Path $projectPath\bin\Release\$NETFRAMEWORK\win-x64\publish\KF2WorkshopUrlConverter.exe -Destination $releaseFolderPath
compressAndRemove -File $releaseFolderPath\KF2WorkshopUrlConverter.exe -ZipName $releaseFolderPath\KF2WorkshopUrlConverter.v$version-win64.zip

Write-Host "Building linux-x64..." -ForegroundColor DarkGreen

clean -ProjectFolder $projectPath
dotnetPublish -Project $projectPath -Runtime "linux-x64"

Move-Item -Path $projectPath\bin\Release\$NETFRAMEWORK\linux-x64\publish\KF2WorkshopUrlConverter -Destination $releaseFolderPath
compressAndRemove -File $releaseFolderPath\KF2WorkshopUrlConverter -ZipName $releaseFolderPath\KF2WorkshopUrlConverter.v$version-linux64.zip

Write-Host "Building osx-x64..." -ForegroundColor DarkGreen

clean -ProjectFolder $projectPath
dotnetPublish -Project $projectPath -Runtime "osx-x64"

Move-Item -Path $projectPath\bin\Release\$NETFRAMEWORK\osx-x64\publish\KF2WorkshopUrlConverter -Destination $releaseFolderPath
compressAndRemove -File $releaseFolderPath\KF2WorkshopUrlConverter -ZipName $releaseFolderPath\KF2WorkshopUrlConverter.v$version-macos64.zip

Write-Host "Done" -ForegroundColor Green
