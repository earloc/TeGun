//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TeGun.Models {
    public class AssemblyReferenceModel : AssemblyBase {

        private readonly BlockingCollection<AssemblyModel> _ReferencedBy = new BlockingCollection<AssemblyModel>();
        public IEnumerable<AssemblyModel> ReferencedBy => _ReferencedBy;

        public bool IsKnown => KnownAssembly != null;

        public AssemblyModel KnownAssembly { get; internal set; }

        public AssemblyReferenceModel IsReferencedBy(AssemblyModel referer) {
            _ReferencedBy.Add(referer);
            return this;
        }

        public override string ToString() => $"{base.ToString()}, ({ReferencedBy.Count()} reference(s))";

    }
}
