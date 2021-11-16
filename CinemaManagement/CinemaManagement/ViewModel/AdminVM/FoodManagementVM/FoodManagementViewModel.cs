using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            set
            {
                _Category = value; OnPropertyChanged();
            }
        }

        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(); }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; OnPropertyChanged(); }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }

        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; OnPropertyChanged(); }
        }
        #endregion

        string filepath;
        string appPath;
        string imgName;
        string imgfullname;
        string extension;
        string oldFoodName;
        bool IsImageChanged = false;
        bool IsAddingProduct = false;

        private ObservableCollection<ProductDTO> _foodList;
        public ObservableCollection<ProductDTO> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
                OnPropertyChanged();
            }
        }

        public ICommand ImportFoodChangeCommand { get; set; }

        public ICommand OpenImportFoodCommand { get; set; }
        public ICommand OpenAddFoodCommand { get; set; }
        public ICommand OpenEditFoodCommand { get; set; }
        public ICommand OpenDeleteFoodCommand { get; set; }

        public ICommand ImportFoodCommand { get; set; }
        public ICommand AddFoodCommand { get; set; }
        public ICommand EditFoodCommand { get; set; }
        public ICommand DeleteFoodCommand { get; set; }

        public ICommand UpLoadImageCommand { get; set; }

        public ICommand MouseMoveCommand { get; set; }
        public ICommand CloseCommand { get; set; }


        //
        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }



        //SelectedProduct Dùng khi nhập hàng thêm số lượng sản phẩm
        private ProductDTO _SelectedProduct;
        public ProductDTO SelectedProduct
        {
            get { return _SelectedProduct; }
            set { _SelectedProduct = value;
                OnPropertyChanged();
            }
        }

        public FoodManagementViewModel()
        {

            LoadProductListView(Operation.READ);
            IsImageChanged = false;
            ImportFoodChangeCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (SelectedProduct != null)
                    {
                        if (File.Exists(Helper.GetProductImgPath(SelectedProduct.Image)) == true)
                        {

                            BitmapImage _image = new BitmapImage();
                            _image.BeginInit();
                            _image.CacheOption = BitmapCacheOption.None;
                            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                            _image.CacheOption = BitmapCacheOption.OnLoad;
                            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            _image.UriSource = new Uri(Helper.GetProductImgPath(SelectedProduct.Image));
                            _image.EndInit();

                            ImageSource = _image;
                        }
                        //else
                        //{
                        //    wd.ImportImage.Source = Helper.GetProductImageSource("null.jpg");
                        //}
                    }
                });

            OpenImportFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    ImportFoodWindow wd = new ImportFoodWindow();
                    LoadImportFoodWindow(wd);
                    wd.ShowDialog();
                });
            ImportFoodCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    ProductDTO a = new ProductDTO();
                    a = SelectedProduct;
                    ImportFood(p);
                });

            OpenAddFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    RenewWindowData();
                    AddFoodWindow wd = new AddFoodWindow();
                    IsAddingProduct = true;
                    wd.ShowDialog();
                });
            AddFoodCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    AddFood(p);
                });


            OpenEditFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    EditFoodWindow wd = new EditFoodWindow();
                    LoadEditFood(wd);
                    Image = SelectedItem.Image;
                    Id = SelectedItem.Id;
                    string x = SelectedItem.Category;
                    if (x == "Đồ ăn")
                    {
                        wd._category.Text = SelectedItem.Category;
                    }
                    else
                    {
                        wd._category.Text = "Thức uống";
                    }
                    wd.ShowDialog();

                });

            EditFoodCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {

                    EditFood(p);
                });

            OpenDeleteFoodCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    DeleteFoodWindow wd = new DeleteFoodWindow();
                    Image = SelectedItem.Image;
                    Id = SelectedItem.Id;
                    wd.ShowDialog();

                });

            DeleteFoodCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    DeleteFood(p);
                });

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
            );

            UpLoadImageCommand = new RelayCommand<Window>((p) => { return true; },
                (p) =>
                {
                    OpenFileDialog openfile = new OpenFileDialog();
                    openfile.Title = "Select an image";
                    openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                    if (openfile.ShowDialog() == DialogResult.OK)
                    {
                        IsImageChanged = true;
                        filepath = openfile.FileName;
                        extension = openfile.SafeFileName.Split('.')[1];
                        LoadImage();
                        return;
                    }
                    IsImageChanged = false;
                });

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

        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();

            ImageSource = _image;
        }

        public void SaveImgToApp()
        {
            try
            {
                appPath = Helper.GetProductImgPath(imgfullname);
                if (File.Exists(Helper.GetProductImgPath(imgfullname)))
                    File.Delete(Helper.GetProductImgPath(imgfullname));
                File.Copy(filepath, appPath, true);
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Unable to open file " + exp.Message);
            }
        }

        public void LoadProductListView(Operation oper, ProductDTO product = null)
        {

            switch (oper)
            {
                case Operation.READ:
                    try
                    {
                        FoodList = new ObservableCollection<ProductDTO>(ProductService.Ins.GetAllProduct());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show("Lỗi hệ thống " + e.Message);
                    }
                    break;
                case Operation.CREATE:
                    FoodList.Add(product);
                    break;
                case Operation.UPDATE:
                    var productFound = FoodList.FirstOrDefault(s => s.Id == product.Id);
                    FoodList[FoodList.IndexOf(productFound)] = product;
                    break;
                case Operation.UPDATE_PROD_QUANTITY:
                    var productFounded = FoodList.FirstOrDefault(s => s.Id == SelectedProduct.Id);
                    ProductDTO updatedProd = new ProductDTO()
                    {
                        Id = productFounded.Id,
                        DisplayName = productFounded.DisplayName,
                        Category = productFounded.Category,
                        Quantity = productFounded.Quantity + Quantity,
                        Image = productFounded.Image,
                        Price = productFounded.Price,
                    };
                    FoodList[FoodList.IndexOf(productFounded)].Quantity += Quantity;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < FoodList.Count; i++)
                    {
                        if (FoodList[i].Id == Id)
                        {
                            FoodList.Remove(FoodList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private bool IsValidData()
        {
            return !string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(Category.Content.ToString());
        }
        public void RenewWindowData()
        {
            DisplayName = "";
            ImageSource = null;
            Price = 0;
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
