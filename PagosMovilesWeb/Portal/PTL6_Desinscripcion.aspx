<%@ Page Title="Desinscripción Pagos Móviles" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    Async="true"
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


        function mostrarConfirmacion() {
            var telefono = document.getElementById('<%= txtTelefono.ClientID %>').value.trim();
            var cuenta = document.getElementById('<%= txtCuenta.ClientID %>').value.trim();

            if (telefono === '' || telefono.length !== 8) {
                return false; 
            }
            if (cuenta === '') {
                return false; 
            }

            var modal = new bootstrap.Modal(document.getElementById('modalConfirmar'));
            modal.show();
            return false; 
        }

        
        function confirmarSi() {
            var modal = bootstrap.Modal.getInstance(document.getElementById('modalConfirmar'));
            modal.hide();
            
            setTimeout(function () {
                __doPostBack('<%= btnDesinscribir.UniqueID %>', '');
            }, 300);
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
        <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-danger shadow-sm mb-4" Visible="false">
            <i class="bi bi-exclamation-circle-fill me-2"></i>
            <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold"></asp:Label>
        </asp:Panel>

        <!-- FORMULARIO -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-header bg-primary text-white py-3">
                <h5 class="mb-0 fw-bold">
                    <i class="bi bi-x-circle me-2"></i>Datos de Desinscripción
                </h5>
            </div>
            <div class="card-body p-4">
                <div class="row g-3">
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
                    <div class="col-lg-6">
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
                    <div class="col-lg-4">
                        
                        <asp:Button ID="btnDesinscribir" runat="server"
                            Text="Desinscribirse"
                            CssClass="btn btn-primary btn-lg shadow-sm px-4 w-100"
                            OnClick="btnDesinscribir_Click"
                            OnClientClick="return mostrarConfirmacion();" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    
    <div class="modal fade" id="modalConfirmar" tabindex="-1" aria-labelledby="modalConfirmarLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title fw-bold" id="modalConfirmarLabel">
                        </>Confirmar desinscripción
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body py-4 text-center">
                 
                    <p class="mt-3 mb-0 fs-5">¿Está seguro que desea desinscribirse del servicio de pagos móviles?</p>
                </div>
                <div class="modal-footer justify-content-center gap-3">
                    <button type="button"
                        class="btn btn-outline-secondary btn-lg px-5"
                        data-bs-dismiss="modal">
                        <i class="bi bi-x-lg me-1"></i> No
                    </button>
                    <button type="button"
                        class="btn btn-danger btn-lg px-5"
                        onclick="confirmarSi();">
                        <i class="bi bi-check-lg me-1"></i> Sí
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
