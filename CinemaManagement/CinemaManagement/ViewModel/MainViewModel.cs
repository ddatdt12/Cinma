using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Threading.Tasks;
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
                var s = "Pause";

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
                List<SeatSettingDTO> seatList = SeatService.Ins.GetSeatsByShowtime(13);
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
                //    ProductId = productDTOs[5].Id,
                //    ImportPrice = 700000,
                //    Quantity = 20
                //});
                //ProductReceiptService.Ins.CreateProductReceipt(new ProductReceiptDTO
                //{
                //    ProductId = productDTOs[6].Id,
                //    ImportPrice = 950000,
                //    Quantity = 40
                //});
                //List<ProductReceiptDTO> productReceipts = ProductReceiptService.Ins.GetProductReceipt();
                //productDTOs = ProductService.Ins.GetAllProduct();

                #endregion
                List<MovieDTO> ListMovieDtosOnDay = MovieService.Ins.GetShowingMovieByDay(DateTime.Today);

                var showtime = ListMovieDtosOnDay[0].Showtimes[0];
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

                //var prodList = ProductService.Ins.GetAllProduct();

                //BillDTO bill = new BillDTO { CustomerId = "KH0002", StaffId = "NV002", TotalPrice = 2 * showtime.TicketPrice + prodList[1].Price  + prodList[2].Price };

                //List<TicketDTO> ticketList = new List<TicketDTO>();
                //ticketList.Add(new TicketDTO { SeatId = 105, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });
                //ticketList.Add(new TicketDTO { SeatId = 106, Price = showtime.TicketPrice, ShowtimeId = showtime.Id });

                //List<ProductBillInfoDTO> orderedProds = new List<ProductBillInfoDTO>();
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[1].Id, PricePerItem = prodList[1].Price, Quantity = 3 });
                //orderedProds.Add(new ProductBillInfoDTO { ProductId = prodList[2].Id, PricePerItem = prodList[2].Price, Quantity = 2 });
                ////ticketList.Add(new TicketDTO { SeatId=43, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                ////ticketList.Add(new TicketDTO { SeatId=44, Price = showtime.TicketPrice , ShowtimeId = showtime.Id });
                //(bool isSuccess, string message) = BookingService.Ins.CreateFullOptionBooking(bill, ticketList, orderedProds);
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


                var billDetails = BillService.Ins.GetBillDetails("HD0010");
                var todayBill = BillService.Ins.GetBillByDate(DateTime.Today);
                var mothnBill = BillService.Ins.GetBillByMonth(11);
                var allBill = BillService.Ins.GetAllBill();
                BillService.Ins.
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
            GenreDTO genre = new GenreDTO { DisplayName = EnteredGenreName };
            (bool isSuccess, string message) = GenreService.Ins.AddGenre(genre);
            if (isSuccess)
            {
                LoadGenreList();
            }
            else
            {
                MessageBox.Show(message);
            }
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

