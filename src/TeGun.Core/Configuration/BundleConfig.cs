//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TeGun.Extensions;
using TeGun.Models;

namespace TeGun.Configuration {
    public class BundleConfig {


        [JsonProperty("searchPattern")]
        public string SearchPattern { get; set; }

        [JsonProperty("packageId")]
        public string PacakgeId { get; set; }

        [JsonProperty("packageVersion")]
        public string PackageVersion { get; set; }

        internal IEnumerable<AssemblyModel> GetMatchingAssemblies(IEnumerable<AssemblyModel> assemblies) {


            var regex = SearchPattern.AsRegex(); ;

            var matches = assemblies
                .Where(x => regex.IsMatch(x.Name))
                .ToArray();

            return matches;
        }
    }
}