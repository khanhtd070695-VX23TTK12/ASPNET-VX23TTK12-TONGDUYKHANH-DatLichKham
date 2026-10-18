<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyChuyenKhoa.aspx.cs" Inherits="DatLichKham.Admin.QuanLyChuyenKhoa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản lý chuyên khoa</h2>

<div class="form-row">
    <label>Tên chuyên khoa</label>
    <asp:TextBox ID="txtTen" runat="server" MaxLength="100"></asp:TextBox>
</div>
<div class="form-row">
    <label>Mô tả</label>
    <asp:TextBox ID="txtMoTa" runat="server" MaxLength="255"></asp:TextBox>
</div>
<asp:Button ID="btnThem" runat="server" Text="Thêm chuyên khoa" CssClass="btn" OnClick="btnThem_Click" />
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<asp:GridView ID="gvChuyenKhoa" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaCK"
    OnRowEditing="gvChuyenKhoa_RowEditing" OnRowCancelingEdit="gvChuyenKhoa_RowCancelingEdit"
    OnRowUpdating="gvChuyenKhoa_RowUpdating" OnRowDeleting="gvChuyenKhoa_RowDeleting"
    EmptyDataText="Chưa có chuyên khoa nào.">
    <Columns>
        <asp:BoundField DataField="MaCK" HeaderText="Mã" ReadOnly="true" />
        <asp:BoundField DataField="TenCK" HeaderText="Tên chuyên khoa" />
        <asp:BoundField DataField="MoTa" HeaderText="Mô tả" />
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Edit">Sửa</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Delete"
                    OnClientClick="return confirm('Xóa chuyên khoa này?');">Xóa</asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" CommandName="Update">Lưu</asp:LinkButton>
                <asp:LinkButton runat="server" CommandName="Cancel">Hủy</asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
