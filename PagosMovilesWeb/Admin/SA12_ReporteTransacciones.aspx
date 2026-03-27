<%@ Page Title="Reporte Transacciones Diarias" Language="C#"
    MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true"
    Async="true"
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
        <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-danger shadow-sm mb-4" Visible="false">
            <i class="bi bi-exclamation-circle-fill me-2"></i>
            <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold"></asp:Label>
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

        <!-- TABLA RESULTADOS -->
        <asp:Panel ID="pnlResultados" runat="server" Visible="false">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-primary text-white py-3">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="mb-0 fw-bold">
                                <i class="bi bi-table me-2"></i>Transacciones Registradas
                            </h5>
                            <small class="opacity-75">
                                Fecha: <asp:Label ID="lblFechaConsultada" runat="server" CssClass="fw-bold" />
                            </small>
                        </div>
                        <span class="badge bg-light text-primary fs-6 px-3 py-2">
                            Total: &#8353; <asp:Label ID="lblTotalDia" runat="server" CssClass="fw-bold" />
                        </span>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvTransacciones" runat="server"
                        CssClass="table table-hover align-middle mb-0"
                        AutoGenerateColumns="false"
                        EmptyDataText="No se encontraron transacciones para la fecha indicada.">
                        <HeaderStyle CssClass="fw-bold border-bottom" />
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
            </div>
        </asp:Panel>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
