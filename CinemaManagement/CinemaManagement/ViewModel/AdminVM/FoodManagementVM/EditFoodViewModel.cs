using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System;
using System.IO;
using System.Net.Cache;
using System.Threading.Tasks;
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
                //imgfullname = SelectedItem.Image;
                IsImageChanged = false;


                ImageSource = SelectedItem.ImgSource;

                //if (File.Exists(Helper.GetProductImgPath(SelectedItem.Image)) == true)
                //{
                //    BitmapImage _image = new BitmapImage();
                //    _image.BeginInit();
                //    _image.CacheOption = BitmapCacheOption.None;
                //    _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                //    _image.CacheOption = BitmapCacheOption.OnLoad;
                //    _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                //    _image.UriSource = new Uri(Helper.GetProductImgPath(SelectedItem.Image));
                //    _image.EndInit();

                //    ImageSource = _image;
                //}
                //else
                //{
                //    wd.EditImage.Source = Helper.GetProductImageSource("null.jpg");
                //}
            }
        }

        public async Task EditFood(Window p)
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
                    //imgName = Helper.CreateImageName(product.DisplayName);
                    //imgfullname = Helper.CreateImageFullName(imgName, extension);
                    product.Image = Helper.ConvertImageToBase64Str(filepath);
                }
                else
                {
                    //filepath = Helper.GetProductImgPath(Image);
                    //product.Image = imgfullname = Helper.CreateImageFullName(Helper.CreateImageName(product.DisplayName), Image.Split('.')[1]);
                    product.Image = Image;
                }
                long s = product.Image.Length;
                (bool successUpdateProduct, string messageFromUpdateProduct) = await ProductService.Ins.UpdateProduct(product);

                if (successUpdateProduct)
                {
                    //SaveImgToApp();
                    LoadProductListView(Operation.UPDATE, product);
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromUpdateProduct, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromUpdateProduct, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đủ thông tin!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }
}
