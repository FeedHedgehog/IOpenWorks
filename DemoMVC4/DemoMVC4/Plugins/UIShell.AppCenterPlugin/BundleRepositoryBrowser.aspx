<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BundleRepositoryBrowser.aspx.cs" Inherits="UIShell.AppCenterPlugin.BundleRepositoryBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>应用仓库</title>
    <link type="text/css" href="Scripts/css/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" />
    <link href="Styles/Pages.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .resultDiv { overflow: scroll; width: 100%; height: 300px; }
    </style>
    
    <script type="text/javascript" src="Scripts/js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript" src="Scripts/js/jquery-ui-1.8.16.custom.min.js"></script>
    <script type="text/javascript" src="Scripts/js/jquery.blockUI.js"></script>
    
    <script type="text/javascript">
        // Initialized the progress bar and finished button click event handler.
        $(function() {
            $("#progressbar").progressbar({
                value: 0
            });
        });
        
        // Show the message when there is a installation in progress.
        function installationInProgress(msg) {
            showInformationArea();
            $("#progressbar").progressbar("value", 100);
            showMessage(msg);
        }
        
        var closeElement = null;
        var titleElement = null;
        
        function showInformationArea() {
            $( "#informationArea" ).dialog(
            { 
                title: '正在下载安装/升级应用',
                resizable: false,
                modal: true,
                width: 500,
                height: 400,
                dialogClass: 'no-close',
                open: function(event, ui){ 
                    // Title bar without style.
                    $(this).parents(".ui-dialog:first").find(".ui-widget-header")
                        .removeClass("ui-widget-header");
                    // Progress bar with style.
                    $(this).parents(".ui-dialog:first").find(".ui-progressbar-value")
                        .addClass("ui-widget-header");
                    titleElement = $(this).parents(".ui-dialog:first").find("span.ui-dialog-title");
                    closeElement = $(this).parents(".ui-dialog:first").find('.ui-dialog-titlebar-close');
                    if(closeElement != null) {
                        closeElement.hide();
                    }
                },
                close: function() { 
                    <%= ClientScript.GetPostBackEventReference(ReBindDataButton, "") %>;
                }
            });
        }
        
        var updateInterval = 500;
        
        // Query the installation progress. It will update the progress every 0.5 second.
        function queryProgress() {
            disableButtons();
            $("#progressbar").progressbar("value", 0);
            showMessage('');
            showInformationArea();
            setTimeout(updateProgress, updateInterval);
        }

        // Query the installation progress on server side by using ajax invoking.
        // And update the progress after invoking completed.
        function updateProgress() {
            $.ajax({
                type: "POST",
                url: "Model/QueryProgressService.asmx/QueryProgressItem",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: onResult,
                error: onError
            });
        }
        
        // Update the progress when progress query completed.
        function onResult(data) {
            var jsonData = null;
			// The message is in data.d property in VS2010.
            if(data.d != undefined && data.d != null){
                jsonData = data.d;
            }
            else { // The message is in data in VS2008.
                jsonData = data;
            }

            var progressArray = jQuery.parseJSON(jsonData);
            var percentage = 0;
            for (var i = 0; progressArray != null && i < progressArray.length; i++) {
                var progressItem = progressArray[i];
                percentage = progressItem.Percentage;
                $("#progressbar").progressbar("value", percentage);
                if(!isEmpty(progressItem.Title)) {
                    setInformationAreaTitle(progressItem.Title);
                }
                if(!isEmpty(progressItem.Message)) {
                    appendMessage(progressItem.Message);
                }

                if (percentage >= 100) {
                    break;
                }
            }
            if (percentage < 100) {
                setTimeout(updateProgress, updateInterval);
            }
            else {
                enableButtons();
            }
        }

        function onError(data) {
            alert("Error occurs!");
            enableButtons();
        }
        
        function showMessage(msg) {
            $("#result").text(msg);
        }

        function appendMessage(msg) {
            $("#result").append(msg + '<br />');
            $("#result").scrollTop($("#result")[0].scrollHeight);
        }
        
        function setInformationAreaTitle(title)
        {
            if(titleElement != null)
            {
                titleElement.text(title);
            }
        }

        function enableButtons() {
            $("#InstallImageButton").removeAttr("disabled");
            if(closeElement != null) {
                closeElement.show();
                $("#informationArea").dialog('option', 'title', '安装完成');
            }
        }

        function disableButtons() {
            $("#InstallImageButton").attr('disabled', 'disabled');
            if(closeElement != null) {
                closeElement.show();
            }
        }
        
        function isEmpty(str) {
            return (!str || 0 === str.length);
        }
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="title">
        <div class="item">
            <asp:Label ID="UseNameLabel" runat="server" Text='用户名:'></asp:Label>
            <asp:TextBox ID="UserNameTextBox" runat="server"></asp:TextBox>
            <asp:Label ID="PasswordLabel" runat="server" Text='密码:'></asp:Label>
            <asp:TextBox ID="PasswordTextBox" TextMode="Password" runat="server"></asp:TextBox>
            <asp:Button ID="QueryButton" runat="server" Text="查询" runat="server" 
                onclick="QueryButton_Click" />
        </div>
        <div class="item">
            <asp:Image ImageUrl="Images/Application.png" CssClass="bigImgCss" ToolTip="当前应用总数"
                AlternateText="应用总数" ID="Image_applicationCount" runat="server" />
            <asp:Label ID="Label_applicationCountText" runat="server" Text='应用总数:'></asp:Label>
            <asp:Label ID="Label_applicationCount" runat="server">0</asp:Label>
        </div>
        <div class="item floatright">
            <asp:ImageButton ID="InstallImageButton" runat="server" ImageUrl="Images/Download.png"
                CssClass="bigImgCss" ToolTip="下载安装应用" AlternateText="下载安装应用" 
                onclick="InstallImageButton_Click" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="GoToMainPage" runat="server" CssClass="bigImgCss" ImageUrl="Images/home.png" 
                Visible="false" onclick="GoToMainPage_Click" AlternateText="已安装了一个主界面应用，点击转到主界面" />
            <asp:ImageButton ID="RebootImageButton" CssClass="bigImgCss" runat="server" Visible="false" ImageUrl="Images/reboot.png" 
                OnClientClick="alert('点击完成重启后，请关闭浏览器重新进入系统。')" AlternateText="系统发生变更，要求重启应用系统，点击完成重启"
                onclick="RebootImageButton_Click" />
            <!-- Re-bind data after installation finished. This button is invisible button. -->
            <asp:Button ID="ReBindDataButton" Text="重新绑定应用" runat="server" 
                onclick="ReBindDataButton_Click" style = "display : none" />
        </div>
        <div class="itemright error">
            <asp:Image ImageUrl="Images/Error.png" CssClass="bigImgCss" ToolTip="警告信息" AlternateText="警告"
                ID="ErrorMessageImage" runat="server" Visible="False" />
            <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div>
        <asp:GridView ID="BundlesGridView" runat="server" AutoGenerateColumns="False"
            DataKeyNames="BundleID" CellPadding="4" ShowFooter="True"
            Width="100%" BorderColor="#288ed0" BorderWidth="1px" AllowPaging="true" PageSize="10"
            OnPageIndexChanging="BundlesGridView_PageIndexChanging">
            <RowStyle ForeColor="#288ed0" BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="BundleIDLabel" runat="server" Text='<%# Bind("BundleID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="HasNewVersion" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="HasNewVersionLabel" runat="server" Text='<%# Bind("HasNewVersion") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="SelectedCheckBox" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="5px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用">
                    <ItemTemplate>
                        <asp:Label ID="NameLabel" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="标识" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="SymbolicNameLabel" runat="server" Text='<%# Bind("SymbolicName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="新版本">
                    <ItemTemplate>
                        <asp:Label ID="HasNewVersionText" runat="server" Text='<%# Bind("NewVersionText") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="版本">
                    <ItemTemplate>
                        <asp:Label ID="VersionLabel" runat="server" Text='<%# Bind("Version") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="开发者">
                    <ItemTemplate>
                        <asp:Label ID="DeveloperLabel" runat="server" Text='<%# Bind("Developer") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="下载">
                    <ItemTemplate>
                        <asp:Label ID="DownloadTimesLabel" runat="server" Text='<%# Bind("DownloadTimes") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="更新">
                    <ItemTemplate>
                        <asp:Label ID="UpdateDateLabel" runat="server" Text='<%# Eval("UpdateDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="描述">
                    <ItemTemplate>
                        <a href='<%# Eval("DetailsUrl") %>' target="_blank">查看</a>
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
    
    <!-- Show the installation progress. -->
    <div id="informationArea" style="display:none; cursor: default">
        <div id="progressbar"></div><span></span>
        <div id="result" class="resultDiv"></div>
    </div>
    </form>
</body>
</html>
