using System;
using System.Windows.Forms;

namespace MarkdownPreviewer
{
    internal static class Program
    {
        public static string[] Args;

        [STAThread]
        private static void Main(string[] args)
        {
            Args = args;

            Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false); Application.Run(new MainWindow());
        }
    }
}