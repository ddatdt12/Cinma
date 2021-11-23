using System;

namespace CinemaManagement.Utils
{
    public enum Operation
    {
        CREATE,
        READ,
        UPDATE,
        DELETE,
        UPDATE_PASSWORD,
        UPDATE_PROD_QUANTITY
    }
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
        public static readonly string ProductsSource = "/CinemaManagement;component/Resouces/Products";
        public static readonly string MoviesSource = "/CinemaManagement;component/Resouces/Movies";
    }
    public class LEVEL
    {
        //LEVEL
        public static readonly string NORMAL = "Bình thường";
        public static readonly string CRITICAL = "Nghiêm trọng";
    }
    public class STATUS
    {
        // STATUS
        public static readonly string WAITING = "Chờ tiếp nhận";
        public static readonly string IN_PROGRESS = "Đang giải quyết";
        public static readonly string DONE = "Đã giải quyết";
        public static readonly string CANCLE = "Đã hủy";
    }

}
