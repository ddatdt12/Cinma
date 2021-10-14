using CinemaManagement.Views.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel
{
    public class MainStaffViewModel : BaseViewModel
    {
        private bool _IsClickLabel;
        public bool IsClickLabel
        {
            get => _IsClickLabel;
            set
            {
                _IsClickLabel = value;
            }
        }
        private bool _IsOverLabel;
        public bool IsOverLabel
        {
            get => _IsOverLabel;
            set
            {
                _IsOverLabel = value;
            }
        }
        #region commands
        public ICommand ChangeLabelBackgroundCM1 { get; set; }
        public ICommand ChangeLabelBackgroundCM2 { get; set; }
        public ICommand CloseMainStaffWindowCM { get; set; }
        public ICommand MinimizeMainStaffWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand ButtonVisibility { get; set; }
        public ICommand LoadMainStaffPageCM { get; set; }
        #endregion
        public MainStaffViewModel()
        {
            CloseMainStaffWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
                {
                    FrameworkElement window = Window.GetWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.Close();
                    }
                });
            MinimizeMainStaffWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
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
            ButtonVisibility = new RelayCommand<StackPanel>((p) => { return p == null ? false : true; }, (p) =>
                {
                    if (p != null)
                    {
                        if (p.Visibility == Visibility.Collapsed)
                        {
                            p.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            p.Visibility = Visibility.Collapsed;
                        }
                    }
                });
            LoadMainStaffPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
             {
                 MainStaffPage w1 = new MainStaffPage();
                 p.Content = w1;
             });
            ChangeLabelBackgroundCM1 = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                IsClickLabel = !IsClickLabel;
                 
                if(IsClickLabel)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    p.Background = new SolidColorBrush(Colors.Transparent);
                }
            });
            ChangeLabelBackgroundCM2 = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                IsOverLabel = !IsOverLabel;

                if (IsOverLabel)
                {
                    p.Background = new SolidColorBrush(Colors.Chocolate);
                    p.FontSize = 22;
                }
                else
                {
                    p.Background = new SolidColorBrush(Colors.Transparent);
                    p.FontSize = 17;
                }
            });
        }

    }
}