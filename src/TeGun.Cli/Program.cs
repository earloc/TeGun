//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using CommandLine;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeGun.Commands;
using TeGun.Configuration;
using TeGun.Cli.Verbs;
using TeGun.Reflection;

namespace TeGun.Cli {
    class Program {
        public static async Task Main(string[] args) {
            var parser = Parser.Default;

            var task = Task.Delay(50);
            var log = new ConsoleLog();

            parser.ParseArguments<InitVerb, NuSpecVerb>(args)
                .WithParsed<InitVerb>(x => task = InitAsync(x, log))
                .WithParsed<NuSpecVerb>(x => task = CreateNuSpecCommand.RunAsync(x.Configuration, log));

            try {
                await task;
            }
            catch (Exception ex) {
                log.Error(ex.Message, ex);
            }

            if (Debugger.IsAttached)
                Console.ReadKey();
        }


        private static async Task InitAsync(InitVerb verb, ILog log) {

            await Task.Yield();

            if (!verb.ConfigFile.Exists || verb.Force) {
                var config = new Config(verb.ConfigName);
                config.Assemblies.SourcePaths = new string[] { verb.AssemblyPath };

                var content = config.Save();

                log.Info(content);


                Console.WriteLine($"Config created at '{verb.ConfigFile.FullName}'");
                return;
            }

            Console.WriteLine($"Config already exists at {verb.ConfigFile.FullName}. Use the --force switch to overwrite it with default values.");
        }

      
    }
}
