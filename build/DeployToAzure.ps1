$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

$scriptPath = Split-Path -parent $PSCommandPath
[Xml]$xml = Get-Content ("{0}\{1}.publishsettings" -f $scriptPath, "chat8705.azurewebsites.net")
$password = $xml.publishData.publishProfile.userPWD.get(0)

$publishXmlFile = "chat8705.pubxml"

&$msbuildExe ..\src\Chat\Chat.csproj /t:Clean
&$msbuildExe ..\src\Chat\Chat.csproj /p:VisualStudioVersion=12.0 /p:AspnetCompilerPath="C:\windows\Microsoft.NET\Framework64\v4.0.30319" /p:Configuration=Cloud /p:DeployOnBuild=true /p:PublishProfile=$publishXmlFile /p:Password=$password


						


