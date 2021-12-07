using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System.Data.Entity;
using System;
using System.Collections.Generic;
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
                                  Password = s.Password,
                                  Email = s.Email
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
                                           StartingDate = s.StartingDate,
                                           Email = s.Email
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
        public async Task<(bool, string, StaffDTO)> AddStaff(StaffDTO newStaff)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    bool usernameIsExist = await context.Staffs.AnyAsync(s => s.Username == newStaff.Username);
                    bool emailIsExist = await context.Staffs.AnyAsync(s => s.Email == newStaff.Email);

                    if (usernameIsExist)
                    {
                        return (false, "Tài khoản đã tồn tại!", null);
                    }
                    if (emailIsExist)
                    {
                        return (false, "Email đã được đăng kí!", null);
                    }

                    var maxId = await context.Staffs.MaxAsync(s => s.Id);

                    Staff st = Copy(newStaff);
                    st.Id = CreateNextStaffId(maxId);
                    st.Password = Helper.MD5Hash(newStaff.Password);
                    context.Staffs.Add(st);
                    await context.SaveChangesAsync();
                }
            }

            catch (Exception)
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
                StartingDate = s.StartingDate,
                Email = s.Email
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
                    staff.Email = updatedStaff.Email;

                    await context.SaveChangesAsync();
                }
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
                    if (staff is null)
                    {
                        return (false, "Tài khoản không tồn tại");
                    }

                    staff.Password = Helper.MD5Hash(newPassword);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật mật khẩu thành công");

        }
        public async Task<(bool, string)> DeleteStaff(string Id)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Staff staff = await (from p in context.Staffs
                                         where p.Id == Id && !p.IsDeleted
                                         select p).FirstOrDefaultAsync();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return (false, "Nhân viên không tồn tại!");
                    }
                    staff.IsDeleted = true;
                    staff.Username = null;
                    staff.Email = null;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return (false, $"Lỗi hệ thống.");
            }
            return (true, "Xóa nhân viên thành công");
        }

        /// <summary>
        /// Dùng để tìm email của staff và gửi mail cho chức năng quên mật khẩu
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public (string error, string email, string Id) GetStaffEmail(string username)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    Staff staff = (from p in context.Staffs
                                   where p.Username == username && !p.IsDeleted
                                   select p).FirstOrDefault();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return ("Tài khoản đăng nhập không tồn tại!", null, null);
                    }

                    if (staff.Email is null)
                    {
                        return ("Tài khoản chưa đăng kí email. Vui lòng liên hệ quản lý để được hỗ trợ", null, null);
                    }

                    return (null, staff.Email, staff.Id);
                }
            }
            catch (Exception)
            {
                return ($"Lỗi hệ thống.", null, null);
            }
        }

    }
}
