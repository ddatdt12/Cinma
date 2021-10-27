using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

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
        public List<StaffDTO> GetAllStaff()
        {
            List<StaffDTO> staffs = null;

            var context = DataProvider.Ins.DB;
            staffs = (from s in context.Staffs
                      where s.IsDeleted == false
                      select new StaffDTO
                      {
                          Id = s.Id,
                          Age = s.Age,
                          BirthDate = s.BirthDate,
                          Gender = s.Gender,
                          Username = s.Username,
                          Name = s.Name,
                          Role = s.Role,
                          PhoneNumber = s.PhoneNumber,
                          StartingDate = s.StartingDate
                      }).ToList();
            return staffs;
        }
        public (bool, string, StaffDTO) Login(string username, string password)
        {
            var context = DataProvider.Ins.DB;


            StaffDTO staff = (from s in context.Staffs
                              where s.Username == username && s.Password == password
                              select new StaffDTO
                              {
                                  Id = s.Id,
                                  Age = s.Age,
                                  BirthDate = s.BirthDate,
                                  Gender = s.Gender,
                                  Username = s.Username,
                                  Name = s.Name,
                                  Role = s.Role,
                                  PhoneNumber = s.PhoneNumber,
                                  StartingDate = s.StartingDate
                              }).FirstOrDefault();
            if (staff == null)
            {
                return (false, "Tài khoản hoặc mật khẩu sai!", null);
            }

            return (true, "", staff);
        }
        public (bool, string) AddStaff(StaffDTO newStaff)
        {
            var context = DataProvider.Ins.DB;

            Staff staff = (from s in context.Staffs
                           where s.Username == newStaff.Username
                           select s).FirstOrDefault();
            if (staff != null)
            {
                return (false, "Tài khoản đã tồn tại!");
            }

            context.Staffs.Add(Copy(newStaff));

            context.SaveChanges();

            return (true, "Thêm nhân viên mới thành công");
        }
        private Staff Copy(StaffDTO s)
        {
            return new Staff
            {
                Age = s.Age,
                BirthDate = s.BirthDate,
                Gender = s.Gender,
                Username = s.Username,
                Password = s.Password,
                Name = s.Name,
                Role = s.Role,
                PhoneNumber = s.PhoneNumber,
                StartingDate = s.StartingDate
            };
        }

        public (bool, string) UpdateStaff(StaffDTO updatedStaff)
        {
            try
            {
                var context = DataProvider.Ins.DB;
                bool usernameIsExist = context.Staffs.Where(s => s.Username == updatedStaff.Username && s.Id != updatedStaff.Id).FirstOrDefault() != null;
                if (usernameIsExist)
                {
                    return (false, "Tài khoản đăng nhập đã tồn tại");
                }

                Staff staff = context.Staffs.Find(updatedStaff.Id);
                if (staff == null)
                {
                    return (false, "Phim không tồn tại");
                }


                PropertyCopier<StaffDTO, Staff>.Copy(updatedStaff, staff);

                DataProvider.Ins.DB.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Console.WriteLine(e);
                return (false, "DbEntityValidationException");

            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Error Server");
            }
            return (true, "Cập nhật thành công");

        }
        public (bool, string) DeleteStaff(int Id)
        {
            try
            {
                Staff staff = (from p in DataProvider.Ins.DB.Staffs
                               where p.Id == Id && !p.IsDeleted
                               select p).SingleOrDefault();
                if (staff == null || staff?.IsDeleted == true)
                {
                    return (false, "Nhân viên không tồn tại!");
                }
                staff.IsDeleted = true;
                staff.Username = null;

                DataProvider.Ins.DB.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Lỗi hệ thống.");
            }
            return (true, "Xóa phim thành công");
        }
    }
}
