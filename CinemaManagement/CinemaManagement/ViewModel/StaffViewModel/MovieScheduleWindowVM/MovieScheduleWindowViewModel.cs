using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.StaffViewModel.MovieScheduleWindowVM
{
    class MovieScheduleWindowViewModel : BaseViewModel
    {
        #region
        public ICommand CloseMovieScheduleWindowCM { get; set; }
        public ICommand MinimizeMovieScheduleWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand ClickButton { get; set; }
        #endregion
        public MovieScheduleWindowViewModel()
        {
            CloseMovieScheduleWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });
            MinimizeMovieScheduleWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.WindowState = WindowState.Minimized;
                }
            });
            MouseMoveWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            ClickButton = new RelayCommand<StackPanel>((p) => { return true; }, (p) =>
            {
                if(p.Visibility == Visibility.Collapsed)
                {
                    p.Visibility = Visibility.Visible;
                }
                else
                {
                    p.Visibility = Visibility.Collapsed;
                }
            }
            );
        }
    }
}
