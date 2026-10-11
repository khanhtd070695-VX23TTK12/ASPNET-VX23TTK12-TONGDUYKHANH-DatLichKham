<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LichCuaToi.aspx.cs" Inherits="DatLichKham.LichCuaToi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Lịch hẹn của tôi</h2>
<p><asp:Label ID="lblThongBao" runat="server"></asp:Label></p>

<asp:GridView ID="gvLich" runat="server" AutoGenerateColumns="false" CssClass="grid"
    DataKeyNames="MaLH" OnRowCommand="gvLich_RowCommand" OnRowDataBound="gvLich_RowDataBound"
    EmptyDataText="Bạn chưa có lịch hẹn nào. Bấm &quot;Đặt lịch&quot; để đặt lịch khám.">
    <Columns>
        <asp:BoundField DataField="MaLH" HeaderText="Mã lịch" />
        <asp:BoundField DataField="TenBacSi" HeaderText="Bác sĩ" />
        <asp:BoundField DataField="TenCK" HeaderText="Chuyên khoa" />
        <asp:BoundField DataField="NgayKham" HeaderText="Ngày khám" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
        <asp:BoundField DataField="KhungGio" HeaderText="Giờ khám" />
        <asp:BoundField DataField="LyDoKham" HeaderText="Lý do khám" />
        <asp:BoundField DataField="TrangThai" HeaderText="Trạng thái" />
        <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <asp:LinkButton ID="lnkHuy" runat="server" CommandName="Huy"
                    CommandArgument='<%# Eval("MaLH") %>'
                    OnClientClick="return confirm('Bạn chắc chắn muốn hủy lịch hẹn này?');">Hủy lịch</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
