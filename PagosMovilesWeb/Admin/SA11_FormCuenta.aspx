<%@ Page Title="Formulario Cuenta" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true" Async="true" CodeBehind="SA11_FormCuenta.aspx.cs" 
Inherits="PagosMovilesWeb.Admin.SA11_FormCuenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    
    <div class="container-fluid py-4 px-3">
        <!-- PAGE HEADER -->
        <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
            <div>
                <h1 class="h3 mb-1 fw-bold text-dark">
                    <asp:Label ID="lblTitulo" runat="server" Text="Nueva Cuenta"></asp:Label>
                </h1>
                <p class="mb-0 text-muted">
                    <asp:Label ID="lblSubtitulo" runat="server" Text="Complete los datos de la nueva cuenta"></asp:Label>
                </p>
            </div>
            <a href="SA11_AdminCuentas.aspx" class="btn btn-secondary btn-lg shadow-sm px-4">
                <i class="bi bi-arrow-left"></i> Volver
            </a>
        </div>

        <!-- ERROR MESSAGE (solo errores) -->
        <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger alert-dismissible fade show shadow-sm mb-4" Visible="false">
            <i class="bi bi-exclamation-triangle-fill me-2"></i>
            <asp:Label ID="lblError" runat="server" CssClass="fw-semibold"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </asp:Panel>

        <!-- FORM -->
        <div class="card border-0 shadow-lg">
    <div class="card-body p-5">
        <div class="row g-4">
            <div class="col-md-4">
                <label class="form-label fw-bold">Identificación Cliente <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text"><i class="bi bi-person-badge"></i></span>
                    <asp:TextBox ID="txtClienteId" runat="server" CssClass="form-control" 
                                placeholder="Ej: 101000001" MaxLength="20" />
                </div>
                <asp:RequiredFieldValidator ID="rfvClienteId" runat="server" 
                                           ControlToValidate="txtClienteId"
                                           ErrorMessage="Requerido" CssClass="text-danger small" 
                                           Display="Dynamic" />
            </div>
            
            <div class="col-md-4">
                <label class="form-label fw-bold">
                    <asp:Label ID="lblNumeroCuenta" runat="server" Text="Número Cuenta"></asp:Label>
                    <asp:Label ID="lblAsteriscoNumero" runat="server" CssClass="text-danger" Text="" />
                </label>
                <div class="input-group">
                    <span class="input-group-text"><i class="bi bi-credit-card"></i></span>
                    <asp:TextBox ID="txtNumeroCuenta" runat="server" CssClass="form-control" 
                                placeholder="Ej: AH00001" MaxLength="20" />
                </div>
            </div>
            
            <div class="col-md-4">
                <label class="form-label fw-bold">Tipo de Cuenta <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlTipoCuenta" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="-- Seleccione --"></asp:ListItem>
                    <asp:ListItem Value="Ahorros">Ahorros</asp:ListItem>
                    <asp:ListItem Value="Corriente">Corriente</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTipoCuenta" runat="server" 
                                           ControlToValidate="ddlTipoCuenta" InitialValue=""
                                           ErrorMessage="Requerido" CssClass="text-danger small" 
                                           Display="Dynamic" />
            </div>
        </div>
        
        <hr class="my-4">
        
        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
            <a href="SA11_AdminCuentas.aspx" class="btn btn-outline-secondary px-4 me-md-2">
                <i class="bi bi-x-circle"></i> Cancelar
            </a>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cuenta" 
                       CssClass="btn btn-primary px-4 shadow-sm" 
                       OnClick="btnGuardar_Click" />
        </div>
    </div>
</div>

    <!-- ✅ MODAL DE ÉXITO -->
    <div class="modal fade" id="modalSuccess" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow-lg">
                <div class="modal-header bg-success text-white border-0">
                    <h5 class="modal-title fw-bold mb-0">
                        <i class="bi bi-check-circle-fill me-2"></i>¡Operación Exitosa!
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body text-center py-4">
                    <i class="bi bi-check-circle display-1 text-success mb-3 opacity-75"></i>
                    <h4 class="fw-bold text-success mb-2">
                        <asp:Label ID="lblModalMensaje" runat="server" Text="Cuenta creada correctamente"></asp:Label>
                    </h4>
                    <p class="text-muted mb-0">Redirigiendo a la lista principal...</p>
                </div>
                <div class="modal-footer border-0 justify-content-center">
                    <a href="SA11_AdminCuentas.aspx" class="btn btn-success btn-lg px-4 shadow-sm">
                        <i class="bi bi-list-ul me-2"></i>Volver a Lista
                    </a>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>