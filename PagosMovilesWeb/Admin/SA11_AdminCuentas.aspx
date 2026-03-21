<%@ Page Title="Administración Cuentas" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true" Async="true" CodeBehind="SA11_AdminCuentas.aspx.cs" 
Inherits="PagosMovilesWeb.Admin.SA11_AdminCuentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    
    <style>
        .header-blue { background-color: #0d6efd !important; }
    </style>
    
    <div class="container-fluid py-4 px-3">
        <!-- PAGE HEADER -->
        <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
            <div>
                <h1 class="h3 mb-1 fw-bold text-dark">Gestión de Cuentas</h1>
                <p class="mb-0 text-muted">Administre las cuentas del sistema</p>
            </div>
            <asp:Button ID="btnNuevaCuenta" runat="server" Text="Nueva Cuenta" 
                       CssClass="btn btn-primary btn-lg shadow-sm px-4" 
                       PostBackUrl="~/Admin/SA11_FormCuenta.aspx" />
        </div>

        <!-- SUCCESS MESSAGE -->
        <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-success alert-dismissible fade show shadow-sm mb-4" Visible="false">
            <i class="bi bi-check-circle-fill me-2"></i>
            <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </asp:Panel>

        <!-- SEARCH BY ACCOUNT -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-header header-blue text-white py-3">
                <h5 class="mb-0 fw-bold"><i class="bi bi-search me-2"></i>Buscar por Número de Cuenta</h5>
            </div>
            <div class="card-body p-4">
                <div class="row g-3 align-items-end">
                    <div class="col-lg-6">
                        <label class="form-label fw-semibold">Número de Cuenta</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-credit-card"></i></span>
                            <asp:TextBox ID="txtBuscarCuenta" runat="server" CssClass="form-control" 
                                        placeholder="Ingrese número de cuenta"/>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <asp:Button ID="btnBuscarCuenta" runat="server" Text="Buscar" 
                                   CssClass="btn btn-primary btn-lg w-100" OnClick="btnBuscarCuenta_Click"/>
                    </div>
                    <div class="col-lg-3">
                        <button type="button" class="btn btn-outline-secondary btn-lg w-100" onclick="limpiarBusquedaCuenta()">
                            <i class="bi bi-arrow-clockwise"></i> Limpiar
                        </button>
                    </div>
                </div>
                <asp:Panel ID="pnlResultadoBusqueda" runat="server" CssClass="mt-4" Visible="false">
                    <hr class="my-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvResultadoBusqueda" runat="server" AutoGenerateColumns="false"
                                     CssClass="table table-hover table-striped mb-0"
                                     EmptyDataText="No se encontraron cuentas">
                            <Columns>
                                <asp:BoundField DataField="ClienteId" HeaderText="Cliente ID" />
                                <asp:BoundField DataField="NumeroCuenta" HeaderText="N° Cuenta" />
                                <asp:BoundField DataField="TipoCuenta" HeaderText="Tipo" />
                                <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="FechaCreacion" HeaderText="Creada" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                            <HeaderStyle CssClass="table-dark" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <!-- SEARCH BY CLIENT -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-header header-blue text-white py-3">
                <h5 class="mb-0 fw-bold"><i class="bi bi-people me-2"></i>Buscar Cuentas por Cliente</h5>
            </div>
            <div class="card-body p-4">
                <div class="row g-3 align-items-end">
                    <div class="col-lg-6">
                        <label class="form-label fw-semibold">Identificación Cliente</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-person-badge"></i></span>
                            <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control" 
                                        placeholder="Ingrese identificación"/>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <asp:Button ID="btnBuscarCliente" runat="server" Text="Buscar" 
                                   CssClass="btn btn-primary btn-lg w-100" OnClick="btnBuscarCliente_Click"/>
                    </div>
                    <div class="col-lg-3">
                        <button type="button" class="btn btn-outline-secondary btn-lg w-100" onclick="limpiarBusquedaCliente()">
                            <i class="bi bi-arrow-clockwise"></i> Limpiar
                        </button>
                    </div>
                </div>
                <asp:Panel ID="pnlCuentasCliente" runat="server" CssClass="mt-4" Visible="false">
                    <hr class="my-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCuentasCliente" runat="server" AutoGenerateColumns="false"
                                     CssClass="table table-hover table-striped mb-0"
                                     EmptyDataText="No se encontraron cuentas para este cliente">
                            <Columns>
                                <asp:BoundField DataField="ClienteId" HeaderText="Cliente ID" />
                                <asp:BoundField DataField="NumeroCuenta" HeaderText="N° Cuenta" />
                                <asp:BoundField DataField="TipoCuenta" HeaderText="Tipo" />
                                <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="FechaCreacion" HeaderText="Creada" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                            <HeaderStyle CssClass="table-dark" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <!-- ACCOUNTS LIST WITH ACTIONS -->
        <div class="card border-0 shadow-sm">
            <div class="card-header header-blue text-white py-3">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-list-ul me-2"></i>Todas las Cuentas
                        </h5>
                        <small class="opacity-75">Listado completo del sistema</small>
                    </div>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvCuentas" runat="server" AutoGenerateColumns="false"
                             CssClass="table table-hover align-middle mb-0"
                             AllowPaging="true" PageSize="10"
                             OnPageIndexChanging="gvCuentas_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ClienteId" HeaderText="Cliente ID" />
                        <asp:BoundField DataField="NumeroCuenta" HeaderText="N° Cuenta" />
                        <asp:BoundField DataField="TipoCuenta" HeaderText="Tipo" />
                        <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="FechaCreacion" HeaderText="Creada" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="w-1 text-center">
                            <ItemTemplate>
                                <div class="btn-group" role="group">

                                    <asp:LinkButton ID="lnkEditar" runat="server" 
                                       CssClass="btn btn-outline-primary btn-sm me-1"
                                       CommandArgument='<%# Eval("ClienteId") + "_" + Eval("NumeroCuenta") %>'
                                       OnCommand="lnkEditar_Command">
                                        <i class="bi bi-pencil"></i> Editar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="lnkEliminar" runat="server" 
                                                   CssClass="btn btn-outline-danger btn-sm"
                                                   CommandArgument='<%# Eval("ClienteId") + "|" + Eval("NumeroCuenta") %>'
                                                   OnClientClick='return confirm("¿Realmente desea eliminar el elemento seleccionado?\n\nCuenta: <%# Eval("NumeroCuenta") %> (Cliente: <%# Eval("ClienteId") %>)");'
                                                   OnClick="lnkEliminarCuenta_Click">
                                        <i class="bi bi-trash"></i> Eliminar
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5 text-muted">
                            <p class="mb-0">No hay cuentas registradas</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function limpiarBusquedaCuenta() {
            document.getElementById('<%= txtBuscarCuenta.ClientID %>').value = '';
            __doPostBack('<%= btnBuscarCuenta.UniqueID %>', '');
            document.getElementById('<%= pnlResultadoBusqueda.ClientID %>').style.display = 'none';
        }
        function limpiarBusquedaCliente() {
            document.getElementById('<%= txtBuscarCliente.ClientID %>').value = '';
            __doPostBack('<%= btnBuscarCliente.UniqueID %>', '');
            document.getElementById('<%= pnlCuentasCliente.ClientID %>').style.display = 'none';
        }
    </script>
</asp:Content>