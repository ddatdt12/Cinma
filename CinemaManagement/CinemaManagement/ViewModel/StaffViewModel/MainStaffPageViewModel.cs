using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.ViewModel.StaffViewModel
{
    public class MainStaffPageViewModel:BaseViewModel
    {

        private string source_Image1;
        private string textBlock_Image1_1;
        private string textBlock_Image1_2;
        private string source_Image2;
        private string textBlock_Image2_1;
        private string textBlock_Image2_2;
        private string source_Image3;
        private string textBlock_Image3_1;
        private string textBlock_Image3_2;
        private string source_Image4;
        private string textBlock_Image4_1;
        private string textBlock_Image4_2;
        /* source là hình ảnh, textblock1 là tên phim, textblock2 là năm sản xuất*/
        public string Source_Image1 { get => source_Image1; set => source_Image1 = value; }
        public string TextBlock_Image1_1 { get => textBlock_Image1_1; set => textBlock_Image1_1 = value; }
        public string TextBlock_Image1_2 { get => textBlock_Image1_2; set => textBlock_Image1_2 = value; }
        public string Source_Image2 { get => source_Image2; set => source_Image2 = value; }
        public string TextBlock_Image2_1 { get => textBlock_Image2_1; set => textBlock_Image2_1 = value; }
        public string TextBlock_Image2_2 { get => textBlock_Image2_2; set => textBlock_Image2_2 = value; }
        public string Source_Image3 { get => source_Image3; set => source_Image3 = value; }
        public string TextBlock_Image3_1 { get => textBlock_Image3_1; set => textBlock_Image3_1 = value; }
        public string TextBlock_Image3_2 { get => textBlock_Image3_2; set => textBlock_Image3_2 = value; }
        public string Source_Image4 { get => source_Image4; set => source_Image4 = value; }
        public string TextBlock_Image4_1 { get => textBlock_Image4_1; set => textBlock_Image4_1 = value; }
        public string TextBlock_Image4_2 { get => textBlock_Image4_2; set => textBlock_Image4_2 = value; }
        public MainStaffPageViewModel()
        {
            Source_Image1 = "/Resources/LayoutSignin/conan_movie_24.jpg";
            TextBlock_Image1_1 = "Tham tu lung danh conan";
            TextBlock_Image1_2 = "2020";
            Source_Image2 = "/Resources/LayoutSignin/bo-gia.jpg";
            TextBlock_Image2_1 = "Bố già";
            TextBlock_Image2_2 = "2021";
            Source_Image3 = "/Resources/LayoutSignin/godzilla_vs_Kong.jpg";
            TextBlock_Image3_1 = "Godzilla vs Kong";
            TextBlock_Image3_2 = "2021";
            Source_Image4 = "/Resources/LayoutSignin/Mat_biec.jpg";
            TextBlock_Image4_1 = "Mắt biếc";
            TextBlock_Image4_2 = "2019";
        }
    }
}
