#Update submodules
git submodule init
git submodule update

#Build EllipticBit.Controls
\"/C/Program Files (x86)/MSBuild/12.0/Bin/MSBuild.exe" EllipticBit.Controls.sln -nologo -p:Configuration=Release -t:Clean\;Build -p:TrackFileAccess=false