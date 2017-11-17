//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeGun.Configuration;
using TeGun.Models;
using TeGun.Reflection;

namespace TeGun.Commands {
    public class CreateNuSpecCommand {

        public static Task RunAsync(Config config, ILog log) {
            var command = new CreateNuSpecCommand(config, log);

            return command.RunAsync();
        }

        private readonly Config _Config;
        private readonly ILog _Log;

        public CreateNuSpecCommand(Config config, ILog log) {
            _Config = config ?? throw new ArgumentNullException(nameof(config), "must not be null");
            _Log = log;
        }

        public async Task RunAsync() {

            var folder = new AssemblySource(_Config.Assemblies.Sources, _Config.Assemblies.ExcludePatterns);
            var assemblies = await folder.GetAssembliesAsync();

            var orderedByLeastReferences = assemblies.OrderBy(x => x.KnownReferences.Count());

            Echo(orderedByLeastReferences);


            var bundler = new Bundler(_Config);
            var bundles = bundler.GetBundles(orderedByLeastReferences);

            Echo(bundles);

            _Config.NuSpecDirectory.Create();

            var writer = new NuSpecWriter(_Config);


            foreach (var bundle in bundles) {
                var nuspecPath = Path.Combine(_Config.NuSpecDirectory.FullName, $"{bundle.PackageFullName}.nuspec");
                var nuspecContent = writer.GetNuSpec(bundle);

                _Log.Info($"writing nuspec file '{nuspecPath}'");
                File.WriteAllText(nuspecPath, nuspecContent);
            }
            

           _Log.Info($"created {bundles.Count()} nuspec(s)");
        }

       

        private void Echo(IEnumerable<AssemblyModel> assemblies) {
            foreach (var assembly in assemblies) {
                _Log?.Info(($"{assembly} ({assembly.KnownReferences.Count()} reference(s)"));
                _Log?.Info(($"{assembly} ({assembly.UnKnownReferences.Count()} ignored reference(s)"));

                _Log.Verbose($"++++++++++++++++++++++++++++++++");
                _Log.Verbose($"respecting:");
                foreach (var reference in assembly.KnownReferences) {
                    _Log.Verbose($"  + {reference.Identity}");
                }
                _Log.Verbose($"--------------------------------");
                _Log.Verbose($"ignoring:");
                foreach (var reference in assembly.UnKnownReferences) {
                    _Log.Verbose($"  - {reference.Identity}");
                }

            }
        }

        private void Echo(IEnumerable<BundleModel> bundles) {
            foreach (var bundle in bundles) {
                _Log.Info("=======================================================");
                _Log.Info(bundle.PackageFullName);
                _Log.Info("=======================================================");


                _Log.Verbose("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                _Log.Verbose("included assemblies");
                _Log.Verbose("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                foreach (var assembly in bundle.Assemblies) {
                    _Log.Verbose($"{assembly.File}");
                }

                _Log.Verbose("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                _Log.Verbose("package references");
                _Log.Verbose("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                foreach (var refrencedBundle in bundle.ReferencedBundles) {
                    _Log.Verbose($"{refrencedBundle.PackageFullName}");
                }
            }

        }
    }
}
