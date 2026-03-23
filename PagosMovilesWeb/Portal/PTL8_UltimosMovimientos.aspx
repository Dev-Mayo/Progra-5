<%@ Page Title="Últimos Movimientos" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL8_UltimosMovimientos.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL8_UltimosMovimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    function soloDigitos(e) { return e.charCode >= 48 && e.charCode <= 57; }
</script>

<div class="container-fluid py-4 px-3">

    <!-- PAGE HEADER -->
    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Últimos 5 Movimientos</h1>
            <p class="mb-0 text-muted">Consulte los movimientos recientes de su cuenta</p>
        </div>
    </div>

    <!-- MENSAJE -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- FORMULARIO -->
    <div class="card border-0 shadow-sm mb-4">
        <div class="card-header bg-primary text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-clock-history me-2"></i>Datos de Consulta
            </h5>
        </div>
        <div class="card-body p-4">
            <div class="row g-3 align-items-end">
                <div class="col-lg-6">
                    <label class="form-label fw-semibold">Número de Teléfono</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-phone"></i></span>
                        <asp:TextBox ID="txtTelefono" runat="server"
                            CssClass="form-control"
                            placeholder="Ej: 88887777"
                            MaxLength="8"
                            onkeypress="return soloDigitos(event);"
                            onpaste="return false;" />
                    </div>
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar Movimientos"
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
                        <i class="bi bi-list-ul me-2"></i>Movimientos de la Cuenta
                    </h5>
                    <small class="opacity-75">
                        Cuenta: <asp:Label ID="lblNumeroCuenta" runat="server" />
                        &nbsp;|&nbsp;
                        Teléfono: <asp:Label ID="lblTelefono" runat="server" />
                    </small>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvMovimientos" runat="server"
                    CssClass="table table-hover align-middle mb-0"
                    AutoGenerateColumns="false"
                    EmptyDataText="No se encontraron movimientos para esta cuenta.">
                    <HeaderStyle CssClass="table-dark" />
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
                            HeaderText="Monto (&#8353;)"
                            DataFormatString="{0:N2}"
                            HtmlEncode="false" />
                        <asp:BoundField DataField="saldoAnterior"
                            HeaderText="Saldo Anterior (&#8353;)"
                            DataFormatString="{0:N2}"
                            HtmlEncode="false" />
                        <asp:BoundField DataField="saldoActual"
                            HeaderText="Saldo Actual (&#8353;)"
                            DataFormatString="{0:N2}"
                            HtmlEncode="false" />
                        <asp:BoundField DataField="descripcion"
                            HeaderText="Descripción" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
