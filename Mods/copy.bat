@echo off

:: ��ȡ�ĵ��ļ���·��
for /f "delims=" %%i in ('powershell -Command "[Environment]::GetFolderPath('MyDocuments')"') do set docpath=%%i

:: ƴ��Ŀ��·��
set targetpath=%docpath%\Klei\OxygenNotIncluded\mods\Dev\��һͳ\��һͳ.dll

:: ִ���ļ����Ʋ���
copy %~dp0��һͳ\��һͳ.dll "%targetpath%"

:: ��ʱ 1 ��
timeout /t 1 /nobreak >nul