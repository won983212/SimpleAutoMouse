using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMouse
{
    public class Logger
    {
        public static void Info(object o)
        {
            Print("[Info] " + o);
        }

        public static void Warn(object o)
        {
            Print("[Warn] " + o);
        }

        public static void Error(object o)
        {
            Print("[Error] " + o);
        }

        private static void Print(string str)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + str);
        }
    }
}
