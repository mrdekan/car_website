@echo off

set "archiveName=publish.zip"

set "scriptDir=%~dp0"

cd "%scriptDir%car_website\car_website" || (
    echo Error: Not found: %scriptDir%car_website\car_website
    pause
    exit /b 1
)

dotnet publish -c Release

cd "bin\Release\net7.0\publish" || (
    echo Error: Not found: %scriptDir%car_website\car_website\bin\Release\net7.0\publish
    pause
    exit /b 1
)

powershell -Command "Compress-Archive -Path * -DestinationPath '%scriptDir%%archiveName%' -Force"

echo The build has been successfully completed

pause
