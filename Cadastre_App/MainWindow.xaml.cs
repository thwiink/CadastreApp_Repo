using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cadastre_App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            SetAccessRights();
            if (SessionManager.IsAuthenticated && SessionManager.CurrentUserRole == "Admin")
            {
                BtnUsers.Visibility = Visibility.Visible;
            }
            else
            {
                BtnUsers.Visibility = Visibility.Collapsed;
            }
            MainFrame.Navigate(new EditPage());
            
        }

        private void SetAccessRights()
        {
            if (SessionManager.CurrentUserRole != "Admin")
            {
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnList.Visibility = Visibility.Collapsed;
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new FullEditPage());
        }

        private void BtnList_Click(object sender, RoutedEventArgs e)
        {
            var currentPage = Manager.MainFrame.Content;

            if (!(currentPage is EditPage))
            {
                while (Manager.MainFrame.CanGoBack)
                {
                    Manager.MainFrame.RemoveBackEntry();
                }
                Manager.MainFrame.Navigate(new EditPage());
            }

        }
        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AboutPage());
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HelpPage());
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            // Проверка прав (дополнительно)
            if (SessionManager.CurrentUserRole == "Admin")
                MainFrame.Navigate(new UserManagementPage());
            else
                MessageBox.Show("Доступ запрещён. Только для администратора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}
