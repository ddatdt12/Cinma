using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.QuanLyPhimPageVM
{

    public class AddMovieWindowViewModel : BaseViewModel
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
        private string _imgpath;

        public string imgpath
        {
            get { return _imgpath; }
            set { _imgpath = value; }
        }


        public ICommand UploadImageCM { get; set; }


        public AddMovieWindowViewModel()
        {
            InsertCountryToComboBox();
            InsertMovieGerneToComboBox();

            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                string appPath = Path.GetDirectoryName(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "/Resources/Images/Movies/";
                if (Directory.Exists(appPath) == false)
                {
                    Directory.CreateDirectory(appPath);
                }


           

                OpenFileDialog openfile = new OpenFileDialog();

                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png|" + "All |*.*";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string iName = openfile.SafeFileName;
                        string filepath = openfile.FileName;
                        File.Copy(filepath, appPath + iName);
                        
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Unable to open file " + exp.Message);
                    }
                }
                else
                {
                    openfile.Dispose();
                }



            });
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
