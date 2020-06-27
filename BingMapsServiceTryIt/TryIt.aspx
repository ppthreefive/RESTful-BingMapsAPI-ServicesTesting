<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TryIt.aspx.cs" Inherits="TryIt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-weight: 700">
            Bing Maps Service</div>
        <p>
            This service allows a user to do a number of things:</p>
        <p>
            1.) Converting an address to a coordinate pair. Method: string getGeocode(string address)</p>
        Address: <asp:TextBox ID="txtAdd1" runat="server" Width="271px"></asp:TextBox>
        <asp:Button ID="btnSubmit1" runat="server" OnClick="btnSubmit1_Click" Text="Submit" />
&nbsp;&nbsp;&nbsp; Coordinates:
        <asp:Label ID="lblCoord1" runat="server"></asp:Label>
        <p>
            2.) Converting a coordinate pair (double, double) to an address. Method: string getAddress(double latitude, double longitude)</p>
        <p>
            Latitude: <asp:TextBox ID="txtLat" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp; Longitude:
            <asp:TextBox ID="txtLong" runat="server"></asp:TextBox>
            <asp:Button ID="btnSubmit2" runat="server" OnClick="btnSubmit2_Click" Text="Submit" />
&nbsp;&nbsp;&nbsp; Address:
            <asp:Label ID="lblAdd" runat="server"></asp:Label>
        </p>
        <p>
            3.) Getting route instructions from starting address to a destination address. Method: string findRoute(string begin, string end)</p>
        <p>
            Starting Address: <asp:TextBox ID="txtStartAdd1" runat="server" Width="288px"></asp:TextBox>
&nbsp;&nbsp;&nbsp; Destination Address:
            <asp:TextBox ID="txtDestAdd1" runat="server" Width="288px"></asp:TextBox>
            <asp:Button ID="btnSubmit3" runat="server" OnClick="btnSubmit3_Click" Text="Submit" />
        </p>
        <p>
            Route Instructions:
            <asp:Label ID="lblInstructions" runat="server"></asp:Label>
        </p>
        <p>
            4.) Generate an image showcasing two addresses and a route between them. Method: string getMap(string begin, string end)</p>
        <p>
            Starting Address:
            <asp:TextBox ID="txtStartAdd2" runat="server" Width="288px"></asp:TextBox>
&nbsp;&nbsp;&nbsp; Destination Address:
            <asp:TextBox ID="txtDestAdd2" runat="server" Width="288px"></asp:TextBox>
            <asp:Button ID="btnSubmit4" runat="server" OnClick="btnSubmit4_Click" Text="Submit" />
        </p>
        <p>
            <asp:Image ID="IMGMap" runat="server" />
        </p>
    <p>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://webstrar58.fulton.asu.edu/index.html">Go back</asp:HyperLink>
        </p>
    </form>
    <p>
        Created by Phillip Pham for ASU course CSE445 with Dr. Yinong Chen, Summer 2020.</p>
    <p>
        Implemented using Bing Maps REST API&#39;s.</p>
</body>
</html>
