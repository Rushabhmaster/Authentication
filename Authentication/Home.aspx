<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Authentication.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/bootstrap-social.css" rel="stylesheet" />
    <div id="ClientDetails">
        <br />
        <br />
        <asp:Label ID="ClientName" runat="server"></asp:Label><br />

        <asp:Label ID="ClientEmail" runat="server"></asp:Label><br />

        <asp:Image ID="imgProfile" runat="server" Width="16%" /><br /><br />

        <asp:Button ID="drvUpload" CssClass="btn btn-google" runat="server" Text="Upload Data To Drive" OnClick="drvUpload_Click" />
    </div>
    
</asp:Content>
