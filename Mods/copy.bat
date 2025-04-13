@echo off

:: 获取文档文件夹路径
for /f "delims=" %%i in ('powershell -Command "[Environment]::GetFolderPath('MyDocuments')"') do set docpath=%%i

set targetdir=%docpath%\Klei\OxygenNotIncluded\mods\Dev\大一统

:: 执行文件夹复制操作（递归复制并覆盖）
xcopy /E /I /Y "%~dp0大一统" "%targetdir%"

:: 延时 1 秒
timeout /t 1 /nobreak >nul