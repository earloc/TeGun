//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using TeGun.Models;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Linq;

namespace TeGun.Reflection {
    internal class AssemblyReader {

        private readonly AssemblyReferenceCollection _Register;

        public AssemblyReader(AssemblyReferenceCollection register) {
            _Register = register ?? throw new ArgumentNullException(nameof(register), "must not be null");
        }

        internal Task<AssemblyModel> TryReadAsync(FileInfo file) {
            return Task.Run(() => {
                using (var stream = file.OpenRead()) {
                    using (var peReader = new PEReader(stream)) {
                        try {
                            var metadataReader = peReader.GetMetadataReader();
                            var definition = metadataReader.GetAssemblyDefinition();

                            var model = new AssemblyModel() {
                                File = file,
                                Name = metadataReader.GetString(definition.Name),
                                Version = definition.Version,
                            };

                            var references = metadataReader.AssemblyReferences.Select(x => {
                                var asm = metadataReader.GetAssemblyReference(x);
                                return _Register.GetOrCreate(
                                    metadataReader.GetString(asm.Name),
                                    asm.Version,
                                    model
                                );
                            }).ToArray();

                            model.References = references;
                            return model;

                        }
                        catch (Exception) {
                            return null;
                        }
                    }
                }
            });

        }
    }
}