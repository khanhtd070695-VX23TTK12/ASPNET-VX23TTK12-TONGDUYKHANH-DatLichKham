<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyBacSi.aspx.cs" Inherits="DatLichKham.Admin.QuanLyBacSi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản lý bác sĩ</h2>

<div class="form-row">
    <label>Họ tên</label>
    <asp:TextBox ID="txtHoTen" runat="server" MaxLength="100"></asp:TextBox>
</div>
<div class="form-row">
    <label>Chuyên khoa</label>
    <asp:DropDownList ID="ddlCKMoi" runat="server"></asp:DropDownList>
</div>
<div class="form-row">
    <label>Học vị</label>
    <asp:TextBox ID="txtHocVi" runat="server" MaxLength="50"></asp:TextBox>
</div>
<div class="form-row">
    <label>Số điện thoại</label>
    <asp:TextBox ID="txtSDT" runat="server" MaxLength="11"></asp:TextBox>
</div>
<asp:Button ID="btnThem" runat="server" Text="Thêm bác sĩ" CssClass="btn" OnClick="btnThem_Click" />
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<asp:GridView ID="gvBacSi" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaBS" OnRowDataBound="gvBacSi_RowDataBound"
    OnRowEditing="gvBacSi_RowEditing" OnRowCancelingEdit="gvBacSi_RowCancelingEdit"
    OnRowUpdating="gvBacSi_RowUpdating" OnRowDeleting="gvBacSi_RowDeleting"
    EmptyDataText="Chưa có bác sĩ nào.">
    <Columns>
        <asp:BoundField DataField="MaBS" HeaderText="Mã" ReadOnly="true" />
        <asp:TemplateField HeaderText="Họ tên">
            <ItemTemplate><%# Eval("HoTen") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtHoTenSua" runat="server" Text='<%# Eval("HoTen") %>' MaxLength="100"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Chuyên khoa">
            <ItemTemplate><%# Eval("TenCK") %></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlCKSua" runat="server"></asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Học vị">
            <ItemTemplate><%# Eval("HocVi") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtHocViSua" runat="server" Text='<%# Eval("HocVi") %>' MaxLength="50"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SĐT">
            <ItemTemplate><%# Eval("SDT") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtSDTSua" runat="server" Text='<%# Eval("SDT") %>' MaxLength="11"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Trạng thái">
            <ItemTemplate><%# (bool)Eval("TrangThai") ? "Đang làm việc" : "Ngừng làm việc" %></ItemTemplate>
            <EditItemTemplate>
                <asp:CheckBox ID="chkTrangThaiSua" runat="server" Text="Đang làm việc"
                    Checked='<%# (bool)Eval("TrangThai") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Edit">Sửa</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Delete"
                    OnClientClick="return confirm('Xóa bác sĩ này?');">Xóa</asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" CommandName="Update">Lưu</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Cancel">Hủy</asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
