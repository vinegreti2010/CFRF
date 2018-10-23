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
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatNE"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LatNE" Type="Double" MinimumValue="-90.0" MaximumValue="90.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -90 e 90 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />

            <label for="sel1">Longitude Norte-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongNE"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LongNE" Type="Double" MinimumValue="-180.0" MaximumValue="180.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -180 e 180 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
        </div>
        
        <div class="form-group">
            <label for="sel1">Latitude Norte-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatNW"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LatNW" Type="Double" MinimumValue="-90.0" MaximumValue="90.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -90 e 90 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />


            <label for="sel1">Longitude Norte-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongNW"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LongNW" Type="Double" MinimumValue="-180.0" MaximumValue="180.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -180 e 180 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
        </div>

        <div class="form-group">
            <label for="sel1">Latitude Sul-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatSE"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LatSE" Type="Double" MinimumValue="-90.0" MaximumValue="90.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -90 e 90 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
            
            <label for="sel1">Longitude Sul-Leste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongSE"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LongSE" Type="Double" MinimumValue="-180.0" MaximumValue="180.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -180 e 180 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
        </div>
        
        <div class="form-group">
            <label for="sel1">Latitude Sul-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Latitude" Width="200" ID="LatSW"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LatSW" Type="Double" MinimumValue="-90.0" MaximumValue="90.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -90 e 90 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
            
            <label for="sel1">Longitude Sul-Oeste: </label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Longitude" Width="200" ID="LongSW"></asp:TextBox>
            <asp:RangeValidator  runat="server" ControlToValidate="LongSW" Type="Double" MinimumValue="-180.0" MaximumValue="180.0" ForeColor="Red" ErrorMessage="Apenas decimais entre -180 e 180 são aceitos"  SetFocusOnError="true"></asp:RangeValidator>
            <br />
        </div>

        <asp:Button runat="server" CssClass="btn btn-primary btn-block" Width="150" Text="Salvar" ID="SaveButton" OnClick="SaveButton_Click" />
    </div>
</asp:Content>
