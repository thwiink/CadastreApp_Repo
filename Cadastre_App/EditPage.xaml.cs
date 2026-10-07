using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cadastre_App
{
    public partial class EditPage : Page
    {
        public EditPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void CbTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter(); // вызываем метод фильтрации
        }
        private void LoadData()
        {
            var context = CadastreBDEntities1.GetContext();
            DGridEdit.ItemsSource = context.RealEstateObject
                .Include("ObjectTypes")
                .Include("Owners")
                .ToList();
        }

        private void RefreshData()
        {
            var context = CadastreBDEntities1.GetContext();
            foreach (var entry in context.ChangeTracker.Entries())
                entry.Reload();
            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var context = CadastreBDEntities1.GetContext();
            var obj = context.RealEstateObject.Find(id);
            Manager.MainFrame.Navigate(new FullEditPage(obj));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var context = CadastreBDEntities1.GetContext();
            var obj = context.RealEstateObject.Find(id);
            if (obj == null) return;

            if (MessageBox.Show($"Удалить объект {obj.CadastralNumber}?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    context.RealEstateObject.Remove(obj);
                    context.SaveChanges();
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            var context = CadastreBDEntities1.GetContext();
            var query = context.RealEstateObject
                               .Include("ObjectTypes")
                               .Include("Owners")
                               .AsQueryable();

            // Фильтрация по типу объекта
            if (cbTypeFilter.SelectedItem != null && ((ObjectTypes)cbTypeFilter.SelectedItem).ID != 0)
            {
                int typeId = ((ObjectTypes)cbTypeFilter.SelectedItem).ID;
                query = query.Where(o => o.TypeID == typeId);
            }

            // Поиск по кад. номеруу
            if (!string.IsNullOrWhiteSpace(txtSearchCadastral.Text))
            {
                string searchText = txtSearchCadastral.Text;
                query = query.Where(o => o.CadastralNumber.Contains(searchText));
            }

            DGridEdit.ItemsSource = query.ToList();
        }

        private void BtnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            cbTypeFilter.SelectedIndex = 0;
            // Очищаем поле поиска по кад номеру
            txtSearchCadastral.Text = "";
            //просто обновляем без условмй таблицу
            ApplyFilter();
        }
        
    }
}