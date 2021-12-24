using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.ViewModel.StaffViewModel.TicketBillVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketVM;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff;
using CinemaManagement.Views.Staff.TicketBill;
using CinemaManagement.Views.Staff.TicketWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM
{
    public class OrderFoodPageViewModel : BaseViewModel
    {
        public static List<SeatSettingDTO> tempListSeatSettings;
        public static bool IsBacking = false;

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }


        #region Commands

        public ICommand StoreCardViewCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand FilterAllProductsCommand { get; set; }
        public ICommand FilterFoodCommand { get; set; }
        public ICommand FilterDrinkCommand { get; set; }
        public ICommand SelectedProductCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand DeleteAllOrderCommand { get; set; }
        public ICommand BuyCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand BackToMovieBookingPageCM { get; set; }

        #endregion

        #region biến
        Card StoreCardView { get; set; }
        string SelectedView { get; set; }

        private decimal _TotalPrice;
        public string TotalPrice { get; set; }

        private TabItem _TabProduct;
        public TabItem TabProduct
        {
            get => _TabProduct;
            set
            {
                _TabProduct = value;
                OnPropertyChanged();
            }
        }

        private ProductDTO _SelectedProductToOrder;
        public ProductDTO SelectedProductToOrder
        {
            get => _SelectedProductToOrder;
            set
            {
                _SelectedProductToOrder = value;
                OnPropertyChanged();
            }
        }

        private ProductDTO _SelectedProductToBill;
        public ProductDTO SelectedProductToBill
        {
            get => _SelectedProductToBill;
            set
            {
                _SelectedProductToBill = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductDTO> _AllProduct;
        public ObservableCollection<ProductDTO> AllProduct
        {
            get => _AllProduct;
            set
            {
                _AllProduct = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductDTO> _menuList;
        public ObservableCollection<ProductDTO> MenuList
        {
            get => _menuList;
            set
            {
                _menuList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductDTO> _orderList;
        public ObservableCollection<ProductDTO> OrderList
        {
            get => _orderList;
            set
            {
                _orderList = value;
                OnPropertyChanged();
            }
        }

        public static ObservableCollection<ProductDTO> ListOrder;

        public static bool checkOnlyFoodOfPage;

        private bool _ShowBackIcon;
        public bool ShowBackIcon
        {
            get => _ShowBackIcon;
            set
            {
                _ShowBackIcon = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public OrderFoodPageViewModel()
        {
            tempListSeatSettings = TicketWindowViewModel.WaitingList;
            ShowBackIcon = true;
            IsBacking = false;

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {


                AllProduct = new ObservableCollection<ProductDTO>();
                OrderList = new ObservableCollection<ProductDTO>();
                MenuList = new ObservableCollection<ProductDTO>();
                ListOrder = new ObservableCollection<ProductDTO>();
                if (!TicketBillViewModel.IsBacking)
                {
                    OrderList = ListOrder;
                }


                if (TicketBillViewModel.ListFood != null && TicketBillViewModel.IsBacking)
                {
                    OrderList = TicketBillViewModel.ListFood;
                    TicketBillViewModel.IsBacking = false;
                }

                if (checkOnlyFoodOfPage)
                {
                    ShowBackIcon = false;
                }

                //Khởi tạo giá trị ban đầu cho tổng giá tiền
                ReCalculate();

                await LoadListProduct();

                //Khởi tạo giá trị ban đầu các item cho MenuList
                MenuList = AllProduct;
            });
            //Filter All Products
            FilterAllProductsCommand = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                MenuList = new ObservableCollection<ProductDTO>(AllProduct);
                SelectedView = "Tất cả";
                ChangeView(p);
            });

            //Filter đồ ăn
            FilterFoodCommand = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                FilterFood();
                SelectedView = "Đồ ăn";
                ChangeView(p);
            });

            //Filter thức uống
            FilterDrinkCommand = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                FilterDrink();
                SelectedView = "Thức uống";
                ChangeView(p);
            });

            //Chọn đồ ăn chuyển qua OrderList
            SelectedProductCommand = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToOrder != null)
                {
                    if (SelectedProductToOrder.Quantity > 0)
                    {
                        for (int i = 0; i < AllProduct.Count; ++i)
                        {
                            if (AllProduct[i].Id == SelectedProductToOrder.Id)
                            {
                                AllProduct[i].Quantity -= 1;
                                FilterMenuList();
                                for (int j = 0; j < OrderList.Count; ++j)
                                {
                                    if (OrderList[j].Id == AllProduct[i].Id)
                                    {
                                        OrderList[j].Quantity += 1;
                                        OrderList = new ObservableCollection<ProductDTO>(OrderList);
                                        ReCalculate();
                                        return;
                                    }
                                }
                                //Thêm mới sản phẩm Order
                                OrderList.Add(new ProductDTO()
                                {
                                    Id = AllProduct[i].Id,
                                    DisplayName = AllProduct[i].DisplayName,
                                    Category = AllProduct[i].Category,
                                    Price = AllProduct[i].Price,
                                    Image = AllProduct[i].Image,
                                    Quantity = 1
                                });
                                ReCalculate();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxCustom mgb = new MessageBoxCustom("Cảnh báo", "Hết hàng!", MessageType.Warning, MessageButtons.OK);
                        mgb.ShowDialog();
                    }
                }
            });

            //Xóa tất cả sản phẩm order
            DeleteAllOrderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MessageBoxCustom mgb;
                if (OrderList.Count == 0)
                {
                    mgb = new MessageBoxCustom("Lỗi", "Danh sách không có sản phẩm nào!", MessageType.Error, MessageButtons.OK);
                    mgb.ShowDialog();
                }
                else
                {
                    mgb = new MessageBoxCustom("Xác nhận", "Xoá danh sách vừa chọn?", MessageType.Warning, MessageButtons.YesNo);
                    if (mgb.ShowDialog() == true)
                    {
                        for (int i = 0; i < OrderList.Count; ++i)
                        {
                            for (int j = 0; j < AllProduct.Count; ++j)
                            {
                                if (OrderList[i].Id == AllProduct[j].Id)
                                {
                                    AllProduct[j].Quantity += OrderList[i].Quantity;
                                    break;
                                }
                            }
                        }
                        FilterMenuList();
                        OrderList.Clear();
                        ReCalculate();
                    }
                }
            });

            //Xóa sản phẩm order
            DeleteProductCommand = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToBill != null)
                {
                    DeleteOrderProduct(SelectedProductToBill);
                }
            });

            //Giảm số lượng trong danh sách Order
            MinusCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToBill != null)
                {
                    if (SelectedProductToBill.Quantity <= 1)
                    {
                        ProductDTO temp = SelectedProductToBill;
                        MessageBoxCustom mgb = new MessageBoxCustom("Xác nhận", "Xoá sản phẩm này?", MessageType.Warning, MessageButtons.YesNo);
                        if (mgb.ShowDialog() == true)
                        {
                            DeleteOrderProduct(temp);
                        }
                    }
                    else
                    {
                        SelectedProductToBill.Quantity -= 1;
                        FilterOrderList();
                        PlusAllProductList(SelectedProductToBill);
                        ReCalculate();
                    }
                }
            });

            //Tăng số lượng trong danh sách Order
            PlusCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToBill != null)
                {
                    for (int i = 0; i < AllProduct.Count; ++i)
                    {
                        if (AllProduct[i].Id == SelectedProductToBill.Id)
                        {
                            if (AllProduct[i].Quantity > 0)
                            {
                                SelectedProductToBill.Quantity += 1;
                                FilterOrderList();
                                MinusAllProductList(SelectedProductToBill);
                                ReCalculate();
                            }
                            else
                            {
                                MessageBoxCustom mgb = new MessageBoxCustom("Cảnh báo", "Số lượng không đủ!", MessageType.Warning, MessageButtons.OK);
                                mgb.ShowDialog();
                            }
                            return;
                        }
                    }
                }
            });

            //Mua hàng, lưu xuống bill
            BuyCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (checkOnlyFoodOfPage)
                {
                    if (OrderList.Count == 0)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Danh sách thanh toán rỗng", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    else
                    {
                        ListOrder = new ObservableCollection<ProductDTO>(OrderList);
                        MainStaffWindow ms = Application.Current.Windows.OfType<MainStaffWindow>().FirstOrDefault();
                        ms.mainFrame.Content = new TicketBillOnlyFoodPage();
                    }
                }
                else
                {
                    if (OrderList.Count == 0)
                    {
                        ListOrder = new ObservableCollection<ProductDTO>(OrderList);
                        TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                        tk.TicketBookingFrame.Content = new TicketBillNoFoodPage();
                    }
                    else if (OrderList.Count > 0)
                    {
                        ListOrder = new ObservableCollection<ProductDTO>(OrderList);
                        TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                        tk.TicketBookingFrame.Content = new TicketBillPage();
                    }
                }
            });
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            StoreCardViewCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                StoreCardView = p;
            });

            BackToMovieBookingPageCM = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    try
                    {
                        IsBacking = true;
                        TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                        tk.TicketBookingFrame.Content = new TicketBookingPage();
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }
                    catch (Exception e)
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                        mess.ShowDialog();
                    }

                });
        }

        private Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
        public void ReCalculate()
        {
            _TotalPrice = 0;
            foreach (ProductDTO item in OrderList)
            {
                _TotalPrice += item.Price * item.Quantity;
            }
            TotalPrice = Helper.FormatVNMoney(_TotalPrice);
            OnPropertyChanged("TotalPrice");
        }
        public void FilterFood()
        {
            ObservableCollection<ProductDTO> temp = new ObservableCollection<ProductDTO>();
            foreach (ProductDTO item in AllProduct)
            {
                if (item.Category == "Đồ ăn")
                    temp.Add(item);
            }
            MenuList = new ObservableCollection<ProductDTO>(temp);
        }
        public void FilterDrink()
        {
            ObservableCollection<ProductDTO> temp = new ObservableCollection<ProductDTO>();
            foreach (ProductDTO item in AllProduct)
            {
                if (item.Category != "Đồ ăn")
                    temp.Add(item);
            }
            MenuList = new ObservableCollection<ProductDTO>(temp);
        }
        public void FilterMenuList()
        {
            if (SelectedView == "Tất cả")
            {
                MenuList = new ObservableCollection<ProductDTO>(AllProduct);
            }
            else
            {
                if (SelectedView != "Thức uống")
                {
                    FilterFood();
                }
                else
                {
                    FilterDrink();
                }
            }
        }
        public void FilterOrderList()
        {
            OrderList = new ObservableCollection<ProductDTO>(OrderList);
        }
        public void MinusAllProductList(ProductDTO item)
        {
            for (int i = 0; i < AllProduct.Count; ++i)
            {
                if (AllProduct[i].Id == item.Id)
                {
                    AllProduct[i].Quantity -= 1;
                    FilterMenuList();
                    return;
                }
            }
        }
        public void PlusAllProductList(ProductDTO item)
        {
            for (int i = 0; i < AllProduct.Count; ++i)
            {
                if (AllProduct[i].Id == item.Id)
                {
                    AllProduct[i].Quantity += 1;
                    FilterMenuList();
                    return;
                }
            }
        }

        public async Task LoadListProduct()
        {
            try
            {
                IsLoading = true;

                AllProduct = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());

                IsLoading = false;
                return;
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }
            catch (Exception)
            {
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }
        }

        public void DeleteOrderProduct(ProductDTO temp)
        {
            for (int i = 0; i < AllProduct.Count; ++i)
            {
                if (temp.Id == AllProduct[i].Id)
                {
                    AllProduct[i].Quantity += temp.Quantity;
                    FilterMenuList();
                    OrderList.Remove(temp);
                    ReCalculate();
                    return;
                }
            }
        }
        public void ChangeView(Card p)
        {
            if (p is null) return;
            StoreCardView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f0f2f5");
            StoreCardView.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth2);
            StoreCardView = p;
            p.Background = new SolidColorBrush(Colors.White);
            p.SetValue(ShadowAssist.ShadowDepthProperty, ShadowDepth.Depth0);
        }
    }
}