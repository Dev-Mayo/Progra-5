<%@ Page Language="C#" AutoEventWireup="true" Async="true"
         CodeBehind="Bienvenida.aspx.cs"
         Inherits="PagosMovilesWeb.Portal.Bienvenida" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Bienvenido – Portal Pagos Móviles</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
    <style>
        body { background: #eef2f7; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="container mt-5">
    <div class="card shadow rounded-4 p-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h4 class="mb-0">💳 Pagos Móviles CUC</h4>
            <asp:LinkButton ID="btnCerrarSesion" runat="server"
                            CssClass="btn btn-outline-danger btn-sm"
                            OnClick="btnCerrarSesion_Click">
                Cerrar sesión
            </asp:LinkButton>
        </div>

        <div class="alert alert-success">
            <h5>Bienvenido, <asp:Label ID="lblNombre" runat="server" /></h5>
        </div>

        <div id="inactivityBar" class="alert alert-warning d-none">
            ⚠️ Su sesión está por expirar por inactividad.
        </div>
    </div>
</div>
</form>

<script>
    (function () {
        const TIMEOUT_MS = 5 * 60 * 1000;
        const WARNING_MS = 4 * 60 * 1000;
        let timerWarning, timerLogout;

        function resetTimers() {
            clearTimeout(timerWarning);
            clearTimeout(timerLogout);
            timerWarning = setTimeout(showWarning, WARNING_MS);
            timerLogout = setTimeout(forceLogout, TIMEOUT_MS);
        }

        function showWarning() {
            document.getElementById('inactivityBar')
                .classList.remove('d-none');
        }

        function forceLogout() {
            window.location.href = '/Portal/Login.aspx?msg=expirado';
        }

        ['mousemove', 'keypress', 'click', 'scroll', 'touchstart']
            .forEach(ev => document.addEventListener(ev, resetTimers));

        resetTimers();
    })();
</script>
</body>
</html>