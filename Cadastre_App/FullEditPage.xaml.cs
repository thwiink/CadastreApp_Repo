using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Cadastre_App
{
    public partial class FullEditPage : Page
    {
        private RealEstateObject _currentObject = new RealEstateObject();

        // Конструктор для добавления
        public FullEditPage()
        {
            InitializeComponent();
            DataContext = _currentObject;
            LoadComboBoxes();
        }

        // Конструктор для редактирования
        public FullEditPage(RealEstateObject selectedObject)
        {
            InitializeComponent();
            _currentObject = selectedObject;
            DataContext = _currentObject;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            var context = CadastreBDEntities1.GetContext();
            ComboTypes.ItemsSource = context.ObjectTypes.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Валидация (как в лекции)
            if (string.IsNullOrWhiteSpace(_currentObject.CadastralNumber))
                errors.AppendLine("Укажите кадастровый номер");
            if (string.IsNullOrWhiteSpace(_currentObject.Address))
                errors.AppendLine("Укажите адрес");
            if (_currentObject.Area <= 0)
                errors.AppendLine("Площадь должна быть больше 0");
            if (_currentObject.CadastralValue <= 0)
                errors.AppendLine("Стоимость должна быть больше 0");
            if (_currentObject.TypeID == 0)
                errors.AppendLine("Выберите тип объекта");
            if (_currentObject.OwnersID == 0)
                errors.AppendLine("Выберите владельца");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var context = CadastreBDEntities1.GetContext();

            if (_currentObject.ID == 0)
                context.RealEstateObject.Add(_currentObject);
            else
                context.Entry(_currentObject).State = System.Data.Entity.EntityState.Modified;

            try
            {
                context.SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}