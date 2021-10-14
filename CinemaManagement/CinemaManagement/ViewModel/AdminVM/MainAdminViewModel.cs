using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CinemaManagement.DTOs;

namespace CinemaManagement.ViewModel
{

    public class MainAdminViewModel : BaseViewModel
    {
        public ICommand SignoutCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand LoadQLPPageCM { get; set; }

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
