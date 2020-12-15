using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkerDiary
{
    static class Program
    {
        public static string FilePath = $@"{Environment.CurrentDirectory}\Employee.txt";
        public static List<string> Shift = new List<string>() { "D1", "D2", "N1", "N2", };
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
