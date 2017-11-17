//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;

namespace TeGun.Models {
    public abstract class AssemblyBase {

        public string Identity => $"{Name} {Version}";

        public string Name { get; internal set; }

        public Version Version { get; set; }

        public override string ToString() => $"{Name}, {Version}";
    }
}
