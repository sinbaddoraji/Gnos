using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace Gnos
{
    internal class App
    {
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(((ICollection)args).Count == 0 ? new Entry("") : new Entry(File.ReadAllText(args[0])));
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Console.ReadKey();
        }
    }
}
