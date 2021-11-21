using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.QuanLyPhimPage;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementViewModel : BaseViewModel
    {

        public ICommand LoadEditMovieCM { get; set; }


        public void LoadEditMovie(EditMovie w1)
        {
            List<GenreDTO> tempgenre = new List<GenreDTO>(SelectedItem.Genres);

            IsImageChanged = false;
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
                ImageSource = Helper.GetMovieImageSource(SelectedItem.Image);
            }
            else
            {
                w1.imgframe.Source = Helper.GetMovieImageSource("null.jpg");
            }
        }
        public void UpdateMovieFunc(Window p)
        {
            if (movieID != null && IsValidData())
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
                    ReleaseYear = int.Parse(movieYear),
                    RunningTime = int.Parse(movieDuration),
                };

                if (IsImageChanged)
                {
                    imgName = Helper.CreateImageName(movieName);
                    imgfullname = Helper.CreateImageFullName(imgName, extension);
                    movie.Image = imgfullname;
                }
                else
                {
                    filepath = Helper.GetMovieImgPath(SelectedItem.Image);
                    movie.Image = imgfullname = Helper.CreateImageFullName(Helper.CreateImageName(movieName), SelectedItem.Image.Split('.')[1]);
                }

                (bool successUpdateMovie, string messageFromUpdateMovie) = MovieService.Ins.UpdateMovie(movie);

                if (successUpdateMovie)
                {
                    //if (SelectedItem.Image != movie.Image)
                    //{
                    //    File.Delete(Helper.GetMovieImgPath(SelectedItem.Image));
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(filepath))
                    //    {
                    //        File.Copy(filepath, Helper.GetMovieImgPath(movie.Image), true);
                    //    }
                    //}
                    SaveImgToApp();
                    MessageBox.Show(messageFromUpdateMovie);
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
