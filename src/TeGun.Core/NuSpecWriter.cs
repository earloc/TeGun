//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeGun.Configuration;
using TeGun.Models;

namespace TeGun {
    public class NuSpecWriter {

        private readonly Config _Config;
        private readonly string _Template;

        public NuSpecWriter(Config config) {
            _Config = config ?? throw new ArgumentNullException(nameof(config), "must not be null");

            if (!string.IsNullOrEmpty(config.Package.NuSpecTemplate))
                _Template = File.ReadAllText(_Config.Package.NuSpecTemplate);
            else _Template = Strings.NuSpecTemplate;
        }

        public string GetNuSpec(BundleModel bundle) {
            var nuspec = _Template
                .Replace("{PackageId}", bundle.PackageId)
                .Replace("{PackageVersion}", bundle.PackageVersion.ToString())
                .Replace("{PackageTitle}", bundle.PackageId)
                .Replace("{Authors}", _Config.Package.Authors)
                .Replace("{Owners}", _Config.Package.Owners)
                .Replace("{Description}", $"auto-generated package based on {bundle.Assemblies.FirstOrDefault()?.File.Name} - TeGun (https://github.com/earloc/TeGun")
                .Replace("{ReleaseNotes}", $"{bundle.PackageVersion} - auto-generated with awesome TeGun (https://github.com/earloc/TeGun)")
                .Replace("{Tags}", CreateTags(bundle.PackageId))
                .Replace("{Dependencies}", CreateDependencies(bundle.ReferencedBundles))
                .Replace("{Files}", CreateFiles(bundle.Assemblies));

            return nuspec;
        }

        private string CreateTags(string packageName) {
            var pacakgeTags = packageName.Split('.');
            var tags = string.Join(" ", string.Join(" ", _Config.Package.Tags), string.Join(" ", pacakgeTags));
            return tags;
        }

        private string CreateDependencies(IEnumerable<BundleModel> referencedBundles) {
            var dependencies = referencedBundles.Select(x => $"      <dependency id=\"{x.PackageId}\" version=\"{x.PackageVersion}\" />");
            return string.Join(Environment.NewLine, dependencies);
        }

        private string CreateFiles(IEnumerable<AssemblyModel> assemblies) {
            
            var files = _Config.Package.TargetFrameworks
                .SelectMany(fx => assemblies
                    .Select(x => $"      <file src=\"{x.File.FullName}\" target=\"lib\\{fx}\" />")
                );

            return string.Join(Environment.NewLine, files);
        }
    }
}
