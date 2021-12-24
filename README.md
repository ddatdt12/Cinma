<div id="Top"></div>

# QUẢN LÝ RẠP CHIẾU PHIM
Hỗ trợ các cụm rạp dễ dàng hơn trong việc quản lý.

<!-- LOGO CINEMA -->
![Logo Cinema](./ImageApp/CinemaSquadin.jpg)

## Mục lục

 [I. Mở đầu](#Modau)

 [II. Mô tả](#Mota)

> [1. Ý tưởng](#Ytuong)
>
> [2. Công nghệ](#Congnghe)
>
> [3. Người dùng](#Doituongsudung)
>
> [4. Mục tiêu](#Muctieu)
>
> [5. Tính năng](#Tinhnang)

[III. Tác giả](#Tacgia)

[IV. Người hướng dẫn](#Nguoihuongdan)

[V. Tổng kết](#Tongket)


<!-- MỞ ĐẦU -->
<div id="Modau"></div>

## I. Mở đầu
Kinh tế ngày càng phát triển, áp lực công việc cũng ngày càng lớn kéo theo nhu cầu giải trí của con người cũng ngày càng tăng lên, trong đó xem phim là hình thức được nhiều người lựa chọn. Điều này là một tiềm năng phát triển rất lớn và được nhiều công ty khai thác. Một vấn đề lớn được đặt ra là việc quản lý các rạp phim sao cho hiệu quả, chính xác, tránh được rủi ro không nên có. Hiểu được điều này, nhóm quyết định xây dựng một ứng dụng hỗ trợ các rạp chiếu phim trong việc quản lý, tận dụng những công nghệ tiên tiến để phát triển, mục tiêu hướng đến chính là nâng cao trải nghiệm người dùng về cả giao diện lẫn tốc độ xử lý, kèm theo đó là những tính năng mở rộng phù hợp với thực tiễn.


<!-- MÔ TẢ -->
<div id="Mota"></div>

## II. Mô tả

<!-- Ý TƯỞNG -->
<div id="Ytuong"></div>

### 1. Ý tưởng
* Hướng đến cải thiện trải nghiệm của người dùng, sử dụng công nghệ WPF, ngôn ngữ XAML đáp ứng được các yêu cầu khắt khe hơn, giao diện mới hơn, hiện đại và trực quan hơn, phù hợp với những tiêu chuẩn hiện tại, ngôn ngữ lập trình dễ hiểu, dễ tiếp cận, dễ dàng tạo và chỉnh sửa GUI.

* Sử dụng mô hình MVVM để tách riêng giao diện và xử lý, tăng khả năng sử dụng lại các thành phần hay việc thay đổi giao diện chương trình mà không cần phải viết lại code quá nhiều, có thể phát triển ứng dụng nhanh, dễ nâng cấp, bảo trì, mở rộng hay sửa chữa.

* Lập trình theo hướng đa luồng (MultiThreading) nhằm tối ưu hoá phần cứng, tăng tốc độ xử lý và cải thiện tốc độ ứng dụng.
* Sử dụng Azure SQL Database, một nền tảng quản lý cơ sở dữ liệu đám mây, nhằm đồng bộ hoá dữ liệu giữa các thiết bị của người dùng, đảm bảo tính nhất quán và chính xác của dữ liệu.
* Sử dụng kỹ thuật mã hoá MD5 trong quản lý tài khoản người dùng nhằm đảm bảo tính bảo mật trong quá trình sử dụng, giảm thiệt hại tối đa khi không may thất thoát dữ liệu ra bên ngoài.


<div id="Congnghe"></div>

### 2. Công nghệ
* Hệ thống API: WPF - Mô hình MVVM
* IDE: Visual Studio 2019 (C#/.Net)
* Database: SQL Server, Azure SQL Database
* Công cụ quản lý: Git, GitHub


<div id="Doituongsudung"></div>

### 3. Đối tượng sử dụng
Hệ thống các rạp phim gồm:
* Chủ rạp phim: vai trò quản lý
* Nhân viên quản lý


<div id="Muctieu"></div>

### 4. Mục tiêu

 * <strong>Ứng dụng thực tế</strong>
 
    * Đáp ứng được các yêu cầu của khách hàng đặt ra, hệ thống mang tính ổn định cao, dễ sử dụng, không gây khó khăn cho người dùng, thiết kế dựa trên cơ sở sử dụng cho khách hàng là người Việt Nam.
    * Được sử dụng rộng rãi trong hệ thống các rạp chiếu phim, thay thế cho các ứng dụng cũ còn nhiều hạn chế, giao diện lỗi thời hoặc các hình thức quản lý theo các thủ công truyền thống gây cồng kềnh, khó quản lý và dễ dẫn đến những sai sót không đáng có.
    * Trở thành một trong những ứng dụng được khách hàng lựa chọn, tin tưởng sử dụng.


 * <strong>Yêu cầu ứng dụng</strong>
 
    * Đáp ứng những tính năng tiêu chuẩn cần có trên những ứng dụng quản lý rạp chiếu phim hiện có trên thị trường. Ngoài ra, mở rộng và phát triển những tính năng mới hỗ trợ tối đa cho người dùng, tự động hóa các giai đoạn và các nghiệp vụ quản lý rạp chiếu phim, khắc phục những hạn chế và yếu kém của hệ thống quản lý hiện tại.
    
    * Nâng cao tính chính xác và bảo mật trong kinh doanh, quản lý thông tin khách hàng và nhân viên.
    
    * Đưa ra các báo cáo, thống kê, cập nhật dữ liệu, đồng bộ giữa các máy tính với nhau phải diễn ra nhanh chóng, chính xác.
    
    * Dễ dàng tra cứu, tìm kiếm các thông tin liên quan đến bộ phim, phòng và thời gian chiếu phim, ...và lịch sử mua hàng của khách hàng.
    
    * Dễ dàng cập nhật và lựa chọn lên lịch chiếu phim phải phù hợp, chính xác hạn chế thấp nhất sai sót để nâng cao chất lượng phục vụ của rạp.
    
    * Giao diện thân thiện, dễ sử dụng, bố cục hợp lý, hài hoà về màu sắc và mang tính đồng bộ cao, phân quyền cho người dùng thông qua tài khoản.
    
    * Ứng dụng phải tương thích với đa số các hệ điều hành phổ biến hiện nay như Window Vista SP1, Window 8.1, Window 10,...Đặc biệt, ứng dụng  trong quá trình sử dụng phải hoạt động ổn định, tránh những trường hợp xảy ra lỗi xung đột với hệ thống gây ra khó chịu cho người dùng trong quá trình sử dụng, tệ hơn là ảnh hưởng trực tiếp đến khách hàng của rạp phim. Việc mở rộng, nâng cấp ứng dụng về sau phải dễ dàng khi người dùng có nhu cầu.


<div id="Tinhnang"></div>

### 5. Tính năng
* Quản lý đăng nhập, hỗ trợ việc khôi phục tài khoản cho người dùng khi quên mật khẩu.

* Với vai trò quản lý (admin):
  * Quản lý suất chiếu phim
  * Quản lý phim
  * Quản lý sản phẩm
  * Quản lý nhân sự
  * Quản lý khách hàng
  * Thống kê
  * Quản lý lịch sử hàng hoá
  * Quản lý voucher khách hàng
  * Tiếp nhận và xử lý sự cố trang thiết bị
 

* Với vai trò nhân viên:
  * Xem thông tin chi tiết của các suất chiếu, cho phép chọn suất chiếu.
  * Xem sơ đồ ghế của phòng chiếu, thông tin chi tiết của ghế, cho phép chọn ghế.
  * Xem thông tin những sản phẩm bán kèm đang được bán tại rạp, cho phép đặt kèm theo vé hoặc mua riêng theo nhu cầu khách hàng.
  * Cho phép nhập thông tin khách mua hàng, thanh toán và in hoá đơn sản phẩm.
  * Thêm, huỷ, cập nhật thông tin sự cố các thiết bị phòng chiếu.
 

<!-- TÁC GIẢ -->
<div id="Tacgia"></div>

## III. Tác giả

* [Đỗ Thành Đạt](https://github.com/ddatdt12)

* [Trần Đình Khôi](https://github.com/TranDKhoi)

* [Lê Hải Phong](https://github.com/HaiPhong146)

* [Kiều Bá Dương](https://github.com/kieubaduong)

* [Huỳnh Trung Thảo](https://github.com/thaoht194)


<!-- NGƯỜI HƯỚNG DẪN -->
<div id="Nguoihuongdan"></div>

## IV. Người hướng dẫn
* Giảng viên: Nguyễn Tấn Toàn



<!-- TỔNG KẾT -->
<div id="Tongket"></div>

## V. Tổng kết
Sản phẩm là kết quả sau quá trình cùng nhau thực hiện đồ án của những thành viên trong nhóm. Thông qua quá trình này, các thành viên đã có cho mình những lượng kiến thức và kỹ năng chuyên môn nhất định về quy trình lập trình thực tế, hiểu hơn về lập trình và có riêng cho mình những bài học quý giá làm hành trang cho công việc sau này.

Ngoài ra, nhóm cũng muốn gửi lời cảm ơn chân thành và sự tri ân sâu sắc đến giảng viên giảng dạy, thầy Nguyễn Tấn Toàn đã cùng đồng hành với nhóm trong suốt quá trình thực hiện đồ án để có được thành quả như hôm nay.

Sản phẩm của nhóm có thể còn nhiều thiếu sót trong quá trình xây dựng và phát triển. Vì vậy, đừng ngần ngại gửi những đóng góp hoặc ý kiến của bạn đến email SquandinCinema@gmail.com. Mỗi đóng góp của các bạn đều sẽ được ghi nhận và sẽ là động lực để nhóm có thể hoàn thiện sản phẩm hơn nữa.

Cảm ơn bạn đã quan tâm!

---

<p align="right"><a href="#Top">Quay lại đầu trang</a></p>
