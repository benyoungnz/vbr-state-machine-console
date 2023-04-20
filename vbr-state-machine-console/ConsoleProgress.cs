using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veeam_repository_reporter
{
    static class ConsoleProgress
    {
        const char _block = '■';
        const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
        const string _twirl = "-\\|/";
        public static void WriteProgressBar(int percent, bool update = false)
        {
            var color = ConsoleColor.Green;
            if (percent >= 80)
                color = ConsoleColor.Yellow;

            if (percent >= 90)
                color = ConsoleColor.Red;

            if (update)
                    Console.Write(_back);

            Console.Write("[");
            var p = (int)((percent / 10f) + .5f);
            for (var i = 0; i < 10; ++i)
            {
                if (i >= p)
                    Console.Write(' ');
                else
                    ColorConsole.Write(_block.ToString(), color);
            }
            Console.Write("] {0,3:##0}%", percent);
        }
        public static void WriteProgress(int progress, bool update = false)
        {
            if (update)
                Console.Write("\b");
            Console.Write(_twirl[progress % _twirl.Length]);
        }
    }
}
