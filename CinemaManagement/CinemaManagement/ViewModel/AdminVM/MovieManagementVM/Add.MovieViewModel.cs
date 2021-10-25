using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{

    public partial class MovieManagementViewModel  : BaseViewModel
    {
        public ICommand LoadAddMovieCM { get; set; }


        public void SaveMovieFunc(Window p)
        {
            
            if (movieID == null && movieName != null && movieCountry != null && movieDirector != null && movieDes != null && filepath != null && movieGenre != null && movieYear != null && movieDuration != null)
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
                    ReleaseDate = DateTime.Today,
                    RunningTime = int.Parse(movieDuration),
                };

                (bool successAddMovie, string messageFromAddMovie) = MovieService.Ins.AddMovie(movie);

                if (successAddMovie)
                {
                    IsAddingMovie = false;
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                    SaveImgToApp();
                    p.Close();
                    ReloadMovieListView();
                }
                else
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                }
            }
        }
    }


}
