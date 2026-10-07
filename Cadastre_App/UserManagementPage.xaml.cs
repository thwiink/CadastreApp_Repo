using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cadastre_App
{
    public partial class UserManagementPage : Page
    {
        public UserManagementPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var context = CadastreBDEntities1.GetContext();
            DGridUsers.ItemsSource = context.Users.ToList();
        }

        private void RefreshData()
        {
            var context = CadastreBDEntities1.GetContext();
            foreach (var entry in context.ChangeTracker.Entries())
                entry.Reload();
            LoadUsers();
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Простой диалог для добавления пользователя (можно сделать отдельную страницу, но для упрощения - через InputBox)
            // В реальном проекте лучше использовать отдельное окно. Здесь для демонстрации.
            string login = Microsoft.VisualBasic.Interaction.InputBox("Введите логин:", "Добавление пользователя", "");
            if (string.IsNullOrWhiteSpace(login)) return;

            string password = Microsoft.VisualBasic.Interaction.InputBox("Введите пароль:", "Добавление пользователя", "");
            if (string.IsNullOrWhiteSpace(password)) return;

            string role = Microsoft.VisualBasic.Interaction.InputBox("Введите роль (Admin/User):", "Добавление пользователя", "User");
            if (role != "Admin" && role != "User") role = "User";

            try
            {
                var context = CadastreBDEntities1.GetContext();
                var newUser = new Users
                {
                    Login = login,
                    Password = password,
                    Role = role
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                RefreshData();
                MessageBox.Show("Пользователь добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridUsers.SelectedItem as Users;
            if (selected == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selected.Login == "admin")
            {
                MessageBox.Show("Нельзя удалить администратора по умолчанию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить пользователя '{selected.Login}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = CadastreBDEntities1.GetContext();
                    context.Users.Remove(selected);
                    context.SaveChanges();
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}