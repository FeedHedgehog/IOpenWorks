<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppFrame.aspx.cs" Inherits="UIShell.CleanWebWin7Plugin.AppFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/AppFrame.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function showOrHide() {
            $("#framecontentLeft").slideToggleWidth();

        }

        jQuery.fn.extend({
            slideRight: function() {
                return this.each(function() {
                    jQuery(this).animate({ width: 'show' }, "normal");
                });
            },
            slideLeft: function() {
                return this.each(function() {
                    jQuery(this).animate({ width: 'hide' }, "normal");
                });
            },
            slideToggleWidth: function() {
                return this.each(function() {
                    var el = jQuery(this);
                    if (el.css('display') == 'none') {
                        el.slideRight();
                        $("#navigateMenuHider").animate({ left: '205' }, "normal");
                        $("#framecontentTop").animate({ left: '225' }, "normal");
                        $("#maincontent").animate({ left: '215' }, "normal");
                    } else {
                        el.slideLeft();
                        $("#navigateMenuHider").animate({ left: '0' }, "normal");
                        $("#maincontent").animate({ left: '10' }, "normal");
                        $("#framecontentTop").animate({ left: '20' }, "normal");
                    }
                });
            }
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
