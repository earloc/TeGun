//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeGun.Configuration;

namespace TeGun.Cli.Verbs {
    [Verb("nuspec")]
    internal class NuSpecVerb {

        [Value(0, Default = "default", HelpText = "Name of config to use")]
        public string ConfigName { get; set; }


        public NuSpecVerb() {
            _Config = new Lazy<Config>(() => Config.From(ConfigName));
            
        }

        private readonly Lazy<Config> _Config;
        public Config Configuration => _Config.Value;


        
    }
}
