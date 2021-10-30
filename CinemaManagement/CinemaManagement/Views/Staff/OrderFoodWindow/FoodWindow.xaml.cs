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
using System.Windows.Shapes;

namespace CinemaManagement.Views.Staff.OrderFoodWindow
{
    /// <summary>
    /// Interaction logic for FoodWindow.xaml
    /// </summary>
    public partial class FoodWindow : Window
    {
        public FoodWindow()
        {
            InitializeComponent();

            List<itemMenu> listMenu = new List<itemMenu>();
            List<itemOrder> listOrder = new List<itemOrder>();

            listMenu.Add(new itemMenu() { Name = "Bulgogi", Price = 100000, Image = "/Resources/FoodLayout/Food/Pizza/Bulgogi.jpg" });
            listMenu.Add(new itemMenu() { Name = "Lamacun", Price = 120000, Image = "/Resources/FoodLayout/Food/Pizza/Lamacun.jpg" });
            listMenu.Add(new itemMenu() { Name = "Langos", Price = 80000, Image = "/Resources/FoodLayout/Food/Pizza/Langos.jpg" });
            listMenu.Add(new itemMenu() { Name = "Sfiha", Price = 110000, Image = "/Resources/FoodLayout/Food/Pizza/Sfiha.jpg" });
            listMenu.Add(new itemMenu() { Name = "Tarte Flambee", Price = 70000, Image = "/Resources/FoodLayout/Food/Pizza/Tarte Flambee.jpg" });

            listOrder.Add(new itemOrder() { Name = "Bulgogi", Price = 100000, Image = "/Resources/FoodLayout/Food/Pizza/Bulgogi.jpg", Amount = 1 });

            OrderMenu.ItemsSource = listOrder;
            ListMenu.ItemsSource = listMenu;
        }
    }

    public class itemMenu
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }

    public class itemOrder: itemMenu
    {
        public int Amount { get; set; }
        public itemOrder()
        {
            Amount = 1;
        }
    }
}
