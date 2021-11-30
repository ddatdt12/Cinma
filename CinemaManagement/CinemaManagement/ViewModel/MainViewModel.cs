using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private string _enteredGenreName;
        public string EnteredGenreName
        {
            get { return _enteredGenreName; }
            set { _enteredGenreName = value; OnPropertyChanged(); }
        }
        private ObservableCollection<GenreDTO> _genreList;
        public ObservableCollection<GenreDTO> GenreList
        {
            get => _genreList;
            set
            {
                _genreList = value;
                OnPropertyChanged();
            }
        }

        private GenreDTO _selectedGenre;

        public GenreDTO SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; OnPropertyChanged(); }
        }

        private List<MovieDTO> _movieList;
        public List<MovieDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
            }
        }
        public ICommand AddGenre { get; set; }
        public ICommand EditGenre { get; set; }
        public ICommand DeleteGenre { get; set; }
        public MainViewModel()
        {
            SelectedGenre = null;
            List<MovieDTO> movieDTOs;
            List<GenreDTO> genreDTOs;
            //List<StaffDTO> staffDTOs;

            try
            {
                //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                //var path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Resources\", "Images");
                //bool isExist = !Directory.Exists(path);

                movieDTOs = MovieService.Ins.GetAllMovie();
                genreDTOs = GenreService.Ins.GetAllGenre();

                #region PRODUCTS
                //const string DRINK = "Nước uống";
                //const string FOOD = "Đồ ăn";
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Pepsi", Price = 45000, Category = DRINK, Image = "pepsi.png" });
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Bánh Chorros", Price = 30000, Category = FOOD, Image = "banh_chorros.png" });
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Bánh quy xoắn Pretzels", Price = 30000, Category = FOOD, Image = "banh_quy_xoan_pretzels.png" });
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Cá viên chiên", Price = 20000, Category = FOOD, Image = "ca_vien_chien.png" });
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Hot dog", Price = 30000, Category = FOOD, Image = "hot_dog.png" });
                //ProductService.Ins.AddProduct(new ProductDTO { DisplayName = "Mực trộn bơ", Price = 30000, Category = FOOD, Image = "muc_tron_bo.png" });
                ////(bool IsSuccess, string messageFromDeleteProduct) = ProductService.Ins.DeleteProduct(1);
                List<ProductDTO> prodDtos = ProductService.Ins.GetAllProduct();
                //List<ProductDTO> prodDtos = ProductService.Ins.GetAllProduct();
                #endregion
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                //// the code that you want to measure comes here
                ////createTicketTask.Wait();
                //watch.Stop();
                //var elapsedMs = watch.ElapsedMilliseconds;

                //staffDTOs = StaffService.Ins.GetAllStaff();

                #region Showtime and Movie
                //List<MovieDTO> ListMovieDtosOnDay = MovieService.Ins.GetShowingMovieByDay(DateTime.Today);

                //List<MovieDTO> ListMovieDtos = MovieService.Ins.GetShowingMovieByDay(new DateTime(2021, 11, 1), 1);


                //List<MovieDTO> ListMovieDtosOnDay = MovieService.Ins.GetShowingMovieByDay(new DateTime(2021, 11, 1));



                // ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 1, RoomId = 1, ShowDate = DateTime.Today, StartTime = new TimeSpan(20, 20, 0) });
                // ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 2, RoomId = 1, ShowDate = DateTime.Today, StartTime = new TimeSpan(6, 20, 0) });
                // ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 3, RoomId = 1, ShowDate = DateTime.Today, StartTime = new TimeSpan(10, 20, 0) });
                // ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 4, RoomId = 1, ShowDate = DateTime.Today, StartTime = new TimeSpan(13, 20, 0) });
                // ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 1, RoomId = 1, ShowDate = DateTime.Today,StartTime = new TimeSpan(16, 20, 0) });

                //List<MovieDTO> movieDTOsByDay = MovieService.Ins.GetShowingMovieByDay(DateTime.Today);
                //List<SeatSettingDTO> seatList = SeatService.Ins.GetSeatsByShowtime(13);
                Console.WriteLine(" ");
                //Add showtime
                //(bool AddSuccess, string message, ShowtimeDTO newShowtime) = ShowtimeService.Ins.AddShowtime(
                //    new ShowtimeDTO { MovieId = 4, RoomId = 1, ShowDate = DateTime.Today.AddDays(2), StartTime = new TimeSpan(19, 0, 0) }
                //    );

                ////Delete showtime 
                //int ShowTimeId = 16;
                //(bool deleteSuccess, string messageFromDeleteShowtime) = ShowtimeService.Ins.DeleteShowtime(ShowTimeId);


                //ListMovieDtos = MovieService.Ins.GetShowingMovieByDay(DateTime.Today.AddDays(-2), 1);

                //ListMovieDtos = MovieService.Ins.GetShowingMovieByDay(DateTime.Today, 1);

                List<MovieDTO> ListMovieDtos = MovieService.Ins.GetShowingMovieByDay(DateTime.Today, 1);
                //string movieName= "Bố già";
                //string imageName = Helper.Slugify(movieName);
                //(bool AddSuccess, string message) = ShowtimeService.Ins.AddShowtime(
                //new ShowtimeDTO { MovieId = 1, RoomId = 1, ShowDate = DateTime.Today, StartTime = new TimeSpan(13, 50, 0) });
                //(bool AddSuccess, string message) = ShowtimeService.Ins.AddShowtime(new ShowtimeDTO { MovieId = 4, RoomId = 1, ShowDate = DateTime.Today.AddDays(2), StartTime = new TimeSpan(19, 0, 0) });

                //ListMovieDtos = MovieService.Ins.GetShowingMovieByDay(DateTime.Today, 1);

                #endregion

                //Console.WriteLine("zxc");
                #region Staff Service
                //StaffDTO staff = new StaffDTO
                //{
                //    Name = "Test",
                //    Username = "test1234",
                //    Password = "123456",
                //    Gender = "Nam",
                //    PhoneNumber = "098312731",
                //    BirthDate = new DateTime(2002, 05, 12),
                //    StartingDate = DateTime.Today
                //};
                ////Add new staff
                //(bool addStaffSuccess, string messageFromAddStaff, StaffDTO newStaff) = StaffService.Ins.AddStaff(staff);


                //Test update Staff
                //staffDTOs.Last().Name = "Test after update";
                //staffDTOs.Last().Age = 25;

                //(bool updateStaffSuccess, string messageFromUpdateStaff) = StaffService.Ins.UpdateStaff(staffDTOs.Last());

                //(bool deleteSuccess, string messageFromDeleteStaff) = StaffService.Ins.DeleteStaff(staffDTOs.Last().Id);

                //staffDTOs = StaffService.Ins.GetAllStaff();
                #endregion

                //(bool successzz, string messageLogin, StaffDTO staffz) = StaffService.Ins.Login("admin", "123456");

                #region Movie Service
                //Test Update movie
                //movieDTOs[0].DisplayName = "BỐ GIÀ";
                //movieDTOs[0].Director = "Vũ Ngọc Đãng & Trấn Thành";
                //(bool updateSuccess, string messageFromUpdateMovie) = MovieService.Ins.UpdateMovie(movieDTOs[0]);

                //Test create new movie
                //MovieDTO newMovie = new MovieDTO
                //{
                //    DisplayName = "Test",
                //    Country = "Việt Nam",
                //    Description = "Test creating new Movie",
                //    ReleaseDate = DateTime.Now,
                //    RunningTime = 169,
                //    Director = "Đạt ĐT",
                //    Genres = new List<GenreDTO>
                //    {
                //        genreDTOs[2],
                //        genreDTOs[3]
                //    }
                //};
                //(bool addSuccess, string messageFromAddMovie) = MovieService.Ins.AddMovie(newMovie);

                //(bool deleteSuccess, string messageFromDeleteMovie) = MovieService.Ins.DeleteMovie(movieDTOs.Last().Id);

                //Test Delete Movie
                //(bool deleteSuccess, string messageFromDeleteMovie) = MovieService.Ins.DeleteMovie(movieDTOs.Last().Id);
                //movieDTOs = MovieService.Ins.GetAllMovie();
                #endregion


                #region ProductReceipt (Nhập hàng và hóa đơn nhập sản phẩm)

                //List<ProductDTO> productDTOs = ProductService.Ins.GetAllProduct();
                //(bool createSuccess, string messageFromCreate, ProductReceiptDTO newReceipt) = ProductReceiptService.Ins.CreateProductReceipt(new ProductReceiptDTO
                //{
                //    ProductId = productDTOs[6].Id,
                //    ImportPrice = 300000,
                //    Quantity = 20,
                //    StaffId = "NV003",
                //});
                //ProductReceiptService.Ins.CreateProductReceipt(new ProductReceiptDTO
                //{
                //    ProductId = productDTOs[7].Id,
                //    ImportPrice = 250000,
                //    Quantity = 10,
                //    StaffId = "NV003",
                //});
                //List<ProductReceiptDTO> productReceipts = ProductReceiptService.Ins.GetProductReceipt();
                //productDTOs = ProductService.Ins.GetAllProduct();

                #endregion



                #region Chỉ dành cho đặt vé

                //Khách hàng thành viên
                //BillDTO bill = new BillDTO { CustomerId = "KH0005", StaffId = "NV002", TotalPrice = 2 * showtime.TicketPrice };
                ////Khách vãng lai
                ////BillDTO bill = new BillDTO { StaffId = "NV002", TotalPrice = 2 * showtime.TicketPrice  };
                //List<TicketDTO> ticketList = new List<TicketDTO>();
                //ticketList.Add(new TicketDTO { SeatId = 96, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 97, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId=43, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId=44, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                //(bool isSuccess, string message) = BookingService.Ins.CreateTicketBooking(bill, ticketList);
                #endregion

                #region Dành cho đặt cả vé và đồ ăn

                //List<MovieDTO> ListMovieDtosOnDay = MovieService.Ins.GetShowingMovieByDay(DateTime.Today);
                //var showtime = DataProvider.Ins.DB.Showtimes.Find(13);
                //var prodList = ProductService.Ins.GetAllProduct();

                //int p1 = 3;
                //int p2 = 4;

                //decimal TotalPrice = 6 * showtime.TicketPrice + 3 * prodList[p1].Price + 2 * prodList[p2].Price;
                //BillDTO bill = new BillDTO { CustomerId = "KH0002", StaffId = "NV005", TotalPrice = TotalPrice };

                //List<TicketDTO> ticketList = new List<TicketDTO>();
                //ticketList.Add(new TicketDTO { SeatId = 50, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 51, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 52, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 53, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 54, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 55, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });

                //List<ProductBillInfoDTO> orderedProds = new List<ProductBillInfoDTO>();
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[p1].Id, PricePerItem = prodList[p1].Price, Quantity = 3 });
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[p2].Id, PricePerItem = prodList[p2].Price, Quantity = 2 });
                ////ticketList.Add(new TicketDTO { SeatId=43, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                ////ticketList.Add(new TicketDTO { SeatId=44, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                //(bool isSuccess, string message) = BookingService.Ins.CreateFullOptionBooking(bill, ticketList, orderedProds);
                //Console.WriteLine("PAUSE");
                #endregion
                #region Dành cho đặt đồ ăn

                //var prodList = ProductService.Ins.GetAllProduct();

                //BillDTO bill = new BillDTO { CustomerId = "KH0001", StaffId = "NV002", TotalPrice = 100000 };

                //List<ProductBillInfoDTO> orderedProds = new List<ProductBillInfoDTO>();
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[1].Id, PricePerItem = prodList[1].Price, Quantity = 3 });
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[2].Id, PricePerItem = prodList[2].Price, Quantity = 2 });
                ////ticketList.Add(new TicketDTO { SeatId=43, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                ////ticketList.Add(new TicketDTO { SeatId=44, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                //(bool isSuccess, string message) = BookingService.Ins.CreateProductOrder(bill, orderedProds);
                #endregion

                //var billDetails = BillService.Ins.GetBillDetails("HD0010");
                //var todayBill = BillService.Ins.GetBillByDate(new DateTime(2021,11,14));
                //var mothnBill = BillService.Ins.GetBillByMonth(11);
                //var allBill = BillService.Ins.GetAllBill();

                #region Statistic


                ////Details
                //var topCustomer = StatisticsService.Ins.GetTop5CustomerExpense();
                //var topStaff = StatisticsService.Ins.GetTop5ContributionStaff();
                //var topMovie = StatisticsService.Ins.GetTop5BestMovie();
                //var topProduct = StatisticsService.Ins.GetTop5BestProduct();

                //var topMovieByMonth = StatisticsService.Ins.GetTop5BestMovieByMonth(11);
                //var topProdByMonth = StatisticsService.Ins.GetTop5BestProductByMonth(11);
                //var topStaffByMonth = StatisticsService.Ins.GetTop5ContributionStaffByMonth(11);
                //var topCusByMonth = StatisticsService.Ins.GetTop5CustomerExpenseByMonth(11);

                ////Overview
                ////(decimal Income, string rateStr) monthReveue = OverviewStatisticService.Ins.StatisticizeMonthIncome(2021, 11);
                ////(decimal Income, string rateStr) yearRevenue = OverviewStatisticService.Ins.StatisticizeYearIncome(2021);
                //(List<decimal> monthlyRevenue, decimal ProductReve, decimal TicketReve, string YearRevenueRateStr) = OverviewStatisticService.Ins.GetRevenueByYear(2021);
                //(List<decimal> dailyRevenue, decimal MonthProductReve, decimal MonthTicketReve, string MonthRateStr) = OverviewStatisticService.Ins.GetRevenueByMonth(DateTime.Now.Year, 11);
                //(List<decimal> monthlyExpense, decimal ProductExpense, decimal RepairCost, string YearExpenseRateStr) = OverviewStatisticService.Ins.GetExpenseByYear(2021);
                //(List<decimal> dailyExpense, decimal MonthProductExpense,decimal MonthRepairCost, string MonthExpenseRateStr)  = OverviewStatisticService.Ins.GetExpenseByMonth(2021, 11);
                // int BillQuantity = OverviewStatisticService.Ins.GetBillQuantity(2021, 11);


                #endregion



                #region Trouble

                //TroubleDTO newTr = new TroubleDTO
                //{
                //    Title = "Vỡ kính",
                //    Description = "Kính bị vỡ ở sảnh chờ,",
                //    Image = "broken_mirror.jfif",
                //    StaffId = "NV004",
                //};
                //TroubleDTO updatedTr = new TroubleDTO
                //{
                //    Id = "TR0001",
                //    Status = "Đã giải quyết",
                //    RepairCost = 1000000
                //};
                //(bool isSuccess, string message, TroubleDTO newTrou) = TroubleService.Ins.CreateNewTrouble(newTr);
                //(bool isS, string messageFromUpdate) = TroubleService.Ins.UpdateStatusTrouble(updatedTr);

                var troubleList = TroubleService.Ins.GetAllTrouble();
                int troubleCount = TroubleService.Ins.GetWaitingTroubleCount();
                #endregion

                #region Voucher
                (string error, List<string> listCode) = Helper.GetListCode(100, 9, "BFR", "6");

                VoucherReleaseDTO newVR = new VoucherReleaseDTO
                {
                    ReleaseName = "Black Friday 2021",
                    StartDate = DateTime.Today,
                    FinishDate = DateTime.Today.AddDays(14),
                    EnableMerge = true,
                    MinimumOrderValue = 150000,
                    ParValue = 30000,
                    ObjectType = VOUCHER_OBJECT_TYPE.ALL,
                    Status = true,
                    StaffId = "NV002"
                };

                //(bool isSucess, string addSuccess, VoucherReleaseDTO newVoucherRelease) = VoucherService.Ins.CreateVoucherRelease(newVR);

                //(bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = VoucherService.Ins.CreateRandomVoucherList("VCRL0001", listCode);
                List<VoucherReleaseDTO> voucherReleases = VoucherService.Ins.GetAllVoucherReleases();
                (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails("VCRL0001");

                //(bool deleteSuccess, string messageFromDelete) = VoucherService.Ins.DeteleVoucherRelease("VCRL0001");

                //Delete multiple vouchers
                //List<int> voucherIdList = voucherReleaseDetail.Vouchers.Take(10).Select(v => v.Id).ToList();

                //(bool deleteSuccess, string messageFromDelete) = VoucherService.Ins.DeteleVouchers(voucherIdList);

                ////Release Voucher
                //(bool releaseSuccess, string messageFromRelease) = VoucherService.Ins.ReleaseMultiVoucher(voucherIdList);

                //(voucherReleaseDetail, haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails("VCRL0001");
                #endregion

                //(bool loginSuccess, string message, StaffDTO staff) = StaffService.Ins.Login("dothanhdat123","123456");
                MovieList = new List<MovieDTO>(movieDTOs);
                GenreList = new ObservableCollection<GenreDTO>(genreDTOs);

            }
            catch (InvalidOperationException e)
            {
            }
            catch (Exception e)
            {
                MessageBox.Show($"Mất kết nối cơ sở dữ liệu! Vui lòng kiểm tra lại", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            EditGenre = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedGenre == null)
                        return false;
                    return true;
                },
                (p) => EditGenreCommandHandler());
            AddGenre = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) => AddGenreCommandHandler());
        }
        private void AddGenreCommandHandler()
        {
            (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = VoucherService.Ins.GetVoucherReleaseDetails("VCRL0001");
            //GenreDTO genre = new GenreDTO { DisplayName = EnteredGenreName };
            //(bool isSuccess, string message) = GenreService.Ins.AddGenre(genre);
            //if (isSuccess)
            //{
            //    LoadGenreList();
            //}
            //else
            //{
            //    MessageBox.Show(message);
            //}
        }

        private void EditGenreCommandHandler()
        {
            if (string.IsNullOrEmpty(EnteredGenreName))
            {
                MessageBox.Show("Tên thể loại phim mới không thể để trống");
                return;
            }
            (bool isSuccess, string message) = GenreService.Ins.EditGenre(SelectedGenre.Id, EnteredGenreName);
            if (isSuccess)
            {
                LoadGenreList();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void LoadGenreList()
        {
            GenreList = new ObservableCollection<GenreDTO>(GenreService.Ins.GetAllGenre());
        }
    }
}

