<%@ Page Title="Últimos Movimientos" Language="C#"
    MasterPageFile="~/MasterPages/Portal.Master"
    AutoEventWireup="true"
    CodeBehind="PTL8_UltimosMovimientos.aspx.cs"
    Inherits="PagosMovilesWeb.Portal.PTL8_UltimosMovimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">

<script type="text/javascript">
    function soloDigitos(e) {
        return e.charCode >= 48 && e.charCode <= 57;
    }
</script>

<div class="container-fluid py-4 px-3">

    <div class="d-flex justify-content-between align-items-center mb-4 bg-light rounded-3 p-3 shadow-sm border">
        <div>
            <h1 class="h3 mb-1 fw-bold text-dark">Últimos 5 Movimientos</h1>
            <p class="mb-0 text-muted">Consulte los movimientos recientes de su cuenta</p>
        </div>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card border-0 shadow-sm mb-4" style="max-width:500px;">
        <div class="card-header bg-primary text-white py-3">
            <h5 class="mb-0 fw-bold">
                <i class="bi bi-clock-history me-2"></i>Datos de Consulta
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
                <div class="col-12 mt-2">
                    <asp:Button ID="btnConsultar" runat="server"
                        Text="Consultar Movimientos"
                        CssClass="btn btn-primary btn-lg shadow-sm px-5"
                        OnClick="btnConsultar_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- Tabla de movimientos --%>
    <asp:Panel ID="pnlResultados" runat="server" Visible="false">

        <div class="mb-3">
            <strong>Cuenta:</strong>
            <asp:Label ID="lblNumeroCuenta" runat="server" />
            &nbsp;&nbsp;
            <strong>Teléfono:</strong>
            <asp:Label ID="lblTelefono" runat="server" />
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvMovimientos" runat="server"
                CssClass="table table-bordered table-hover table-striped shadow-sm"
                AutoGenerateColumns="false"
                EmptyDataText="No se encontraron movimientos para esta cuenta.">
                <HeaderStyle BackColor="#1f2a44" ForeColor="White" />
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
                        HeaderText="Monto (₡)"
                        DataFormatString="{0:N2}"
                        HtmlEncode="false" />
                    <asp:BoundField DataField="saldoAnterior"
                        HeaderText="Saldo Anterior (₡)"
                        DataFormatString="{0:N2}"
                        HtmlEncode="false" />
                    <asp:BoundField DataField="saldoActual"
                        HeaderText="Saldo Actual (₡)"
                        DataFormatString="{0:N2}"
                        HtmlEncode="false" />
                    <asp:BoundField DataField="descripcion"
                        HeaderText="Descripción" />
                </Columns>
            </asp:GridView>
        </div>

    </asp:Panel>

</div>

</asp:Content>
