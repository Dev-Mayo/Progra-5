<%@ Page Title="Consulta de Saldo" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL7_ConsultaSaldo.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL7_ConsultaSaldo" %>

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
            <h1 class="h3 mb-1 fw-bold text-dark">Consulta de Saldo</h1>
            <p class="mb-0 text-muted">Consulte el saldo disponible de su cuenta</p>
        </div>
    </div>

    <!-- MENSAJE ERROR -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- FORMULARIO -->
    <div class="row justify-content-center">
        <div class="col-lg-6">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-primary text-white py-3">
                    <h5 class="mb-0 fw-bold">
                        <i class="bi bi-wallet2 me-2"></i>Datos de Consulta
                    </h5>
                </div>
                <div class="card-body p-4">
                    <div class="mb-4">
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
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar Saldo"
                        CssClass="btn btn-primary btn-lg shadow-sm px-5 w-100"
                        OnClick="btnConsultar_Click" />
                </div>
            </div>

            <!-- RESULTADO -->
            <asp:Panel ID="pnlResultado" runat="server" Visible="false">
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-success text-white py-3">
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-check-circle-fill me-2"></i>Información de Cuenta
                        </h5>
                    </div>
                    <div class="card-body text-center p-4">
                        <p class="mb-1">
                            <span class="fw-semibold">Número de cuenta: </span>
                            <asp:Label ID="lblNumeroCuenta" runat="server" />
                        </p>
                        <p class="mb-3">
                            <span class="fw-semibold">Teléfono: </span>
                            <asp:Label ID="lblTelefono" runat="server" />
                        </p>
                        <hr />
                        <h5 class="fw-bold text-muted mb-1">Saldo Disponible</h5>
                        <h2 class="fw-bold text-success">
                            &#8353; <asp:Label ID="lblSaldo" runat="server" />
                        </h2>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
