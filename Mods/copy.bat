@echo off

:: ��ȡ�ĵ��ļ���·��
for /f "delims=" %%i in ('powershell -Command "[Environment]::GetFolderPath('MyDocuments')"') do set docpath=%%i

set targetdir=%docpath%\Klei\OxygenNotIncluded\mods\Dev\��һͳ

:: ִ���ļ��и��Ʋ������ݹ鸴�Ʋ����ǣ�
xcopy /E /I /Y "%~dp0��һͳ" "%targetdir%"

:: ��ʱ 1 ��
timeout /t 1 /nobreak >nul