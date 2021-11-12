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
            (bool isValid, string error) = IsValidData(Operation.CREATE);
            if (isValid)
            {
                imgName = Helper.CreateImageName(DisplayName);
                imgfullname = Helper.CreateImageFullName(imgName, extension);
                ProductDTO product = new ProductDTO();

                product.DisplayName = DisplayName;
                product.Category = Category.Content.ToString();
                product.Price = Price;
                product.Image = "x";
                string x = product.Image;

                (bool successAddProduct, string messageFromAddProduct, ProductDTO newProduct) = ProductService.Ins.AddProduct(product);

                if (successAddProduct)
                {
                    IsAddingMovie = false;
                    SaveImgToApp();
                    LoadProductListView(Operation.CREATE, newProduct);
                    p.Close();
                }
                MessageBox.Show(messageFromAddProduct);
            }
            else
                MessageBox.Show(error);
        }
    }
}
