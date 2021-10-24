﻿using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyNhanVienPageVM
{
    public class SuaNVWindowViewModel : BaseViewModel
    {


        public ICommand MouseMoveCommand { get; set; }
        public ICommand DoneCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public SuaNVWindowViewModel()
        {
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            }
           );

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
           );
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
