<%@ Page Title="Transferencias" Language="C#" MasterPageFile="~/MasterPages/Portal.Master"
AutoEventWireup="true" Async="true" CodeBehind="PTL9_Transfer.aspx.cs"
Inherits="PagosMovilesWeb.Portal.PTL9_Transfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<div class="container-fluid py-4 px-3">

    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Transferencia SINPE Móvil</h1>
            <p class="mb-0 text-muted">Realice transferencias a otros usuarios</p>
        </div>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" CssClass="alert alert-info shadow-sm mb-4" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card border-0 shadow-sm">
        <div class="card-header bg-primary text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-cash-coin me-2"></i>Datos de Transferencia
            </h5>
        </div>

        <div class="card-body p-4">
            <div class="row g-3">

                <div class="col-lg-6">
                    <label class="form-label fw-semibold">Teléfono origen</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-phone"></i></span>
                        <asp:TextBox ID="txtTelefonoOrigen" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="col-lg-6">
                    <label class="form-label fw-semibold">Nombre origen</label>
                    <asp:TextBox ID="txtNombreOrigen" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="col-lg-6">
                    <label class="form-label fw-semibold">Teléfono destino</label>
                    <div class="input-group">
                        <span class="input-group-text"><i class="bi bi-phone-fill"></i></span>
                        <asp:TextBox ID="txtTelefonoDestino" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="col-lg-6">
                    <label class="form-label fw-semibold">Monto</label>
                    <div class="input-group">
                        <span class="input-group-text">₡</span>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="col-12">
                    <label class="form-label fw-semibold">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12">
                    <label class="form-label fw-semibold">Entidad destino (si es externo)</label>
                    <asp:TextBox ID="txtEntidadDestino" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 mt-4">
                    <asp:Button ID="btnTransferir" runat="server"
                        Text="Realizar Transferencia"
                        CssClass="btn btn-success btn-lg shadow-sm px-5"
                        OnClick="btnTransferir_Click" />
                </div>

            </div>
        </div>
    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>