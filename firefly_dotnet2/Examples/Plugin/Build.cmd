PATH %windir%\Microsoft.NET\Framework\v3.5;%PATH%
if not exist Bin md Bin

cd Src
MSBuild /t:Rebuild /p:Configuration=Release
cd ..

copy ..\..\Bin\System.Core.dll Bin\
copy ..\..\Bin\System.Speech.dll Bin\
copy ..\..\Bin\Firefly.Core.dll Bin\
copy ..\..\Bin\Firefly.GUI.dll Bin\
copy ..\..\Bin\Firefly.Project.dll Bin\
copy ..\..\Bin\TextLocalizer.exe Bin\
copy ..\..\Bin\TextLocalizer.*.dll Bin\
copy *.locproj Bin\
copy *.locplugin Bin\
