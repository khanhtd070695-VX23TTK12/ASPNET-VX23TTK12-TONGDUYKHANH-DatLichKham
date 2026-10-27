# Xây dựng website đặt lịch hẹn khám bệnh ngoài giờ

Đồ án học phần: **Chuyên đề ASP.NET**
Trường Kỹ thuật và Công nghệ – Đại học Trà Vinh

## Thông tin sinh viên

- Họ tên: Tống Duy Khanh
- MSSV: 470123159
- Lớp: VX23TTK12
- Email: khanhtd070695@sv-onuni.edu.vn
- Điện thoại: 0342844731

## Giới thiệu

Website cho phép bệnh nhân đặt lịch hẹn khám ngoài giờ trực tuyến (chọn chuyên khoa, bác sĩ, ngày, khung giờ còn trống); lễ tân duyệt lịch; quản trị viên quản lý chuyên khoa, bác sĩ, khung giờ, bệnh nhân và xem thống kê, báo cáo.

### Chức năng

| Mã | Chức năng | Vai trò |
|---|---|---|
| UC01 | Đăng ký tài khoản | Khách |
| UC02 | Đăng nhập / Đăng xuất, phân quyền | Tất cả |
| UC03 | Đặt lịch hẹn khám | Bệnh nhân |
| UC04 | Xem, hủy lịch của tôi (trước giờ khám ≥ 2 giờ) | Bệnh nhân |
| UC05 | Quản lý, duyệt lịch hẹn (xác nhận, từ chối, đã khám) | Lễ tân, Admin |
| UC06 | Tìm kiếm tương đối lịch hẹn | Lễ tân, Admin |
| UC07 | Quản lý chuyên khoa (thêm, sửa, xóa) | Admin |
| UC08 | Quản lý bác sĩ (thêm, sửa, xóa) | Admin |
| UC09 | Quản lý khung giờ khám (thêm, sửa, xóa, lọc) | Admin |
| UC10 | Quản lý bệnh nhân (tìm kiếm, sửa, khóa/mở khóa, xóa) | Admin |
| UC11 | Thống kê, báo cáo theo năm | Admin |

## Công nghệ

- ASP.NET Web Forms – .NET Framework 4.8, C#
- ADO.NET (System.Data.SqlClient), truy vấn tham số
- SQL Server Express, SQL Server Management Studio
- Visual Studio 2026, IIS Express
- HTML5, CSS3

## Cấu trúc thư mục

```
├── README.md
├── setup/QLDatLichKham.sql      Script tạo CSDL + dữ liệu mẫu + view + thủ tục
├── src/DatLichKham/             Mã nguồn (mở DatLichKham.slnx)
├── progress-report/             Báo cáo tiến độ hằng tuần
├── thesis/                      doc, pdf, html, abs (slide, video), refs
└── docker/                      (không sử dụng)
```

## Hướng dẫn cài đặt và chạy

### Yêu cầu

- Windows 10/11
- Visual Studio 2022 trở lên, workload **ASP.NET and web development** (có .NET Framework 4.8)
- SQL Server Express (hoặc bản cao hơn) và SQL Server Management Studio

### Các bước

1. **Tải mã nguồn**
   ```
   git clone https://github.com/khanhtd070695-VX23TTK12/ASPNET-VX23TTK12-TONGDUYKHANH-DatLichKham.git
   ```
   hoặc giải nén tệp `.zip` của đồ án.

2. **Tạo cơ sở dữ liệu**
   - Mở SSMS, kết nối tới `localhost\SQLEXPRESS` (Windows Authentication).
   - File → Open → `setup/QLDatLichKham.sql` → bấm **Execute (F5)**.
   - Script tạo CSDL `QLDatLichKham` (7 bảng, 2 view, 4 thủ tục) và dữ liệu mẫu; khung giờ mẫu được tạo cho 7 ngày kể từ ngày chạy script.

3. **Cấu hình chuỗi kết nối** (chỉ khi tên SQL Server khác `localhost\SQLEXPRESS`)
   Mở `src/DatLichKham/DatLichKham/Web.config`, sửa `Data Source` trong chuỗi kết nối `QLDatLichKham`:
   ```xml
   <add name="QLDatLichKham"
        connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=QLDatLichKham;Integrated Security=True;TrustServerCertificate=True"
        providerName="System.Data.SqlClient" />
   ```

4. **Chạy website**
   - Mở `src/DatLichKham/DatLichKham.slnx` bằng Visual Studio.
   - Build → Rebuild Solution (lần đầu Visual Studio tự khôi phục gói NuGet).
   - Bấm **F5** (IIS Express). Trang chủ mở trên trình duyệt.

> Khung giờ mẫu chỉ có trong 7 ngày sau khi chạy script. Nếu chạy thử sau thời gian đó, chạy lại script hoặc đăng nhập admin để thêm khung giờ mới.

### Tài khoản dùng thử (mật khẩu: `123456`)

| Tên đăng nhập | Vai trò |
|---|---|
| `admin` | Quản trị viên |
| `letan01` | Lễ tân |
| `bn01`, `bn02`, `bn03` | Bệnh nhân |

## Quy tắc nghiệp vụ chính

- Mỗi khung giờ chỉ có một lịch hẹn còn hiệu lực (unique filtered index `UX_LichHen_KhungGio`).
- Không đặt lịch cho thời điểm đã qua; chỉ hủy lịch trước giờ khám tối thiểu 2 giờ (`sp_DatLich`, `sp_HuyLich`).
- Khung giờ ngày thường 17:00–21:00, thứ Bảy, Chủ nhật 08:00–21:00; không chồng lấn với khung giờ khác của cùng bác sĩ.
- Trạng thái lịch: Chờ xác nhận → Đã xác nhận → Đã khám, hoặc → Đã hủy.
- Mật khẩu lưu dạng băm SHA-256; mọi truy vấn dùng tham số; các trang quản trị kiểm tra vai trò trong `Page_Load`.

## Tiến độ

- Tuần 1 (05/10 – 11/10/2026): Tạo repo, phân tích yêu cầu, thiết kế và cài đặt CSDL QLDatLichKham. Chi tiết: [progress-report/Tuan01.md](progress-report/Tuan01.md)
- Tuần 2 (12/10 – 18/10/2026): Dựng project ASP.NET Web Forms, Master Page, chức năng đăng ký, đăng nhập, đặt lịch, xem và hủy lịch. Chi tiết: [progress-report/Tuan02.md](progress-report/Tuan02.md)
- Tuần 3 (19/10 – 25/10/2026): Quản lý lịch hẹn (duyệt, tìm kiếm), phân quyền, quản lý chuyên khoa, quản lý bác sĩ. Chi tiết: [progress-report/Tuan03.md](progress-report/Tuan03.md)
- Tuần 4 (26/10 – 01/11/2026): Quản lý khung giờ, quản lý bệnh nhân, thống kê báo cáo; hoàn thành đủ chức năng. Chi tiết: [progress-report/Tuan04.md](progress-report/Tuan04.md)

## Tài liệu đồ án

- Báo cáo: `thesis/doc/` (Word), `thesis/pdf/` (PDF)
- Slide thuyết trình, video demo: `thesis/abs/`
