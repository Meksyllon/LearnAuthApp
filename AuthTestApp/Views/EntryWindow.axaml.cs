using System;
using System.IO;
using System.Linq;
using AuthTestApp.DataAccess;
using AuthTestApp.DataAccess.Reposotories;
using AuthTestApp.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;

namespace AuthTestApp;

public partial class EntryWindow : Window
{
    private readonly UsersRepository usersRepository;
    private readonly TasksRepository tasksRepository;

    public EntryWindow()
    {
        InitializeComponent();
        var dbContext = new AuthDBContext();
        usersRepository = new UsersRepository(dbContext);
        tasksRepository = new TasksRepository(dbContext);
        Width = WindowSizes.StartWidth;
        Height = WindowSizes.StartHeight;
    }
    public EntryWindow(UsersRepository usersRepository, TasksRepository tasksRepository)
    {
        InitializeComponent();
        this.usersRepository = usersRepository;
        this.tasksRepository = tasksRepository;
        Width = WindowSizes.StartWidth;
        Height = WindowSizes.StartHeight;
        var a = tasksRepository.Get();
        var b = usersRepository.Get();
    }

    private void SignUpButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SignUpErrorLabel.Content = null;

        var username = SignUpUsernameTB.Text;
        var password = SignUpPasswordTB.Text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

        if (usersRepository.UsernameUsed(username))
        {
            SignUpErrorLabel.Content = "This username already taken!";
            return;
        }
        usersRepository.Add(username, password, UserRoles.Employee);
        var user = usersRepository
            .Get()
            .FirstOrDefault(user => user.Name == username && user.Password == password);
        if (user == null) throw new Exception("Unhandled exception");
        OpenMainWindow(user);   
    }

    private void LogInButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SignInErrorLabel.Content = null;

        var username = SignInUsernameTB.Text;
        var password = SignInPasswordTB.Text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;
        var user = usersRepository.Get().FirstOrDefault(user => user.Name == username && user.Password == password);
        if (user == null)
        {
            SignInErrorLabel.Content = "Incorrect username or password!";
            return;
        }
        OpenMainWindow(user);
    }

    private void OpenMainWindow(User user)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(usersRepository, tasksRepository, user);
            desktop.MainWindow.Show();
            this.Close();
        }
    }
}