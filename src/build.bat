%NuGet% restore %SourcesPath%\src\TinyLittleMvvm.sln -PackagesDirectory %SourcesPath%\src\packages\

"%MsBuildExe%" %SourcesPath%\src\TinyLittleMvvm.sln /t:%Targets% /p:Configuration=%Configuration%