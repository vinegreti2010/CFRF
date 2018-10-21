<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Layout.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Front.Pages.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Inicio</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="container-fluid">
        <h1>Inicio</h1>
        <hr>
        <div class="card mb-3">
            <div class="card-header">
                <i class="fa fa-area-chart"></i> Acurácia
            </div>
            <div class="card-body">
                <canvas id="myAreaChart" width="100%" height="30"></canvas>
            </div>
        </div>
    </div>
    <!-- Page level plugin JavaScript-->
    <script src="../Scripts/vendor/chart.js/Chart.min.js"></script>
    <script src="../Scripts/js/sb-admin-charts.min.js"></script>
</asp:Content>