using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaManagement.DTOs
{
    public class VoucherDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public VoucherDTO()
        {
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string VoucherReleaseId { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        //Use for get voucher info
        public int ParValue { get; set; }
        public string VoucherInfoStr { get; set; }
        public bool EnableMerge { get; set; }
        public string ObjectType { get; set; }

        public  VoucherReleaseDTO VoucherInfo;

        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; OnPropertyChanged(); }
        }
        public Nullable<System.DateTime> UsedAt { get; set; }
        public Nullable<System.DateTime> ReleaseAt { get; set; }
    }
}
