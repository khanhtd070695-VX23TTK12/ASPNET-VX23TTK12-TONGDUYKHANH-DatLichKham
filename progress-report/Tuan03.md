# BÁO CÁO TIẾN ĐỘ – TUẦN 3

**Đề tài:** Xây dựng website đặt lịch hẹn khám bệnh ngoài giờ
**Học phần:** Chuyên đề ASP.NET – Trường Kỹ thuật và Công nghệ, Đại học Trà Vinh
**Sinh viên:** Tống Duy Khanh – MSSV 470123159 – Lớp VX23TTK12
**Thời gian:** 19/10/2026 – 25/10/2026

## 1. Công việc đã hoàn thành

| STT | Nội dung | Kết quả |
|---|---|---|
| 1 | Trang Quản lý lịch hẹn cho Lễ tân/Admin: xem danh sách có phân trang, lọc theo trạng thái | Hoàn thành |
| 2 | Chức năng tìm kiếm tương đối (LIKE) theo tên bệnh nhân, tên bác sĩ, số điện thoại, mã lịch | Hoàn thành |
| 3 | Chức năng duyệt lịch: xác nhận, từ chối, đánh dấu đã khám (chỉ cho phép chuyển trạng thái hợp lệ) | Hoàn thành |
| 4 | Phân quyền theo vai trò: menu thay đổi theo Admin/Lễ tân/Bệnh nhân; chặn truy cập trái quyền vào trang quản trị | Hoàn thành |
| 5 | Trang Quản lý chuyên khoa (thêm, sửa, xóa): kiểm tra dữ liệu nhập, báo lỗi trùng tên, chặn xóa chuyên khoa đang có bác sĩ | Hoàn thành |
| 6 | Trang Quản lý bác sĩ (thêm, sửa, xóa): chọn chuyên khoa bằng danh sách thả xuống, kiểm tra số điện thoại, đổi trạng thái làm việc, chặn xóa bác sĩ đã có khung giờ khám | Hoàn thành |

## 2. Sản phẩm đã đưa lên GitHub

- `src/DatLichKham/DatLichKham/Admin/QuanLyLichHen.aspx` và `.cs`
- `src/DatLichKham/DatLichKham/Admin/QuanLyChuyenKhoa.aspx` và `.cs`
- `src/DatLichKham/DatLichKham/Admin/QuanLyBacSi.aspx` và `.cs`
- Cập nhật `Site.Master` (menu theo vai trò)
- `progress-report/Tuan03.md`

## 3. Khó khăn và cách giải quyết

- Chưa ghi nhận khó khăn đáng kể trong tuần.

## 4. Kế hoạch tuần 4

1. Xây dựng trang Quản lý khung giờ khám (thêm, sửa, xóa lịch làm việc ngoài giờ của bác sĩ).
2. Xây dựng trang Quản lý bệnh nhân (xem, sửa, xóa).
3. Xây dựng chức năng Thống kê, báo cáo (theo tháng, theo bác sĩ, theo chuyên khoa, theo trạng thái) kèm biểu đồ.
4. Kiểm thử toàn bộ chức năng, chụp ảnh kết quả cho báo cáo.
5. Bắt đầu viết báo cáo đồ án theo mẫu (Mở đầu, Chương 2, Chương 3).
6. Commit mã nguồn lên GitHub ít nhất 1 lần trong tuần.
