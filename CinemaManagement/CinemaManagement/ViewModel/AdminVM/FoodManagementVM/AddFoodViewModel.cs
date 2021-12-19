using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public async Task AddFood(Window p)
        {
            if (filepath != null && IsValidData())
            {
                ProductDTO product = new ProductDTO();

                product.DisplayName = DisplayName;
                product.Category = Category.Content.ToString();
                product.Price = Price;
                product.Quantity = 0;

                //Upload image
                product.Image = await CloudinaryService.Ins.UploadImage(filepath);

                if (product.Image is null)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", MessageType.Error, MessageButtons.OK);
                    return;
                }
                (bool successAddProduct, string messageFromAddProduct, ProductDTO newProduct) = await ProductService.Ins.AddNewProduct(product);

                if (successAddProduct)
                {
                    isSaving = false;
                    LoadProductListView(Operation.CREATE, newProduct);
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromAddProduct, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    filepath = null;
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromAddProduct, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đủ thông tin", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }

        }
    }
}