<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Layout.Master" AutoEventWireup="true" CodeBehind="Charts.aspx.cs" Inherits="Front.Pages.Charts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Graficos</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="container-fluid">
        <h1>Gráficos</h1>
        <hr>
    
        <div class="card mb-3">
            <div class="card-header">
                <i class="fa fa-area-chart"></i> Acurácia
            </div>
            <div class="card-body">
                <canvas id="myAreaChart" width="100%" height="30"></canvas>
            </div>
        </div>
        
        <div class="row">
            <div class="col-lg-8">
                <div class="card mb-3">
                    <div class="card-header">
                        <i class="fa fa-bar-chart"></i> Tempo Medio de Execução
                    </div>
                    <div class="card-body">
                        <canvas id="myBarChart" width="100" height="50"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../Scripts/vendor/chart.js/Chart.min.js"></script>
    <script src="../Scripts/js/sb-admin-charts.min.js"></script>
</asp:Content>
