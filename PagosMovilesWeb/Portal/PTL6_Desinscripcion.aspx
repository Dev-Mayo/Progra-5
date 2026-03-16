<%@ Page Title="Desinscripción - Pagos Móviles" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL6_Desinscripcion.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL6_Desinscripcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    // Solo dígitos para teléfono
    function soloDigitos(e) {
        return e.charCode >= 48 && e.charCode <= 57;
    }
    // Solo letras y números para número de cuenta
    function soloAlfanumerico(e) {
        var c = e.charCode;
        return (c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122);
    }
</script>

<div class="welcome-card">
    <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />
    <h1>Desinscripción de Pagos Móviles</h1>
    <p>Ingrese los datos para desasociar su teléfono del servicio de pagos móviles.</p>
</div>

<br />

<h3>Datos de Desinscripción</h3>

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
    </tr>
    <tr>
        <td>Número de Cuenta:</td>
        <td>
            <asp:TextBox ID="txtCuenta" runat="server"
                MaxLength="50"
                onkeypress="return soloAlfanumerico(event);"
                onpaste="return false;" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvCuenta" runat="server"
                ControlToValidate="txtCuenta"
                ErrorMessage="El número de cuenta es obligatorio."
                ForeColor="Red"
                Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <br />
            <asp:Button ID="btnDesinscribir" runat="server"
                Text="Desinscribirse"
                OnClick="btnDesinscribir_Click"
                OnClientClick="return confirm('¿Está seguro que desea desinscribirse del servicio de pagos móviles?');" />
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

<%-- Mensaje de éxito --%>
<asp:Panel ID="pnlExito" runat="server" Visible="false">
    <p style="color:green; font-weight:bold;">
        <asp:Label ID="lblExito" runat="server" />
    </p>
</asp:Panel>

</asp:Content>
