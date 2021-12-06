using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class StaffService
    {
        private StaffService() { }
        private static StaffService _ins;
        public static StaffService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StaffService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        public async Task<List<StaffDTO>> GetAllStaff()
        {
            using (var context = new CinemaManagementEntities())
            {
                var staffs = (from s in context.Staffs
                          where s.IsDeleted == false
                          select new StaffDTO
                          {
                              Id = s.Id,
                              BirthDate = s.BirthDate,
                              Gender = s.Gender,
                              Username = s.Username,
                              Name = s.Name,
                              Role = s.Role,
                              PhoneNumber = s.PhoneNumber,
                              StartingDate = s.StartingDate,
                              Password = s.Password
                          }).ToListAsync();
                return await staffs;
            }
        }
        public async Task<(bool, string, StaffDTO)> Login(string username, string password)
        {

            string hassPass = Helper.MD5Hash(password);

            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var staff = await (from s in context.Staffs
                                       where s.Username == username && s.Password == hassPass
                                       select new StaffDTO
                                       {
                                           Id = s.Id,
                                           BirthDate = s.BirthDate,
                                           Gender = s.Gender,
                                           Name = s.Name,
                                           Role = s.Role,
                                           PhoneNumber = s.PhoneNumber,
                                           StartingDate = s.StartingDate
                                       }).FirstOrDefaultAsync();

                    if (staff == null)
                    {
                        return (false, "Sai tài khoản hoặc mật khẩu", null);
                    }
                    return (true, "", staff);
                }

            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }


        }
        private string CreateNextStaffId(string maxId)
        {
            //NVxxx
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "NV" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        public (bool, string, StaffDTO) AddStaff(StaffDTO newStaff)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    bool usernameIsExist = context.Staffs.Any(s => s.Username == newStaff.Username);

                    if (usernameIsExist)
                    {
                        return (false, "Tài khoản đã tồn tại!", null);
                    }
                    var maxId = context.Staffs.Max(s => s.Id);

                    Staff st = Copy(newStaff);
                    st.Id = CreateNextStaffId(maxId);
                    st.Password = Helper.MD5Hash(newStaff.Password);
                    context.Staffs.Add(st);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                return (false, "Lỗi hệ thống", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Thêm nhân viên mới thành công", newStaff);
        }
        private Staff Copy(StaffDTO s)
        {
            return new Staff
            {
                BirthDate = s.BirthDate,
                Gender = s.Gender,
                Username = s.Username,
                Name = s.Name,
                Role = s.Role,
                PhoneNumber = s.PhoneNumber,
                StartingDate = s.StartingDate
            };
        }

        public async Task<(bool, string)> UpdateStaff(StaffDTO updatedStaff)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    bool usernameIsExist = context.Staffs.Any(s => s.Username == updatedStaff.Username && s.Id != updatedStaff.Id);
                    if (usernameIsExist)
                    {
                        return (false, "Tài khoản đăng nhập đã tồn tại");
                    }

                    Staff staff = context.Staffs.Find(updatedStaff.Id);
                    if (staff == null)
                    {
                        return (false, "Nhân viên không tồn tại");
                    }
                    staff.BirthDate = updatedStaff.BirthDate;
                    staff.Gender = updatedStaff.Gender;
                    staff.Username = updatedStaff.Username;
                    staff.Name = updatedStaff.Name;
                    staff.Role = updatedStaff.Role;
                    staff.PhoneNumber = updatedStaff.PhoneNumber;
                    staff.StartingDate = updatedStaff.StartingDate;

                    await context.SaveChangesAsync();
                }
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return (false, "DbEntityValidationException");

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Error Server");
            }
            return (true, "Cập nhật thành công");

        }

        public async Task<(bool, string)> UpdatePassword(string StaffId, string newPassword)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Staff staff = await context.Staffs.FindAsync(StaffId);
                    if (staff == null)
                    {
                        return (false, "Tài khoản không tồn tại");
                    }

                    staff.Password = newPassword;

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Lỗi Server");
            }
            return (true, "Cập nhật mật khẩu thành công");

        }
        public (bool, string) DeleteStaff(string Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Staff staff = (from p in context.Staffs
                                   where p.Id == Id && !p.IsDeleted
                                   select p).SingleOrDefault();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return (false, "Nhân viên không tồn tại!");
                    }
                    staff.IsDeleted = true;
                    staff.Username = null;

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Lỗi hệ thống.");
            }
            return (true, "Xóa nhân viên thành công");
        }

    }
}
