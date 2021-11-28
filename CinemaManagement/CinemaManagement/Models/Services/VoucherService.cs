using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private string CreateNextVoucherReleaseId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "VCRL0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "VCRL" + newIdString.Substring(newIdString.Length - 4, 4);
        }
        public List<VoucherReleaseDTO> GetAllVoucherReleases()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var VrList = (from vr in context.VoucherReleases
                                  orderby vr.Id descending
                                  select new VoucherReleaseDTO
                                  {
                                      Id = vr.Id,
                                      ReleaseName = vr.ReleaseName,
                                      StartDate = vr.StartDate,
                                      FinishDate = vr.FinishDate,
                                      MinimumOrderValue = vr.MinimumOrderValue,
                                      ParValue = vr.ParValue,
                                      ObjectType = vr.ObjectType,
                                      Status = vr.Status,
                                      StaffId = vr.StaffId,
                                      StaffName = vr.Staff.Name,
                                      VCount = vr.Vouchers.Count(),
                                      UnusedVCount = vr.Vouchers.Count(v => v.Status == VOUCHER_STATUS.UNRELEASED),
                                  }).ToList();
                    return VrList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public (VoucherReleaseDTO, bool haveAnyUsedVoucher) GetVoucherReleaseDetails(string Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var voucherRelease = context.VoucherReleases.Find(Id);
                    bool haveAnyUsedVoucher = voucherRelease.Vouchers.Any(v => v.Status == VOUCHER_STATUS.USED);
                    return (new VoucherReleaseDTO
                    {
                        Id = voucherRelease.Id,
                        ReleaseName = voucherRelease.ReleaseName,
                        StartDate = voucherRelease.StartDate,
                        FinishDate = voucherRelease.FinishDate,
                        EnableMerge = voucherRelease.EnableMerge,
                        MinimumOrderValue = voucherRelease.MinimumOrderValue,
                        ParValue = voucherRelease.ParValue,
                        ObjectType = voucherRelease.ObjectType,
                        Status = voucherRelease.Status,
                        StaffId = voucherRelease.StaffId,
                        StaffName = voucherRelease.Staff.Name,
                        VCount = voucherRelease.Vouchers.Count(),
                        UnusedVCount = voucherRelease.Vouchers.Count(v => v.Status == VOUCHER_STATUS.UNRELEASED),
                        Vouchers = voucherRelease.Vouchers.Select(vR => new VoucherDTO
                        {
                            Id = vR.Id,
                            Code = vR.Code,
                            Status = vR.Status,
                            VoucherReleaseId = vR.VoucherReleaseId,
                            UsedAt = vR.UsedAt,
                            CustomerName = vR.Customer?.Name,
                        }).ToList()
                    }, haveAnyUsedVoucher);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public (bool, string, VoucherReleaseDTO newVR) CreateVoucherRelease(VoucherReleaseDTO newVR)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    string maxId = context.VoucherReleases.Max(vR => vR.Id);
                    VoucherRelease voucherRelease = new VoucherRelease
                    {
                        Id = CreateNextVoucherReleaseId(maxId),
                        ReleaseName = newVR.ReleaseName,
                        StartDate = newVR.StartDate,
                        FinishDate = newVR.FinishDate,
                        EnableMerge = newVR.EnableMerge,
                        MinimumOrderValue = newVR.MinimumOrderValue,
                        ParValue = newVR.ParValue,
                        ObjectType = newVR.ObjectType,
                        Status = newVR.Status,
                        StaffId = newVR.StaffId
                    };

                    context.VoucherReleases.Add(voucherRelease);
                    context.SaveChanges();

                    newVR.Id = voucherRelease.Id;
                    return (true, "Thêm đợt phát hành mới thành công", newVR);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public (bool, string) UpdateVoucherRelease(VoucherReleaseDTO upVoucherR)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {

                    var voucherRelease = context.VoucherReleases.Find(upVoucherR.Id);

                    voucherRelease.ReleaseName = upVoucherR.ReleaseName;
                    voucherRelease.StartDate = upVoucherR.StartDate;
                    voucherRelease.FinishDate = upVoucherR.FinishDate;
                    voucherRelease.EnableMerge = upVoucherR.EnableMerge;
                    voucherRelease.MinimumOrderValue = upVoucherR.MinimumOrderValue;
                    voucherRelease.ParValue = upVoucherR.ParValue;
                    voucherRelease.ObjectType = upVoucherR.ObjectType;
                    voucherRelease.Status = upVoucherR.Status;
                    voucherRelease.StaffId = upVoucherR.StaffId;

                    context.SaveChanges();

                    return (true, "Cập nhật đợt phát hành thành công!");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public (bool, string, List<VoucherDTO> voucherList) CreateInputVoucherList(string voucherReleaseId, List<VoucherDTO> ListVoucher)
        {
            try
            {
                List<string> ListCode = ListVoucher.Select(v => v.Code).ToList();
                using (var context = new CinemaManagementEntities())
                {
                    var IsExist = context.Vouchers.Any(v => ListCode.Contains(v.Code));
                    if (IsExist)
                    {
                        return (false, "Danh sách voucher đã tồn tại mã bạn vừa nhập!", null);
                    }

                    List<Voucher> vouchers = ListCode.Select(c => new Voucher
                    {
                        Code = c,
                        VoucherReleaseId = voucherReleaseId,
                        Status = VOUCHER_STATUS.UNRELEASED,
                    }).ToList();

                    context.Vouchers.AddRange(vouchers);
                    context.SaveChanges();
                    return (true, "Thêm danh sách voucher thành công", vouchers.Select(v => new VoucherDTO
                    {
                        VoucherReleaseId = v.VoucherReleaseId,
                        Id = v.Id,
                        Code = v.Code,
                        CustomerName = v.Customer?.Name,
                        Status = VOUCHER_STATUS.UNRELEASED,
                        UsedAt = v.UsedAt,
                    }).ToList());
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public (bool, string, List<VoucherDTO> voucherList) CreateRandomVoucherList(string voucherReleaseId, List<string> ListCode)
        {
            try
            {
                List<Voucher> vouchers = ListCode.Select(c => new Voucher
                {
                    Code = c,
                    VoucherReleaseId = voucherReleaseId,
                    Status = VOUCHER_STATUS.UNRELEASED,
                }).ToList();
                using (var context = new CinemaManagementEntities())
                {
                    context.Vouchers.AddRange(vouchers);
                    context.SaveChanges();
                    return (true, "Thêm danh sách voucher thành công", vouchers.Select(v => new VoucherDTO
                    {
                        VoucherReleaseId = v.VoucherReleaseId,
                        Id = v.Id,
                        Code = v.Code,
                        Status = v.Status,
                    }).ToList());
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public (bool, string) DeteleVouchers(List<int> ListCodeId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    context.Vouchers.RemoveRange(context.Vouchers.Where(v => ListCodeId.Contains(v.Id)));
                    context.SaveChanges();
                    return (true, "Xóa danh sách voucher thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public (bool, string) DeteleVoucherRelease(string VrId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    context.VoucherReleases.Remove(context.VoucherReleases.Find(VrId));
                    context.SaveChanges();
                    return (true, "Xóa đợt phát hành thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public (bool, string) ReleaseMultiVoucher(List<int> ListCodeId)
        {
            try
            {
                string idList = string.Join(",", ListCodeId);
                using (var context = new CinemaManagementEntities())
                {
                    var sql = $@"Update [Voucher] SET Status = '{VOUCHER_STATUS.REALEASED}', ReleaseAt = GETDATE()  WHERE Id IN ({idList})";
                    context.Database.ExecuteSqlCommand(sql);
                }
                return (true, "Phát hành thành công");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
    }
}
