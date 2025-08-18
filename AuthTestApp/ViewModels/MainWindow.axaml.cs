using AuthTestApp.DataAccess;
using AuthTestApp.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AuthTestApp
{

    public partial class MainWindow : Window
    {
        private readonly UsersRepository usersRepository;
        private User userAccount;
        public MainWindow() : this(new UsersRepository(new AuthDBContext()), new User("admin", "admin", UserRoles.Admin))
        { }

        public MainWindow(UsersRepository usersRepository, User userAccount)
        {
            this.usersRepository = usersRepository;
            this.userAccount = userAccount;
            InitializeComponent();
            Width = WindowSizes.MainMenuWidth;
            Height = WindowSizes.MainMenuHeight;
            LoggedName.Content = userAccount?.Name;
            LoggedRole.Content = userAccount?.Role;
        }

        private void LogOutButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var entryWindow = new EntryWindow(usersRepository);
                entryWindow.Show();
                this.Close();
            } 
        }

        private void ChangePassButton_OnClick(object? sender, RoutedEventArgs e)
        {      
            ChangePassButton.IsVisible = false;
            ChangePassOldTB.IsVisible = true;
            ChangePassNewTB.IsVisible = true;
            ApplyChangePassButton.IsVisible = true;
        }

        private void ApplyChangePassButton_OnClick(object? sender, RoutedEventArgs e)
        {
            ChangePassErrorLabel.Content = null;
            var oldPass = ChangePassOldTB.Text;
            var newPass = ChangePassNewTB.Text;
            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass))
                return;
            if (oldPass == newPass)
            {
                ChangePassErrorLabel.Content = "New password may not match the old one!";
                return;
            }
            if (userAccount.Password != oldPass)
            {
                ChangePassErrorLabel.Content = "Incorrect password!";
                return;
            }
            userAccount.Password = newPass;
            usersRepository.Update(userAccount.Name, newPass, userAccount.Role);
            ChangePassErrorLabel.Content = "Password has been changed!";
            ChangePassOldTB.Clear();
            ChangePassNewTB.Clear();

            ChangePassButton.IsVisible = true;
            ChangePassOldTB.IsVisible = false;
            ChangePassNewTB.IsVisible = false;
            ApplyChangePassButton.IsVisible = false;
        }

        private void OpenAdminPanelButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var adminPanelWindow = desktop.Windows.FirstOrDefault(w => w.Name == "AdminPanelWind");
                if (adminPanelWindow == null)
                    adminPanelWindow = new AdminPanelWindow(usersRepository);
                adminPanelWindow.Show();                
            }
        }
    }
}