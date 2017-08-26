#Update submodules
git submodule init
git submodule update

#Build EllipticBit.Controls
"/C/Program Files (x86)/Microsoft Visual Studio/2017/Enterprise/MSBuild/MSBuild.exe" EllipticBit.Controls.sln -nologo -p:Configuration=Release -t:Clean\;Build -p:TrackFileAccess=false