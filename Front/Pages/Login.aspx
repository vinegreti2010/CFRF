<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Front.Pages.Login" %>
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="">
        <meta name="author" content="">
        <title>SB Admin - Start Bootstrap Template</title>
        <!-- Bootstrap core CSS-->
        <link href="../Scripts/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
        <!-- Custom fonts for this template-->
        <link href="../Scripts/vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
        <!-- Custom styles for this template-->
        <link href="../Scripts/css/sb-admin.css" rel="stylesheet">
    </head>

    <body class="bg-dark">
        <div class="container">
            <div class="card card-login mx-auto mt-5">
                <div class="card-header">Login</div>
                    <div class="card-body">
                        <form method="post" runat="server" id="LoginForm">
                            <asp:Label runat="server" ID="invalidFld" ForeColor="Red" Font-Size="Large" Visible="false"></asp:Label>
                            <div class="form-group">
                                <label for="username">Username</label>
                                <asp:TextBox CssClass="form-control" ID="usernameFld" placeholder="Username" MaxLength="20" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="exampleInputPassword1">Password</label>
                                <asp:TextBox CssClass="form-control" ID="passwordFld" TextMode="Password" placeholder="Password" MaxLength="20" runat="server"></asp:TextBox>
                            </div>
                            <asp:Button ID="tryLogin" Text="Login" CssClass="btn btn-primary btn-block" runat="server" OnClick="TryLogin_Click"/>
                        </form>
                    </div>
                <div class="text-center">
                    <a class="d-block small mt-3" href="forgot-password.html">Forgot Password?</a>
                </div>
            </div>
        </div>
    </body>
</html>