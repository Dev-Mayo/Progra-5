<%@ Page Title="Consulta de Saldo" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL7_ConsultaSaldo.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL7_ConsultaSaldo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    // Solo dígitos — no deja escribir letras en el teléfono
    function soloDigitos(e) {
        return e.charCode >= 48 && e.charCode <= 57;
    }
</script>

<div class="container-fluid py-4 px-3">

    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Consulta de Saldo</h1>
            <p class="mb-0 text-muted">Consulte el saldo disponible de su cuenta</p>
        </div>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card border-0 shadow-sm" style="max-width:500px;">
        <div class="card-header bg-primary text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-wallet2 me-2"></i>Datos de Consulta
            </h5>
        </div>
        <div class="card-body p-4">
            <div class="row g-3">

                <div class="col-12">
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

                <div class="col-12 mt-3">
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar Saldo"
                        CssClass="btn btn-primary btn-lg shadow-sm px-5"
                        OnClick="btnConsultar_Click" />
                </div>

            </div>
        </div>
    </div>

    <%-- Resultado del saldo --%>
    <asp:Panel ID="pnlResultado" runat="server" Visible="false">
        <div class="card border-0 shadow-sm mt-4" style="max-width:400px;">
            <div class="card-body text-center p-4">
                <i class="bi bi-check-circle-fill text-success fs-1"></i>
                <h4 class="mt-3 fw-bold">Saldo Disponible</h4>
                <h2 class="text-success fw-bold">
                    ₡ <asp:Label ID="lblSaldo" runat="server" />
                </h2>
                <hr />
                <p class="mb-1">
                    <strong>Número de cuenta:</strong>
                    <asp:Label ID="lblNumeroCuenta" runat="server" />
                </p>
                <p class="mb-0">
                    <strong>Teléfono:</strong>
                    <asp:Label ID="lblTelefono" runat="server" />
                </p>
            </div>
        </div>
    </asp:Panel>

</div>

</asp:Content>
