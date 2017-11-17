//Copyright (C) Alexander Clare. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace TeGun.Cli {
    internal class ConsoleLog : ILog {
        public void Debug(string s, Exception ex = null) {
            Write(ConsoleColor.Blue, s, ex);
        }

        public void Error(string s, Exception ex = null) {
            Write(ConsoleColor.Red, s, ex);
        }

        public void Info(string s, Exception ex = null) {
            Write(ConsoleColor.Gray, s, ex);
        }

        public void Verbose(string s, Exception ex = null) {
            Write(ConsoleColor.White, s, ex);
        }

        public void Warn(string s, Exception ex = null) {
            Write(ConsoleColor.Yellow, s, ex);
        }

        private object _SyncRoot = new object();

        private void Write(ConsoleColor color, string s, Exception ex) {
            lock (_SyncRoot) {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(s);

                if (ex != null)
                    Console.WriteLine(ex.ToString());

                Console.ForegroundColor = prevColor;
            }
        }
    }
}
