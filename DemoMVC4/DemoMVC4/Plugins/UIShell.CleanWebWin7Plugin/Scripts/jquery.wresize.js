/*  
===============================================================================
WResize is the jQuery plugin for fixing the IE window resize bug
...............................................................................
Copyright 2007 / Andrea Ercolino
-------------------------------------------------------------------------------
LICENSE: http://www.opensource.org/licenses/mit-license.php
WEBSITE: http://noteslog.com/
===============================================================================
*/

(function($) {
    $.fn.wresize = function(f) {
        version = '1.1';
        wresize = { fired: false, width: 0 };

        function resizeOnce() {
            if ($.browser.msie) {
                if (!wresize.fired) {
                    wresize.fired = true;
                }
                else {
                    var version = parseInt($.browser.version, 10);
                    wresize.fired = false;
                    if (version < 7) {
                        return false;
                    }
                    else if (version == 7) {
                        //a vertical resize is fired once, an horizontal resize twice
                        var width = $(window).width();
                        if (width != wresize.width) {
                            wresize.width = width;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        function handleWResize(e) {
            if (resizeOnce()) {
                return f.apply(this, [e]);
            }
        }

        this.each(function() {
            if (this == window) {
                $(this).resize(handleWResize);
            }
            else {
                $(this).resize(f);
            }
        });

        return this;
    };

})(jQuery);

jQuery(function($) {
    function content_resize() {
        var height = $(window).height() - 50;
        var num = parseInt((height - 20) / 80);
        var line = 1;
        var row = 0;

        $('.iconDiv').each(function(index, domEle) {
            if (index >= num * line) {
                line++;
                row = 0;
            }

            $(domEle).css("left", ((line - 1) * 80 + 10) + "px");
            $(domEle).css("top", 10 + (80 * row) + "px");
            row++;
        });

        $('.AeroWindow').each(function(index, domEle) {
            var maxBtn = $(domEle).find(".WinBtnSet.winmax");
            var winContent = $(domEle).find(".table-mm-content");
            var winContainer = $(domEle).find(".table-mm-container");
            if (winContainer.css('visibility') == 'visible' && maxBtn.css('display') == 'none' && winContent.find('iframe').attr('src') != '')
                maxBtn.click();
        })
    }

    $(window).wresize(content_resize);

    content_resize();
});

var ignoreLogoutSysWarning = false;

var OnLogout = function() {
    if (!ignoreLogoutSysWarning)
        return '请谨慎操作，尚未保存的数据将丢失。';
}

$(document).ready(function() {
    $('#LogoffIcon').click(function() {
        if (confirm("您确实要退出吗？退出后需重新登录。\n请谨慎操作，尚未保存的数据将丢失。")) {
            ignoreLogoutSysWarning = true;
            location.href = "Login.aspx";
        }
    });
});

var PopupApplicationWindow = function(id, menu) {
    $('body').data('PopupAppWindowSrc' + id, menu);
    $('#Icon' + id + 'OnDesktop').click();
    $('body').data('PopupAppWindowSrc' + id, null);
}
