using AuthTestApp.DataAccess;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using AuthTestApp.DataAccess.Reposotories;

namespace AuthTestApp;

public partial class AdminPanelWindow : Window
{
    private readonly UsersRepository usersRepository;
    private readonly TasksRepository tasksRepository;
    public AdminPanelWindow()
    {
        InitializeComponent();
        var dbContext = new AuthDBContext();
        usersRepository = new UsersRepository(dbContext);
        tasksRepository = new TasksRepository(dbContext);
        Width = WindowSizes.AdminPanelWidth;
        Height = WindowSizes.AdminPanelHeight;
        UpdateUsersList();
    }
    public AdminPanelWindow(UsersRepository usersRepository, TasksRepository tasksRepository)
    {
        InitializeComponent();
        this.usersRepository = usersRepository;
        this.tasksRepository = tasksRepository;
        Width = WindowSizes.AdminPanelWidth;
        Height = WindowSizes.AdminPanelHeight;
        UpdateUsersList();
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
            DeleteErrorLabel.Content = "User with this name does not exists!";
            return;
        }
        usersRepository.Delete(username);
        UpdateUsersList();
        DeleteTextBox.Clear();
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