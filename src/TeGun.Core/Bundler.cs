//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using TeGun.Configuration;
using TeGun.Models;

namespace TeGun {
    public class Bundler {
        private readonly Config _Config;

        public Bundler(Config config) {
            this._Config = config ?? throw new ArgumentNullException(nameof(config), "must not be null");
        }

        public IEnumerable<BundleModel> GetBundles(IEnumerable<AssemblyModel> assemblies) {

            var bundles = new List<BundleModel>();

            

            foreach (var assembly in assemblies) {
                var bundle = new BundleModel() {
                    PackageId = GetPackageNameFor(assembly.Name),
                    PackageVersion = GetPackagVersion(assembly.Version),
                    Assemblies = new AssemblyModel[] { assembly }
                };
                bundles.Add(bundle);
            }


            var bundleLookup = bundles.ToDictionary(x => x.PackageFullName);

            foreach (var bundle in bundles) {
                foreach (var assembly in bundle.Assemblies) {
                    bundle.ReferencedBundles = assembly.KnownReferences.Select(x => bundleLookup[$"{GetPackageNameFor(x.Name)}.{GetPackagVersion(x.Version)}"]);
                }
            }

            foreach (var bundleInfo in _Config.Assemblies.Bundles) {
                var matches = bundleInfo.GetMatchingAssemblies(assemblies);
                
                bundles.Add(new BundleModel() {
                    PackageId = bundleInfo.PacakgeId,
                    PackageVersion = new Version(bundleInfo.PackageVersion),
                    ReferencedBundles = matches.Select(x => bundleLookup[$"{GetPackageNameFor(x.Name)}.{GetPackagVersion(x.Version)}"])
                });
            }
            
            return bundles;
        }

        private Version GetPackagVersion(Version version) {
            var versionString = _Config.Package.VersionFormat
                .Replace("{Major}", version.Major.ToString())
                .Replace("{Minor}", version.Minor.ToString())
                .Replace("{Build}", version.Build.ToString())
                .Replace("{Revision}", version.Revision.ToString());

            return new Version(versionString);
        }

        private string GetPackageNameFor(string name) {
            foreach (var substitution in _Config.Assemblies.Substitutions)
                name = name.Replace(substitution.Replace, substitution.With);

            return $"{_Config.Package?.Title?.Prefix}{name}{_Config.Package?.Title?.Suffix}";
        }
    }
}