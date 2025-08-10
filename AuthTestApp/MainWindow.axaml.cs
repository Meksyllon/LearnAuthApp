using AuthTestApp.DataAccess;
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
        public MainWindow()
        {
            InitializeComponent();
            usersRepository = new UsersRepository(new AuthDBContext());
            userAccount = new User("admin", "admin", UserRoles.Admin);
            Width = userAccount.Role == UserRoles.Admin ? WindowSizes.FullWidth : WindowSizes.StartWidth;
            Height = WindowSizes.StartHeight;
            ShowUserInfo();
        }
        public MainWindow(UsersRepository usersRepository, User userAccount)
        {
            InitializeComponent();
            this.usersRepository = usersRepository;
            this.userAccount = userAccount;
            Width = userAccount.Role == UserRoles.Admin ? WindowSizes.FullWidth : WindowSizes.StartWidth;
            Height = WindowSizes.StartHeight;
            ShowUserInfo();
        }

        private void ShowUserInfo()
        {
            LoggedName.Content = userAccount.Name;
            LoggedRole.Content = userAccount.Role;
            if (userAccount.Role == UserRoles.Admin)
            {
                AdminPanel.IsVisible = true;
                UpdateUsersList();
            }
        }

        private void ButtonAdd_OnClick(object? sender, RoutedEventArgs e)
        {
            AddErrorLabel.Content = null;
            var username = AddNameTB.Text;
            var password = AddPasswordTB.Text;
            var role = RoleComboBox.SelectionBoxItem?.ToString();
            if (role == null) throw new ArgumentException("Role of new user can't be readed");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;
            if (usersRepository.UsernameUsed(username))
            { 
                AddErrorLabel.Content = "User with this name already exists!";
                return;
            }
            usersRepository.Add(username, password, role);
            UpdateUsersList();
            AddNameTB.Clear();
            AddPasswordTB.Clear();
        }

        private void ButtonDelete_OnClick(object? sender, RoutedEventArgs e)
        {
            DeleteErrorLabel.Content = null;
            var username = DeleteTextBox.Text;
            if (string.IsNullOrEmpty(username)) return;

            if (!usersRepository.UsernameUsed(username))
            {
                DeleteErrorLabel.Content = "User with this name is not exists!";
                return;
            }            
            usersRepository.Delete(username);
            UpdateUsersList();
            DeleteTextBox.Clear();                        
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
            UpdateUsersList();

            ChangePassButton.IsVisible = true;
            ChangePassOldTB.IsVisible = false;
            ChangePassNewTB.IsVisible = false;
            ApplyChangePassButton.IsVisible = false;
        }

        private void UpdateUsersList()
        {
            StringBuilder name = new StringBuilder();
            StringBuilder pass = new StringBuilder();
            StringBuilder role = new StringBuilder();

            var users = usersRepository.Get();
            foreach (var user in users)
            {
                name.Append($"{user.Name}\n");
                pass.Append($"{user.Password}\n");
                role.Append($"{user.Role}\n");
            }
            UsersNameLabel.Content = name.ToString();
            UsersPasswordLabel.Content = pass.ToString();
            UsersRoleLabel.Content = role.ToString();
        }
    }
}