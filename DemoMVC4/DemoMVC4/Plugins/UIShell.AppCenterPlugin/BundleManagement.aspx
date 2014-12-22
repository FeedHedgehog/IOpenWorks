<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BundleManagement.aspx.cs"
    Inherits="UIShell.AppCenterPlugin.BundleManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>应用管理</title>
    <link href="Styles/Pages.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="title">
        <div class="item">
            <asp:Image ImageUrl="Images/Application.png" CssClass="bigImgCss" ToolTip="当前应用总数"
                AlternateText="安装的应用总数" ID="Image_applicationCount" runat="server" />
            <asp:Label ID="Label_applicationCountText" runat="server" Text='安装的应用总数:'></asp:Label>
            <asp:Label ID="Label_applicationCount" runat="server"></asp:Label>
        </div>
        <div class="item error floatright">
            <asp:Image ImageUrl="Images/Error.png" CssClass="bigImgCss" ToolTip="警告信息" AlternateText="警告"
                ID="Image_errorMessage" runat="server" Visible="False" />
            <asp:Label ID="Label_errorMessage" runat="server"></asp:Label>
        </div>
        <div class="item floatright">
            <asp:ImageButton ID="RebootImageButton" CssClass="bigImgCss" runat="server" Visible="false" ImageUrl="Images/reboot.png" 
                OnClientClick="alert('点击完成重启后，请关闭浏览器重新进入系统。')" AlternateText="系统发生变更，要求重启应用系统，点击完成重启"
                onclick="RebootImageButton_Click" />
        </div>
        
        <div style="clear: both">
        </div>
    </div>
    <div>
        <asp:GridView ID="GridView_applicationInfo" runat="server" AutoGenerateColumns="False"
            DataKeyNames="BundleID" CellPadding="4" OnRowCommand="GridView_applicationInfo_RowCommanded"
            OnRowDataBound="GridView_applicationInfo_RowDataBound" OnRowUpdating="GridView_applicationInfo_RowUpdating"
            OnRowCancelingEdit="GridView_applicationInfo_OnRowCancelingEdit" ShowFooter="True"
            Width="100%" BorderColor="#288ed0" BorderWidth="1px" AllowPaging="true" PageSize="10"
            OnPageIndexChanging="GridView_applicationInfo_PageIndexChanging" OnDataBound="GridView_applicationInfo_DataBound">
            <RowStyle ForeColor="#288ed0" BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Label_applicationInfoID" runat="server" Text='<%# Bind("BundleID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用名称">
                    <ItemTemplate>
                        <asp:Label ID="Label_applicationTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用标识" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Label_applicationSymbolicName" runat="server" Text='<%# Bind("SymbolicName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用版本">
                    <ItemTemplate>
                        <asp:Label ID="Label_applicationVersion" runat="server" Text='<%# Bind("Version") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <asp:Label ID="Label_applicationStatus" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton_start" runat="server" CommandName="start" ImageUrl="Images/Start.png"
                            CssClass="bigImgCss" AlternateText="启动应用" ToolTip="启动应用" Visible='<%# Bind("CanBeStarted") %>' />&nbsp;
                        <asp:ImageButton ID="ImageButton_stop" runat="server" CommandName="stop" ImageUrl="Images/Stop.png"
                            CssClass="bigImgCss" AlternateText="停止应用" ToolTip="停止应用" Visible='<%# Bind("CanBeStopped") %>' />&nbsp;
                        <asp:ImageButton ID="ImageButton_uninstall" runat="server" CommandName="uninstall"
                            ImageUrl="Images/Uninstall.png" CssClass="bigImgCss" AlternateText="卸载应用（一旦卸载，应用将被彻底删除）"
                            ToolTip="卸载应用（一旦卸载，应用将被彻底删除）" Visible='<%# Bind("CanBeUninstalled") %>' OnClientClick="return confirm('警告：应用一旦卸载，该应用包含的所有文件（包括数据库）将被彻底删除，您是否要继续卸载此应用？\r\n卸载应用后，需要重启系统生效。');" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#288ed0" ForeColor="#FFFFFF" Font-Bold="true" />
            <PagerStyle CssClass="PagerCss" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <asp:Label ID="Label_Description" runat="server" Text="暂无应用信息"></asp:Label>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#207bc2" Font-Bold="True" ForeColor="#FFFFCC" />
            <HeaderStyle BackColor="#1867b2" Font-Bold="True" ForeColor="#FFFFFF" Height="40px" />
            <PagerTemplate>
                第<asp:Label ID="Label_CurrentPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>'></asp:Label>页/每页<asp:DropDownList
                    ID="DropDownList_PageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_PageSize_SelectedIndexChanged"
                    ToolTip="请点击下拉菜单选择每页显示记录条数">
                </asp:DropDownList>
                条记录，共<asp:Label ID="Label_PageCount" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageCount %>'></asp:Label>页
                <asp:LinkButton ID="LinkButton_First" runat="server" OnClick="LinkButton_First_Click">首页</asp:LinkButton>
                <asp:LinkButton ID="LinkButton_Back" runat="server" OnClick="LinkButton_Back_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="LinkButton_Next" runat="server" OnClick="LinkButton_Next_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="LinkButton_Last" runat="server" OnClick="LinkButton_Last_Click">末页</asp:LinkButton>
            </PagerTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
