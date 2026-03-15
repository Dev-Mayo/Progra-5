<%@ Page Title="Formulario Cliente" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true" Async="true" CodeBehind="SA10_FormCliente.aspx.cs" 
Inherits="PagosMovilesWeb.Admin.SA10_FormCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    
    <div class="container-fluid py-4 px-3 min-vh-100">
        <!-- BREADCRUMB -->
        <nav aria-label="breadcrumb" class="mb-4">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="SA10_AdminClientes.aspx" class="text-decoration-none">Clientes</a></li>
                <li class="breadcrumb-item active fw-bold" aria-current="page">
                    <asp:Label ID="lblBreadcrumb" runat="server" Text="Nuevo Cliente"></asp:Label>
                </li>
            </ol>
        </nav>

        <!-- SUCCESS MODAL -->
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
                            <asp:Label ID="lblModalMensaje" runat="server" Text="Cliente creado correctamente"></asp:Label>
                        </h4>
                        <p class="text-muted mb-0">Redirigiendo a la lista principal...</p>
                    </div>
                    <div class="modal-footer border-0 justify-content-center">
                        <a href="SA10_AdminClientes.aspx" class="btn btn-success btn-lg px-4 shadow-sm">
                            <i class="bi bi-list-ul me-2"></i>Volver a Lista
                        </a>
                    </div>
                </div>
            </div>
        </div>

        <!-- FORM CARD -->
        <div class="row justify-content-center">
            <div class="col-lg-8 col-xl-6">
                <div class="card border-0 shadow-lg">
                    <div class="card-header bg-primary text-white py-4 text-center">
                        <i class="bi bi-person-plus-fill fs-1 opacity-75 mb-3 d-block"></i>
                        <h3 class="mb-1 fw-bold">
                            <asp:Label ID="lblFormTitle" runat="server" Text="Nuevo Cliente"></asp:Label>
                        </h3>
                        <small class="opacity-90">Complete todos los campos requeridos</small>
                    </div>
                    <div class="card-body p-4 p-md-5">
                        <div class="row g-4">
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Identificación <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control form-control-lg" 
                                            placeholder="1234567890" required="true"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Tipo ID <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlTipoIdentificacion" runat="server" CssClass="form-select form-select-lg" required="true">
                                    <asp:ListItem Value="">-- Seleccione --</asp:ListItem>
                                    <asp:ListItem Value="1">Cédula</asp:ListItem>
                                    <asp:ListItem Value="2">Pasaporte</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Nombre <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control form-control-lg" 
                                            placeholder="Ingrese nombre" required="true"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control-lg" 
                                            placeholder="Ingrese apellido"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-lg" 
                                            placeholder="cliente@ejemplo.com" TextMode="Email"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-lg" 
                                            placeholder="0999999999"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Fecha Nacimiento</label>
                                <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control form-control-lg" 
                                            TextMode="Date"/>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold text-dark">Contraseña</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control form-control-lg" 
                                                TextMode="Password" placeholder="********"/>
                                    <button class="btn btn-outline-secondary" type="button" onclick="togglePassword(this)">
                                        <i class="bi bi-eye" id="toggleIcon"></i>
                                    </button>
                                </div>
                            </div>
                        </div>

                        <hr class="my-4">

                        <div class="d-flex gap-3 justify-content-between flex-wrap">
                            <a href="SA10_AdminClientes.aspx" class="btn btn-outline-secondary btn-lg px-5">
                                <i class="bi bi-arrow-left me-2"></i>Cancelar
                            </a>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cliente" 
                                       CssClass="btn btn-primary btn-lg px-5 shadow-sm" 
                                       OnClick="btnGuardar_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function togglePassword(btn) {
            const passwordField = btn.parentElement.querySelector('input[type="password"]');
            const icon = document.getElementById('toggleIcon');
            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                passwordField.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        }
    </script>
</asp:Content>