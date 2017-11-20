//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeGun.Configuration;

namespace TeGun.Cli.Verbs {
    [Verb("init")]
    internal class InitVerb {

        [Value(0, Default=".", HelpText = "path to assemblies")]
        public string AssemblyPath { get; set; }

        [Value(1, Default = "default", HelpText = "name of config to initialize")]
        public string ConfigName { get; set; }

        [Option(Default = false, HelpText = "force initialization even if file exists")]
        public bool Force { get; set; }


        private readonly Lazy<FileInfo> _ConfigFile;

        public InitVerb() {
            _ConfigFile = new Lazy<FileInfo>(() => Config.FileFor(ConfigName));
        }

        [JsonIgnore]
        public FileInfo ConfigFile => _ConfigFile.Value;

    }
}
