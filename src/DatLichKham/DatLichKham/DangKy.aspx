<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="DatLichKham.DangKy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Đăng ký tài khoản bệnh nhân</h2>
<asp:ValidationSummary ID="vsLoi" runat="server" CssClass="error" HeaderText="Vui lòng kiểm tra lại:" />

<div class="form-row">
    <label>Tên đăng nhập</label>
    <asp:TextBox ID="txtTenDangNhap" runat="server" MaxLength="30"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTenDangNhap" Display="Dynamic" CssClass="error" ErrorMessage="Nhập tên đăng nhập">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtTenDangNhap" Display="Dynamic" CssClass="error" ValidationExpression="^[a-zA-Z0-9_]{4,30}$" ErrorMessage="Tên đăng nhập 4-30 ký tự, chỉ gồm chữ không dấu, số, dấu gạch dưới">*</asp:RegularExpressionValidator>
</div>
<div class="form-row">
    <label>Mật khẩu</label>
    <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMatKhau" Display="Dynamic" CssClass="error" ErrorMessage="Nhập mật khẩu">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtMatKhau" Display="Dynamic" CssClass="error" ValidationExpression="^.{6,}$" ErrorMessage="Mật khẩu tối thiểu 6 ký tự">*</asp:RegularExpressionValidator>
</div>
<div class="form-row">
    <label>Nhập lại mật khẩu</label>
    <asp:TextBox ID="txtNhapLai" runat="server" TextMode="Password"></asp:TextBox>
    <asp:CompareValidator runat="server" ControlToValidate="txtNhapLai" ControlToCompare="txtMatKhau" Display="Dynamic" CssClass="error" ErrorMessage="Mật khẩu nhập lại không khớp">*</asp:CompareValidator>
</div>
<div class="form-row">
    <label>Họ và tên</label>
    <asp:TextBox ID="txtHoTen" runat="server" MaxLength="100"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtHoTen" Display="Dynamic" CssClass="error" ErrorMessage="Nhập họ tên">*</asp:RequiredFieldValidator>
</div>
<div class="form-row">
    <label>Ngày sinh</label>
    <asp:TextBox ID="txtNgaySinh" runat="server" TextMode="Date"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNgaySinh" Display="Dynamic" CssClass="error" ErrorMessage="Chọn ngày sinh">*</asp:RequiredFieldValidator>
</div>
<div class="form-row">
    <label>Giới tính</label>
    <asp:DropDownList ID="ddlGioiTinh" runat="server">
        <asp:ListItem Text="Nam" Value="Nam"></asp:ListItem>
        <asp:ListItem Text="Nữ" Value="Nữ"></asp:ListItem>
    </asp:DropDownList>
</div>
<div class="form-row">
    <label>Số điện thoại</label>
    <asp:TextBox ID="txtSDT" runat="server" MaxLength="11"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSDT" Display="Dynamic" CssClass="error" ErrorMessage="Nhập số điện thoại">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtSDT" Display="Dynamic" CssClass="error" ValidationExpression="^0\d{9,10}$" ErrorMessage="Số điện thoại phải bắt đầu bằng 0 và gồm 10-11 số">*</asp:RegularExpressionValidator>
</div>
<div class="form-row">
    <label>Địa chỉ</label>
    <asp:TextBox ID="txtDiaChi" runat="server" MaxLength="200"></asp:TextBox>
</div>
<asp:Button ID="btnDangKy" runat="server" Text="Đăng ký" CssClass="btn" OnClick="btnDangKy_Click" />
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>
</asp:Content>
