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
    public class LEVEL
    {
        public static readonly string NORMAL = "Bình thường";
        public static readonly string CRITICAL = "Nghiêm trọng";
    }
    public class STATUS
    {
        public static readonly string WAITING = "Chờ tiếp nhận";
        public static readonly string IN_PROGRESS = "Đang giải quyết";
        public static readonly string DONE = "Đã giải quyết";
        public static readonly string CANCLE = "Đã hủy";
    }
    public class VOUCHER_OBJECT_TYPE
    {
        public static readonly string PRODUCT = "Sản phẩm";
        public static readonly string TICKET = "Vé xem phim";
        public static readonly string ALL = "Toàn bộ";
    }
    public class VOUCHER_STATUS
    {
        public static readonly string REALEASED = "Đã phát hành";
        public static readonly string UNRELEASED = "Chưa phát hành";
        public static readonly string USED = "Đã sử dụng";
    }
}
