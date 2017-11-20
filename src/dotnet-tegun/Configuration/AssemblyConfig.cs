//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeGun.Configuration {
    public class AssemblyConfig {

        [JsonProperty("sources")]
        public string[] SourcePaths { get; set; } = new string[] { "Path/To/Assemblies", "Another/Path" };

        public AssemblyConfig() {
            _Sources = new Lazy<IEnumerable<DirectoryInfo>>(() => SourcePaths.Select(x => new DirectoryInfo(x)).ToArray());
        }

        private readonly Lazy<IEnumerable<DirectoryInfo>> _Sources;
        [JsonIgnore]
        public IEnumerable<DirectoryInfo> Sources => _Sources.Value;

        public string[] ExcludePatterns { get; set; } = new string[] { "System.*", "Microsoft.*" };

        [JsonProperty("bundles")]
        public BundleConfig[] Bundles { get; set; } = new BundleConfig[] {
            new BundleConfig() {
                PacakgeId = "Custom.Bundle.PackageId",
                PackageVersion = "1.2.3.4",
                SearchPattern = "Custom.*.Bundle"
            }
        };

        [JsonProperty("substitutions")]
        public SubstitutionConfig[] Substitutions { get; set; } = new SubstitutionConfig[] {
            new SubstitutionConfig() {
                Replace = "SearchString",
                With = "Replacement"
            }
        };
    }
}
