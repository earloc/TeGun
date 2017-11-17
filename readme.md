# TeGun - The KickAss DevOps utility #

for creating NuGet-Packages out of ordinary, unpackaged assemblies.

## Synopsis ##
TeGun (NuGet reversed) is a simple, yet handy cross-platform(.netcore) commandline-utility for creating nuget packages (nupkg) from a bunch of ordinary assembliy files. 
The main goal is to provide a fast tooling experience when dealing with third-party assemblies that do not come packaged via e.g. Nuget.org.
Such assemblies quick become unhandy when confronted with any CI/CD-Environment, as those often need to be either installed on (a variaty of)build-machines or - most of the time - get their way into version control systems, which unnessecarely bloats the code repository (especially in cloud-hosted build environments, where such dependencies can´t even be installed).

Even though most of the open-source based goodness out there is consumable via NuGet-feeds, various assemblies and whole frameworks  exists that are not accessible via this route (maybe because they require a paid subscription and therfore are not in any public feeds).

To help out in such on-premise scenarios, this project was born.

## How it works ##
TeGun simply scans a directory for assemblies and analyzes their dependency-hierarchy. This hierarchy-information is then used to generate nuspec-files, which in turn can be used in conjunction with NuGet.exe to generate packages. The nuspec-files can then be used to automate package-creation or be used as a basis for more or finer grained modifications.

## Download ##
Sorry folks, no pre-built releases yet.
Just clone the repo and built for your self.

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
- Microsoft.Office.Interop.SharePointDesigner.dll
- Microsoft.Office.Interop.SharePointDesignerPage.dll
- Microsoft.Office.Interop.SmartTag.dll
- Microsoft.Office.Interop.Visio.dll
- Microsoft.Office.Interop.Visio.SaveAsWeb.dll
- Microsoft.Office.Interop.VisOcx.dll
- Microsoft.Office.Interop.Word.dll
- Microsoft.Vbe.Interop.dll
- Microsoft.Vbe.Interop.Forms.dll
- Office.dll
- IPDMCTRL.dll
- Microsoft.Office.InfoPath.Permission.dll
- Microsoft.Office.interop.access.dao.dll
- Microsoft.Office.Interop.Access.dll
- Microsoft.Office.Interop.Excel.dll
- Microsoft.Office.Interop.Graph.dll
- Microsoft.Office.Interop.InfoPath.dll
- Microsoft.Office.Interop.InfoPath.SemiTrust.dll
- Microsoft.Office.Interop.InfoPath.Xml.dll
- Microsoft.Office.Interop.MSProject.dll

TeGun uses a config file for most of it´s settings atm. Think of it as C#-project file or a meta-nuspec. Let´s initialize a new config:

    tegun init office

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


If we know invoke the command:

    tegun nuspec office

tegun will create nuspec-files according to the above settings in the subfolder *office/nuspecs*:

- OfficeBundle.Sample.1.2.3.4.nuspec
- earloc.Microsoft.Office.xxx.InfoPath.SemiTrust.11.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.InfoPath.Xml.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.MSProject.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.OneNote.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Outlook.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.OutlookViewCtl.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.PowerPoint.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Publisher.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.SharePointDesigner.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.SharePointDesignerPage.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.SmartTag.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Visio.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Visio.SaveAsWeb.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.VisOcx.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Word.14.0.0.0.nuspec
- earloc.Microsoft.Vbe.xxx.14.0.0.0.nuspec
- earloc.Microsoft.Vbe.xxx.Forms.11.0.0.0.nuspec
- earloc.office.14.0.0.0.nuspec
- earloc.ipdmctrl.11.0.0.0.nuspec
- earloc.Microsoft.Office.InfoPath.Permission.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Access.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Access.Dao.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Excel.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.Graph.14.0.0.0.nuspec
- earloc.Microsoft.Office.xxx.InfoPath.14.0.0.0.nuspec

These nuspecs can now be further tweaked to your needs and be used to produce packages, e.g. with a simple batch file in the *office* subfolder:

    set nuget=nuget.exe
    for /R nuspecs %%f in (*.nuspec) do (
    	%nuget% pack %%f  -OutputDirectory packages
    )

will produce the following packages at *office/packages*:

- OfficeBundle.Sample.1.2.3.4.nupkg
- earloc.Microsoft.Office.xxx.SharePointDesigner.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.SharePointDesignerPage.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.SmartTag.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Visio.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Visio.SaveAsWeb.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.VisOcx.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Word.14.0.0.nupkg
- earloc.Microsoft.Vbe.xxx.14.0.0.nupkg
- earloc.Microsoft.Vbe.xxx.Forms.11.0.0.nupkg
- earloc.office.14.0.0.nupkg
- earloc.ipdmctrl.11.0.0.nupkg
- earloc.Microsoft.Office.InfoPath.Permission.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Access.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Access.Dao.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Excel.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Graph.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.InfoPath.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.InfoPath.SemiTrust.11.0.0.nupkg
- earloc.Microsoft.Office.xxx.InfoPath.Xml.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.MSProject.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.OneNote.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Outlook.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.OutlookViewCtl.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.PowerPoint.14.0.0.nupkg
- earloc.Microsoft.Office.xxx.Publisher.14.0.0.nupkg




