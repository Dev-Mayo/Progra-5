<%@ Page Title="Editar Rol" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Editar.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA7_Roles.Editar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <div class="form-actions">
            <asp:Button ID="btnRegresar" runat="server" Text="←"
                PostBackUrl="~/Admin/SA7_Roles/Lista.aspx"
                CssClass="btn-back" CausesValidation="false" />
            <asp:Button ID="btnActualizar" runat="server" Text="💾"
                OnClick="btnActualizar_Click" CssClass="btn-save" />
        </div>
    </div>

    <div class="form-card">
        <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

        <h3>Formulario Rol</h3>

        <label>ID</label>
        <asp:TextBox ID="txtId" runat="server" ReadOnly="true"></asp:TextBox>

        <label>Nombre</label>
        <asp:TextBox ID="txtNombre" runat="server" placeholder="Ingrese el nombre del rol"></asp:TextBox>

        <label>Pantallas asociadas</label>
        <div class="checkbox-list">
            <asp:CheckBoxList ID="cblPantallas" runat="server"></asp:CheckBoxList>
        </div>
    </div>
</asp:Content>