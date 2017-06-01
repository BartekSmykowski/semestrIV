using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDbContext dbContext;
        public MainWindow()
        {
            InitializeComponent();

            dbContext = new MyDbContext();
            refreshCarsList();

        }

        private void customSorting(object sender, DataGridSortingEventArgs e)
        {
        }

        private void search(object sender, RoutedEventArgs e)
        {
            actualSearch();
        }

        private void search(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                actualSearch();
        }

        private void actualSearch()
        {
            string searchString = searchedPhrase.Text;
            string selectedColumn = columnComboBox.Text;
            if (selectedColumn.Equals("Year"))
            {
                var carsList = dbContext.Cars
                            .Include("Engine")
                            .Select(x => new {
                                x.Id,
                                x.Model,
                                Engine = x.Engine.Model + " " + x.Engine.Displacement + " (" + x.Engine.HorsePower + ")",
                                x.Year,
                                EngineType = x.Engine.Model.Equals("TDI") ? "diesel" : "petrol"
                            })
                            .ToList()
                            .Where(x => x.Year.ToString().Equals(searchString));
                carsDataGrid.DataContext = carsList;

            }
            else if (selectedColumn.Equals("Model"))
            {
                var carsList = dbContext.Cars
                            .Include("Engine")
                            .Select(x => new {
                                x.Id,
                                x.Model,
                                Engine = x.Engine.Model + " " + x.Engine.Displacement + " (" + x.Engine.HorsePower + ")",
                                x.Year,
                                EngineType = x.Engine.Model.Equals("TDI") ? "diesel" : "petrol"
                            })
                            .ToList()
                            .Where(x => x.Model.Contains(searchString));
                carsDataGrid.DataContext = carsList;
            }
        }

        private void addCarButton_Click(object sender, RoutedEventArgs e)
        {
            AddingCarWindow addingCarWindow = new AddingCarWindow(dbContext, this, -1);
            addingCarWindow.Show();
        }

        public void refreshCarsList()
        {
            var carsList = dbContext.Cars
            .Include("Engine")
            .Select(x => new {
                x.Id,
                x.Model,
                Engine = x.Engine.Model + " " + x.Engine.Displacement + " (" + x.Engine.HorsePower + ")",
                x.Year,
                EngineType = x.Engine.Model.Equals("TDI") ? "diesel" : "petrol"
            })
            .ToList();
            carsDataGrid.DataContext = carsList;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Type type = row.Item.GetType();
            PropertyInfo propertyInfo = type.GetProperty("Id");

            int carId = (int)propertyInfo.GetValue(row.Item);

            var dr = row.Item;

            AddingCarWindow addingCarWindow = new AddingCarWindow(dbContext, this, carId);
            addingCarWindow.Show();
        }

    }
}
