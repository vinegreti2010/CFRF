<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Front.Pages.ForgotPassword" %>

<!DOCTYPE html>
<html lang="en">
    <head runat="server">
        <title>Esqueceu sua senha?</title>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="">
        <meta name="author" content="">
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
            <div class="card-header">
                Recuperar Senha
            </div>
            <div class="card-body">
                <div class="text-center mt-4 mb-5">
                    <h4>Esqueceu sua senha?</h4>
                    <p>Informe seu usuário e enviaremos por email instruções de como recuperá-la.</p>
                </div>
                <form runat="server">
                    <asp:Label runat="server" ID="msgFld" Font-Size="Large" Visible="false"></asp:Label>
                    <div class="form-group">
                        <asp:TextBox CssClass="form-control" ID="usernameFld" placeholder="Usuário" MaxLength="20" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="tryRecoveryPass" Text="Recuperar Senha" CssClass="btn btn-primary btn-block" runat="server" OnClick="TryRecoveryPass_Click"/>
                </form>
                <div class="text-center">
                    <a class="d-block small mt-3" href="Login.aspx">Página de Login</a>
                </div>
            </div>
            </div>
        </div>
    </body>

</html>
