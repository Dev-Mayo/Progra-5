<%@ Page Title="Reporte Transacciones Diarias" Language="C#"
    MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="SA12_ReporteTransacciones.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA12_ReporteTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    // Solo dígitos y guiones para la fecha (extra seguridad además del date picker)
    function soloFecha(e) {
        var c = e.charCode;
        return (c >= 48 && c <= 57) || c === 45;
    }
</script>

<div class="welcome-card">
    <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />
    <h1>Reporte de Transacciones Diarias</h1>
    <p>Consulte las transacciones realizadas en pagos móviles por día.</p>
</div>

<br />

<h3>Seleccionar Fecha</h3>

<table>
    <tr>
        <td>Fecha:</td>
        <td>
            <asp:TextBox ID="txtFecha" runat="server"
                TextMode="Date"
                onkeypress="return soloFecha(event);"
                onpaste="return false;" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                ControlToValidate="txtFecha"
                ErrorMessage="Debe indicar la fecha del reporte."
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

<%-- Resultados del reporte --%>
<asp:Panel ID="pnlResultados" runat="server" Visible="false">

    <h3>
        Transacciones del día:
        <asp:Label ID="lblFechaConsultada" runat="server" />
    </h3>

    <asp:GridView ID="gvTransacciones" runat="server"
        AutoGenerateColumns="false"
        EmptyDataText="No se encontraron transacciones para la fecha indicada."
        BorderWidth="1px"
        CellPadding="5"
        HeaderStyle-BackColor="#1f2a44"
        HeaderStyle-ForeColor="White">
        <Columns>
            <asp:BoundField DataField="fecha"
                HeaderText="Fecha"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="telefonoOrigen"
                HeaderText="Teléfono Origen" />
            <asp:BoundField DataField="telefonoDestino"
                HeaderText="Teléfono Destino" />
            <asp:BoundField DataField="monto"
                HeaderText="Monto"
                DataFormatString="{0:N2}"
                HtmlEncode="false" />
        </Columns>
    </asp:GridView>

    <br />

    <%-- SA12: sumatoria del total de transacciones del día --%>
    <table style="border:1px solid #ccc; padding:10px; background:#f0f4f8;">
        <tr>
            <td><strong>Total de transacciones del día:</strong></td>
            <td style="padding-left:15px; font-size:1.1em; font-weight:bold; color:#1f2a44;">
                <asp:Label ID="lblTotalDia" runat="server" />
            </td>
        </tr>
    </table>

</asp:Panel>

</asp:Content>
