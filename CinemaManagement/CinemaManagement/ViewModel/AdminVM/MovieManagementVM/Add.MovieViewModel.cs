﻿using CinemaManagement.DTOs;
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

    public partial class MovieManagementViewModel : BaseViewModel
    {
        public ICommand LoadAddMovieCM { get; set; }
        public void SaveMovieFunc(Window p)
        {
            if (movieID == null && filepath != null && IsValidData())
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
                    IsAddingMovie = false;
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                    SaveImgToApp();
                    LoadMovieListView(Operation.CREATE, newMovie);
                    p.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show(messageFromAddMovie);
                }
            }
        }
    }


}
