
//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeGun.Configuration {
    public class PackageConfig {
        [JsonProperty("authors")]
        public string Authors { get; set; } = Environment.UserName;

        [JsonProperty("owners")]
        public string Owners { get; set; } = Environment.UserName;

        [JsonProperty("tags")]
        public string[] Tags { get; set; } = new string[] { "tags", "go", "here" };

        [JsonProperty("targetframeworks")]
        public string[] TargetFrameworks { get; set; } = new string[] { "net45" };

        [JsonProperty("versionFormat")]
        public string VersionFormat { get; set; } = "{Major}.{Minor}.{Build}.{Revision}";

        [JsonProperty("title")]
        public PreSuffixConfig Title { get; set; } = new PreSuffixConfig();

        [JsonProperty("nuspecTemplate")]
        public string NuSpecTemplate { get; set; }
    }
}
