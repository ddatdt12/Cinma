using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public void AddFood(Window p)
        {
            if (filepath != null && IsValidData())
            {
                imgName = Helper.CreateImageName(DisplayName);
                imgfullname = Helper.CreateImageFullName(imgName, extension);
                ProductDTO product = new ProductDTO();

                product.DisplayName = DisplayName;
                product.Category = Category.Content.ToString();
                product.Price = Price;
                product.Image = imgfullname;

                (bool successAddProduct, string messageFromAddProduct, ProductDTO newProduct) = ProductService.Ins.AddNewProduct(product);

                if (successAddProduct)
                {
                    IsAddingProduct = false;
                    MessageBox.Show(messageFromAddProduct);
                    SaveImgToApp();
                    LoadProductListView(Operation.CREATE, newProduct);
                    p.Close();
                }
                else
                {
                    MessageBox.Show(messageFromAddProduct);
                }
            }
            else
                MessageBox.Show("Vui lòng nhập đủ thông tin");
        }
    }
}