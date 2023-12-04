<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewRecord.aspx.cs" Inherits="gridviewpagewebform.AddNewRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            height: 330px;
        }
        .auto-style2 {
            width: 170px;
            height: 75px;
        }
        .auto-style3 {
            width: 439px;
            height: 75px;
        }
        .auto-style4 {
            width: 170px;
            height: 104px;
        }
        .auto-style5 {
            width: 439px;
            height: 104px;
        }
        .auto-style6 {
            height: 104px;
        }
        .auto-style7 {
            width: 170px;
            height: 103px;
        }
        .auto-style8 {
            width: 439px;
            height: 103px;
        }
        .auto-style9 {
            height: 103px;
        }
        .auto-style10 {
            width: 170px;
            height: 84px;
        }
        .auto-style11 {
            width: 439px;
            height: 84px;
        }
        .auto-style12 {
            height: 84px;
        }
        .auto-style13 {
            height: 75px;
        }
        .auto-style14 {
            margin-left: 255px;
        }
        .auto-style15 {
            margin-left: 53px;
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ADD NEW RECORD</div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style4">name</td>
                <td class="auto-style5">
                    <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                </td>
                <td class="auto-style6"></td>
                <td class="auto-style6"></td>
            </tr>
            <tr>
                <td class="auto-style7">rollnumber</td>
                <td class="auto-style8">
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style9"></td>
                <td class="auto-style9"></td>
            </tr>
            <tr>
                <td class="auto-style10">phoenumber</td>
                <td class="auto-style11">
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style12"></td>
                <td class="auto-style12"></td>
            </tr>
            <tr>
                <td class="auto-style2">address</td>
                <td class="auto-style3">
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style13"></td>
                <td class="auto-style13"></td>
            </tr>
        </table>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style15" Height="37px" OnClick="Button2_Click" Text="back" Width="86px" />
        <asp:Button ID="Button1" runat="server" CssClass="auto-style14" Height="52px" Text="Insert" Width="72px" />
    </form>
</body>
</html>
