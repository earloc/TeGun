//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeGun.Models;

namespace TeGun.Reflection {
    public class AssemblyReferenceCollection : IEnumerable<AssemblyReferenceModel> {

        private ConcurrentDictionary<string, AssemblyReferenceModel> _Register = new ConcurrentDictionary<string, AssemblyReferenceModel>();

        internal AssemblyReferenceModel GetOrCreate(string name, Version version, AssemblyModel referrer) {
            var model = new AssemblyReferenceModel() {
                Name = name,
                Version = version,
            };

            return _Register
                .GetOrAdd(model.Identity, model)
                .IsReferencedBy(referrer);
        }

        public IEnumerator<AssemblyReferenceModel> GetEnumerator() {
            return _Register.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
