using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public void LoadEditFood(EditFoodWindow wd)
        {

            wd._displayName.Text = SelectedItem.DisplayName;
            wd._category.Text = SelectedItem.Category;
            wd._price.Text = SelectedItem.Price.ToString();
            oldFoodName = DisplayName;
            imgfullname = SelectedItem.Image;

            if (File.Exists(Helper.GetProductImgPath()) == true)
            {
                ImageSource = Helper.GetImageSource(SelectedItem.Image);
            }
            else
            {
                wd.EditImage.Source = Helper.GetImageSource("null.jpg");
            }
            
        }
        public void EditFood(Window p)
        {
            (bool isValid, string error) = IsValidData(Operation.CREATE);
            if (isValid)
            {
                if (!IsImageChanged)
                    extension = Image.Split('.')[1];
                imgName = Helper.CreateImageName(DisplayName);
                imgfullname = Helper.CreateImageFullName(imgName, extension);


                ProductDTO product = new ProductDTO();

                product.DisplayName = DisplayName;
                product.Category = Category.Content.ToString();
                product.Price = Price;
                product.Id = Id;

                if (product.Image != Image)
                {
                    product.Image = imgfullname;
                }
                else
                {
                    filepath = Helper.GetProductImgPath();
                    product.Image = imgfullname = Helper.CreateImageFullName(DisplayName, Image.Split('.')[1]);
                }

                (bool successUpdateProduct, string messageFromUpdateProduct) = ProductService.Ins.UpdateProduct(product);

                if (successUpdateProduct)
                {
                    MessageBox.Show(messageFromUpdateProduct);

                    if (Image != product.Image)
                    {
                        SaveImgToApp();
                        File.Delete(Helper.GetProductImgPath());
                    }
                    else
                    {
                        File.Copy(filepath, Helper.GetProductImgPath(), true);
                    }

                    LoadProductListView(Operation.UPDATE, product);
                    p.Close();
                }
                else
                {
                    MessageBox.Show(messageFromUpdateProduct);
                }
            }
            else
                MessageBox.Show(error);
        }
    }
}
