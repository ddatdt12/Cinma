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
        public void LoadEditFood(EditFoodWindow wd)
        {

            wd._displayName.Text = SelectedItem.DisplayName;
            wd._category.Text = SelectedItem.Category;
            wd._price.Text = SelectedItem.Price.ToString();
            oldFoodName = DisplayName;
            imgfullname = SelectedItem.Image;

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

        public void EditFood(Window p)
        {
            if (Id != -1 && IsValidData())
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
                    filepath = Helper.GetProductImgPath(Image);
                    product.Image = imgfullname = Helper.CreateImageFullName(DisplayName, Image.Split('.')[1]);
                }

                (bool successUpdateProduct, string messageFromUpdateProduct) = ProductService.Ins.UpdateProduct(product);

                if (successUpdateProduct)
                {
                    MessageBox.Show(messageFromUpdateProduct);

                    if (Image != product.Image)
                    {
                        SaveImgToApp();
                        File.Delete(Helper.GetProductImgPath(Image));
                    }
                    else
                    {
                        File.Copy(filepath, Helper.GetProductImgPath(product.Image), true);
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
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
        }
    }
}
