using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            try
            {
                movieDTOs = MovieService.Ins.GetAllMovie();
                genreDTOs = GenreService.Ins.GetAllGenre();
                MovieList = new List<MovieDTO>(movieDTOs);
                GenreList = new ObservableCollection<GenreDTO>(genreDTOs);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
            if (string.IsNullOrEmpty(EnteredGenreName)){
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

