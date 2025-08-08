using System;
using System.Windows;
using NullAI.Services;

namespace NullAI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Optionally hook global exception handling
            this.DispatcherUnhandledException += (sender, args) =>
            {
                ErrorLogger.Log(args.Exception);
                MessageBox.Show("An unexpected error occurred: " + args.Exception.Message);
                args.Handled = true;
            };
        }
    }
}