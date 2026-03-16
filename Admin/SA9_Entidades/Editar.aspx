<%@ Page Title="Editar Entidad" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Editar.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA9_Entidades.Editar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <div class="form-actions">
            <asp:Button ID="btnRegresar" runat="server" Text="←"
                PostBackUrl="~/Admin/SA9_Entidades/Lista.aspx"
                CssClass="btn-back" CausesValidation="false" />
            <asp:Button ID="btnActualizar" runat="server" Text="💾"
                OnClick="btnActualizar_Click" CssClass="btn-save" />
        </div>
    </div>

    <div class="form-card">
        <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

        <h3>Formulario Entidad</h3>

        <label>ID Entidad</label>
        <asp:TextBox ID="txtId" runat="server" ReadOnly="true"></asp:TextBox>

        <label>Nombre</label>
        <asp:TextBox ID="txtNombre" runat="server" placeholder="Ingrese el nombre"></asp:TextBox>

        <label>URL Externa</label>
        <asp:TextBox ID="txtUrl" runat="server" placeholder="Ingrese la URL externa"></asp:TextBox>

        <label>Estado</label>
        <asp:DropDownList ID="ddlEstado" runat="server">
            <asp:ListItem Text="Activo" Value="Activo"></asp:ListItem>
            <asp:ListItem Text="Inactivo" Value="Inactivo"></asp:ListItem>
        </asp:DropDownList>
    </div>
</asp:Content>