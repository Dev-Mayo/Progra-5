<%@ Page Title="Bienvenida Admin" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" 
         AutoEventWireup="true"
         CodeBehind="SA2_Welcome.aspx.cs" 
         Inherits="PagosMovilesWeb.Admin.SA2_Welcome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome-card">
        <img src="../Assets/img/logo.png" alt="Logo" class="logo" />
        <h1>
            Bienvenido,
            <asp:Label ID="lblNombreCompleto" runat="server"></asp:Label>
        </h1>
    </div>
</asp:Content>