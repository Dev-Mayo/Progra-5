<%@ Page Language="C#" AutoEventWireup="true" Async="true"
         CodeBehind="Login.aspx.cs"
         Inherits="PagosMovilesWeb.Portal.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" />
<title>Portal de Usuarios – Pagos Móviles</title>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
<style>
        body { background: #eef2f7; }
        .card-login {
            max-width: 420px;
            margin: 80px auto;
            border-radius: 14px;
            overflow: hidden;
            box-shadow: 0 6px 28px rgba(0,0,0,.15);
        }
        .card-header-custom {
            background: #1a3a5c;
            padding: 36px 24px;
            text-align: center;
            color: white;
        }
        .logo-emoji { margin-bottom: 10px; }
</style>
</head>
<body>
<form id="form1" runat="server">
<div class="card-login bg-white">
<div class="card-header-custom">
<div class="logo-emoji">
<svg width="64" height="64" viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
<rect x="8" y="16" width="48" height="32" rx="4" fill="#4A90E2" stroke="white" stroke-width="2"/>
<rect x="8" y="22" width="48" height="8" fill="#2E5C8A"/>
<circle cx="18" cy="36" r="3" fill="#FFD700"/>
<rect x="26" y="34" width="12" height="4" rx="1" fill="white" opacity="0.8"/>
<rect x="26" y="40" width="8" height="3" rx="1" fill="white" opacity="0.6"/>
</svg>
</div>
<h5 class="mt-2 mb-0 fw-bold">Pagos Móviles CUC</h5>
<small class="opacity-75">Portal de Usuarios</small>
</div>
<div class="p-4">
<asp:Panel ID="pnlMensaje" runat="server" Visible="false">
<div id="divMensaje" runat="server" class="alert mb-3"></div>
</asp:Panel>
<asp:Panel ID="pnlBloqueado" runat="server" Visible="false">
<div class="alert alert-danger">
<strong>Usuario bloqueado.</strong>
                Se superaron 3 intentos fallidos.
</div>
</asp:Panel>
<div class="mb-3">
<label class="form-label fw-semibold">Correo electrónico</label>
<asp:TextBox ID="txtUsuario" runat="server"
                         CssClass="form-control"
                         placeholder="Ingrese su correo electrónico" />
<asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtUsuario"
                ErrorMessage="El correo es requerido."
                CssClass="text-danger small" Display="Dynamic" />
</div>
<div class="mb-4">
<label class="form-label fw-semibold">Contraseña</label>
<asp:TextBox ID="txtPassword" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         placeholder="Ingrese su contraseña" />
<asp:RequiredFieldValidator runat="server"
                ControlToValidate="txtPassword"
                ErrorMessage="La contraseña es requerida."
                CssClass="text-danger small" Display="Dynamic" />
</div>
<asp:Button ID="btnIngresar" runat="server"
                    Text="Ingresar"
                    CssClass="btn btn-primary w-100 py-2 fw-semibold"
                    OnClick="btnIngresar_Click" />
</div>
</div>
</form>
</body>
</html>