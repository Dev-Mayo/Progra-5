<%@ Page Title="Roles" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Lista.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA7_Roles.Lista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h2 class="page-title">Roles</h2>
        <asp:Button ID="btnNuevo" runat="server" Text="＋ Nuevo Rol"
            PostBackUrl="~/Admin/SA7_Roles/Crear.aspx" CssClass="btn-nuevo" />
    </div>

    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

    <div class="buscador">
        <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por ID..."></asp:TextBox>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
            OnClick="btnBuscar_Click" CssClass="btn-secondary" />
    </div>

    <div class="grid-container">
        <div class="grid-header">Información</div>
        <asp:GridView ID="gvRoles" runat="server"
            AutoGenerateColumns="false"
            OnRowCommand="gvRoles_RowCommand"
            EmptyDataText="No hay roles registrados.">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="✎"
                            CommandName="Editar"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn-accion btn-editar" />
                        <asp:Button runat="server" Text="🗑"
                            CommandName="Eliminar"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn-accion btn-eliminar"
                            OnClientClick="return confirm('¿Realmente desea eliminar el elemento seleccionado?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>