<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DatLich.aspx.cs" Inherits="DatLichKham.DatLich" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Đặt lịch hẹn khám ngoài giờ</h2>

<div class="form-row">
    <label>Chuyên khoa</label>
    <asp:DropDownList ID="ddlChuyenKhoa" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlChuyenKhoa_SelectedIndexChanged"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Bác sĩ</label>
    <asp:DropDownList ID="ddlBacSi" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlBacSi_SelectedIndexChanged"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Ngày khám</label>
    <asp:DropDownList ID="ddlNgay" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlNgay_SelectedIndexChanged"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Khung giờ trống</label>
    <asp:DropDownList ID="ddlKhungGio" runat="server"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Lý do khám</label>
    <asp:TextBox ID="txtLyDo" runat="server" TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
</div>
<asp:Button ID="btnDatLich" runat="server" Text="Đặt lịch" CssClass="btn" OnClick="btnDatLich_Click" />
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>
</asp:Content>
