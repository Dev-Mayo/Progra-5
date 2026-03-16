<%@ Page Title="Consulta de Saldo" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL7_ConsultaSaldo.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL7_ConsultaSaldo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    // Solo dígitos para teléfono
    function soloDigitos(e) {
        return e.charCode >= 48 && e.charCode <= 57;
    }
</script>

<div class="welcome-card">
    <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />
    <h1>Consulta de Saldo</h1>
    <p>Ingrese su número de teléfono para consultar el saldo de su cuenta.</p>
</div>

<br />

<h3>Consultar Saldo</h3>

<table>
    <tr>
        <td>Número de Teléfono:</td>
        <td>
            <asp:TextBox ID="txtTelefono" runat="server"
                MaxLength="20"
                onkeypress="return soloDigitos(event);"
                onpaste="return false;" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                ControlToValidate="txtTelefono"
                ErrorMessage="El número de teléfono es obligatorio."
                ForeColor="Red"
                Display="Dynamic" />
        </td>
        <td>
            <asp:Button ID="btnConsultar" runat="server"
                Text="Consultar Saldo"
                OnClick="btnConsultar_Click" />
        </td>
    </tr>
</table>

<br />

<%-- Mensaje de error --%>
<asp:Panel ID="pnlError" runat="server" Visible="false">
    <p style="color:red; font-weight:bold;">
        <asp:Label ID="lblError" runat="server" />
    </p>
</asp:Panel>

<%-- Resultado del saldo --%>
<asp:Panel ID="pnlResultado" runat="server" Visible="false">

    <div class="welcome-card" style="max-width:400px; text-align:center;">
        <p>
            <strong>Número de cuenta:</strong>
            <asp:Label ID="lblNumeroCuenta" runat="server" />
        </p>
        <p>
            <strong>Teléfono:</strong>
            <asp:Label ID="lblTelefono" runat="server" />
        </p>
        <hr />
        <h3 style="color:#1f2a44;">Saldo disponible</h3>
        <h2 style="color:green;">
            <asp:Label ID="lblSaldo" runat="server" />
        </h2>
    </div>

</asp:Panel>

</asp:Content>
