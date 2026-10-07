using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cadastre_App
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text; // предполагается, что есть TextBox с именем txtLogin
            string password = txtPassword.Password; // PasswordBox, используем свойство Password

            var context = CadastreBDEntities1.GetContext();
            var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                SessionManager.Login(user.ID, user.Role);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}