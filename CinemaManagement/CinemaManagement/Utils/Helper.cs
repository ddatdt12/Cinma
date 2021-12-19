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

        public static string GetImagePath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images", $"{imageName}" /*SelectedItem.Image*/);
        }

        public static string GetEmailTemplatePath(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\EmailTemplate", $"{fileName}" /*SelectedItem.Image*/);
        }

        public static string FormatVNMoney(decimal money)
        {
            if (money == 0)
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
