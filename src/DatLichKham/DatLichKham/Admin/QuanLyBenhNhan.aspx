<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyBenhNhan.aspx.cs" Inherits="DatLichKham.Admin.QuanLyBenhNhan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản lý bệnh nhân</h2>

<div class="form-row">
    <asp:TextBox ID="txtTuKhoa" runat="server" placeholder="Tìm theo họ tên, SĐT, tên đăng nhập, mã..."></asp:TextBox>
    <asp:Button ID="btnTim" runat="server" Text="Tìm kiếm" CssClass="btn" OnClick="btnTim_Click" />
</div>
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<asp:GridView ID="gvBenhNhan" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaBN,MaTK" AllowPaging="true" PageSize="10"
    OnPageIndexChanging="gvBenhNhan_PageIndexChanging" OnRowDataBound="gvBenhNhan_RowDataBound"
    OnRowEditing="gvBenhNhan_RowEditing" OnRowCancelingEdit="gvBenhNhan_RowCancelingEdit"
    OnRowUpdating="gvBenhNhan_RowUpdating" OnRowDeleting="gvBenhNhan_RowDeleting"
    OnRowCommand="gvBenhNhan_RowCommand"
    EmptyDataText="Không có bệnh nhân phù hợp.">
    <Columns>
        <asp:BoundField DataField="MaBN" HeaderText="Mã" ReadOnly="true" />
        <asp:BoundField DataField="TenDangNhap" HeaderText="Tên đăng nhập" ReadOnly="true" />
        <asp:TemplateField HeaderText="Họ tên">
            <ItemTemplate><%#: Eval("HoTen") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtHoTenSua" runat="server" Text='<%# Eval("HoTen") %>' MaxLength="100"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ngày sinh">
            <ItemTemplate><%# Ngay(Eval("NgaySinh")) %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtNgaySinhSua" runat="server" TextMode="Date" Text='<%# NgayIso(Eval("NgaySinh")) %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Giới tính">
            <ItemTemplate><%#: Eval("GioiTinh") %></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlGioiTinhSua" runat="server">
                    <asp:ListItem Text="Nam" Value="Nam"></asp:ListItem>
                    <asp:ListItem Text="Nữ" Value="Nữ"></asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SĐT">
            <ItemTemplate><%#: Eval("SDT") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtSDTSua" runat="server" Text='<%# Eval("SDT") %>' MaxLength="11"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Địa chỉ">
            <ItemTemplate><%#: Eval("DiaChi") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtDiaChiSua" runat="server" Text='<%# Eval("DiaChi") %>' MaxLength="200"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="SoLich" HeaderText="Số lịch hẹn" ReadOnly="true" />
        <asp:TemplateField HeaderText="Tài khoản">
            <ItemTemplate><%# (bool)Eval("TrangThai") ? "Hoạt động" : "Đã khóa" %></ItemTemplate>
            <EditItemTemplate><%# (bool)Eval("TrangThai") ? "Hoạt động" : "Đã khóa" %></EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Edit">Sửa</asp:LinkButton>
                <asp:LinkButton ID="lnkKhoa" runat="server" CommandName="KhoaMo"
                    CommandArgument='<%# Eval("MaTK") %>'>Khóa</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Delete"
                    OnClientClick="return confirm('Xóa bệnh nhân này?');">Xóa</asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" CommandName="Update">Lưu</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Cancel">Hủy</asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
