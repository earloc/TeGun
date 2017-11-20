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

            _Log.Info("(+) => included references");
            _Log.Info("(-) => ignored references");
            _Log.Info("(~) => identified package rerferences");

            _Log.Info("");

            _Log.Info($"analyzing assemblies in");

            foreach (var path in _Config.Assemblies.SourcePaths)
                _Log.Info($"  {path}");

            

            var folder = new AssemblySource(_Config.Assemblies.Sources, _Config.Assemblies.ExcludePatterns);
            var assemblies = await folder.GetAssembliesAsync();

            var orderedByLeastReferences = assemblies.OrderBy(x => x.KnownReferences.Count());

            _Log.Info("");
            Echo(orderedByLeastReferences);

            _Log.Info("");
            _Log.Info("deriving bundles / pacakges");
            _Log.Info("");

            var bundler = new Bundler(_Config);
            var bundles = bundler.GetBundles(orderedByLeastReferences);

            Echo(bundles);

            _Config.NuSpecDirectory.Create();

            var writer = new NuSpecWriter(_Config);

            _Log.Info($"writing nuspec files to : {_Config.NuSpecDirectory}");

            foreach (var bundle in bundles) {
                var target = new FileInfo(Path.Combine(_Config.NuSpecDirectory.FullName, $"{bundle.PackageFullName}.nuspec"));
                var nuspecContent = writer.GetNuSpec(bundle);

                _Log.Info($"  {target.Name}");
                File.WriteAllText(target.FullName, nuspecContent);
            }

           _Log.Info($"created {bundles.Count()} nuspec(s)");
        }

       

        private void Echo(IEnumerable<AssemblyModel> assemblies) {
            foreach (var assembly in assemblies) {
                _Log?.Info(($"{assembly.File.Name}"));


                foreach (var reference in assembly.KnownReferences) {
                    _Log.Verbose($"  (+) {reference.Identity}");
                }
                foreach (var reference in assembly.UnKnownReferences) {
                    _Log.Verbose($"  (-) {reference.Identity}");
                }

                _Log?.Info("");
            }

        }

        private void Echo(IEnumerable<BundleModel> bundles) {
            foreach (var bundle in bundles) {
                _Log.Info(bundle.PackageFullName);

                foreach (var assembly in bundle.Assemblies) {
                    _Log.Verbose($"  (+) {assembly.File}");
                }

                foreach (var refrencedBundle in bundle.ReferencedBundles) {
                    _Log.Verbose($"  (~) {refrencedBundle.PackageFullName}");
                }

                _Log?.Info("");
            }
            

        }
    }
}
