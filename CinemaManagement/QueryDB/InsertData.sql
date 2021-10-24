use CinemaManagement


--SELECT
SELECT * FROM Staff
SELECT * FROM Genre
SELECT * FROM Movie
SELECT * FROM GenreDetails

SET DATEFORMAT dmy


--STAFF
INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender, Role)
VALUES (N'Admin','admin','admin','0987582999', 18,'1/1/2002',N'Nam', N'Quản lý');

INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender, Role)
VALUES (N'Đỗ Thành Đạt','dothanhdat','123456','0987582042', 18, '12/05/2002' ,N'Nam', N'Quản lý');

INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender, Role)
VALUES (N'Trần Đình Khôi','trandinhkhoi','123456','0287582003', 18, '26/10/2002',N'Nữ', N'Nhân viên');

INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender)
VALUES (N'Lê Hải Phong','lehaiphong','123456','0312301233', 18 ,'9/10/2002',N'Nam');

INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender)
VALUES (N'Kiều Bá Dương','kieubaduong','123456','0123456789', 18,'14/05/2002',N'Nam');

INSERT INTO Staff(Name, Username, Password, PhoneNumber, Age, BirthDate, Gender)
VALUES (N'Huỳnh Trung Thảo','huynhtrungthao','123456','0987312312', 18,'14/5/2002',N'Nữ');


--Genre

INSERT INTO Genre(DisplayName)
VALUES  
		(N'Gia đình'),
		(N'Hài'),
		(N'Kinh dị'),
		(N'Tình cảm'),
		(N'Khoa Học Viễn Tưởng'),
		(N'Tâm Lý'),
		(N'Phiêu Lưu'),
		(N'Hoạt Hình'),
		(N'Thần thoại'),
		(N'Lịch sử'),
		(N'Giật gân'),
		(N'Chính kịch');


--Movie

INSERT INTO Movie(DisplayName, RunningTime, Country, Description, Director, ReleaseDate)
VALUES 
	(N'BỐ GIÀ',128,N'Việt Nam',
	N'Phim sẽ xoay quanh lối sống thường nhật của một xóm lao động nghèo, ở đó có bộ tứ anh em Giàu - Sang - Phú - Quý với Ba Sang sẽ là nhân vật chính, hay lo chuyện bao đồng nhưng vô cùng thương con cái. Câu chuyện phim tập trung về hai cha con Ba Sang (Trấn Thành) và Quắn (Tuấn Trần). Dù yêu thương nhau nhưng khoảng cách thế hệ đã đem đến những bất đồng lớn giữa hai cha con. Liệu cả hai có thể cho nhau cơ hội thấu hiểu đối phương, thu hẹp khoảng cách và tạo nên hạnh phúc từ sự khác biệt?'
	,N'Vũ Ngọc Đãng & Trấn Thành ','12/03/2021'),
	(N'GODZILLA VS. KONG', 113 ,N'Mỹ',
	N'Khi hai kẻ thù truyền kiếp gặp nhau trong một trận chiến ngoạn mục, số phận của cả thế giới vẫn còn bị bỏ ngỏ… Bị đưa khỏi Đảo Đầu Lâu, Kong cùng Jia, một cô bé mồ côi có mối liên kết mạnh mẽ với mình và đội bảo vệ đặc biệt hướng về mái nhà mới. Bất ngờ, nhóm đụng độ phải Godzilla hùng mạnh, tạo ra một làn sóng hủy diệt trên toàn cầu.'
	, N'Adam Wingard','26/03/2021'),
	(N'BÀN TAY DIỆT QUỶ',128,N'Hàn Quốc',
	N'Sau khi bản thân bỗng nhiên sở hữu “Bàn tay diệt quỷ”, võ sĩ MMA Yong Hoo (Park Seo Joon thủ vai) đã dấn thân vào hành trình trừ tà, trục quỷ đối đầu với Giám Mục Bóng Tối (Woo Do Hwan) – tên quỷ Satan đột lốt người.'
	, 'Kim Joo Hwan','09/04/2021'),
	(N'PALM SPRINGS: MỞ MẮT THẤY HÔM QUA',89 ,N'Hàn Quốc',
	N'Mở Mắt Thấy Hôm Qua (tựa gốc: Palm Springs) – đúng như tên gọi, bộ phim là một vòng lặp bất tận của thời gian, với thật nhiều những rắc rối lặp đi lặp lại không có điểm dừng. Anh chàng Nyles (Andy Samberg) và nàng phù dâu bất đắc dĩ Sarah (Cristin Milioti) tình cờ gặp nhau tại đám cưới ở Palm Springs, mọi thứ trở nên phức tạp khi Nyles và Sarah “mắc kẹt” mãi ở ngày vui của người khác. Trong khi Sarah điên cuồng tìm cách thoát ra thì Nyles bình thản chấp nhận sống lại ngày hôm qua thêm một lần nữa. Họ sẽ làm gì để có thể thoát khỏi nơi này, thoát khỏi những vấn đề của chính mình khi giờ đây còn “vướng” phải nhau nữa?'
	,N'Max Barbakow','12/03/2021'),
	(N'Con Lắc Tà Thuật',85 ,N'Hàn Quốc',
	N'Sinh viên chuyên ngành Tiếng Anh Do Hyun nhận lời giúp đỡ một sinh viên trao đổi đang phải trải qua điều trị tâm lý sau một tai nạn bí ẩn. Qua lời giới thiệu của người bạn này, Do Hyun đã được Giáo sư Choi thôi miên. Tuy nhiên, sau đó, những hình ảnh rùng rợn liên quan đến một vụ án trong quá khứ luôn đeo bám Do Hyun, dù đó là ký ức chưa từng tồn tại trong trí nhớ của anh. Cùng lúc, hội bạn chơi lâu năm của Do Hyun gặp phải các tai nạn bí ẩn hay bị tra tấn bởi những ảo giác kỳ lạ.'
	,'Choi Jae Hoon', '21/4/2021');


--GenreDetails


INSERT INTO GenreDetails(MovieId, GenreId)
VALUES
	(2, 2),
	(2, 13),
	(3, 9),
	(3, 1),
	(4, 1),
	(4, 3),
	(5, 4),
	(5, 2),
	(6, 3);
