<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessingReport.aspx.cs" Inherits="Front.Pages.ProcessingReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Processando</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Seu relatório está sendo gerado, por favor aguarde</h1>
            <h2>Assim que o arquivo estiver pronto ele será exibido nessa página</h2>

            <asp:Label runat="server" ID="Msg" Font-Size="X-Large"></asp:Label>
        </div>
    </form>
</body>
</html>
