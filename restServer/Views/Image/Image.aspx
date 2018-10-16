<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Image.aspx.cs" Inherits="restServer.Views.Image.Image" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <body>
        <div class="content-wrapper">
            <div class="container-fluid">
                <h1>Carregar Base de Imagens</h1>
	            <p>Para treinar realizar a primeira verificação das faces dos novos alunos é necessário importar um arquivo .ZIP com as imagens dos alunos, onde o nome dos arquivos deve ser o código do respectivo aluno</p>
                <hr />
                <form id="form1" runat="server">
                    <div class="form-group">
                        <label for="exampleInputFile">Anexar arquivo</label>
                        <input class="form-control" id="exampleInputFile" type="file" accept=".zip" aria-describedby="fileHelp" placeholder="Nenhum arquivo selecionado" style="width:500px"/>
                    </div>
		            <a class="btn btn-primary btn-block" href="diario_de_frequencia.pdf" target="blank"  style="width:150px">Executar</a>
                </form>
	        </div>
	    </div>
        </body>
</html>
