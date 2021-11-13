using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System.IO;
using System.Windows;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {
        public void DeleteFood(Window p)
        {
            (bool successDelMovie, string messageFromDelMovie) = ProductService.Ins.DeleteProduct(Id);

            if (successDelMovie)
            {
                File.Delete(Helper.GetProductImgPath(Image));
                MessageBox.Show(messageFromDelMovie);
                LoadProductListView(Operation.DELETE);
                SelectedItem = null;
                p.Close();
                return;
            }
            else
            {
                MessageBox.Show(messageFromDelMovie);
                p.Close();
                return;
            }
        }
    }
}
