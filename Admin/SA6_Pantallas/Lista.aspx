<%@ Page Title="Pantallas" Language="C#" MasterPageFile="~/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Lista.aspx.cs"
    Inherits="PagosMovilesWeb.Admin.SA6_Pantallas.Lista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h2 class="page-title">Pantallas</h2>
        <asp:Button ID="btnNuevo" runat="server" Text="＋ Nueva Pantalla"
            PostBackUrl="~/Admin/SA6_Pantallas/Crear.aspx" CssClass="btn-nuevo" />
    </div>

    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

    <div class="buscador">
        <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por nombre..."></asp:TextBox>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
            OnClick="btnBuscar_Click" CssClass="btn-secondary" />
    </div>

    <div class="grid-container">
        <div class="grid-header">Información</div>
        <asp:GridView ID="gvPantallas" runat="server"
            AutoGenerateColumns="false"
            OnRowCommand="gvPantallas_RowCommand"
            EmptyDataText="No hay pantallas registradas.">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="RutaAcceso" HeaderText="Ruta de Acceso" />
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