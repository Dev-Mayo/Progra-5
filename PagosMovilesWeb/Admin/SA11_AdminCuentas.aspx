<%@ Page Title="Administración Cuentas" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true"
Async="true"
CodeBehind="SA11_AdminCuentas.aspx.cs"
Inherits="PagosMovilesWeb.Admin.SA11_AdminCuentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="welcome-card">

<img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />

<h1>Administración de Cuentas</h1>
<p>Gestione las cuentas del sistema.</p>

</div>

<br />

<h3>Buscar Cuenta</h3>

<table>
<tr>
<td>Número Cuenta</td>
<td><asp:TextBox ID="txtBuscarCuenta" runat="server"></asp:TextBox></td>
<td>
<asp:Button ID="btnBuscarCuenta"
runat="server"
Text="Buscar"
OnClick="btnBuscarCuenta_Click"/>
</td>
</tr>
</table>

<br />

<asp:GridView ID="gvResultadoBusqueda"
runat="server"
AutoGenerateColumns="true"
Width="100%" />

<br />

<hr />

<h3>Buscar Cuentas por Cliente</h3>

<table>
<tr>
<td>Identificacion</td>
<td><asp:TextBox ID="txtBuscarCliente" runat="server"></asp:TextBox></td>
<td>
<asp:Button ID="btnBuscarCliente"
runat="server"
Text="Buscar"
OnClick="btnBuscarCliente_Click"/>
</td>
</tr>
</table>

<br />

<asp:GridView ID="gvCuentasCliente"
runat="server"
AutoGenerateColumns="true"
Width="100%" />

<br />

<hr />

<h3>Formulario Cuenta</h3>

<table>

<tr>
<td>Identificacion *</td>
<td><asp:TextBox ID="txtClienteId" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Número Cuenta</td>
<td><asp:TextBox ID="txtNumeroCuenta" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Tipo Cuenta</td>
<td>
<asp:DropDownList ID="ddlTipoCuenta" runat="server">
<asp:ListItem Value="Ahorros">Ahorros</asp:ListItem>
<asp:ListItem Value="Corriente">Corriente</asp:ListItem>
</asp:DropDownList>
</td>
</tr>

</table>

<br />

<asp:Button ID="btnCrearCuenta"
runat="server"
Text="Crear Cuenta"
OnClick="btnCrearCuenta_Click"/>

&nbsp;

<asp:Button ID="btnActualizarCuenta"
runat="server"
Text="Actualizar Cuenta"
OnClick="btnActualizarCuenta_Click"/>

<br /><br />

<hr />

<h3>Eliminar Cuenta</h3>

<table>
<tr>
<td>Identificacion</td>
<td><asp:TextBox ID="txtEliminarCliente" runat="server"></asp:TextBox></td>

<td>Número Cuenta</td>
<td><asp:TextBox ID="txtEliminarCuenta" runat="server"></asp:TextBox></td>

<td>
<asp:Button ID="btnEliminarCuenta"
runat="server"
Text="Eliminar"
OnClick="btnEliminarCuenta_Click"/>
</td>
</tr>
</table>

<br />

<hr />

<h3>Lista de Cuentas</h3>

<asp:GridView ID="gvCuentas"
runat="server"
AutoGenerateColumns="true"
Width="100%" />

</asp:Content>