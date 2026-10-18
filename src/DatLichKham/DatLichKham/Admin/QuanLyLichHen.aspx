<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyLichHen.aspx.cs" Inherits="DatLichKham.Admin.QuanLyLichHen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản lý lịch hẹn</h2>

<div class="form-row">
    <asp:TextBox ID="txtTuKhoa" runat="server" placeholder="Tìm theo tên bệnh nhân, bác sĩ, SĐT, mã lịch..."></asp:TextBox>
    <asp:DropDownList ID="ddlTrangThai" runat="server">
        <asp:ListItem Text="Tất cả trạng thái" Value=""></asp:ListItem>
        <asp:ListItem Text="Chờ xác nhận" Value="Chờ xác nhận"></asp:ListItem>
        <asp:ListItem Text="Đã xác nhận" Value="Đã xác nhận"></asp:ListItem>
        <asp:ListItem Text="Đã khám" Value="Đã khám"></asp:ListItem>
        <asp:ListItem Text="Đã hủy" Value="Đã hủy"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnTim" runat="server" Text="Tìm kiếm" CssClass="btn" OnClick="btnTim_Click" />
</div>
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<asp:GridView ID="gvLich" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaLH" AllowPaging="true" PageSize="10"
    OnPageIndexChanging="gvLich_PageIndexChanging"
    OnRowCommand="gvLich_RowCommand" OnRowDataBound="gvLich_RowDataBound"
    EmptyDataText="Không có lịch hẹn phù hợp.">
    <Columns>
        <asp:BoundField DataField="MaLH" HeaderText="Mã" />
        <asp:BoundField DataField="TenBenhNhan" HeaderText="Bệnh nhân" />
        <asp:BoundField DataField="SDT" HeaderText="SĐT" />
        <asp:BoundField DataField="TenBacSi" HeaderText="Bác sĩ" />
        <asp:BoundField DataField="TenCK" HeaderText="Chuyên khoa" />
        <asp:BoundField DataField="NgayKham" HeaderText="Ngày" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
        <asp:BoundField DataField="KhungGio" HeaderText="Giờ" />
        <asp:BoundField DataField="LyDoKham" HeaderText="Lý do" />
        <asp:BoundField DataField="TrangThai" HeaderText="Trạng thái" />
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton ID="lnkXacNhan" runat="server" CommandName="XacNhan" CommandArgument='<%# Eval("MaLH") %>'>Xác nhận</asp:LinkButton>
                <asp:LinkButton ID="lnkDaKham" runat="server" CommandName="DaKham" CommandArgument='<%# Eval("MaLH") %>'>Đã khám</asp:LinkButton>
                <asp:LinkButton ID="lnkTuChoi" runat="server" CommandName="TuChoi" CommandArgument='<%# Eval("MaLH") %>'
                    OnClientClick="return confirm('Hủy / từ chối lịch hẹn này?');">Từ chối</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
