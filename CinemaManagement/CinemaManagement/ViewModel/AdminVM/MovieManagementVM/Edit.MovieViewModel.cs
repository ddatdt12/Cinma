using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementViewModel : BaseViewModel
    {

        public ICommand LoadEditMovieCM { get; set; }
     

        public void LoadEditMovie(EditMovie w1)
        {
            List<GenreDTO> tempgenre = new List<GenreDTO>(SelectedItem.Genres);
            DateTime tempdate = (DateTime)SelectedItem.ReleaseDate;


            imgfullname = SelectedItem.Image;
            movieID = SelectedItem.Id.ToString();
            movieName = SelectedItem.DisplayName;
            movieGenre = tempgenre[0];
            movieYear = tempdate.ToShortDateString(); ;
            movieDirector = SelectedItem.Director;
            movieCountry = SelectedItem.Country;
            movieDuration = SelectedItem.RunningTime.ToString();
            movieDes = SelectedItem.Description;
            w1._Genre.Text = tempgenre[0].DisplayName;

            if (SelectedItem.Image != null )
            {
                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(Helper.GetMovieImgPath(SelectedItem.Image));
                _image.EndInit();

                w1.imgframe.Source = _image;
            }
            else
            {
                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(Helper.GetMovieImgPath("null.jpg"));
                _image.EndInit();

                w1.imgframe.Source = _image;
            }
        }
        public void UpdateMovieFunc(Window p)
        {
            if (movieID != null && movieName != null && movieCountry != null && movieDirector != null && movieDes != null && movieGenre != null && movieYear != null && movieDuration != null)
            {


                List<GenreDTO> temp = new List<GenreDTO>();
                temp.Add(movieGenre);

                MovieDTO movie = new MovieDTO
                {
                    Id = int.Parse(movieID),
                    DisplayName = movieName,
                    Country = movieCountry,
                    Director = movieDirector,
                    Description = movieDes,
                    Genres = temp,
                    ReleaseDate = DateTime.Today,
                    RunningTime = int.Parse(movieDuration),
                };

                if (IsImageChanged)
                {
                    movie.Image = imgfullname;
                }
                else
                {
                    movie.Image = Helper.CreateImageFullName(movieName, SelectedItem.Image.Split('.')[1]);
                }


                (bool successUpdateMovie, string messageFromUpdateMovie) = MovieService.Ins.UpdateMovie(movie);

                if (successUpdateMovie)
                {
                    System.Windows.MessageBox.Show(messageFromUpdateMovie);
                    if (SelectedItem.Image != movie.Image)
                    {
                        File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                        SaveImgToApp();
                    }
                    else
                    {
                        filepath = Helper.GetMovieImgPath(SelectedItem.Image);
                        File.Copy(filepath, Helper.GetMovieImgPath(movie.Image));
                        File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));

                    }
                    p.Close();
                    ReloadMovieListView();
                }
                else
                {
                    System.Windows.MessageBox.Show(messageFromUpdateMovie);
                }
            }
        }

    }
}
