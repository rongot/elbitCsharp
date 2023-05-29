dotnet restore elbit.sln
dotnet build elbit.sln --no-restore -p:Configuration=release -p:Platform:x64
dotnet test bin\x64\Release\net6.0\elbit.dll --settings config.runsettings --filter TestCategoty=SiteManagment
pause