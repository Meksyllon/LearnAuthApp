using AuthTestApp.DataAccess;
using AuthTestApp.DataAccess.Reposotories;
using AuthTestApp.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AuthTestApp
{

    public partial class MainWindow : Window
    {
        private readonly UsersRepository usersRepository;
        private readonly TasksRepository tasksRepository;
        private User userAccount;
        public MainWindow() 
        {
            InitializeComponent();
            var dbContext = new AuthDBContext();
            usersRepository = new UsersRepository(dbContext);
            tasksRepository = new TasksRepository(dbContext);
            Width = WindowSizes.MainMenuWidth;
            Height = WindowSizes.MainMenuHeight;
            LoggedName.Content = "admin";
            LoggedRole.Content = "admin";
            userAccount = usersRepository.GetByUsername("admin");
            OpenAdminPanelButton.IsVisible = true;
        }

        public MainWindow(UsersRepository usersRepository, TasksRepository tasksRepository, User userAccount)
        {
            InitializeComponent();
            this.usersRepository = usersRepository;
            this.tasksRepository = tasksRepository;
            this.userAccount = userAccount;
            Width = WindowSizes.MainMenuWidth;
            Height = WindowSizes.MainMenuHeight;
            LoggedName.Content = userAccount.Name;
            LoggedRole.Content = userAccount.Role;
            OpenAdminPanelButton.IsVisible = this.userAccount.Role == UserRoles.Admin;
        }

        private void LogOutButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var entryWindow = new EntryWindow(usersRepository, tasksRepository);
                desktop.MainWindow = entryWindow;
                desktop.MainWindow.Show();
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
            if (userAccount.Password != oldPass)
            {
                ChangePassErrorLabel.Content = "Incorrect password!";
                return;
            }
            if (oldPass == newPass)
            {
                ChangePassErrorLabel.Content = "New password may not match the\nold one!";
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
                    adminPanelWindow = new AdminPanelWindow(usersRepository, tasksRepository);
                adminPanelWindow.Show();
            }
        }
        private void AddToDoButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ToDoTB.Text)) return;
            tasksRepository.Add(ToDoTB.Text, userAccount.Id);
            ToDoTB.Text = string.Empty;
        }
    }
}