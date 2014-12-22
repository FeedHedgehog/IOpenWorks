<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UIShell.CleanWebWin7Plugin._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开放工厂Web界面框架</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="Styles/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Styles/AeroWindow.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Desktop.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Scripts/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery-ui-1.8.1.custom.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery.easing.1.3.js"></script>

    <script type="text/javascript" src="Scripts/jquery-AeroWindow.js"></script>

    <script type="text/javascript" src="Scripts/jquery.wresize.js"></script>

</head>
<body>
    <noscript>
        <div id="error">
            你好，需要浏览器开启JavaScript功能来帮助你使用本网站。<br />
            请设置浏览器开启 JavaScript功能，然后重试。
        </div>
    </noscript>
    <form id="form1" runat="server">
    <asp:Panel ID="iconContainer" runat="server">
    </asp:Panel>
    <div id="DesktopIconWindow" style="display: none;">
    </div>
    </form>
</body>
</html>
