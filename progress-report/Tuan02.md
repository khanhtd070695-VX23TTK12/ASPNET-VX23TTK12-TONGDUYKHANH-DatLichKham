# BÁO CÁO TIẾN ĐỘ – TUẦN 2

**Đề tài:** Xây dựng website đặt lịch hẹn khám bệnh ngoài giờ
**Học phần:** Chuyên đề ASP.NET – Trường Kỹ thuật và Công nghệ, Đại học Trà Vinh
**Sinh viên:** Tống Duy Khanh – MSSV 470123159 – Lớp VX23TTK12
**Thời gian:** 12/10/2026 – 18/10/2026

## 1. Công việc đã hoàn thành

| STT | Nội dung | Kết quả |
|---|---|---|
| 1 | Vẽ sơ đồ Use case (3 tác nhân, 10 chức năng) và mô tả kiến trúc hệ thống 3 lớp | Hoàn thành |
| 2 | Cài đặt môi trường: Visual Studio, tạo project ASP.NET Web Forms (.NET Framework 4.8) trong `src/` | Hoàn thành |
| 3 | Kết nối project với CSDL `QLDatLichKham` qua chuỗi kết nối trong `Web.config` | Hoàn thành |
| 4 | Xây dựng lớp `DatabaseHelper` (truy vấn có tham số, băm mật khẩu SHA-256) | Hoàn thành |
| 5 | Thiết kế Master Page, file CSS, trang chủ; menu thay đổi theo vai trò đăng nhập | Hoàn thành |
| 6 | Chức năng Đăng ký tài khoản bệnh nhân (kiểm tra dữ liệu nhập, chống trùng tên đăng nhập, dùng giao dịch ghi 2 bảng) | Hoàn thành |
| 7 | Chức năng Đăng nhập, Đăng xuất (quản lý bằng Session, phân quyền theo vai trò) | Hoàn thành |
| 8 | Chức năng Đặt lịch hẹn: chọn chuyên khoa, bác sĩ, ngày, khung giờ trống; gọi thủ tục `sp_DatLich` | Hoàn thành |
| 9 | Trang Lịch của tôi: xem lịch sử, hủy lịch (gọi thủ tục `sp_HuyLich`, chỉ hủy trước giờ khám 2 giờ) | Hoàn thành |

## 2. Sản phẩm đã đưa lên GitHub

- Mã nguồn project trong `src/DatLichKham/`: `Site.Master`, `Default`, `DangKy`, `DangNhap`, `DatLich`, `LichCuaToi`, `Helpers/DatabaseHelper.cs`, `Content/Site.css`
- `progress-report/Tuan02.md`

## 3. Khó khăn và cách giải quyết

- Lỗi *Could not load type 'DatLichKham.Site'* do thiếu dấu đóng ngoặc trong code-behind → bổ sung, build lại thành công.
- Đoạn `<connectionStrings>` đặt ngoài thẻ `<configuration>` trong `Web.config` → chuyển vào trong thẻ.

## 4. Kế hoạch tuần 3

1. Xây dựng trang quản lý lịch hẹn cho Lễ tân/Admin: xem danh sách, duyệt, từ chối, đánh dấu đã khám.
2. Chức năng tìm kiếm tương đối (theo tên bệnh nhân, bác sĩ, số điện thoại, mã lịch).
3. Chức năng CRUD: chuyên khoa, bác sĩ, khung giờ khám; quản lý bệnh nhân.
4. Bảo vệ các trang quản trị theo vai trò Admin/Lễ tân.
5. Commit mã nguồn lên GitHub ít nhất 1 lần trong tuần.
