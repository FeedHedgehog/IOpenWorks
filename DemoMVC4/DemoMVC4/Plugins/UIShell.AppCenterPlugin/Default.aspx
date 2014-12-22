<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UIShell.AppCenterPlugin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>使用说明</title>
    <link href="Styles/Pages.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="Div_View" runat="server" class="divCss">
        <table cellspacing="0" cellpadding="4" width="100%">
            <tr>
                <td style="height: 30px; text-align: center">
                    <div runat="server" id="Div_Title" class="divTitle">
                        应用仓库浏览 应用下载安装 应用管理</div>
                </td>
            </tr>            
            <tr valign="top">
                <td class="tableTdCss">
                    使用说明 :
                </td>
            </tr>
            <tr class="viewTr">
                <td class="tableTdPadding">
                    
                    1 您可以通过“应用仓库”来浏览应用，并选择应用进行下载安装。<br />
                    2 您可以通过应用管理来查看当前安装的应用，并启动/停止/卸载所需应用。</td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
