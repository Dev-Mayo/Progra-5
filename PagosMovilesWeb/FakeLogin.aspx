<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FakeLogin.aspx.cs" Inherits="PagosMovilesWeb.FakeLogin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Fake Login</title>
</head>
<body>
    <form id="form1" runat="server">

        <h2>Login de prueba</h2>

        Nombre:<br />
        <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>

        <br /><br />

        Rol:<br />
        <asp:DropDownList ID="ddlRol" runat="server">
            <asp:ListItem Value="ADMIN">ADMIN</asp:ListItem>
            <asp:ListItem Value="CLIENTE">CLIENTE</asp:ListItem>
        </asp:DropDownList>

        <br /><br />

        <asp:Button ID="btnLogin" runat="server" Text="Entrar" OnClick="btnLogin_Click" />

    </form>
</body>
</html>