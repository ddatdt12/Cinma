using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{

    public class ForgotPassViewModel
    {
        public ICommand ConfirmCM { get; set; }
        public ICommand CancelCM { get; set; }



        public ForgotPassViewModel()
        {
            CancelCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
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
