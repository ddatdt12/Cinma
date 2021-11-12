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
                ImageSource = Helper.GetImageSource(SelectedItem.Image);
            }
            else
            {
                w1.imgframe.Source = Helper.GetImageSource("null.jpg");
            }
        }
        public void UpdateMovieFunc(Window p)
        {
            if (movieID != null && IsValidData())
            {
                if (!IsImageChanged)
                    extension = SelectedItem.Image.Split('.')[1];
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

                if (movie.Image != SelectedItem.Image)
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
                        File.Copy(filepath, Helper.GetMovieImgPath(movie.Image), true);
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
