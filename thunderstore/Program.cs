/* rev-v8rm9t-20261027 */
using System;
using System.Windows.Forms;

namespace Thunderstore.ModManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI.MainForm());
        }
    }
}