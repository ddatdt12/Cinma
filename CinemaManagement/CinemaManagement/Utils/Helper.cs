using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.Utils
{
    public class Helper
    {
        public static string GetHourMinutes(TimeSpan t)
        {
            return t.ToString(@"hh\:mm");
        }
        public static string CreateImageName(string imageName)
        {
            imageName = RemoveUnicode(imageName);
            return String.Join("_", imageName.Split(' ')).ToLower();
        }
        public static string CreateImageFullName(string imageName, string ext)
        {
            return $"{imageName}.{ext}";
        }

        public static ImageSource GetProductSource(string fileName)
        {
            return new BitmapImage(new Uri($@"{SOURCE.ProductsSource}/{fileName}", UriKind.Relative));
        }
        public static ImageSource GetMovieSource(string fileName)
        {
            return new BitmapImage(new Uri($@"{SOURCE.MoviesSource}/{fileName}", UriKind.Relative));
        }

        public static string GetMovieImgPath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images\Movies", $"{imageName}" /*SelectedItem.Image*/);
        }
        public static string GetAdminPath(string filename)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Admin", $"{filename}" /*SelectedItem.Image*/);
        }

        public static string GetProductImgPath()
        {
            string appPath = Path.GetDirectoryName(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "/Resources/Images/Products/";
            if (Directory.Exists(appPath) == false)
            {
                Directory.CreateDirectory(appPath);
            }
            return appPath;
        }

        private static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }


    }
}
