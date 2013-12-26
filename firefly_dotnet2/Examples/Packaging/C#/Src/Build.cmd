PATH %windir%\Microsoft.NET\Framework\v3.5;%PATH%

MSBuild /t:Rebuild /p:Configuration=Release
pause
