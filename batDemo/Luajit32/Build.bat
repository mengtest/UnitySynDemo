@echo off
set srcPath=%1
cd /d %srcPath%

for /r %%i in (*.lua) do (
   call %~dp0\luajit32.exe -b -g %%~fi %%~fi.bytes
)
rem pause
