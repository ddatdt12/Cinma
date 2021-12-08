using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
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

        public async Task<(string error, VoucherDTO)> GetVoucherInfo(string Code)
        {
            using (var context = new CinemaManagementEntities())
            {
                try
                {
                    var voucher = await context.Vouchers.Where(v => v.Code == Code).Select(v => new VoucherDTO
                    {
                        Id = v.Id,
                        Code = v.Code,
                        Status = v.Status,
                        VoucherReleaseId = v.VoucherReleaseId,
                        UsedAt = v.UsedAt,
                        CustomerName = v.Customer != null ? v.Customer.Name : null,
                        ReleaseAt = v.ReleaseAt,
                        VoucherInfo = new VoucherReleaseDTO
                        {
                            Id = v.VoucherRelease.Id,
                            ReleaseName = v.VoucherRelease.ReleaseName,
                            StartDate = v.VoucherRelease.StartDate,
                            FinishDate = v.VoucherRelease.FinishDate,
                            MinimumOrderValue = v.VoucherRelease.MinimumOrderValue,
                            ParValue = v.VoucherRelease.ParValue,
                            ObjectType = v.VoucherRelease.ObjectType,
                            Status = v.VoucherRelease.Status,
                            EnableMerge = v.VoucherRelease.EnableMerge,
                        }
                    }).FirstOrDefaultAsync();

                    if (voucher is null || !voucher.VoucherInfo.Status || voucher.Status == VOUCHER_STATUS.UNRELEASED)
                    {
                        return ("Mã giảm giá không tồn tại", null);
                    }

                    if (voucher.VoucherInfo.FinishDate < DateTime.Now)
                    {
                        return ("Mã giảm giá đã hết hạn sử dụng", null);
                    }

                    if (voucher.Status == VOUCHER_STATUS.USED)
                    {
                        return ("Mã giảm giá đã sử dụng", null);
                    }

                    voucher.ParValue = voucher.VoucherInfo.ParValue;
                    voucher.ObjectType = voucher.VoucherInfo.ObjectType;
                    voucher.EnableMerge = voucher.VoucherInfo.EnableMerge;

                    voucher.VoucherInfoStr = $"Giảm {String.Format(CultureInfo.InvariantCulture, "{0:#,#}", voucher.ParValue)} đ ({voucher.ObjectType})";

                    return (null, voucher);
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    return ("Mất kết nối cơ sở dữ liệu", null);
                }
                catch (Exception)
                {
                    return ("Lỗi hệ thống", null);
                }
            }
        }
        private string CreateNextVoucherReleaseId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "VCRL0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(4)) + 1}";
            return "VCRL" + newIdString.Substring(newIdString.Length - 4, 4);
        }

        public async Task<List<VoucherReleaseDTO>> GetAllVoucherReleases()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var VrList = await (from vr in context.VoucherReleases
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
                                        }).ToListAsync();
                    return VrList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(VoucherReleaseDTO, bool haveAnyUsedVoucher)> GetVoucherReleaseDetails(string Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var voucherRelease = await context.VoucherReleases.FindAsync(Id);
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
                            ReleaseAt = vR.ReleaseAt,
                        }).ToList()
                    }, haveAnyUsedVoucher);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<VoucherReleaseDTO> GetVoucherReleaseInfo(string Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    //var voucherRelease = await context.VoucherReleases.FindAsync(Id);
                    return await context.VoucherReleases.Where(v => v.Id == Id).Select(vR => new VoucherReleaseDTO
                    {
                        Id = vR.Id,
                        ReleaseName = vR.ReleaseName,
                        StartDate = vR.StartDate,
                        FinishDate = vR.FinishDate,
                        EnableMerge = vR.EnableMerge,
                        MinimumOrderValue = vR.MinimumOrderValue,
                        ParValue = vR.ParValue,
                        ObjectType = vR.ObjectType,
                        Status = vR.Status,
                        StaffId = vR.StaffId,
                        StaffName = vR.Staff.Name,
                        VCount = vR.Vouchers.Count(),
                        UnusedVCount = vR.Vouchers.Count(v => v.Status == VOUCHER_STATUS.UNRELEASED),
                    }).FirstOrDefaultAsync();

                    //return (new VoucherReleaseDTO
                    //{
                    //    Id = voucherRelease.Id,
                    //    ReleaseName = voucherRelease.ReleaseName,
                    //    StartDate = voucherRelease.StartDate,
                    //    FinishDate = voucherRelease.FinishDate,
                    //    EnableMerge = voucherRelease.EnableMerge,
                    //    MinimumOrderValue = voucherRelease.MinimumOrderValue,
                    //    ParValue = voucherRelease.ParValue,
                    //    ObjectType = voucherRelease.ObjectType,
                    //    Status = voucherRelease.Status,
                    //    StaffId = voucherRelease.StaffId,
                    //    StaffName = voucherRelease.Staff.Name,
                    //    VCount = voucherRelease.Vouchers.Count(),
                    //    UnusedVCount = voucherRelease.Vouchers.Count(v => v.Status == VOUCHER_STATUS.UNRELEASED),
                    //});
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string, VoucherReleaseDTO newVR)> CreateVoucherRelease(VoucherReleaseDTO newVR)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    string maxId = await context.VoucherReleases.MaxAsync(vR => vR.Id);
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
                    await context.SaveChangesAsync();

                    newVR.Id = voucherRelease.Id;
                    return (true, "Thêm đợt phát hành mới thành công", newVR);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string)> UpdateVoucherRelease(VoucherReleaseDTO upVoucherR)
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

                    await context.SaveChangesAsync();

                    return (true, "Cập nhật đợt phát hành thành công!");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string, List<VoucherDTO> voucherList)> CreateInputVoucherList(string voucherReleaseId, List<VoucherDTO> ListVoucher)
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
                    await context.SaveChangesAsync();
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
        public async Task<(bool, string, List<VoucherDTO> voucherList)> CreateRandomVoucherList(string voucherReleaseId, List<string> ListCode)
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
                    await context.SaveChangesAsync();
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

        public async Task<(bool, string)> DeteleVouchers(List<int> ListCodeId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    context.Vouchers.RemoveRange(context.Vouchers.Where(v => ListCodeId.Contains(v.Id)));
                    await context.SaveChangesAsync();
                    return (true, "Xóa danh sách voucher thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<(bool, string)> DeteleVoucherRelease(string VrId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    context.VoucherReleases.Remove(context.VoucherReleases.Find(VrId));
                    await context.SaveChangesAsync();
                    return (true, "Xóa đợt phát hành thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> ReleaseMultiVoucher(List<int> ListCodeId)
        {
            try
            {
                string idList = string.Join(",", ListCodeId);
                using (var context = new CinemaManagementEntities())
                {
                    var sql = $@"Update [Voucher] SET Status = '{VOUCHER_STATUS.REALEASED}', ReleaseAt = GETDATE()  WHERE Id IN ({idList})";
                    await context.Database.ExecuteSqlCommandAsync(sql);
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
