//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TeGun.Extensions {
    internal static class StringExtensions {
        public static Regex AsRegex(this string that) {
            return new Regex($@"^{Regex.Escape(that).Replace(@"\*", ".*").Replace(@"\?", ".")}$");
        }

    }
}
