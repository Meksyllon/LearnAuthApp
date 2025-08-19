using System;
using System.IO;
using AuthTestApp.DataAccess;
using AuthTestApp.DataAccess.Reposotories;
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
                var tasksRepository = new TasksRepository(dbContext);
                var entryWindow = new EntryWindow(usersRepository, tasksRepository);
                desktop.MainWindow = entryWindow;
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                entryWindow.Show();
                //desktop.MainWindow = new MainWindow();
            }
        }
    }
}