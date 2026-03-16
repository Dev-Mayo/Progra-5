<%@ Page Title="Últimos Movimientos" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL8_UltimosMovimientos.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL8_UltimosMovimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    // Solo dígitos para teléfono
    function soloDigitos(e) {
        return e.charCode >= 48 && e.charCode <= 57;
    }
</script>

<div class="welcome-card">
    <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />
    <h1>Últimos 5 Movimientos</h1>
    <p>Ingrese su número de teléfono para consultar los últimos movimientos de su cuenta.</p>
</div>

<br />

<h3>Consultar Movimientos</h3>

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
                Text="Consultar"
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

<%-- Resultados --%>
<asp:Panel ID="pnlResultados" runat="server" Visible="false">

    <p>
        <strong>Cuenta: </strong><asp:Label ID="lblNumeroCuenta" runat="server" />
        &nbsp;&nbsp;
        <strong>Teléfono: </strong><asp:Label ID="lblTelefono" runat="server" />
    </p>

    <asp:GridView ID="gvMovimientos" runat="server"
        AutoGenerateColumns="false"
        EmptyDataText="No se encontraron movimientos para esta cuenta."
        BorderWidth="1px"
        CellPadding="5"
        HeaderStyle-BackColor="#1f2a44"
        HeaderStyle-ForeColor="White">
        <Columns>
            <asp:BoundField DataField="fechaMovimiento"
                HeaderText="Fecha"
                DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:TemplateField HeaderText="Tipo">
                <ItemTemplate>
                    <%# FormatearTipo(Eval("tipoMovimiento").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="monto"
                HeaderText="Monto"
                DataFormatString="{0:N2}"
                HtmlEncode="false" />
            <asp:BoundField DataField="saldoAnterior"
                HeaderText="Saldo Anterior"
                DataFormatString="{0:N2}"
                HtmlEncode="false" />
            <asp:BoundField DataField="saldoActual"
                HeaderText="Saldo Actual"
                DataFormatString="{0:N2}"
                HtmlEncode="false" />
            <asp:BoundField DataField="descripcion"
                HeaderText="Descripción" />
        </Columns>
    </asp:GridView>

</asp:Panel>

</asp:Content>
