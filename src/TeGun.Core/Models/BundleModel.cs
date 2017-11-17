//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeGun.Models {
    public class BundleModel {


        public string PackageId { get; internal set; }

        public IEnumerable<AssemblyModel> Assemblies { get; internal set; } = Enumerable.Empty<AssemblyModel>();
        public Version PackageVersion { get; internal set; }
        public IEnumerable<BundleModel> ReferencedBundles { get; internal set; } = Enumerable.Empty<BundleModel>();



        public string PackageFullName => $"{PackageId}.{PackageVersion}";
    }
}
