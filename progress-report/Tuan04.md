# BÁO CÁO TIẾN ĐỘ – TUẦN 4

**Đề tài:** Xây dựng website đặt lịch hẹn khám bệnh ngoài giờ
**Học phần:** Chuyên đề ASP.NET – Trường Kỹ thuật và Công nghệ, Đại học Trà Vinh
**Sinh viên:** Tống Duy Khanh – MSSV 470123159 – Lớp VX23TTK12
**Thời gian:** 26/10/2026 – 01/11/2026

## 1. Công việc đã hoàn thành

| STT | Nội dung | Kết quả |
|---|---|---|
| 1 | Trang Quản lý khung giờ khám (thêm, sửa, xóa); kiểm tra ngày đã qua, giờ ngoài quy định (ngày thường 17:00–21:00, cuối tuần 08:00–21:00), trùng và chồng lấn giờ; không cho sửa/xóa khung giờ đã có lịch hẹn | Hoàn thành |
| 2 | Trang Quản lý bệnh nhân: tìm kiếm tương đối, sửa thông tin, khóa/mở khóa tài khoản, xóa (có giao dịch, chặn xóa bệnh nhân đã có lịch hẹn) | Hoàn thành |
| 3 | Trang Thống kê, báo cáo theo năm: số lịch theo tháng, theo bác sĩ, theo chuyên khoa, theo trạng thái, kèm biểu đồ thanh và các ô tổng hợp | Hoàn thành |
| 4 | Hoàn thành đủ 10 chức năng theo bảng yêu cầu chức năng (mục 3.2) | Hoàn thành |

## 2. Sản phẩm đã đưa lên GitHub

- `src/DatLichKham/DatLichKham/Admin/QuanLyKhungGio.aspx` và `.cs`
- `src/DatLichKham/DatLichKham/Admin/QuanLyBenhNhan.aspx` và `.cs`
- `src/DatLichKham/DatLichKham/Admin/ThongKe.aspx` và `.cs`
- Cập nhật `Site.Master`, `Content/Site.css`
- `progress-report/Tuan04.md`

## 3. Khó khăn và cách giải quyết

- Chưa ghi nhận khó khăn đáng kể trong tuần.

## 4. Kế hoạch tuần 5

1. Kiểm thử toàn bộ chức năng theo từng vai trò (Bệnh nhân, Lễ tân, Admin), sửa lỗi phát sinh.
2. Rà soát ràng buộc dữ liệu và bảo mật (tham số hóa truy vấn, phân quyền, mã hóa mật khẩu).
3. Hoàn thiện README: hướng dẫn cài đặt và chạy, tài khoản thử nghiệm.
4. Viết báo cáo đồ án theo mẫu của học phần, chụp ảnh kết quả đưa vào Chương 4.
5. Commit mã nguồn lên GitHub ít nhất 1 lần trong tuần.
