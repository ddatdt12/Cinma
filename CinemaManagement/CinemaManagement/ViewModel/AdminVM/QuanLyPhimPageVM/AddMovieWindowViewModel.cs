using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagement.DTOs;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{
  
    public class AddMovieWindowViewModel: BaseViewModel
    {
        private List<string> _ListCountrySource = new List<string>();
        public List<string> ListCountrySource
        {
            get { return _ListCountrySource; }
            set { _ListCountrySource.Add(value.ToString()); OnPropertyChanged(); }
        }

        private List<string> _ListMovieGenreSource = new List<string>();
        public List<string> ListMovieGenreSource
        {
            get { return _ListMovieGenreSource; }
            set { _ListMovieGenreSource.Add(value.ToString()); OnPropertyChanged(); }
        }



        public AddMovieWindowViewModel()
        {
            InsertCountryToComboBox();
            InsertMovieGerneToComboBox();
        }


        public void InsertCountryToComboBox()
        {
            FileStream file = new FileStream(@"CountrySource.txt", FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    ListCountrySource.Add(reader.ReadLine());
                }
            }
        }
        public void InsertMovieGerneToComboBox()
        {
            FileStream file = new FileStream(@"MovieGenreSource.txt", FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(file, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    ListMovieGenreSource.Add(reader.ReadLine());
                }
            }
        }
    }
}
