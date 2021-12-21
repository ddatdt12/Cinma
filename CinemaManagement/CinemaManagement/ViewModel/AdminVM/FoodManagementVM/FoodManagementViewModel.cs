using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
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
        bool IsImageChanged = false;
        public static Grid MaskName { get; set; }

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

        private static ObservableCollection<ProductDTO> storeAllFood;
        public static ObservableCollection<ProductDTO> StoreAllFood
        {
            get { return storeAllFood; }
            set { storeAllFood = value; }
        }


        public ICommand FilterCboxFoodCommand { get; set; }
        public ICommand ImportFoodChangeCommand { get; set; }

        public ICommand OpenImportFoodCommand { get; set; }
        public ICommand OpenAddFoodCommand { get; set; }
        public ICommand OpenEditFoodCommand { get; set; }

        public ICommand ImportFoodCommand { get; set; }
        public ICommand AddFoodCommand { get; set; }
        public ICommand EditFoodCommand { get; set; }
        public ICommand DeleteFoodCommand { get; set; }

        public ICommand UpLoadImageCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand LoadProductListViewCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }


        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private bool isLoadding;
        public bool IsLoadding
        {
            get { return isLoadding; }
            set { isLoadding = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }


        //SelectedProduct Dùng khi nhập hàng thêm số lượng sản phẩm
        private ProductDTO _SelectedProduct;
        public ProductDTO SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged();
            }
        }

        public FoodManagementViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
             {
                 try
                 {
                     FoodList = new ObservableCollection<ProductDTO>();
                     IsLoadding = true;
                     StoreAllFood = new ObservableCollection<ProductDTO>(await Task.Run(() => ProductService.Ins.GetAllProduct()));
                     IsLoadding = false;

                     FoodList = new ObservableCollection<ProductDTO>(StoreAllFood);
                 }
                 catch (System.Data.Entity.Core.EntityException e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e);
                     MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                     mb.ShowDialog();
                 }

                 IsImageChanged = false;
             });

            FilterCboxFoodCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
                {
                    try
                    {
                        FoodList = new ObservableCollection<ProductDTO>();

                        if (Category.Content.ToString() == "Tất cả")
                        {
                            FoodList = new ObservableCollection<ProductDTO>(StoreAllFood);
                        }
                        else if (Category.Content.ToString() == "Đồ ăn")
                        {
                            for (int i = 0; i < StoreAllFood.Count; i++)
                            {
                                if (StoreAllFood[i].Category == "Đồ ăn")
                                {
                                    FoodList.Add(StoreAllFood[i]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < StoreAllFood.Count; i++)
                            {
                                if (StoreAllFood[i].Category != "Đồ ăn")
                                {
                                    FoodList.Add(StoreAllFood[i]);
                                }
                            }
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        Console.WriteLine(e);
                        MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mb.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mb.ShowDialog();
                    }

                });

            ImportFoodChangeCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
                 {
                     if (SelectedProduct != null && SelectedProduct.Image != null)
                     {
                         ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedProduct.Image);
                     }
                 });

            OpenImportFoodCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    ImportFoodWindow wd = new ImportFoodWindow();
                    LoadImportFoodWindow(wd);
                    MaskName.Visibility = Visibility.Visible;
                    wd.ShowDialog();
                });
            ImportFoodCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
                 {
                     IsSaving = true;
                     ProductDTO a = new ProductDTO();
                     a = SelectedProduct;
                     await ImportFood(p);
                     IsSaving = false;
                 });

            OpenAddFoodCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
                {
                    RenewWindowData();
                    AddFoodWindow wd = new AddFoodWindow();
                    MaskName.Visibility = Visibility.Visible;
                    wd.ShowDialog();
                });
            AddFoodCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
                {
                    IsSaving = true;

                    await AddFood(p);

                    IsSaving = false;
                });
            OpenEditFoodCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
                 {
                     EditFoodWindow wd = new EditFoodWindow();

                     IsLoadding = true;

                     Quantity = SelectedItem.Quantity;

                     await LoadEditFood(wd);

                     IsLoadding = false;

                     if (SelectedItem != null)
                     {
                         string tempCategory = SelectedItem.Category;
                         if (tempCategory == "Đồ ăn")
                         {
                             wd._category.Text = SelectedItem.Category;
                         }
                         else
                         {
                             wd._category.Text = "Nước uống";
                         }

                     }

                     MaskName.Visibility = Visibility.Visible;
                     wd.ShowDialog();

                 });
            EditFoodCommand = new RelayCommand<Window>((p) => { if (IsSaving) { return false; } return true; }, async (p) =>
            {
                isSaving = true;
                await EditFood(p);
                isSaving = false;
            });
            DeleteFoodCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
                  {
                      Image = SelectedItem.Image;
                      Id = SelectedItem.Id;
                      MessageBoxCustom mbx = new MessageBoxCustom("Cảnh báo", "Bạn có chắc muốn xoá sản phẩm này không?", MessageType.Warning, MessageButtons.YesNo);
                      mbx.ShowDialog();
                      if (mbx.DialogResult == true)
                      {
                          IsLoadding = true;

                          (bool successDelMovie, string messageFromDelMovie) = await ProductService.Ins.DeleteProduct(Id);

                          IsLoadding = false;

                          if (successDelMovie)
                          {
                              MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromDelMovie, MessageType.Success, MessageButtons.OK);
                              mb.ShowDialog();
                              LoadProductListView(Operation.DELETE);
                              SelectedItem = null;
                              return;
                          }
                          else
                          {
                              MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromDelMovie, MessageType.Error, MessageButtons.OK);
                              mb.ShowDialog();
                              return;
                          }
                      }
                  });

            CloseCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    w.Close();
                }
            });

            UpLoadImageCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    OpenFileDialog openfile = new OpenFileDialog();
                    openfile.Title = "Select an image";
                    openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                    if (openfile.ShowDialog() == DialogResult.OK)
                    {
                        IsImageChanged = true;
                        filepath = openfile.FileName;
                        LoadImage();
                        return;
                    }
                    IsImageChanged = false;
                });

            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
              {
                  MaskName = p;
              });
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


        public void LoadProductListView(Operation oper, ProductDTO product = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    FoodList.Add(product);
                    StoreAllFood.Add(product);
                    break;
                case Operation.UPDATE:
                    var productFound = FoodList.FirstOrDefault(s => s.Id == product.Id);
                    FoodList[FoodList.IndexOf(productFound)] = product;
                    StoreAllFood[StoreAllFood.IndexOf(productFound)] = product;
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
                    FoodList[FoodList.IndexOf(productFounded)] = updatedProd;
                    StoreAllFood[StoreAllFood.IndexOf(productFounded)] = updatedProd;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < FoodList.Count; i++)
                    {
                        if (FoodList[i].Id == Id)
                        {
                            FoodList.Remove(FoodList[i]);
                            StoreAllFood.Remove(StoreAllFood[i]);
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
            filepath = null;
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
