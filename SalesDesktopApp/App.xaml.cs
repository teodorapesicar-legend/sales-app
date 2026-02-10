using Microsoft.EntityFrameworkCore;
using SalesDesktopApp.Data;
using System;
using System.Windows;

namespace SalesDesktopApp
{
    // Application class for the WPF application, handling startup and global exception handling
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Catch all unhandled exceptions to prevent crashes and show error messages
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            try
            {
                
                using (var db = new AppDbContext())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup Error: {ex.Message}\n\n{ex.InnerException?.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Critical Error:\n{e.Exception.Message}\n\nInner:\n{e.Exception.InnerException?.Message}\n\nStack:\n{e.Exception.StackTrace}",
                "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true; // Mark exception as handled to prevent app crash
        }
    }
}