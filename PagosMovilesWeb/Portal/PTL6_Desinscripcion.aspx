<%@ Page Title="Desinscripción Pagos Móviles" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL6_Desinscripcion.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL6_Desinscripcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    function soloDigitos(e) { return e.charCode >= 48 && e.charCode <= 57; }
    function soloAlfanumerico(e) {
        var c = e.charCode;
        return (c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122);
    }
</script>

<div class="container-fluid py-4 px-3">

    <!-- PAGE HEADER -->
    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Desinscripción de Pagos Móviles</h1>
            <p class="mb-0 text-muted">Desasociar su teléfono del servicio de pagos móviles</p>
        </div>
    </div>

    <!-- MENSAJE -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- FORMULARIO -->
    <div class="row justify-content-center">
        <div class="col-lg-6">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-warning text-dark py-3">
                    <h5 class="mb-0 fw-bold">
                        <i class="bi bi-x-circle me-2"></i>Datos de Desinscripción
                    </h5>
                </div>
                <div class="card-body p-4">
                    <div class="mb-3">
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
                    <div class="mb-4">
                        <label class="form-label fw-semibold">Número de Cuenta a Desasociar</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-credit-card"></i></span>
                            <asp:TextBox ID="txtCuenta" runat="server"
                                CssClass="form-control"
                                placeholder="Número de cuenta"
                                MaxLength="50"
                                onkeypress="return soloAlfanumerico(event);"
                                onpaste="return false;" />
                        </div>
                    </div>
                    <asp:Button ID="btnDesinscribir" runat="server"
                        Text="Desinscribirse"
                        CssClass="btn btn-warning btn-lg shadow-sm px-5 w-100"
                        OnClick="btnDesinscribir_Click"
                        OnClientClick="return confirm('¿Está seguro que desea desinscribirse del servicio de pagos móviles?');" />
                </div>
            </div>
        </div>
    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
