//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace TeGun {
    public interface ILog {

        void Debug(string s, Exception ex = null);
        void Info(string s, Exception ex = null);
        void Warn(string s, Exception ex = null);
        void Error(string s, Exception ex = null);
        void Verbose(string s, Exception ex = null);
    }
}
