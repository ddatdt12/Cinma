using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public void LoadImportFoodWindow(ImportFoodWindow wd)
        {
            SelectedProduct = null;
            ImageSource = null;
            Price = 0;
            Quantity = 0;
        }
        public void ImportFood(Window p)
        {
            if (SelectedProduct != null)
            {
                if (Quantity>0 && Price>=0)
                {
                    ProductReceiptDTO productReceipt = new ProductReceiptDTO();
                    productReceipt.ProductId = SelectedProduct.Id;
                    productReceipt.ImportPrice = Price;
                    productReceipt.Quantity = Quantity;
                    productReceipt.StaffId = 1;

                    (bool successAddProductReceipt, string messageFromAddProductReceipt) = ProductReceiptService.Ins.CreateProductReceipt(productReceipt);

                    if (successAddProductReceipt)
                    {
                        ProductDTO productDTO = new ProductDTO();
                        LoadProductListView(Operation.UPDATE_PROD_QUANTITY, productDTO);
                        p.Close();
                    }
                    MessageBox.Show(messageFromAddProductReceipt);
                }
                else
                {
                    MessageBox.Show("Số lượng hoặc giá nhập không hợp lệ!");
                }
            }
            else
                MessageBox.Show("Vui lòng chọn sản phẩm!");
        }
    }
}
