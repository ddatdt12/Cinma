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

            imgfullname = SelectedItem.Image;
            movieID = SelectedItem.Id.ToString();
            movieName = SelectedItem.DisplayName;
            movieGenre = tempgenre[0];
            movieYear = SelectedItem.ReleaseYear.ToString(); ;
            movieDirector = SelectedItem.Director;
            movieCountry = SelectedItem.Country;
            movieDuration = SelectedItem.RunningTime.ToString();
            movieDes = SelectedItem.Description;
            w1._Genre.Text = tempgenre[0].DisplayName;

            if (File.Exists(Helper.GetMovieImgPath(SelectedItem.Image)) == true)
            {
                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(Helper.GetMovieImgPath(SelectedItem.Image));
                _image.EndInit();

                ImageSource = _image;
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
            if (movieID != null && IsValidData())
            {
                imgName = Helper.CreateImageName(movieName);
                imgfullname = Helper.CreateImageFullName(imgName, extension);

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
                    ReleaseYear = int.Parse(movieYear),
                    RunningTime = int.Parse(movieDuration),
                };

                if (IsImageChanged)
                {
                    movie.Image = imgfullname;
                }
                else
                {
                    filepath = Helper.GetMovieImgPath(SelectedItem.Image);
                    movie.Image = imgfullname = Helper.CreateImageFullName(movieName, SelectedItem.Image.Split('.')[1]);
                }

                (bool successUpdateMovie, string messageFromUpdateMovie) = MovieService.Ins.UpdateMovie(movie);

                if (successUpdateMovie)
                {
                    MessageBox.Show(messageFromUpdateMovie);

                    if (SelectedItem.Image != movie.Image)
                    {
                        SaveImgToApp();
                        File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                    }
                    else
                    {
                        filepath = Helper.GetMovieImgPath(SelectedItem.Image);
                        File.Copy(filepath, Helper.GetMovieImgPath(movie.Image));
                        File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                    }

                    LoadMovieListView(Operation.UPDATE, movie);
                    p.Close();
                }
                else
                {
                    MessageBox.Show(messageFromUpdateMovie);
                }
            }
            else
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
        }

    }
}
