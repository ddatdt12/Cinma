using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.FoodManagementPage;
using System.Threading.Tasks;
using System.Windows;

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
                IsImageChanged = false;

                ImageSource = CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image);
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
                    product.Image = await CloudinaryService.Ins.UploadImage(filepath);
                }
                else
                {
                    product.Image = Image;
                }

                (bool successUpdateProduct, string messageFromUpdateProduct) = await ProductService.Ins.UpdateProduct(product);

                if (successUpdateProduct)
                {
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
