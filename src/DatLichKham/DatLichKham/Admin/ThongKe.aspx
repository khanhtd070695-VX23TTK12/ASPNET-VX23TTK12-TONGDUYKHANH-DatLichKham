<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThongKe.aspx.cs" Inherits="DatLichKham.Admin.ThongKe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thống kê, báo cáo</h2>

<div class="form-row">
    <label>Năm</label>
    <asp:DropDownList ID="ddlNam" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlNam_SelectedIndexChanged"></asp:DropDownList>
</div>

<div class="cards">
    <div class="card"><asp:Label ID="lblTong" runat="server" CssClass="num"></asp:Label>Tổng lịch hẹn</div>
    <div class="card"><asp:Label ID="lblCho" runat="server" CssClass="num"></asp:Label>Chờ xác nhận</div>
    <div class="card"><asp:Label ID="lblXacNhan" runat="server" CssClass="num"></asp:Label>Đã xác nhận</div>
    <div class="card"><asp:Label ID="lblDaKham" runat="server" CssClass="num"></asp:Label>Đã khám</div>
    <div class="card"><asp:Label ID="lblDaHuy" runat="server" CssClass="num"></asp:Label>Đã hủy</div>
    <div class="card"><asp:Label ID="lblBenhNhan" runat="server" CssClass="num"></asp:Label>Bệnh nhân (toàn hệ thống)</div>
</div>

<h3>1. Số lịch hẹn theo tháng</h3>
<asp:GridView ID="gvThang" runat="server" AutoGenerateColumns="false" CssClass="grid">
    <Columns>
        <asp:BoundField DataField="Thang" HeaderText="Tháng" />
        <asp:BoundField DataField="TongLich" HeaderText="Tổng lịch" />
        <asp:BoundField DataField="DaKham" HeaderText="Đã khám" />
        <asp:BoundField DataField="DaHuy" HeaderText="Đã hủy" />
        <asp:TemplateField HeaderText="Biểu đồ">
            <ItemTemplate><div class="bar"><div class="bar-fill" style="width:<%# Eval("TyLe") %>%"></div></div></ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<h3>2. Số lịch hẹn theo bác sĩ (không tính lịch đã hủy)</h3>
<asp:GridView ID="gvBacSi" runat="server" AutoGenerateColumns="false" CssClass="grid"
    EmptyDataText="Chưa có dữ liệu trong năm này.">
    <Columns>
        <asp:BoundField DataField="TenBacSi" HeaderText="Bác sĩ" />
        <asp:BoundField DataField="TenCK" HeaderText="Chuyên khoa" />
        <asp:BoundField DataField="SoLich" HeaderText="Số lịch" />
        <asp:TemplateField HeaderText="Biểu đồ">
            <ItemTemplate><div class="bar"><div class="bar-fill" style="width:<%# Eval("TyLe") %>%"></div></div></ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<h3>3. Số lịch hẹn theo chuyên khoa (không tính lịch đã hủy)</h3>
<asp:GridView ID="gvChuyenKhoa" runat="server" AutoGenerateColumns="false" CssClass="grid"
    EmptyDataText="Chưa có dữ liệu trong năm này.">
    <Columns>
        <asp:BoundField DataField="TenCK" HeaderText="Chuyên khoa" />
        <asp:BoundField DataField="SoLich" HeaderText="Số lịch" />
        <asp:TemplateField HeaderText="Biểu đồ">
            <ItemTemplate><div class="bar"><div class="bar-fill" style="width:<%# Eval("TyLe") %>%"></div></div></ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<h3>4. Số lịch hẹn theo trạng thái</h3>
<asp:GridView ID="gvTrangThai" runat="server" AutoGenerateColumns="false" CssClass="grid">
    <Columns>
        <asp:BoundField DataField="TrangThai" HeaderText="Trạng thái" />
        <asp:BoundField DataField="SoLich" HeaderText="Số lịch" />
        <asp:TemplateField HeaderText="Biểu đồ">
            <ItemTemplate><div class="bar"><div class="bar-fill" style="width:<%# Eval("TyLe") %>%"></div></div></ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
