﻿using CinemaManagement.DTOs;
using CinemaManagement.Models;
using CinemaManagement.Models.Services;
using CinemaManagement.Views.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM
{
    public class OrderFoodViewModel : BaseViewModel
    {
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand UncheckedCommand { get; set; }
        public ICommand FilterFoodCommand { get; set; }
        public ICommand FilterDrinkCommand { get; set; }
        public ICommand SelectedProductCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand BuyCommand { get; set; }


        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get => _TotalPrice;
            set
            {
                _TotalPrice = value;
            }
        }

        private ProductDTO _SelectedProductToOrder;
        public ProductDTO SelectedProductToOrder
        {
            get => _SelectedProductToOrder;
            set
            {
                _SelectedProductToOrder = value;
            }
        }
        public ProductDTO SelectedProductToBill { get; set; }

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

        public OrderFoodViewModel()
        {
            AllProduct = new ObservableCollection<ProductDTO>();
            OrderList = new ObservableCollection<ProductDTO>();
            MenuList = new ObservableCollection<ProductDTO>();

            //Gán giá trị demo mẫu đồ ăn và thức uống
            for (int i = 1; i <= 10; ++i)
            {
                AllProduct.Add(new ProductDTO(i, "Bỏng ngô", "Đồ ăn", 15000, "/Resources/FoodLayout/Food/Burger/Chicken.jpg"));
            }

            for (int i = 11; i <= 15; ++i)
            {
                AllProduct.Add(new ProductDTO(i, "Trà sữa", "Thức uống", 20000, "/Resources/FoodLayout/Food/Drink/Milk Tea.jpg"));
            }

            for (int i = 1; i <=9; ++i)
            {
                if (i%2==0)
                {
                    OrderList.Add(new ProductDTO(i, "Bỏng ngô", "Đồ ăn", 15000, "/Resources/FoodLayout/Food/Burger/Chicken.jpg"));
                }
                else
                {
                    OrderList.Add(new ProductDTO(i, "Trà sữa", "Thức uống", 20000, "/Resources/FoodLayout/Food/Drink/Milk Tea.jpg"));
                }
            }

            //Khởi tạo giá trị ban đầu các item cho MenuList
            MenuList = AllProduct;

            //Khởi tạo giá trị ban đầu cho tổng giá tiền
            TotalPrice = 0;

            //Tạo resource lưu trạng thái trước khi Checked
            ObservableCollection<ProductDTO> tempResource = new ObservableCollection<ProductDTO>();

            //CheckedCommand
            UncheckedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MenuList = tempResource;
            });

            //CheckedCommand
            CheckedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                tempResource = MenuList;
                MenuList = AllProduct;
            });

            //Filter đồ ăn
            FilterFoodCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObservableCollection<ProductDTO> temp = new ObservableCollection<ProductDTO>();
                foreach (ProductDTO item in AllProduct.ToList())
                {
                    if (item.Category == "Đồ ăn")
                        temp.Add(item);
                }
                MenuList = temp;
            });

            //Filter thức uống
            FilterDrinkCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObservableCollection<ProductDTO> temp = new ObservableCollection<ProductDTO>();
                foreach (ProductDTO item in AllProduct.ToList())
                {
                    if (item.Category == "Thức uống")
                        temp.Add(item);
                }
                MenuList = temp;
            });

            //Chọn đồ ăn chuyển qua OrderList
            SelectedProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObservableCollection<ProductDTO> temp = OrderList;
                bool flag = false;
                if (SelectedProductToOrder != null)
                {
                    for (int i= 0; i < temp.Count; ++i)
                    {
                        if (temp[i].Id == SelectedProductToOrder.Id)
                        {
                            ++temp[i].Quantity;
                            flag = true;
                            TotalPrice += temp[i].Price;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        temp.Add(SelectedProductToOrder);
                        TotalPrice += SelectedProductToOrder.Price;
                    }
                    OrderList = temp;
                }
            });

            //Xóa sản phẩm order
            DeleteProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                foreach (ProductDTO item in OrderList)
                {
                    if (item.Id == SelectedProductToBill.Id)
                    {
                        OrderList.Remove(item);
                        TotalPrice -= item.Price * item.Quantity;
                        return;
                    }
                }
            });

            //Giảm số lượng
            MinusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToBill != null)
                {
                    for (int i = 0; i < OrderList.Count; ++i)
                    {
                        if (OrderList[i].Id == SelectedProductToBill.Id)
                        {
                            --OrderList[i].Quantity;
                            TotalPrice -= OrderList[i].Price;
                            break;
                        }
                    }
                }
            });

            //Tăng số lượng
            PlusCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedProductToBill != null)
                {
                    for (int i = 0; i < OrderList.Count; ++i)
                    {
                        if (OrderList[i].Id == SelectedProductToBill.Id)
                        {
                            ++OrderList[i].Quantity;
                            TotalPrice += OrderList[i].Price;
                            break;
                        }
                    }
                }
            });

            //Mua hàng, lưu xuống bill
            BuyCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });

            CloseWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {

                if (p != null)
                {
                    p.Close();
                }

            });
            MinimizeWindowCM = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {

                if (p != null)
                {
                    p.WindowState = WindowState.Minimized;
                }
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
        private Window GetWindowParent(Window p)
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