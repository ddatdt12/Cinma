using CinemaManagement.DTOs;
using CinemaManagement.Views.Staff.OrderFoodWindow;
using CinemaManagement.Views.Staff.TicketWindow;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagement.ViewModel.StaffViewModel.TicketVM
{
    public partial class TicketWindowViewModel : BaseViewModel
    {
        public ICommand CloseTicketWindowCM { get; set; }
        public ICommand MinimizeTicketWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand LoadTicketBookingPageCM { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public TicketWindowViewModel()
        {
            CaculateTime();
            Output_ToString();
            GenerateSeat();
            WaitingList = new List<SeatSettingDTO>();

            sumCurrentSeat = "Số ghế   (" + (ListSeat.Count - ListStatusSeat.Count).ToString() + "/128)";
            CloseTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DataContext = new TicketWindowViewModel();
                    w.Close();
                }
            });
            MinimizeTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
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
            SelectedSeatCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    foreach (var st in ListStatusSeat)
                        if (p.Content.ToString() == st.SeatPosition)
                        {
                            MessageBox.Show("Ghế này đã bán vui lòng chọn ghế khác!");
                            return;
                        }
                    if (IsExist(p.Content.ToString()))
                    {
                        p.Background = new SolidColorBrush(Colors.Transparent);
                        p.Foreground = new SolidColorBrush(Colors.Black);
                        WaitingSeatList(p);
                        return;
                    }
                    p.Background = new SolidColorBrush(Colors.Green);
                    p.Foreground = new SolidColorBrush(Colors.White);
                    WaitingSeatList(p);
                }


            });
            LoadStatusSeatCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {

                if (p != null)
                {
                    foreach (var item in ListSeat)
                    {
                        if (item.SeatPosition == p.Content.ToString() && item.Status == true)
                        {
                            p.Background = new SolidColorBrush(Colors.Brown);
                            p.Foreground = new SolidColorBrush(Colors.White);
                            return;
                        }

                    }
                }
            });
            LoadTicketBookingPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new TicketBookingPage();
            });
            //SetStatusSeatCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            //{
            //    foreach (var item in listlabel)
            //    {
            //      item.Background = new SolidColorBrush(Colors.Brown);
            //      item.Foreground = new SolidColorBrush(Colors.White);
            //    }
            //    MessageBox.Show("Đặt ghế thành công!");
            //});
            LoadFoodPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new FoodPage();

            });
        }
    }
}