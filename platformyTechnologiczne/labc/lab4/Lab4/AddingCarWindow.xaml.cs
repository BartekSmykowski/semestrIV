using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab2
{
    public partial class AddingCarWindow : Window
    {
        MyDbContext dbContext;
        MainWindow mainWindow;
        int carId;

        public AddingCarWindow(MyDbContext dbContext, MainWindow mainWindow, int carId)
        {
            this.dbContext = dbContext;
            this.mainWindow = mainWindow;
            this.carId = carId;
            InitializeComponent();

            if(carId >= 0)
            {
                Car oldCar = dbContext.Cars.Include("Engine").Where(x => x.Id == carId).FirstOrDefault();
                modelText.Text = oldCar.Model;
                yearText.Text = oldCar.Year.ToString();
                engineModelText.Text = oldCar.Engine.Model;
                displacementText.Text = oldCar.Engine.Displacement.ToString();
                horsePowerText.Text = oldCar.Engine.HorsePower.ToString();
            }

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            if (carId < 0)
            {
                Car car = new Car("", new Engine(0, 0, ""), 0);
                setCarPropertiesFromTextValues(car);
                saveNewCar(car);
            }
            else
            {
                Car car = dbContext.Cars.Include("Engine").Where(x => x.Id == carId).FirstOrDefault();
                setCarPropertiesFromTextValues(car);
                saveEdditedCar(car);
            }


            this.Close();
        }

        private void setCarPropertiesFromTextValues(Car car)
        {
            string model = modelText.Text;
            int year;
            Int32.TryParse(yearText.Text, out year);
            string engineModel = engineModelText.Text;
            double displacement;
            Double.TryParse(displacementText.Text, out displacement);
            double horsePower;
            Double.TryParse(horsePowerText.Text, out horsePower);

            car.Model = model;
            car.Year = year;
            car.Engine.Model = engineModel;
            car.Engine.Displacement = displacement;
            car.Engine.HorsePower = horsePower;
        }

        private void saveEdditedCar(Car car)
        {

            MyDbContext newContext = new MyDbContext();

            newContext.Entry(car).State = System.Data.Entity.EntityState.Modified;
            newContext.Entry(car.Engine).State = System.Data.Entity.EntityState.Modified;
            newContext.SaveChanges();

            mainWindow.refreshCarsList();
        }

        private void saveNewCar(Car newCar)
        {

            dbContext.Cars.Add(newCar);
            dbContext.SaveChanges();

            mainWindow.refreshCarsList();

        }
    }
}
