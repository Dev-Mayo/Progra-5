<%@ Page Title="Reporte Transacciones Diarias" Language="C#"
    MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="SA12_ReporteTransacciones.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA12_ReporteTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    // Solo dígitos y guión para fecha (por si el browser no soporta date picker)
    function soloFecha(e) {
        var c = e.charCode;
        return (c >= 48 && c <= 57) || c === 45;
    }
</script>

<div class="container-fluid py-4 px-3">

    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Reporte de Transacciones Diarias</h1>
            <p class="mb-0 text-muted">Consulte las transacciones de pagos móviles por día</p>
        </div>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card border-0 shadow-sm mb-4" style="max-width:500px;">
        <div class="card-header bg-dark text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-calendar3 me-2"></i>Seleccionar Fecha
            </h5>
        </div>
        <div class="card-body p-4">
            <div class="row g-3">
                <div class="col-12">
                    <label class="form-label fw-semibold">Fecha del reporte</label>
                    <asp:TextBox ID="txtFecha" runat="server"
                        CssClass="form-control"
                        TextMode="Date"
                        onkeypress="return soloFecha(event);"
                        onpaste="return false;" />
                </div>
                <div class="col-12 mt-2">
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar"
                        CssClass="btn btn-dark btn-lg shadow-sm px-5"
                        OnClick="btnConsultar_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- Resultados --%>
    <asp:Panel ID="pnlResultados" runat="server" Visible="false">

        <h5 class="fw-bold mb-3">
            Transacciones del día:
            <asp:Label ID="lblFechaConsultada" runat="server" CssClass="text-primary" />
        </h5>

        <div class="table-responsive">
            <asp:GridView ID="gvTransacciones" runat="server"
                CssClass="table table-bordered table-hover table-striped shadow-sm"
                AutoGenerateColumns="false"
                EmptyDataText="No se encontraron transacciones para la fecha indicada.">
                <HeaderStyle BackColor="#1f2a44" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="fecha"
                        HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField DataField="telefonoOrigen"
                        HeaderText="Teléfono Origen" />
                    <asp:BoundField DataField="telefonoDestino"
                        HeaderText="Teléfono Destino" />
                    <asp:BoundField DataField="monto"
                        HeaderText="Monto (₡)"
                        DataFormatString="{0:N2}"
                        HtmlEncode="false" />
                </Columns>
            </asp:GridView>
        </div>

        <%-- SA12: sumatoria total del día --%>
        <div class="card border-0 shadow-sm mt-3" style="max-width:350px;">
            <div class="card-body text-center py-3">
                <h5 class="fw-bold mb-1">Total de transacciones del día</h5>
                <h3 class="text-primary fw-bold">
                    ₡ <asp:Label ID="lblTotalDia" runat="server" />
                </h3>
            </div>
        </div>

    </asp:Panel>

</div>

</asp:Content>
