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
        Label Flag = new Label();

        private bool _IsClickLabel;
        public bool IsClickLabel
        {
            get => _IsClickLabel;
            set
            {
                _IsClickLabel = value;
                OnPropertyChanged();
            }
        }
        private bool _IsClickLabel1;
        public bool IsClickLabel1
        {
            get => _IsClickLabel1;
            set
            {
                _IsClickLabel1 = value;
                OnPropertyChanged();
            }
        }
        private bool _IsClickLabel2;
        public bool IsClickLabel2
        {
            get => _IsClickLabel2;
            set
            {
                _IsClickLabel2 = value;
                OnPropertyChanged();
            }
        }
        private bool _IsClickLabel3;
        public bool IsClickLabel3
        {
            get => _IsClickLabel3;
            set
            {
                _IsClickLabel3 = value;
                OnPropertyChanged();
            }
        }
        #region commands
        public ICommand CloseMainStaffWindowCM { get; set; }
        public ICommand MinimizeMainStaffWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand CategoryFilmVisibility { get; set; }
        public ICommand CategoryFilmLabel { get; set; }
        public ICommand LoadMainStaffPageCM { get; set; }
        public ICommand TheFirstHomePageCM { get; set; }
        public ICommand ChangeBackgroundAndFontSizeLabel1 { get; set; }
        public ICommand ChangeBackgroundAndFontSizeLabel2 { get; set; }
        public ICommand ChangeBackgroundAndFontSizeLabel3 { get; set; }
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
            CategoryFilmLabel = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
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
            CategoryFilmVisibility = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (p != null)
                {
                    if (p.Visibility == Visibility.Visible && IsClickLabel3 == true)
                    {
                        p.Visibility = Visibility.Collapsed;
                        IsClickLabel3 = false;
                    }
                }
            });
            LoadMainStaffPageCM = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
             {
                 MainStaffPage w1 = new MainStaffPage();
                 p.Content = w1;
             });
            ChangeBackgroundAndFontSizeLabel1 = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if(IsClickLabel2 == true || IsClickLabel3 == true)
                {
                    IsClickLabel2 = false;
                    IsClickLabel3 = false;
                }
                IsClickLabel1 = !IsClickLabel1;
            });
            ChangeBackgroundAndFontSizeLabel2 = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (IsClickLabel1 == true || IsClickLabel3 == true)
                {
                    IsClickLabel1 = false;
                    IsClickLabel3 = false;
                }
                IsClickLabel2 = !IsClickLabel2;
            });
            ChangeBackgroundAndFontSizeLabel3 = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (IsClickLabel1 == true || IsClickLabel2 == true)
                {
                    IsClickLabel1 = false;
                    IsClickLabel2 = false;
                }
                IsClickLabel3 = !IsClickLabel3;
            });
        }
    }
}