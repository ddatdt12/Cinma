using CinemaManagement.ViewModel.StaffViewModel.OrderFoodWindowVM;
using CinemaManagement.ViewModel.StaffViewModel.TicketBillVM;
using CinemaManagement.Views;
using CinemaManagement.Views.Staff.OrderFoodWindow;
using CinemaManagement.Views.Staff.TicketWindow;
using System.Linq;
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
        public ICommand FirstLoadCM { get; set; }
        public TicketWindowViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await GenerateSeat();
                CaculateTime();
                Output_ToString();
                sumCurrentSeat = "Số ghế   (" + (ListSeat.Count - ListStatusSeat.Count).ToString() + "/128)";
            });
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
                            new MessageBoxCustom("Lỗi", "Ghế này đã bán vui lòng chọn ghế khác!", MessageType.Error, MessageButtons.OK).ShowDialog();
                            return;
                        }
                    if (IsExist(p.Content.ToString()))
                    {
                        p.Background = new SolidColorBrush(Colors.Transparent);
                        p.Foreground = new SolidColorBrush(Colors.Black);
                        WaitingSeatList(p);
                        return;
                    }
                    if(WaitingList.Count + 1 > 7)
                    {
                        new MessageBoxCustom("Lỗi", "Bạn chỉ được đặt tối đa 7 ghế!", MessageType.Error, MessageButtons.OK).ShowDialog();
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
            LoadTicketBookingPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                tk.TicketBookingFrame.Content = new TicketBookingPage();
            });
            LoadFoodPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(WaitingList.Count == 0)
                {
                    new MessageBoxCustom("Cảnh báo", "Vui lòng chọn ghế trước khi sang bước tiếp theo", MessageType.Warning, MessageButtons.OK).ShowDialog();
                    return;
                }
                if (OrderFoodPageViewModel.ListOrder != null)
                {
                    OrderFoodPageViewModel.ListOrder.Clear();
                }
                TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                tk.TicketBookingFrame.Content = new FoodPage();
            });
        }
    }
}