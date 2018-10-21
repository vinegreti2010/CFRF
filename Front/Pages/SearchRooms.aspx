<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Layout.Master" AutoEventWireup="true" CodeBehind="SearchRooms.aspx.cs" Inherits="Front.Pages.ClassRooms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Gerenciamento de Salas</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <h1>Gerenciamento de Salas</h1>
        <hr>

        <div class="form-group">
        <label for="sel1">Sala</label>

        <asp:SqlDataSource runat="server" ID="DataSource" ConnectionString="<%$ ConnectionStrings:CFRFApp_dbConnectionString %>" SelectCommand="SELECT [facility_id], [descr] FROM [facility_tbl] ORDER BY [descr]"></asp:SqlDataSource>
        <asp:DropDownList runat="server" ID="DropDownRooms" CssClass="form-control" Width="400" DataSourceID="DataSource" DataTextField="descr" DataValueField="facility_id"></asp:DropDownList>
    </div>
        <asp:Button runat="server" CssClass="btn btn-primary btn-block" ID="TryAccessRoom" Width="150" Text="Pesquisar" OnClick="TryAccessRoom_Click"/>
        <asp:Button runat="server" CssClass="btn btn-secondary btn-block" ID="TryCreateRoom" Width="150" Text="Inserir Novo Valor" OnClick="TryCreateRoom_Click"/>
    </div>
</asp:Content>
