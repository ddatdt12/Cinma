using CinemaManagement.DTOs;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        #region Biến lưu dữ liệu thêm

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Category;
        public ComboBoxItem Category
        {
            get { return _Category; }
            set { _Category = value; OnPropertyChanged(); }
        }

        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(); }
        }

        #endregion

        private ObservableCollection<ProductDTO> _foodList;
        public ObservableCollection<ProductDTO> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
            }
        }

        public ICommand OpenAddFoodCommand { get; set; }
        public ICommand OpenEditFoodCommand { get; set; }
        public ICommand OpenDeleteFoodCommand { get; set; }

        public ICommand MouseMoveCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }



        public FoodManagementViewModel()
        {
            
            FoodList = new ObservableCollection<ProductDTO>() { };
            for (int i = 0; i < 12; i++)
            {
                FoodList.Add(new ProductDTO("Bắp ngô","Thức uống",i));
            }

            OpenAddFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) => {
                    AddFoodWindow wd = new AddFoodWindow();
                    DisplayName = null;
                    Category = null;
                    wd.ShowDialog();

                });

            OpenEditFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) => {
                    EditFoodWindow wd = new EditFoodWindow();
                    ProductDTO a = new ProductDTO();
                    wd._displayName.Text = SelectedItem.DisplayName;
                    wd._category.Text = SelectedItem.Category;
                    wd._price.Text = SelectedItem.Price.ToString();
                    wd.ShowDialog();

                });

            OpenDeleteFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) => {
                    DeleteFoodWindow wd = new DeleteFoodWindow();
                    wd.ShowDialog();

                });

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
            );

            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            }
           );

        }

        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
    }
    
}
