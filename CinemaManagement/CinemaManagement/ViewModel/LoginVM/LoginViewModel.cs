using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand ForgotPassCM { get; set; }
        
        
        public LoginViewModel()
        {
            CloseWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null? false : true; }, (p) => {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
                
            });
            MinimizeWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.WindowState = WindowState.Minimized;
                }

            });
            MouseLeftButtonDownWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }

            });
            ForgotPassCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    ForgotPassWindow w1 = new ForgotPassWindow();
                    w1.ShowDialog();
                }

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
