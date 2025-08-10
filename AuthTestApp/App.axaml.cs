using System;
using System.IO;
using AuthTestApp.DataAccess;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AuthTestApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var dbContext = new AuthDBContext();
                var usersRepository = new UsersRepository(dbContext);
                desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
                var entryWindow = new EntryWindow(usersRepository);
                entryWindow.Show();
            }
        }
    }
}