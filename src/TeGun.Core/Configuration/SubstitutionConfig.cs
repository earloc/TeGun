//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;

namespace TeGun.Configuration {
    public class SubstitutionConfig {
        [JsonProperty("replace")]
        public string Replace { get; set; }

        [JsonProperty("with")]
        public string With { get; set; }
    }
}