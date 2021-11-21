using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class TroubleService
    {
        const string WAITING = "Chờ tiếp nhận";
        const string IN_PROGRESS = "Đang giải quyết";
        const string DONE = "Đã giải quyết";
        const string CANCLE = "Đã hủy";
        private static TroubleService _ins;
        public static TroubleService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new TroubleService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private TroubleService()
        {
        }
        public List<TroubleDTO> GetAllTrouble()
        {
            try
            {
                List<TroubleDTO> troubleList = (from trou in DataProvider.Ins.DB.Troubles
                                                select new TroubleDTO
                                                {
                                                    Id = trou.Id,
                                                    Title = trou.Title,
                                                    Description = trou.Description,
                                                    Image = trou.Image,
                                                    Status = trou.Status,
                                                    RepairCost = trou.RepairCost,
                                                    SubmittedAt = trou.SubmittedAt,
                                                    StartDate = trou.StartDate,
                                                    FinishDate = trou.FinishDate,
                                                    StaffId = trou.StaffId,
                                                    StaffName = trou.Staff.Name,
                                                }).ToList();

                DataProvider.Ins.DB.SaveChanges();
                return troubleList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public (bool, string, TroubleDTO) CreateNewTrouble(TroubleDTO newTrouble)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                Trouble tr = new Trouble()
                {
                    Title = newTrouble.Title,
                    Description = newTrouble.Description,
                    Image = newTrouble.Image,
                    Status = WAITING,
                    SubmittedAt = DateTime.Now,
                    StaffId = newTrouble.StaffId,
                };
                context.Troubles.Add(tr);
                newTrouble.Id = tr.Id;
                return (true, null, newTrouble);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public (bool, string) UpdateTroubleInfo(TroubleDTO updatedTrouble)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var trouble = context.Troubles.Find(updatedTrouble.Id);

                trouble.Title = updatedTrouble.Title;
                trouble.Description = updatedTrouble.Description;
                trouble.Image = updatedTrouble.Image;
                trouble.SubmittedAt = DateTime.Now;
                trouble.StaffId = updatedTrouble.StaffId;

                return (true, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public (bool, string) UpdateStatusTrouble(TroubleDTO updatedTrouble)
        {
            try
            {
                var context = DataProvider.Ins.DB;

                var trouble = context.Troubles.Find(updatedTrouble.Id);

                switch (updatedTrouble.Status)
                {
                    case IN_PROGRESS:
                        trouble.StartDate = DateTime.Now;
                        break;
                    case DONE:
                        if (trouble.Status == WAITING)
                        {
                            trouble.StartDate = DateTime.Now;
                        }
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = updatedTrouble.RepairCost;
                        break;
                    case CANCLE:
                        break;
                    default:
                        break;
                }
                trouble.Status = updatedTrouble.Status;
                return (true, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetWaitingTroubleCouny()
        {
            try
            {
                return DataProvider.Ins.DB.Troubles.Count();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
