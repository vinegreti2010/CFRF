<%@ Page Title="" Language="C#" MasterPageFile="~/Models/Layout.Master" AutoEventWireup="true" CodeBehind="RunClassDiary.aspx.cs" Inherits="Front.Pages.RunClassDiary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Diário de Classe</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <h1>Diário de Classe</h1>
        <hr>

        <div class="form-group">
            <asp:Label runat="server" ID="Msg"></asp:Label>
            <br />

            <label for="sel1">Período Letivo</label>

            <asp:SqlDataSource runat="server" ID="StrmDataSource" ConnectionString="<%$ ConnectionStrings:CFRFApp_dbConnectionString %>" SelectCommand="SELECT [strm], [descr] FROM [term_tbl] ORDER BY [strm]"></asp:SqlDataSource>
            <asp:DropDownList runat="server" ID="StrmDropDown" AutoPostBack="True" CssClass="form-control" Width="400px" DataSourceID="StrmDataSource" DataTextField="descr" DataValueField="strm"></asp:DropDownList>

            <label for="sel1">Aula</label>

            <asp:SqlDataSource runat="server" ID="ClassDataSource" ConnectionString="<%$ ConnectionStrings:CFRFApp_dbConnectionString %>" SelectCommand="SELECT DISTINCT [class_nbr], [descr] FROM [class_tbl] WHERE ([strm] = @strm) ORDER BY [descr]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="StrmDropDown" Name="strm" PropertyName="SelectedValue" Type="String"/>
                </SelectParameters>
            </asp:SqlDataSource>
           <%-- <asp:SqlDataSource runat="server" ID="ClassDataSource" ConnectionString="<%$ ConnectionStrings:CFRFApp_dbConnectionString %>" SelectCommand="SELECT DISTINCT [class_nbr], [descr], [strm] FROM [class_tbl] ORDER BY [descr]" FilterExpression="strm = '{0}'">
                <FilterParameters>
                    <asp:ControlParameter ControlID="StrmDropDown" Name="strm" PropertyName="SelectedValue" />
                </FilterParameters>
            </asp:SqlDataSource>--%>
            <asp:DropDownList runat="server" ID="ClassDropDownList" CssClass="form-control" Width="400" DataSourceID="ClassDataSource" DataTextField="descr" DataValueField="class_nbr"></asp:DropDownList>
        </div>
        <div>
            <asp:Button runat="server" ID="TryRunDiary" Width="150" CssClass="btn btn-primary btn-block" Text="Executar" OnClick="TryRunDiary_Click" />
        </div>
    </div>
</asp:Content>
