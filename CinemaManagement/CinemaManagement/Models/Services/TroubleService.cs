using CinemaManagement.DTOs;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public async Task<List<TroubleDTO>> GetAllTrouble()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    List<TroubleDTO> troubleList = await (from trou
                                                          in context.Troubles
                                                          orderby trou.SubmittedAt descending
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
                                                          }).ToListAsync();

                    return troubleList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string, TroubleDTO)> CreateNewTrouble(TroubleDTO newTrouble)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var maxId = await context.Troubles.MaxAsync(t => t.Id);
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

                    await context.SaveChangesAsync();

                    newTrouble.Id = tr.Id;
                    return (true, null, newTrouble);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<(bool, string)> UpdateTroubleInfo(TroubleDTO updatedTrouble)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {

                    var trouble = await context.Troubles.FindAsync(updatedTrouble.Id);

                    trouble.Title = updatedTrouble.Title;
                    trouble.Description = updatedTrouble.Description;

                    trouble.Image = updatedTrouble.Image;
                    trouble.SubmittedAt = DateTime.Now;
                    trouble.StaffId = updatedTrouble.StaffId;
                    trouble.Level = updatedTrouble.Level ?? trouble.Level;

                    await context.SaveChangesAsync();

                    return (true, null);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string)> UpdateStatusTrouble(TroubleDTO updatedTrouble)
        {
            try
            {

                using (var context = new CinemaManagementEntities())
                {

                    var trouble = await context.Troubles.FindAsync(updatedTrouble.Id);

                    if (updatedTrouble.Status == STATUS.IN_PROGRESS)
                    {
                        trouble.StartDate = updatedTrouble.StartDate;
                    }
                    else if (updatedTrouble.Status == STATUS.DONE)
                    {
                        if (trouble.Status == STATUS.WAITING)
                        {
                            trouble.StartDate = DateTime.Now;
                        }
                        trouble.FinishDate = updatedTrouble.FinishDate;
                        trouble.RepairCost = updatedTrouble.RepairCost;
                    }
                    else if (updatedTrouble.Status == STATUS.CANCLE)
                    {
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = 0;
                    }

                    trouble.Status = updatedTrouble.Status;

                    await context.SaveChangesAsync();

                    return (true, "Cập nhật thành công");
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
        public async Task<int> GetWaitingTroubleCount()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    return await context.Troubles.CountAsync(t => t.Status == STATUS.WAITING);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
