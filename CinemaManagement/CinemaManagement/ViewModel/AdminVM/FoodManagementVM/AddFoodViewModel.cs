using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
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
                product.Quantity = 0;


                (bool successAddProduct, string messageFromAddProduct, ProductDTO newProduct) = ProductService.Ins.AddNewProduct(product);

                if (successAddProduct)
                {
                    SaveImgToApp();
                    IsAddingProduct = false;
                    LoadProductListView(Operation.CREATE, newProduct);
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                MessageBox.Show(messageFromAddProduct);
            }
            else
                MessageBox.Show("Vui lòng nhập đủ thông tin");
        }
    }
}