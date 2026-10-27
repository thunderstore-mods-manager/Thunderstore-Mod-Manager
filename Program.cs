// thunderstore mod manager — main entry point
// thunderstore without overwolf, standalone build
using System;
using System.Windows;

namespace Thunderstore
{
    public class App : Application
    {
        [STAThread]
        static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run(new MainWindow());
        }
    }
}
