<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="DatLichKham.DangNhap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Đăng nhập</h2>
<asp:ValidationSummary ID="vsLoi" runat="server" CssClass="error" />
<div class="form-row">
    <label>Tên đăng nhập</label>
    <asp:TextBox ID="txtTenDangNhap" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTenDangNhap" Display="Dynamic" CssClass="error" ErrorMessage="Nhập tên đăng nhập">*</asp:RequiredFieldValidator>
</div>
<div class="form-row">
    <label>Mật khẩu</label>
    <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMatKhau" Display="Dynamic" CssClass="error" ErrorMessage="Nhập mật khẩu">*</asp:RequiredFieldValidator>
</div>
<asp:Button ID="btnDangNhap" runat="server" Text="Đăng nhập" CssClass="btn" OnClick="btnDangNhap_Click" />
<p><asp:Label ID="lblThongBao" runat="server" CssClass="error"></asp:Label></p>
<p>Chưa có tài khoản? <a href="DangKy.aspx">Đăng ký</a></p>
</asp:Content>
