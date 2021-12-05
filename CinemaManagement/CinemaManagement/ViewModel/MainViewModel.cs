using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private string _enteredGenreName;
        public string EnteredGenreName
        {
            get { return _enteredGenreName; }
            set { _enteredGenreName = value; OnPropertyChanged(); }
        }
        private ObservableCollection<GenreDTO> _genreList;
        public ObservableCollection<GenreDTO> GenreList
        {
            get => _genreList;
            set
            {
                _genreList = value;
                OnPropertyChanged();
            }
        }

        private GenreDTO _selectedGenre;

        public GenreDTO SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; OnPropertyChanged(); }
        }

        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
        }
        public ICommand AddGenre { get; set; }
        public ICommand EditGenre { get; set; }
        public ICommand DeleteGenre { get; set; }
        public MainViewModel()
        {
            SelectedGenre = null;
            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            List<GenreDTO> genreDTOs = new List<GenreDTO>();
            //List<StaffDTO> staffDTOs;

            try
            {
                

            }
            catch (InvalidOperationException e)
            {
            }
            catch (Exception e)
            {
                MessageBox.Show($"Mất kết nối cơ sở dữ liệu! Vui lòng kiểm tra lại", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            EditGenre = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedGenre == null)
                        return false;
                    return true;
                },
                (p) => EditGenreCommandHandler());
            AddGenre = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) => AddGenreCommandHandler());
        }
        private void AddGenreCommandHandler()
        {
            //(VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = await VoucherService.Ins.GetVoucherReleaseDetails("VCRL0001");
            //GenreDTO genre = new GenreDTO { DisplayName = EnteredGenreName };
            //(bool isSuccess, string message) = GenreService.Ins.AddGenre(genre);
            //if (isSuccess)
            //{
            //    LoadGenreList();
            //}
            //else
            //{
            //    MessageBox.Show(message);
            //}
        }

        private void EditGenreCommandHandler()
        {
            if (string.IsNullOrEmpty(EnteredGenreName))
            {
                MessageBox.Show("Tên thể loại phim mới không thể để trống");
                return;
            }
            (bool isSuccess, string message) = GenreService.Ins.EditGenre(SelectedGenre.Id, EnteredGenreName);
            if (isSuccess)
            {
                LoadGenreList();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void LoadGenreList()
        {
            GenreList = new ObservableCollection<GenreDTO>(GenreService.Ins.GetAllGenre());
        }
    }
}

