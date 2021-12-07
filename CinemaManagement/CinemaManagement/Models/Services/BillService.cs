using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class BillService
    {
        private static BillService _ins;
        public static BillService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BillService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private BillService()
        {
        }
        public async Task<List<BillDTO>> GetAllBill()
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var billList = (from b in context.Bills
                                    orderby b.CreatedAt descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.Name,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        CustomerId = b.CustomerId,
                                        CustomerName = b.Customer.Name,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = b.CreatedAt
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get Bill by particular date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<BillDTO>> GetBillByDate(DateTime date)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var billList = (from b in context.Bills
                                    where DbFunctions.TruncateTime(b.CreatedAt) == date.Date
                                    orderby b.CreatedAt descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.Name,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        CustomerId = b.CustomerId,
                                        CustomerName = b.Customer.Name,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = b.CreatedAt
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy hóa đơn trong tháng nào đó
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<List<BillDTO>> GetBillByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var billList = (from b in context.Bills
                                    where b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == month
                                    orderby b.CreatedAt descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.Name,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        CustomerId = b.CustomerId,
                                        CustomerName = b.Customer.Name,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = b.CreatedAt
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của hóa đơn 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public async Task<BillDTO> GetBillDetails(string billId)
        {
            try
            {
                using (var context = new CinemaManagementEntities())
                {
                    var bill = await context.Bills.FindAsync(billId);

                    BillDTO billInfo = new BillDTO
                    {
                        Id = bill.Id,
                        StaffId = bill.Staff.Id,
                        StaffName = bill.Staff.Name,
                        DiscountPrice = bill.DiscountPrice,
                        TotalPrice = bill.TotalPrice,
                        CreatedAt = bill.CreatedAt,
                        ProductBillInfoes = (from pi in bill.ProductBillInfoes
                                             select new ProductBillInfoDTO
                                             {
                                                 BillId = pi.BillId,
                                                 ProductId = pi.ProductId,
                                                 ProductName = pi.Product.DisplayName,
                                                 PricePerItem = pi.PricePerItem,
                                                 Quantity = pi.Quantity
                                             }).ToList(),
                    };
                    if (bill.CustomerId != null)
                    {
                        billInfo.CustomerId = bill.Customer.Id;
                        billInfo.CustomerName = bill.Customer.Name;
                        billInfo.PhoneNumber = bill.Customer.PhoneNumber;
                    }

                    var tickets = bill.Tickets;
                    if (tickets.Count != 0)
                    {
                        var showtime = tickets.FirstOrDefault().Showtime;
                        int roomId = 0;
                        List<string> seatList = new List<string>();
                        foreach (var t in tickets)
                        {
                            if (roomId == 0)
                            {
                                roomId = t.Seat.RoomId;
                            }
                            seatList.Add($"{t.Seat.Row}{t.Seat.SeatNumber}");
                        }
                        billInfo.TicketInfo = new TicketBillInfoDTO()
                        {
                            roomId = roomId,
                            movieName = showtime.Movie.DisplayName,
                            ShowDate = showtime.ShowtimeSetting.ShowDate,
                            StartShowTime = showtime.StartTime,
                            TotalPriceTicket = tickets.Count() * showtime.TicketPrice,
                            seats = seatList,
                        };
                    }
                    return billInfo;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
