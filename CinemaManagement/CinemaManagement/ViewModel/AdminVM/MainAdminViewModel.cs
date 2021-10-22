using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CinemaManagement.Views.Admin.QuanLySuatChieuPage;

namespace CinemaManagement.ViewModel
{

    public class MainAdminViewModel : BaseViewModel
    {
        public ICommand SignoutCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand LoadQLPPageCM { get; set; }
        public ICommand LoadQLNVPagePageCM { get; set; }
        public ICommand LoadSuatChieuPageCM { get; set; }
        

        public MainAdminViewModel()
        {
            SignoutCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
               {
                   FrameworkElement window = GetParentWindow(p);
                   var w = window as Window;
                   if (w != null)
                   {
                       w.Hide();
                       LoginWindow w1 = new LoginWindow();
                       w1.ShowDialog();
                       w.Close();
                   }
               });
            MouseLeftButtonDownWindowCM = new RelayCommand<FrameworkElement>((p) => { return true; }, (p) =>
             {
                 FrameworkElement window = GetParentWindow(p);
                 var w = window as Window;
                 if (w != null)
                 {
                     w.DragMove();
                 }
             });
            LoadQLPPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (p != null)
                    p.Content = new QuanLyPhimPage();
            });
            LoadSuatChieuPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                if (p != null)
                    p.Content = new QuanLySuatChieuPage();
            });

        }


        FrameworkElement GetParentWindow(FrameworkElement p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
