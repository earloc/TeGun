//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeGun.Models {

    public class AssemblyModel : AssemblyBase {

        public string FrameworkMoniker { get; set; }

        public FileInfo File { get; internal set; }

        public IEnumerable<AssemblyReferenceModel> References { get; internal set; } = Enumerable.Empty<AssemblyReferenceModel>();
        public IEnumerable<AssemblyReferenceModel> KnownReferences => References.Where(x => x.IsKnown);
        public IEnumerable<AssemblyReferenceModel> UnKnownReferences => References.Where(x => !x.IsKnown);
    }
}
