<%@ Page Title="Administración Cuentas" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="SA11_AdminCuentas.aspx.cs" Inherits="PagosMovilesWeb.Admin.SA11_AdminCuentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="welcome-card">

    <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />

    <h1>Administración de Cuentas</h1>

    <p>Gestione las cuentas del sistema.</p>

</div>

<br />

<!-- BUSCAR CUENTA -->

<h3>Buscar Cuenta</h3>

Número de Cuenta:

<asp:TextBox ID="txtBuscarCuenta" runat="server"></asp:TextBox>

<asp:Button 
    ID="btnBuscarCuenta"
    runat="server"
    Text="Buscar"
    OnClick="btnBuscarCuenta_Click"
/>

<br /><br />

<!-- BUSCAR POR CLIENTE -->

<h3>Buscar Cuentas por Cliente</h3>

Identificación Cliente:

<asp:TextBox ID="txtBuscarCliente" runat="server"></asp:TextBox>

<asp:Button 
    ID="btnBuscarCliente"
    runat="server"
    Text="Buscar"
    OnClick="btnBuscarCliente_Click"
/>

<br /><br />

<hr />

<!-- FORMULARIO CUENTA -->

<h3>Formulario Cuenta</h3>

<table>

<tr>
<td>Número Cuenta</td>
<td>
<asp:TextBox ID="txtNumeroCuenta" runat="server"></asp:TextBox>
</td>
</tr>

<tr>
<td>Cliente</td>
<td>
<asp:TextBox ID="txtCliente" runat="server"></asp:TextBox>
</td>
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

<tr>
<td>Saldo Inicial</td>
<td>
<asp:TextBox ID="txtSaldo" runat="server"></asp:TextBox>
</td>
</tr>

</table>

<br />

<asp:Button 
    ID="btnCrearCuenta"
    runat="server"
    Text="Crear Cuenta"
    OnClick="btnCrearCuenta_Click"
/>

<asp:Button 
    ID="btnActualizarCuenta"
    runat="server"
    Text="Actualizar Cuenta"
    OnClick="btnActualizarCuenta_Click"
/>

<br /><br />

<hr />

<!-- LISTA CUENTAS -->

<h3>Lista de Cuentas</h3>

<asp:GridView 
    ID="gvCuentas"
    runat="server"
    AutoGenerateColumns="false"
    Width="100%">

<Columns>

<asp:BoundField DataField="numeroCuenta" HeaderText="Cuenta"/>

<asp:BoundField DataField="identificacionCliente" HeaderText="Cliente"/>

<asp:BoundField DataField="tipoCuenta" HeaderText="Tipo Cuenta"/>

<asp:BoundField DataField="saldo" HeaderText="Saldo"/>

<asp:TemplateField HeaderText="Acciones">

<ItemTemplate>

<asp:Button
    ID="btnEditar"
    runat="server"
    Text="Editar"
    CommandArgument='<%# Eval("numeroCuenta") %>'
    OnClick="EditarCuenta"
/>

<asp:Button
    ID="btnEliminar"
    runat="server"
    Text="Eliminar"
    CommandArgument='<%# Eval("numeroCuenta") %>'
    OnClick="EliminarCuenta"
/>

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</asp:Content>