using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using CinemaManagement.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.MovieManagementVM
{

    public partial class MovieManagementViewModel : BaseViewModel
    {
        public ICommand LoadAddMovieCM { get; set; }
        public async Task SaveMovieFunc(Window p)
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

                (bool successAddMovie, string messageFromAddMovie, MovieDTO newMovie) = await MovieService.Ins.AddMovie(movie);

                if (successAddMovie)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", messageFromAddMovie, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    SaveImgToApp();
                    IsAddingMovie = false;
                    LoadMovieListView(Operation.CREATE, newMovie);
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Lỗi", messageFromAddMovie, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Vui lòng nhập đủ thông tin!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
    }


}
