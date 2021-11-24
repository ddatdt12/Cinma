using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class VoucherService
    {
        private static VoucherService _ins;
        public static VoucherService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new VoucherService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private VoucherService()
        {
        }

        public List<VoucherReleaseDTO> GetAllVoucherReleases(VoucherReleaseDTO newVR)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var VrList = (from vr in context.VoucherReleases
                             select new VoucherReleaseDTO
                             {
                                 ReleaseName = vr.ReleaseName,
                                 StartDate = vr.StartDate,
                                 FinishDate = vr.FinishDate,
                                 EnableMerge = vr.EnableMerge,
                                 MaximumDiscount = vr.MaximumDiscount,
                                 MinimumOrderValue = vr.MinimumOrderValue,
                                 DiscountType = vr.DiscountType,
                                 ParValue = vr.ParValue,
                                 ObjectType = vr.ObjectType,
                                 Status = vr.Status,
                                 StaffId = vr.StaffId,
                                 StaffName = vr.Staff.Name
                             }).ToList();

                return VrList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public (bool, string, string NewVrId) CreateVoucherRelease(VoucherReleaseDTO newVR)
        {
            try
            {
                var context = DataProvider.Ins.DB;
                VoucherRelease voucherRelease = new VoucherRelease
                {
                    ReleaseName = newVR.ReleaseName,
                    StartDate = newVR.StartDate,
                    FinishDate = newVR.FinishDate,
                    EnableMerge = newVR.EnableMerge,
                    MaximumDiscount = newVR.MaximumDiscount,
                    MinimumOrderValue = newVR.MinimumOrderValue,
                    DiscountType = newVR.DiscountType,
                    ParValue = newVR.ParValue,
                    ObjectType = newVR.ObjectType,
                    Status = newVR.Status,
                    StaffId = newVR.StaffId
                };

                context.VoucherReleases.Add(voucherRelease);
                context.SaveChanges();
                return (true, "Thêm đợt phát hành mới thành công", voucherRelease.Id);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

    }
}
