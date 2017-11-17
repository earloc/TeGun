//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeGun.Extensions;
using TeGun.Models;

namespace TeGun.Reflection {
    public class AssemblySource {

        private readonly IEnumerable<DirectoryInfo> _Sources;
        private readonly IEnumerable<Regex> _ExcludePatterns;

        public AssemblySource(IEnumerable<DirectoryInfo> sources, string[] excludePatterns) {
            _Sources = sources ?? throw new ArgumentNullException(nameof(sources), "must not be null");

            excludePatterns = excludePatterns ?? Enumerable.Empty<string>().ToArray();
            _ExcludePatterns = excludePatterns
                .Select(x => x.AsRegex())
                .ToArray();
        }

        public async Task<IEnumerable<AssemblyModel>> GetAssembliesAsync() {

            var assemblyRegistry = new AssemblyReferenceCollection();
            var reader = new AssemblyReader(assemblyRegistry);

            var files = _Sources
                .SelectMany(x => x.GetFiles("*.dll"))
                .OrderBy(x => x.Name)
                .Where(x => !IsExcluded(x))
                .ToArray()
                .AsEnumerable();

            var assemblyTasks = files
                .Select(x => reader.TryReadAsync(x));

            var assemblies = (await Task.WhenAll(assemblyTasks))
                .Where(x => x != null)
                .ToArray();

            var knownAssemblies = assemblies.ToDictionary(x => x.Identity);

            foreach (var reference in assemblyRegistry) {
                knownAssemblies.TryGetValue(reference.Identity, out AssemblyModel knownAssembly);
                reference.KnownAssembly = knownAssembly;
            }

            return assemblies;

        }

        private bool IsExcluded(FileInfo file) {
            return _ExcludePatterns.Any(x => x.IsMatch(file.Name));
        }
    }
}
