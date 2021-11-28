using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class TroubleService
    {
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
                using (var context = new CinemaManagementEntities())
                {
                    List<TroubleDTO> troubleList = (from trou in context.Troubles
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
                using (var context = new CinemaManagementEntities())
                {
                    var maxId = context.Troubles.Max(t => t.Id);
                    Trouble tr = new Trouble()
                    {
                        Id = CreateNextTroubleId(maxId),
                        Title = newTrouble.Title,
                        Description = newTrouble.Description,
                        Image = newTrouble.Image,
                        Status = STATUS.WAITING,
                        Level = newTrouble.Level ?? LEVEL.NORMAL,
                        SubmittedAt = DateTime.Now,
                        StaffId = newTrouble.StaffId,
                    };
                    context.Troubles.Add(tr);

                    context.SaveChanges();

                    newTrouble.Id = tr.Id;
                    return (true, null, newTrouble);
                }
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
                using (var context = new CinemaManagementEntities())
                {

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

                using (var context = new CinemaManagementEntities())
                {

                    var trouble = context.Troubles.Find(updatedTrouble.Id);

                    if (updatedTrouble.Status == STATUS.IN_PROGRESS)
                    {
                        trouble.StartDate = DateTime.Now;
                    }
                    else if (updatedTrouble.Status == STATUS.DONE)
                    {
                        if (trouble.Status == STATUS.WAITING)
                        {
                            trouble.StartDate = DateTime.Now;
                        }
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = updatedTrouble.RepairCost;
                    }
                    else if (updatedTrouble.Status == STATUS.CANCLE)
                    {
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = 0;
                    }

                    trouble.Status = updatedTrouble.Status;

                    context.SaveChanges();

                    return (true, "Cập nhật thành công");
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
        public int GetWaitingTroubleCount()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    return context.Troubles.Count();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
