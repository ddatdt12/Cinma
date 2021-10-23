using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<MovieDTO> movieDTOs;
            List<GenreDTO> genreDTOs;
            List<StaffDTO> staffDTOs;

            try
            {
                movieDTOs = MovieService.Ins.GetAllMovie();
                genreDTOs = GenreService.Ins.GetAllGenre();
                staffDTOs = StaffService.Ins.GetAllStaff();

                #region Staff Service
                //StaffDTO staff = new StaffDTO
                //{
                //    Name = "Test",
                //    Username = "test1234",
                //    Password = "123456",
                //    Age = 22,
                //    Gender = "Nam",
                //    PhoneNumber = "098312731",
                //    BirthDate = new DateTime(2002, 05, 12),
                //    StartingDate = DateTime.Today
                //};
                //Add new staff
                //(bool addStaffSuccess, string messageFromAddStaff) = StaffService.Ins.AddStaff(staff);

                //Test update Staff
                //staffDTOs.Last().Name = "Test after update";
                //staffDTOs.Last().Age = 25;

                //(bool updateStaffSuccess, string messageFromUpdateStaff) = StaffService.Ins.UpdateStaff(staffDTOs.Last());

                //(bool deleteSuccess, string messageFromDeleteStaff) = StaffService.Ins.DeleteStaff(staffDTOs.Last().Id);

                //staffDTOs = StaffService.Ins.GetAllStaff();
                #endregion


                #region Movie Service
                //Test Update movie
                //movieDTOs[0].DisplayName = "BỐ GIÀ";
                //movieDTOs[0].Director = "Vũ Ngọc Đãng & Trấn Thành";
                //(bool updateSuccess, string messageFromUpdateMovie) = MovieService.Ins.UpdateMovie(movieDTOs[0]);

                //Test create new movie
                //MovieDTO newMovie = new MovieDTO
                //{
                //    DisplayName = "Test",
                //    Country = "Việt Nam",
                //    Description = "Test creating new Movie",
                //    ReleaseDate = DateTime.Now,
                //    RunningTime = 169,
                //    Director = "Đạt ĐT",
                //    Genres = new List<GenreDTO>
                //    {
                //        genreDTOs[2],
                //        genreDTOs[3]
                //    }
                //};
                //(bool addSuccess, string messageFromAddMovie) = MovieService.Ins.AddMovie(newMovie);

                //(bool deleteSuccess, string messageFromDeleteMovie) = MovieService.Ins.DeleteMovie(movieDTOs.Last().Id);

                //Test Delete Movie
                //(bool deleteSuccess, string messageFromDeleteMovie) = MovieService.Ins.DeleteMovie(movieDTOs.Last().Id);
                //movieDTOs = MovieService.Ins.GetAllMovie();
                #endregion


                //(bool loginSuccess, string message, StaffDTO staff) = StaffService.Ins.Login("dothanhdat123","123456");
                MovieList = new List<MovieDTO>(movieDTOs);
                GenreList = new ObservableCollection<GenreDTO>(genreDTOs);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                //MessageBox.Show($"InvalidOperationException : {e.Message}");

            }
            catch (EntityException e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Mất kết nối cơ sở dữ liệu! Vui lòng kiểm tra lại");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Lỗi hệ thống!");
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
            GenreDTO genre = new GenreDTO { DisplayName = EnteredGenreName };
            (bool isSuccess, string message) = GenreService.Ins.AddGenre(genre);
            if (isSuccess)
            {
                LoadGenreList();
            }
            else
            {
                MessageBox.Show(message);
            }
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

