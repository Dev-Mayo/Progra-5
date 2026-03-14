<%@ Page Title="Administración Clientes" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
AutoEventWireup="true"
Async="true"
CodeBehind="SA10_AdminClientes.aspx.cs"
Inherits="PagosMovilesWeb.Admin.SA10_AdminClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="welcome-card">

<img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />

<h1>Administración de Clientes</h1>

<p>Gestione los clientes del sistema.</p>

</div>

<br />

<h3>Buscar Cliente</h3>

<table>
<tr>
<td>Identificación</td>
<td><asp:TextBox ID="txtBuscarIdentificacion" runat="server"></asp:TextBox></td>
<td>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
</td>
</tr>
</table>

<br />

<h4>Resultado de búsqueda</h4>

<asp:GridView ID="gvResultadoBusqueda" runat="server" AutoGenerateColumns="true" Width="100%"></asp:GridView>

<br />

<hr />

<h3>Formulario Cliente</h3>

<table>

<tr>
<td>Identificación *</td>
<td><asp:TextBox ID="txtIdentificacion" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Nombre</td>
<td><asp:TextBox ID="txtNombre" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Apellido</td>
<td><asp:TextBox ID="txtApellido" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Fecha Nacimiento</td>
<td><asp:TextBox ID="txtFechaNacimiento" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Tipo Identificación</td>
<td>
<asp:DropDownList ID="ddlTipoIdentificacion" runat="server">
<asp:ListItem Value="">-- Seleccione --</asp:ListItem>
<asp:ListItem Value="1">Cédula</asp:ListItem>
<asp:ListItem Value="2">Pasaporte</asp:ListItem>
</asp:DropDownList>
</td>
</tr>

<tr>
<td>Teléfono</td>
<td><asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Email</td>
<td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
</tr>

<tr>
<td>Contraseña</td>
<td><asp:TextBox ID="txtContrasena" runat="server" TextMode="Password"></asp:TextBox></td>
</tr>

</table>

<br />

<asp:Button ID="btnCrear" runat="server" Text="Crear Cliente" OnClick="btnCrear_Click"/>

&nbsp;

<asp:Button ID="btnActualizar" runat="server" Text="Actualizar Cliente" OnClick="btnActualizar_Click"/>

<br />
<br />

<hr />

<h3>Eliminar Cliente</h3>

<table>
<tr>
<td>Identificación</td>
<td><asp:TextBox ID="txtEliminarIdentificacion" runat="server"></asp:TextBox></td>
<td>
<asp:Button ID="btnEliminarCliente" runat="server" Text="Eliminar Cliente" OnClick="btnEliminarCliente_Click"/>
</td>
</tr>
</table>

<br />

<hr />

<h3>Lista de Clientes</h3>

<asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false" Width="100%">

<Columns>

<asp:BoundField DataField="identificacion" HeaderText="Identificación"/>

<asp:BoundField DataField="nombre" HeaderText="Nombre"/>

<asp:BoundField DataField="apellido" HeaderText="Apellido"/>

<asp:BoundField DataField="fecha_nacimiento" HeaderText="Fecha Nacimiento"/>

<asp:BoundField DataField="tipoIdentificacion" HeaderText="Tipo Identificación"/>

<asp:BoundField DataField="telefono" HeaderText="Teléfono"/>

<asp:BoundField DataField="email" HeaderText="Email"/>

</Columns>

</asp:GridView>

</asp:Content>