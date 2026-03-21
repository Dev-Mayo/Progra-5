<%@ Page Title="Reporte Transacciones Diarias" Language="C#"
    MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="SA12_ReporteTransacciones.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA12_ReporteTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    function soloFecha(e) {
        var c = e.charCode;
        return (c >= 48 && c <= 57) || c === 45;
    }
</script>

<div class="container-fluid py-4 px-3">

    <!-- PAGE HEADER -->
    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Reporte de Transacciones Diarias</h1>
            <p class="mb-0 text-muted">Consulte las transacciones de pagos móviles por día</p>
        </div>
    </div>

    <!-- MENSAJE -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- FILTRO FECHA -->
    <div class="card border-0 shadow-sm mb-4">
        <div class="card-header bg-primary text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-calendar3 me-2"></i>Seleccionar Fecha
            </h5>
        </div>
        <div class="card-body p-4">
            <div class="row g-3 align-items-end">
                <div class="col-lg-4">
                    <label class="form-label fw-semibold">Fecha del reporte</label>
                    <asp:TextBox ID="txtFecha" runat="server"
                        CssClass="form-control"
                        TextMode="Date"
                        onkeypress="return soloFecha(event);"
                        onpaste="return false;" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar"
                        CssClass="btn btn-primary btn-lg w-100"
                        OnClick="btnConsultar_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- RESULTADOS -->
    <asp:Panel ID="pnlResultados" runat="server" Visible="false">
        <div class="card border-0 shadow-sm">
            <div class="card-header bg-primary text-white py-3">
                <div class="d-flex justify-content-between align-items-center">
                    <h5 class="mb-0 fw-bold">
                        <i class="bi bi-table me-2"></i>Transacciones del día:
                        <asp:Label ID="lblFechaConsultada" runat="server" CssClass="fw-bold" />
                    </h5>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvTransacciones" runat="server"
                    CssClass="table table-hover align-middle mb-0"
                    AutoGenerateColumns="false"
                    EmptyDataText="No se encontraron transacciones para la fecha indicada.">
                    <HeaderStyle CssClass="table-dark" />
                    <Columns>
                        <asp:BoundField DataField="fecha"
                            HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="telefonoOrigen"
                            HeaderText="Teléfono Origen" />
                        <asp:BoundField DataField="telefonoDestino"
                            HeaderText="Teléfono Destino" />
                        <asp:BoundField DataField="monto"
                            HeaderText="Monto (&#8353;)"
                            DataFormatString="{0:N2}"
                            HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
            <!-- TOTAL -->
            <div class="card-footer bg-light p-3">
                <div class="d-flex justify-content-end align-items-center gap-3">
                    <h5 class="mb-0 fw-bold text-muted">Total de transacciones del día:</h5>
                    <h4 class="mb-0 fw-bold text-primary">
                        &#8353; <asp:Label ID="lblTotalDia" runat="server" />
                    </h4>
                </div>
            </div>
        </div>
    </asp:Panel>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
