# dotnet-tegun - The KickAss DevOps utility #

for creating NuGet-Packages out of ordinary, unpackaged assemblies.

[![Build status](https://ci.appveyor.com/api/projects/status/sjvm65cyqa07kung/branch/master?svg=true)](https://ci.appveyor.com/project/earloc/tegun/branch/master)

[![NuGet](https://img.shields.io/badge/nuget-v0.4.*--pre-blue.svg)](https://img.shields.io/badge/nuget-v0.4.*--pre-blue.svg)



## Synopsis ##
TeGun (NuGet reversed) is a simple, yet handy cross-platform(.netcore) commandline-utility for creating nuget packages (nupkg) from a bunch of ordinary, unpackaged assemblies. 
The main goal is to provide a fast tooling experience when dealing with third-party assemblies that do not come packaged via e.g. Nuget.org.
Such assemblies quick become unhandy when confronted with any CI/CD-Environment, as those often need to be either installed on (a variaty of) build-machines or - most of the time - get their way into version control systems, which unnecessarily bloats the code repository (especially in cloud-hosted build environments, where such dependencies can´t even be installed).

Even though most of the open-source based goodness out there is consumable via NuGet-feeds, various assemblies and whole frameworks exists that are not accessible via this route (maybe because they require a paid subscription and therfore are not in any public feed).

To help out in such on-premise scenarios, this project was born.

## How it works ##
TeGun simply scans a directory for assemblies and analyzes their dependency-hierarchy. This hierarchy-information is then used to generate nuspec-files, which in turn can be used in conjunction with NuGet.exe to generate packages. The nuspec-files can then be used to automate package-creation or be used as a basis for more or finer grained modifications.

## Get the bits

###Download
Sorry folks, no pre-built / standalone releases yet. There might be a Chocolately distribution in the future, see [#5](https://github.com/earloc/TeGun/issues/5) and vote it up to get it considered.

### DotnetCliTool
TeGun is available as a dotnet cli extension. Just place an ItemGroup in any project-file that supports .NetCore tooling :

    <ItemGroup>
      <DotNetCliToolReference Include="dotnet-tegun" Version="0.3.0-pre"/>
    </ItemGroup>

and run

    dotnet restore

### Project template
To autmotate above mentioned steps, there is also a project template available. When installed and invoked, it will create a project-file with the needed *DotNetCliToolReference*

Just run

    dotnet new -i dotnet-tegun.template
to install it. Afterwards, executing
    
    dotnet new tegun
    dotnet restore

will get you going.

### Any other options?
Just clone the repo and built for your self ;).

## Prerequisits ##
- [.net core 2 SDK](https://www.microsoft.com/net/learn/get-started/windows) installed for building and running
- basic knowledge of [nuspec](https://docs.microsoft.com/en-us/nuget/schema/nuspec)

## Sample ##
Let´s assume we have an app that does some MS Office-automation. Hence it requires certain assemblies from the Office-SDKs. We identify a bunch of available assemblies and copy them to c:\temp\officeSDK. The contents of the directory might look like his:

- Microsoft.Office.Interop.OneNote.dll
- Microsoft.Office.Interop.Outlook.dll
- Microsoft.Office.Interop.OutlookViewCtl.dll
- Microsoft.Office.Interop.PowerPoint.dll
- Microsoft.Office.Interop.Publisher.dll
- ...


In order to invoke TeGun, we utilize the dotnet cli extension point as described above. So let´s create a TeGun-enabled project

    dotnet new tegun
    dotnet restore

> This will create a project with a *DotNetCliToolReference* to the actual TeGun-Executable, which will be downloaded upon restore


TeGun uses a config file for most of it´s settings atm (see [#1](https://github.com/earloc/TeGun/issues/1)). Let´s initialize a new config with:

    dotnet tegun init office

This wil create an initial *office.config.json* pointing to assemblies in the current directory.

Open the config to tweak the package-creation to your needs:

    {
      "assemblies": {
        "sources": [ 
          "."  //scan the local directory for assemblies
        ],
        "excludePatterns": [ 
          "System.*" //exclude matching assemblies
        ],
        "bundles": [
          {
            "searchPattern": "Microsoft.Office.*", //Bundle all matching assemblies
            "packageId": "OfficeBundle.Sample",    //into a package
            "packageVersion": "1.2.3.4"            //with this version
          }
        ],
        "substitutions": [
          {
            "replace": "Interop",
            "with": "xxx"
          },
        ]
      },
      //general nuspec-creation settings
      "package": { 
        "authors": "earloc",
        "owners": "earloc",
        "tags": [ //additional tags
          "tags",
          "go",
          "here"
        ],
        "targetframeworks": [
          "net45"
        ],
        "versionFormat": "{Major}.{Minor}.{Build}.{Revision}", //Use full version number from scanned assembly as package version
        "title": {
          "prefix": "earloc.", //prefix package names
          "suffix": ""
        },
        "nuspecTemplate": null
      }
    }


If we now invoke the command:

    dotnet tegun nuspec office

tegun will create nuspec-files according to the above settings in the subfolder *office/nuspecs*:

- OfficeBundle.Sample.1.2.3.4.nuspec
- earloc.Microsoft.Office.xxx.OneNote.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Outlook.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.OutlookViewCtl.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.PowerPoint.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Publisher.14.0.0.0.nuspec
- ...

These nuspecs can now be further tweaked to your needs and be used to produce packages, e.g. with a simple batch file in the *office* subfolder:

    set nuget=nuget.exe
    for /R nuspecs %%f in (*.nuspec) do (
    	%nuget% pack %%f  -OutputDirectory packages
    )

will produce the following packages at *office/packages*:

- OfficeBundle.Sample.1.2.3.4.nupkg
- earloc.Microsoft.Office.xxx.OneNote.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Outlook.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.OutlookViewCtl.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.PowerPoint.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Publisher.14.0.0.nupkg
- ...



## Contributing

- Fork it
- Create your feature branch 
#
    git checkout -b my-new-feature

- Commit your changes 
#
    git commit -am 'Add some feature'

- Push to the branch
# 
    git push origin my-new-feature

- Create new Pull Request

Also consider joining the slack-channel, see [#6](https://github.com/earloc/TeGun/issues/6)
