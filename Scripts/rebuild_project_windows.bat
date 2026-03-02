@echo off
setlocal

REM Run from project root
if exist Binaries rmdir /s /q Binaries
if exist Intermediate rmdir /s /q Intermediate
if exist .vs rmdir /s /q .vs

echo Deleted Binaries/Intermediate/.vs

echo.
echo Next steps:
echo 1) Right click NebulaStrike.uproject -> Generate Visual Studio project files
echo 2) Open NebulaStrike.sln
echo 3) Build Development Editor Win64
echo 4) Open NebulaStrike.uproject

endlocal
