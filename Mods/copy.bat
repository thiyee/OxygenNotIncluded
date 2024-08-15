@echo off

:: 获取文档文件夹路径
for /f "delims=" %%i in ('powershell -Command "[Environment]::GetFolderPath('MyDocuments')"') do set docpath=%%i

:: 拼接目标路径
set targetpath=%docpath%\Klei\OxygenNotIncluded\mods\Dev\大一统\大一统.dll

:: 执行文件复制操作
copy %~dp0大一统\大一统.dll "%targetpath%"

:: 延时 1 秒
timeout /t 1 /nobreak >nul