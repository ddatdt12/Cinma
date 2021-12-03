using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.IO;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public void LoadEditFood(EditFoodWindow wd)
        {
            if (SelectedItem != null)
            {
                DisplayName = SelectedItem.DisplayName;
                wd._category.Text = SelectedItem.Category;
                Price = SelectedItem.Price;
                Image = SelectedItem.Image;
                Id = SelectedItem.Id;
                oldFoodName = DisplayName;
                imgfullname = SelectedItem.Image;
                IsImageChanged = false;

                if (File.Exists(Helper.GetProductImgPath(SelectedItem.Image)) == true)
                {
                    BitmapImage _image = new BitmapImage();
                    _image.BeginInit();
                    _image.CacheOption = BitmapCacheOption.None;
                    _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    _image.CacheOption = BitmapCacheOption.OnLoad;
                    _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    _image.UriSource = new Uri(Helper.GetProductImgPath(SelectedItem.Image));
                    _image.EndInit();

                    ImageSource = _image;
                }
                else
                {
                    wd.EditImage.Source = Helper.GetProductImageSource("null.jpg");
                }
            }
        }

        public void EditFood(Window p)
        {
            if (Id != -1 && IsValidData())
            {

                ProductDTO product = new ProductDTO();

                product.DisplayName = DisplayName;
                product.Category = Category.Content.ToString();
                product.Price = Price;
                product.Id = Id;

                if (IsImageChanged)
                {
                    imgName = Helper.CreateImageName(product.DisplayName);
                    imgfullname = Helper.CreateImageFullName(imgName, extension);
                    product.Image = imgfullname;
                }
                else
                {
                    filepath = Helper.GetProductImgPath(Image);
                    product.Image = imgfullname = Helper.CreateImageFullName(Helper.CreateImageName(product.DisplayName), Image.Split('.')[1]);
                }
                (bool successUpdateProduct, string messageFromUpdateProduct) = ProductService.Ins.UpdateProduct(product);

                if (successUpdateProduct)
                {
                    SaveImgToApp();
                    LoadProductListView(Operation.UPDATE, product);
                    MessageBox.Show(messageFromUpdateProduct);
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                    MessageBox.Show(messageFromUpdateProduct);
            }
            else
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
        }
    }
}
