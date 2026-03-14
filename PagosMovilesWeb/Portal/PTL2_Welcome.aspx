<%@ Page Title="Bienvenida Portal" Language="C#" MasterPageFile="~/MasterPages/Portal.Master" AutoEventWireup="true" CodeBehind="PTL2_Welcome.aspx.cs" Inherits="PagosMovilesWeb.Portal.PTL2_Welcome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome-card">
        <img src="../Assets/img/logo_cuc.png" alt="Logo" class="logo" />
        <h1>
            Bienvenido,
            <asp:Label ID="lblNombreCompleto" runat="server"></asp:Label>
        </h1>
        <p>Has ingresado correctamente al portal de usuario.</p>
    </div>
</asp:Content>