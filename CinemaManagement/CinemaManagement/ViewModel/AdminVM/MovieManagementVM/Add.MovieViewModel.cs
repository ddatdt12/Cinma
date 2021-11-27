using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{

    public partial class MovieManagementViewModel : BaseViewModel
    {
        public ICommand LoadAddMovieCM { get; set; }
        public void SaveMovieFunc(Window p)
        {
            if (filepath != null && IsValidData())
            {
                imgName = Helper.CreateImageName(movieName);
                imgfullname = Helper.CreateImageFullName(imgName, extension);
                List<GenreDTO> temp = new List<GenreDTO>();
                temp.Add(movieGenre);


                MovieDTO movie = new MovieDTO
                {
                    DisplayName = movieName,
                    Country = movieCountry,
                    Director = movieDirector,
                    Description = movieDes,
                    Image = imgfullname,
                    Genres = temp,
                    ReleaseYear = int.Parse(movieYear),
                    RunningTime = int.Parse(movieDuration),
                };

                (bool successAddMovie, string messageFromAddMovie, MovieDTO newMovie) = MovieService.Ins.AddMovie(movie);

                if (successAddMovie)
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                    SaveImgToApp();
                    IsAddingMovie = false;
                    LoadMovieListView(Operation.CREATE, newMovie);
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }
    }


}
