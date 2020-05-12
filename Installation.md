# Local Build, Installation And Development Guide

## Local Build & Installation
1. Old Scripts
    - Colne the Repo (C: is recommended)
    - Build it in VS
    - Change directory to the build directory `cd path\to\ExceptionalReSharper\build\OldScripts\`
    - Run `CreatePackage.bat`
    - Now you should find a new package in `path\to\ExceptionalReSharper\build\Packages`
    - In VS => Extensions => Resharper => Options => Extension Manager => Add => 
        Name: Exceptional Local
        Path: `path\to\ExceptionalReSharper\build\Packages` (Recommended to clone the Repo on the local volume "C:")
    - In VS => Extensions => Resharper => Extension Manager => "Find the extension & Install it"

2. New Build
    - Change directory to the build directory `cd path\to\ExceptionalReSharper\build`
    - Run `build.ps1`
    - Now you should find a new package in `path\to\ExceptionalReSharper\build\Packages`
    - Copy the *.nupkg file to your local repository
    - In VS => Extensions => Resharper => Options => Extension Manager => Add => 
        Name: Exceptional Local
        Path: `path\to\ExceptionalReSharper\build\Packages` (Recommended to clone the Repo on the local volume "C:")
    - In VS => Extensions => Resharper => Extension Manager => "Find the extension & Install it"   

## Development Build

### Setup your Environment

You can find a full guide on [JetBrains.com](https://www.jetbrains.com/help/resharper/sdk/HowTo/Start/SetUpEnvironment.html).

1. Install ReSharper to a experimental Visual Studio hive
2. Install Exceptional to the hive
3. Configure Debuging (`Exceptional project -> Properties -> Debug`)
    1. Change `Start action` to `Start external program` and enter `devenv.exe` (Visual Studio, `Common7\IDE\devenv.exe`)
    2. Add `Command line arguments`: `/rootSuffix {name of your experimental hive}`
    3. Optional: Add `/ReSharper.Internal` and a path to a testing project
4. Add `<PropertyGroup><HostFullIdentifier>{ReSharper installation of the experimental hive}</HostFullIdentifier></PropertyGroup>`

Running MSBuild (e. g. by starting debugging) will copy the assembly to your ReSharper installation. Please see the `Build` log in your `Output` window for possible errors (e. g. ReSharper installation cannot be found).

## Debugging

Please read this [guide](https://www.jetbrains.com/help/resharper/sdk/Extensions/Plugins/Debugging.html).

## Troubleshooting

### I cannot install my local version

Uninstall previous installations of Exceptional for ReSharper and clear `C:\Users\%username%\AppData\Local\JetBrains\plugins` and `C:\Users\%username%\AppData\Local\NuGet\Cache`, after that restart VS.
