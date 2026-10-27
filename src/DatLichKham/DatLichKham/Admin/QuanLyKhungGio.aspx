<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyKhungGio.aspx.cs" Inherits="DatLichKham.Admin.QuanLyKhungGio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản lý khung giờ khám</h2>

<div class="form-row">
    <label>Bác sĩ</label>
    <asp:DropDownList ID="ddlBacSiMoi" runat="server"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Ngày khám</label>
    <asp:TextBox ID="txtNgay" runat="server" TextMode="Date"></asp:TextBox>
</div>
<div class="form-row">
    <label>Giờ bắt đầu</label>
    <asp:TextBox ID="txtGioBD" runat="server" TextMode="Time"></asp:TextBox>
</div>
<div class="form-row">
    <label>Giờ kết thúc</label>
    <asp:TextBox ID="txtGioKT" runat="server" TextMode="Time"></asp:TextBox>
</div>
<asp:Button ID="btnThem" runat="server" Text="Thêm khung giờ" CssClass="btn" OnClick="btnThem_Click" />
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<hr />
<div class="form-row">
    <asp:DropDownList ID="ddlLocBacSi" runat="server"></asp:DropDownList>
    <asp:TextBox ID="txtLocNgay" runat="server" TextMode="Date"></asp:TextBox>
    <asp:Button ID="btnLoc" runat="server" Text="Lọc" CssClass="btn" OnClick="btnLoc_Click" />
</div>

<asp:GridView ID="gvKhungGio" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaKG,MaBS" AllowPaging="true" PageSize="15"
    OnPageIndexChanging="gvKhungGio_PageIndexChanging"
    OnRowEditing="gvKhungGio_RowEditing" OnRowCancelingEdit="gvKhungGio_RowCancelingEdit"
    OnRowUpdating="gvKhungGio_RowUpdating" OnRowDeleting="gvKhungGio_RowDeleting"
    EmptyDataText="Không có khung giờ phù hợp.">
    <Columns>
        <asp:BoundField DataField="MaKG" HeaderText="Mã" ReadOnly="true" />
        <asp:BoundField DataField="TenBacSi" HeaderText="Bác sĩ" ReadOnly="true" />
        <asp:TemplateField HeaderText="Ngày khám">
            <ItemTemplate><%# Ngay(Eval("NgayKham")) %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtNgaySua" runat="server" TextMode="Date" Text='<%# NgayIso(Eval("NgayKham")) %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Giờ bắt đầu">
            <ItemTemplate><%# Gio(Eval("GioBatDau")) %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtGioBDSua" runat="server" TextMode="Time" Text='<%# Gio(Eval("GioBatDau")) %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Giờ kết thúc">
            <ItemTemplate><%# Gio(Eval("GioKetThuc")) %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtGioKTSua" runat="server" TextMode="Time" Text='<%# Gio(Eval("GioKetThuc")) %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TinhTrang" HeaderText="Tình trạng" ReadOnly="true" />
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Edit">Sửa</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Delete"
                    OnClientClick="return confirm('Xóa khung giờ này?');">Xóa</asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" CommandName="Update">Lưu</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Cancel">Hủy</asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
