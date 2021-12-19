using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.ShowtimeManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.ShowtimeManagementViewModel
{
    public partial class ShowtimeManagementViewModel : BaseViewModel
    {
        private List<SeatSettingDTO> _ListSeat;
        public List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SeatSettingDTO> _ListSeat1;
        public ObservableCollection<SeatSettingDTO> ListSeat1
        {
            get { return _ListSeat1; }
            set { _ListSeat1 = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SeatSettingDTO> _ListSeat2;
        public ObservableCollection<SeatSettingDTO> ListSeat2
        {
            get { return _ListSeat2; }
            set { _ListSeat2 = value; OnPropertyChanged(); }
        }

        private Infor_EditShowtimeWindow _EditShowtimeWindow;
        public Infor_EditShowtimeWindow EditShowtimeWindow
        {
            get { return _EditShowtimeWindow; }
            set { _EditShowtimeWindow = value; }
        }

        private ObservableCollection<ShowtimeDTO> _ListShowtimeofMovie;
        public ObservableCollection<ShowtimeDTO> ListShowtimeofMovie
        {
            get { return _ListShowtimeofMovie; }
            set { _ListShowtimeofMovie = value; OnPropertyChanged(); }
        }

        private int _IsBought;
        public int IsBought
        {
            get { return _IsBought; }
            set { _IsBought = value; OnPropertyChanged(); }
        }
        private int _IsFree;
        public int IsFree
        {
            get { return _IsFree; }
            set { _IsFree = value; OnPropertyChanged(); }
        }





        public ICommand LoadInfor_EditShowtime { get; set; }
        public ICommand CloseEditCM { get; set; }
        public ICommand LoadSeatCM { get; set; }
        public ICommand EditPriceCM { get; set; }


        private ShowtimeDTO _selectedShowtime; //the showtime being selected
        public ShowtimeDTO SelectedShowtime
        {
            get { return _selectedShowtime; }
            set
            {
                _selectedShowtime = value;
                OnPropertyChanged();
            }
        }


        public void Infor_EditFunc()
        {
            if (SelectedItem != null)
            {
                Infor_EditShowtimeWindow p = new Infor_EditShowtimeWindow();
                LoadDataEditWindow(p);
                EditShowtimeWindow = p;
                oldSelectedItem = SelectedItem;
                ShadowMask.Visibility = System.Windows.Visibility.Visible;
                ListSeat1 = new ObservableCollection<SeatSettingDTO>();
                ListSeat2 = new ObservableCollection<SeatSettingDTO>();
                IsFree = IsBought = 0;
                p.ShowDialog();
            }
        }


        public void LoadDataEditWindow(Infor_EditShowtimeWindow p)
        {
            p._movieName.Text = SelectedItem.DisplayName;
            p._ShowtimeDate.Text = SelectedDate.ToString("dd-MM-yyyy");

            if (SelectedRoomId != -1)
                p._ShowtimeRoom.Text = SelectedRoomId.ToString();

            ListShowtimeofMovie = new ObservableCollection<ShowtimeDTO>(SelectedItem.Showtimes);

            moviePrice = 0;
        }
        public async Task GenerateSeat()
        {
            try
            {
                ListSeat = new List<SeatSettingDTO>(await SeatService.Ins.GetSeatsByShowtime(SelectedShowtime.Id));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }

            ListSeat1 = new ObservableCollection<SeatSettingDTO>();
            ListSeat2 = new ObservableCollection<SeatSettingDTO>();
            IsBought = 0;
            IsFree = 0;
            foreach (var item in ListSeat)
            {
                if (item.SeatPosition.Length == 2 && item.SeatPosition[1] < '3')
                {
                    ListSeat2.Add(item);
                }
                else
                {
                    ListSeat1.Add(item);
                }
                if (item.Status)
                    IsBought++;
            }
            IsFree = ListSeat.Count - IsBought;
        }
    }
}
