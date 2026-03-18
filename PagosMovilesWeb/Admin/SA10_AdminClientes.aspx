<%@ Page Title="Administración Clientes" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true" Async="true" CodeBehind="SA10_AdminClientes.aspx.cs" 
Inherits="PagosMovilesWeb.Admin.SA10_AdminClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    
    <div class="container-fluid py-4 px-3">
        <!-- PAGE HEADER -->
        <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
            <div>
                <h1 class="h3 mb-1 fw-bold text-dark">Gestión de Clientes</h1>
                <p class="mb-0 text-muted">Administre clientes del sistema</p>
            </div>
            <asp:Button ID="btnNuevoCliente" runat="server" Text="Nuevo Cliente" 
                       CssClass="btn btn-primary btn-lg shadow-sm px-4" 
                       PostBackUrl="~/Admin/SA10_FormCliente.aspx" />
        </div>

        <!-- SUCCESS MESSAGE -->
        <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-success alert-dismissible fade show shadow-sm mb-4" Visible="false">
            <i class="bi bi-check-circle-fill me-2"></i>
            <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </asp:Panel>

        <!-- SEARCH SECTION -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-header bg-primary text-white py-3">
                <h5 class="mb-0 fw-bold">
                    <i class="bi bi-search me-2"></i>Buscar Cliente
                </h5>
            </div>
            <div class="card-body p-4">
                <div class="row g-3 align-items-end">
                    <div class="col-lg-6">
                        <label class="form-label fw-semibold">Identificación</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-person-badge"></i></span>
                            <asp:TextBox ID="txtBuscarIdentificacion" runat="server" CssClass="form-control" 
                                        placeholder="Ingrese cédula o pasaporte"/>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                   CssClass="btn btn-primary btn-lg w-100" OnClick="btnBuscar_Click"/>
                    </div>
                    <div class="col-lg-3">
                        <button type="button" class="btn btn-outline-secondary btn-lg w-100" onclick="limpiarBusqueda()">
                            <i class="bi bi-arrow-clockwise"></i> Limpiar
                        </button>
                    </div>
                </div>

                <!-- SEARCH RESULTS -->
                <asp:Panel ID="pnlResultados" runat="server" CssClass="mt-4" Visible="false">
                    <hr class="my-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvResultadoBusqueda" runat="server" 
                                     AutoGenerateColumns="true"
                                     CssClass="table table-hover table-striped mb-0"
                                     EmptyDataText="No se encontraron clientes">
                            <HeaderStyle CssClass="table-dark" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <!-- CLIENTS LIST -->
        <div class="card border-0 shadow-sm">
            <div class="card-header bg-primary text-white py-3">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h5 class="mb-0 fw-bold">
                            <i class="bi bi-people-fill me-2"></i>Clientes Registrados
                        </h5>
                        <small class="opacity-75">Listado completo del sistema</small>
                    </div>
                    <span class="badge bg-light text-primary fs-6 px-3 py-2">
                        <asp:Label ID="lblTotalClientes" runat="server" Text="0" CssClass="fw-bold"></asp:Label> clientes
                    </span>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvClientes" runat="server" 
                             AutoGenerateColumns="false"
                             CssClass="table table-hover align-middle mb-0"
                             AllowPaging="true" PageSize="10"
                             OnPageIndexChanging="gvClientes_PageIndexChanging"
                             OnRowCommand="gvClientes_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="identificacion" HeaderText="ID" ItemStyle-CssClass="fw-semibold" />
                        <asp:TemplateField HeaderText="Nombre Completo">
                            <ItemTemplate>
                                <strong><%# Eval("nombre") %> <%# Eval("apellido") %></strong>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:BoundField DataField="fecha_nacimiento" HeaderText="Nacimiento" 
                                       DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="w-1 text-center">
                            <ItemTemplate>
                                <div class="btn-group" role="group">
                                    <asp:LinkButton ID="lnkEditar" runat="server" 
                                                   CssClass="btn btn-outline-primary btn-sm me-1"
                                                   CommandName="Editar"
                                                   CommandArgument='<%# Eval("identificacion") %>'
                                                   PostBackUrl='<%# "~/Admin/SA10_FormCliente.aspx?edit=" + Eval("identificacion") %>'>
                                        <i class="bi bi-pencil"></i> Editar
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="lnkEliminar" runat="server" 
                                       CssClass="btn btn-outline-danger btn-sm"
                                       CommandArgument='<%# Eval("identificacion") %>'
                                       OnClientClick='return confirm("¿Realmente desea eliminar el elemento seleccionado?\n\nID: <%# Eval("identificacion") %>");'
                                       OnClick="lnkEliminar_Click">
                                    <i class="bi bi-trash"></i> Eliminar
                                </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5 text-muted">
                            <p class="mb-0">No hay clientes registrados</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function limpiarBusqueda() {
            document.getElementById('<%= txtBuscarIdentificacion.ClientID %>').value = '';
            __doPostBack('<%= btnBuscar.UniqueID %>', '');
        }
    </script>
</asp:Content>