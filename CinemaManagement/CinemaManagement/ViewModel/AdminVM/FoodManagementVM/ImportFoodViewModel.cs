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
        public void LoadImportFoodWindow(ImportFoodWindow wd)
        {
            SelectedProduct = null;
            ImageSource = null;
            Price = 0;
            Quantity = 0;
        }
        public async Task ImportFood(Window p)
        {
            if (SelectedProduct != null)
            {
                if (Quantity > 0 && Price >= 0)
                {
                    ProductReceiptDTO productReceipt = new ProductReceiptDTO();
                    productReceipt.ProductId = SelectedProduct.Id;
                    productReceipt.ImportPrice = Price;
                    productReceipt.Quantity = Quantity;
                    productReceipt.StaffId = MainAdminViewModel.currentStaff.Id;

                    (bool successAddProductReceipt, string messageFromAddProductReceipt, ProductReceiptDTO newProductReceipt) = await ProductReceiptService.Ins.CreateProductReceipt(productReceipt);

                    if (successAddProductReceipt)
                    {
                        LoadProductListView(Operation.UPDATE_PROD_QUANTITY);
                        MaskName.Visibility = Visibility.Collapsed;
                        p.Close();
                        MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromAddProductReceipt, MessageType.Success, MessageButtons.OK);
                        mb.ShowDialog();
                    }
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Số lượng hoặc giá nhập không hợp lệ!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng chọn sản phẩm!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }
}
