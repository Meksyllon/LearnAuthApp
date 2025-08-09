using AuthTestApp.DataAccess;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        const int START_WIDTH = 230;
        const int START_HEIGTH = 400;
        const int FULL_WIDTH = 900;
        const string DEFAULT_ROLE = "Employee";

        private AuthDBContext _dbContext;
        private UsersRepository _usersRepository;
        public MainWindow()
        {
            InitializeComponent();
            _dbContext = new AuthDBContext();
            _usersRepository = new UsersRepository(_dbContext);
            Width = START_WIDTH;
            Height = START_HEIGTH;
            UpdateUsersList();
        }

        private void ButtonAdd_OnClick(object? sender, RoutedEventArgs e)
        {
            AddErrorLabel.Content = null;
            var username = AddNameTB.Text;
            var password = AddPasswordTB.Text;
            var role = RoleComboBox.SelectionBoxItem?.ToString();
            if (username == null || username == string.Empty || password == null || password == string.Empty) return;

            if (_usersRepository.UsernameUsed(username))
                AddErrorLabel.Content = "User with this name already exists!";
            else
            {
                if (role == null) throw new ArgumentException();
                _usersRepository.Add(username, password, role);
                UpdateUsersList();
                AddNameTB.Clear();
                AddPasswordTB.Clear();
            }
        }

        private void ButtonDelete_OnClick(object? sender, RoutedEventArgs e)
        {
            DeleteErrorLabel.Content = null;
            var username = DeleteTextBox.Text;
            if (username == null || username == string.Empty) return;

            if (!_usersRepository.UsernameUsed(username))
                DeleteErrorLabel.Content = "User with this name is not exists!";
            else
            {
                _usersRepository.Delete(username);
                UpdateUsersList();
                DeleteTextBox.Clear();
            }            
        }
        private void SignUpButton_OnClick(object? sender, RoutedEventArgs e)
        {
            SignUpErrorLabel.Content = null;

            var username = SignUpUsernameTB.Text;
            var password = SignUpPasswordTB.Text;
            if (username == null || username == string.Empty || password == null || password == string.Empty) return;

            if (_usersRepository.UsernameUsed(username))
                SignUpErrorLabel.Content = "This username already taken!";
            else
            {
                _usersRepository.Add(username, password, DEFAULT_ROLE);
                UpdateUsersList();
                AddNameTB.Clear();
                AddPasswordTB.Clear();
                var user = _usersRepository.Get().FirstOrDefault(user => user.Name == username && user.Password == password);
                LogIn(user);
                SignUpUsernameTB.Clear();
                SignUpPasswordTB.Clear();
            }
        }

        private void LogInButton_OnClick(object? sender, RoutedEventArgs e)
        {
            SignInErrorLabel.Content = null;

            var username = SignInUsernameTB.Text;
            var password = SignInPasswordTB.Text;
            if (username == null || username == string.Empty || password == null || password == string.Empty) return;
            var user = _usersRepository.Get().FirstOrDefault(user => user.Name == username && user.Password == password);
            if (user == null)
                SignInErrorLabel.Content = "Incorrect username or password!";
            else
            {
                LogIn(user);
                SignInUsernameTB.Clear();
                SignInPasswordTB.Clear();
            }
        }

        private void LogIn(User user)
        {
            if (user.Role == "Admin")
            {
                Width = FULL_WIDTH;
                AdminPanel.IsVisible = true;
            }
            LoggedName.Content = user.Name;
            LoggedRole.Content = user.Role;
            SignUpInPanel.IsVisible = false;
            LoggedInfo.IsVisible = true;
        }

        private void LogOutButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (LoggedRole?.Content?.ToString() == "Admin")
            {
                Width = START_WIDTH;
                AdminPanel.IsVisible = false;
            }
            SignUpInPanel.IsVisible = true;
            LoggedInfo.IsVisible = false;
            LoggedName.Content = null;
            LoggedRole.Content = null;
            
        }

        private void ShangePassButton_OnClick(object? sender, RoutedEventArgs e)
        {            
            ShangePassNewTB.IsVisible = true;
            ShangePassOldTB.IsVisible = true;
        }

        private void UpdateUsersList()
        {
            StringBuilder name = new StringBuilder();
            StringBuilder pass = new StringBuilder();
            StringBuilder role = new StringBuilder();

            var users = _usersRepository.Get();
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