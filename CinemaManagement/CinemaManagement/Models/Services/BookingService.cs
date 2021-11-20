using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Models.Services
{
    public class BookingService
    {
        private static BookingService _ins;
        public static BookingService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BookingService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private BookingService()
        {
        }
        private string CreateNextBillId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "HD0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "HD" + newIdString.Substring(newIdString.Length - 4, 4);
        }
        /// <summary>
        ///  Đặt những vé xem phim khi biết danh sách ghế => tạo list ticket
        /// </summary>
        /// <param name="newTicketList"></param>
        /// <returns></returns>
        public (bool IsSuccess, string message) CreateTicketBooking(BillDTO bill, List<TicketDTO> newTicketList)
        {
            if (newTicketList.Count() == 0)
            {
                return (false, "Vui lòng chọn ghế!");
            }
            var context = DataProvider.Ins.DB;
            try
            {
                //Update seat 
                var idSeatList = new List<int>();
                newTicketList.ForEach(s => idSeatList.Add(s.SeatId));
                //Make seat of showtime status = true
                int showtimeId = newTicketList[0].ShowtimeId;
                var seatSets = context.SeatSettings.Where(s => s.ShowtimeId == showtimeId && idSeatList.Contains(s.SeatId)).ToList();
                foreach (var s in seatSets)
                {
                    if (s.Status)
                    {
                        var seat = s.Seat;
                        return (false, $"Ghế {seat.Row}{seat.SeatNumber} đã được đặt!");
                    }
                    s.Status = true;
                }

                //Bill
                string billId = CreateNewBill(ref context, bill);

                //Ticket
                AddNewTickets(billId, newTicketList);

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}");
            }
            return (true, "Đặt vé thành công");
        }

         /// <summary>
         /// (Dành cho cả mua vé và đặt hàng) Tạo hóa đơn khi biết Bill (chứa CustomerId, StaffId , totalPrice) , danh sách vé, danh sách các sản phẩm đc đặt
         /// </summary>
         /// <param name="bill"></param>
         /// <param name="newTicketList"></param>
         /// <param name="orderedProductList"></param>
         /// <returns></returns>
        public (bool IsSuccess, string message) CreateFullOptionBooking(BillDTO bill, List<TicketDTO> newTicketList, List<ProductBillInfoDTO> orderedProductList )
        {
            if (newTicketList.Count() == 0)
            {
                return (false, "Vui lòng chọn ghế!");
            }
            var context = DataProvider.Ins.DB;
            try
            {
                //Update seat 
                var idSeatList = new List<int>();
                newTicketList.ForEach(s => idSeatList.Add(s.SeatId));
                //Make seat of showtime status = true
                int showtimeId = newTicketList[0].ShowtimeId;
                var seatSets = context.SeatSettings.Where(s => s.ShowtimeId == showtimeId && idSeatList.Contains(s.SeatId)).ToList();
                foreach (var s in seatSets)
                {
                    if (s.Status)
                    {
                        var seat = s.Seat;
                        return (false, $"Ghế {seat.Row}{seat.SeatNumber} đã được đặt!");
                    }
                    s.Status = true;
                }
                //Bill
                string billId = CreateNewBill(ref context, bill);

                //Ticket
                AddNewTickets(billId, newTicketList);

                //Product
                bool addSuccess = AddNewProductBills(billId, orderedProductList);
                if (!addSuccess)
                {
                    return (false, "Số lượng sản phẩm không đủ để đáp ứng!");
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}");
            }
            return (true, "Thực hiện giao dịch thành công");
        }

        /// <summary>
        /// (Dành cho chỉ đặt hàng) Tạo hóa đơn đặt hàng
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderedProductList"></param>
        /// <returns></returns>
        public (bool IsSuccess, string message) CreateProductOrder(BillDTO bill, List<ProductBillInfoDTO> orderedProductList)
        {
            try
            {
                var context = DataProvider.Ins.DB;
                //Bill
                string billId = CreateNewBill(ref context, bill);

                //Product
               bool addSuccess = AddNewProductBills(billId, orderedProductList);
                if (!addSuccess)
                {
                    return (false, "Số lượng sản phẩm không đủ để đáp ứng!");
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}");
            }
            return (true, "Mua sản phẩm thành công");
        }


        private string CreateNewBill(ref CinemaManagementEntities context, BillDTO bill)
        {
            string maxId = context.Bills.Max(b => b.Id);
            string billId = CreateNextBillId(maxId);
            Bill newBill = new Bill
            {
                Id = billId,
                DiscountPrice = bill.DiscountPrice,
                TotalPrice = bill.TotalPrice,
                CustomerId = bill.CustomerId,
                CreatedAt = DateTime.Now,
                StaffId = bill.StaffId
            };
            context.Bills.Add(newBill);
            return billId;
        }
        private void AddNewTickets(string billId, List<TicketDTO> newTicketList)
        {
            List<Ticket> ticketList = new List<Ticket>();
            for (int i = 0; i < newTicketList.Count; i++)
            {
                ticketList.Add(new Ticket
                {
                    BillId = billId,
                    Price = newTicketList[i].Price,
                    SeatId = newTicketList[i].SeatId,
                    ShowtimeId = newTicketList[i].ShowtimeId,
                });
            }
            DataProvider.Ins.DB.Tickets.AddRange(ticketList);
        }

        private bool AddNewProductBills(string billId, List<ProductBillInfoDTO> orderedProductList)
        {
            var context = DataProvider.Ins.DB;
            List<ProductBillInfo> prodBillList = new List<ProductBillInfo>();

            List<int> prodIdList = new List<int>();

            for (int i = 0; i < orderedProductList.Count; i++)
            {
                prodBillList.Add(new ProductBillInfo
                {
                    BillId = billId,
                    ProductId = orderedProductList[i].ProductId,
                    PricePerItem = orderedProductList[i].PricePerItem,
                    Quantity = orderedProductList[i].Quantity
                });
                var Product = context.Products.Find(orderedProductList[i].ProductId);
                Product.Quantity -= orderedProductList[i].Quantity;

                if(Product.Quantity < 0)
                {
                    return false;
                }
            }

            context.ProductBillInfoes.AddRange(prodBillList);
            return true;

        }

    }
}
