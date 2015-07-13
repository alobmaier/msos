﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msos
{
    class ConsolePrinter : PrinterBase
    {
        private uint _rowsPrinted = 0;

        class ConsoleColorChanger : IDisposable
        {
            private ConsoleColor _oldColor;

            public ConsoleColorChanger(ConsoleColor foregroundColor)
            {
                _oldColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
            }

            public void Dispose()
            {
                Console.ForegroundColor = _oldColor;
            }
        }

        public override void WriteInfo(string value)
        {
            using (new ConsoleColorChanger(ConsoleColor.Green))
            {
                Console.WriteLine(value);
            }
        }

        public override void WriteCommandOutput(string value)
        {
            using (new ConsoleColorChanger(ConsoleColor.Gray))
            {
                // If paging is enabled, stop after a certain number of lines
                // and wait for user confirmation before proceeding.
                ++_rowsPrinted;
                if (RowsPerPage != 0 && _rowsPrinted >= RowsPerPage)
                {
                    Console.WriteLine("--- Press any key for more ---");
                    Console.ReadKey(intercept: true);
                    _rowsPrinted = 0;
                }
                Console.WriteLine(value);
            }
        }

        public override void WriteError(string value)
        {
            using (new ConsoleColorChanger(ConsoleColor.Red))
            {
                Console.WriteLine(value);
            }
        }

        public override void WriteWarning(string value)
        {
            using (new ConsoleColorChanger(ConsoleColor.DarkYellow))
            {
                Console.WriteLine(value);
            }
        }

        public override void ClearScreen()
        {
            Console.Clear();
        }

        public override void CommandEnded()
        {
            _rowsPrinted = 0;
        }
    }
}
