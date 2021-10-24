using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyNhanVienPageVM
{
    public class ThemNVWindowViewModel : BaseViewModel
    {
        public ICommand DoneCommand { get; set; }

        public ThemNVWindowViewModel()
        {
        }

        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
    }
}
