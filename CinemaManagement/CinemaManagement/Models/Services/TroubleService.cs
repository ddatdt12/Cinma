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
        //LEVEL
        const string NORMAL = "Bình thường";
        const string CRITICAL = "Nghiêm trọng";

        // STATUS
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

        private string CreateNextTroubleId(string maxId)
        {
            //TRxxxx
            if (maxId is null)
            {
                return "TR0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "TR" + newIdString.Substring(newIdString.Length - 4, 4);
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
                                                    Level = trou.Level,
                                                    Status = trou.Status,
                                                    RepairCost = trou.RepairCost,
                                                    SubmittedAt = trou.SubmittedAt,
                                                    StartDate = trou.StartDate,
                                                    FinishDate = trou.FinishDate,
                                                    StaffId = trou.StaffId,
                                                    StaffName = trou.Staff.Name,
                                                }).ToList();

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

                var maxId = context.Troubles.Max(t => t.Id);

                Trouble tr = new Trouble()
                {
                    Id = CreateNextTroubleId(maxId),
                    Title = newTrouble.Title,
                    Description = newTrouble.Description,
                    Image = newTrouble.Image,
                    Status = WAITING,
                    Level = newTrouble.Level ?? NORMAL,
                    SubmittedAt = DateTime.Now,
                    StaffId = newTrouble.StaffId,
                };
                context.Troubles.Add(tr);

                context.SaveChanges();

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
                trouble.Level = updatedTrouble.Level ?? trouble.Level;

                context.SaveChanges();

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
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = 0;
                        break;
                    default:
                        break;
                }
                trouble.Status = updatedTrouble.Status;

                context.SaveChanges();

                return (true, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetWaitingTroubleCount()
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
