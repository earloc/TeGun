//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeGun.Configuration {
    public class PreSuffixConfig {

        [JsonProperty("prefix")]
        public string Prefix { get; set; } = "Prefix--";

        [JsonProperty("suffix")]
        public string Suffix { get; set; } = "--Suffix";
    }
}
