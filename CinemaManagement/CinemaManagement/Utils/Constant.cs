using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Utils
{
    public class TIME
    {
        public static readonly TimeSpan BreakTime = new TimeSpan(0, 15, 0);
    }
    public class ROLE
    {
        public static readonly string Admin = "Quản lý";
        public static readonly string Staff = "Nhân viên";
    }
    public class SOURCE
    {
        public static readonly string ProductsSource= "/CinemaManagement;component/Resouces/Products";
        public static readonly string MoviesSource = "/CinemaManagement;component/Resouces/Movies";
    }

}
