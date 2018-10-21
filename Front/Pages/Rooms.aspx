<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Layout.Master" AutoEventWireup="true" CodeBehind="Rooms.aspx.cs" Inherits="Front.Pages.Rooms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Gerenciamento de Salas</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <h1>Gerenciamento de Salas</h1>
        <hr>
        <div class="form-group">
            <asp:Label runat="server" ID="Msg"></asp:Label>
            <br />
            <strong><asp:Label runat="server" ID="RoomCode"></asp:Label></strong>
            <br>
            <label for="sel1">Descrição</label>
            <asp:TextBox runat="server" ID="Descr" CssClass="form-control" Width="400"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="sel1">Latitude Norte-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatNE" TextMode="Number"></asp:TextBox>
            
            <label for="sel1">Longitude Norte-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongNE" TextMode="Number"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="sel1">Latitude Norte-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatNW" TextMode="Number"></asp:TextBox>
            
            <label for="sel1">Longitude Norte-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongNW" TextMode="Number"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="sel1">Latitude Sul-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatSE" TextMode="Number"></asp:TextBox>
            
            <label for="sel1">Longitude Sul-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongSE" TextMode="Number"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="sel1">Latitude Sul-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatSW" TextMode="Number"></asp:TextBox>
            
            <label for="sel1">Longitude Sul-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongSW" TextMode="Number"></asp:TextBox>
        </div>

        <asp:Button runat="server" CssClass="btn btn-primary btn-block" Width="150" Text="Salvar" ID="SaveButton" OnClick="SaveButton_Click" />
    </div>
</asp:Content>
