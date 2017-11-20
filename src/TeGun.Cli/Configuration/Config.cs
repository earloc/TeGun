//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeGun.Configuration {
    public class Config {

        public static FileInfo FileFor(string name) => new FileInfo($"{name}.config.json");

        private string _Name;
        public Config(string name) : this() {
            _Name = name ?? throw new ArgumentNullException(nameof(name), "must not be null");
            _Name = _Name.Replace(".config.json", "");
            _File = FileFor(name);
        }

        public Config() {
            _NuspecDirectory = new Lazy<DirectoryInfo>(() => new DirectoryInfo(Path.Combine(_Name, "nuspecs")));
        }

        public static Config From(string name) {
            name = name.Replace(".config.json", "");

            var file = FileFor(name);
            var content = File.ReadAllText(file.FullName);

            var config = JsonConvert.DeserializeObject<Config>(content);
            config._File = file;
            config._Name = name;

            return config;
        }

        private FileInfo _File;

        [JsonProperty("assemblies")]
        public AssemblyConfig Assemblies { get; set; } = new AssemblyConfig();

        public string Save() {
            var content = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(_File.FullName, content);

            return content;
        }

        [JsonProperty("package")]
        public PackageConfig Package { get; set; } = new PackageConfig();

        private readonly Lazy<DirectoryInfo> _NuspecDirectory;

        [JsonIgnore]
        public DirectoryInfo NuSpecDirectory => _NuspecDirectory.Value;
    }
}
