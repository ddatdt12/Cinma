using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.Utils
{
    public class Helper
    {

        public static (string, List<string>) GetListCode(int quantity, int length, string firstChars, string lastChars)
        {
            List<string> ListCode = new List<string>();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            int randomLength = length - firstChars.Length - lastChars.Length;
            if (randomLength <= 0)
            {
                return ("Độ dài của voucher phải lớn hơn độ dài chuỗi kí tự đầu + độ dài chuỗi kí tự cuối", null);
            }
            if (randomLength < 4)
            {
                return ($"Độ dài của voucher phải lớn hơn độ dài chuỗi kí tự đầu + độ dài chuỗi kí tự cuối + 4 ", null);
            }
            for (int i = 0; i < quantity; i++)
            {

                var stringChars = new char[randomLength];
                for (int j = 0; j < stringChars.Length; j++)
                {
                    stringChars[j] = chars[random.Next(chars.Length)];
                }
                string newCode = new String(stringChars);
                var isExist = ListCode.Any(code => code == newCode);
                if (isExist)
                {
                    i--;
                    continue;
                }
                ListCode.Add(firstChars + newCode + lastChars);
            }
           
            return (null, ListCode);
        }
        public static string ConvertDoubleToPercentageStr(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("P", CultureInfo.InvariantCulture);
        }
        public static string MD5Hash(string str)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("X2"));
            }
            return hash.ToString();
        }
        public static bool IsPhoneNumber(string number)
        {
            if (number is null) return false;
            return Regex.Match(number, @"(([03+[2-9]|05+[6|8|9]|07+[0|6|7|8|9]|08+[1-9]|09+[1-4|6-9]]){3})+[0-9]{7}\b").Success;
        }
        public static string GetHourMinutes(TimeSpan t)
        {
            return t.ToString(@"hh\:mm");
        }
        public static string CreateImageName(string imageName)
        {
            imageName = RemoveUnicode(imageName).Replace(@"\", string.Empty);

            Regex reg = new Regex("[*'\",_&#^@:|<>?/]");
            imageName = reg.Replace(imageName, string.Empty);

            return String.Join("_", imageName.Split(' ')).ToLower();
        }
        public static string CreateImageFullName(string imageName, string ext)
        {
            return $"{imageName}.{ext}";
        }

        public static ImageSource GetMovieImageSource(string imageName)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(GetMovieImgPath(imageName));
            _image.EndInit();
            return _image;
        }
        public static ImageSource GetProductImageSource(string imageName)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(GetProductImgPath(imageName));
            _image.EndInit();
            return _image;
        }

        public static ImageSource GetTroubleImageSource(string imageName)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(GetTroubleImgPath(imageName));
            _image.EndInit();
            return _image;
        }
        public static string GetImagePath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images", $"{imageName}" /*SelectedItem.Image*/);
        }
        public static string GetMovieImgPath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images\Movies", $"{imageName}" /*SelectedItem.Image*/);
        }
        public static string GetTroubleImgPath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images\Troubles", $"{imageName}" /*SelectedItem.Image*/);
        }

        public static string GetAdminPath(string filename)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Admin", $"{filename}" /*SelectedItem.Image*/);
        }
  

        public static string GetProductImgPath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images\Products", $"{imageName}" /*SelectedItem.Image*/);
        }

        public static string GetEmailTemplatePath(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\EmailTemplate", $"{fileName}" /*SelectedItem.Image*/);
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
        public static string FormatVNMoney(decimal money)
        {
            if (money == 0 )
            {
                return "0 ₫";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} ₫", money);
        }
        public static string FormatDecimal(decimal n)
        {
            if (n == 0)
            {
                return "0";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#}", n);
        }
    }
}
