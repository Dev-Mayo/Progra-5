<%@ Page Title="Crear Pantalla" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Crear.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA6_Pantallas.Crear" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <div class="form-actions">
            <asp:Button ID="btnRegresar" runat="server" Text="←"
                PostBackUrl="~/Admin/SA6_Pantallas/Lista.aspx"
                CssClass="btn-back" CausesValidation="false" />
            <asp:Button ID="btnGuardar" runat="server" Text="💾"
                OnClick="btnGuardar_Click" CssClass="btn-save" />
        </div>
    </div>

    <div class="form-card">
        <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

        <h3>Formulario Pantalla</h3>

        <label>Nombre</label>
        <asp:TextBox ID="txtNombre" runat="server" placeholder="Ingrese el nombre"></asp:TextBox>

        <label>Descripción</label>
        <asp:TextBox ID="txtDescripcion" runat="server" placeholder="Ingrese la descripción"></asp:TextBox>

        <label>Ruta de Acceso</label>
        <asp:TextBox ID="txtRutaAcceso" runat="server" placeholder="Ingrese la ruta de acceso"></asp:TextBox>
    </div>
</asp:Content>