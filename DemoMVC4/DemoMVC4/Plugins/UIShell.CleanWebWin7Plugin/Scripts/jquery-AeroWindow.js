/*
* AeroWindow - jQuery Plugin (v3.51)
* Copyright 2010, Christian Goldbach
* Dual licensed under the MIT or GPL Version 2 licenses.
* 
* Project Website:
* http://www.soyos.net/aerowindow-jquery.html
* http://www.soyos.net
*
*
*
* Requires Easing Plugin for Window Animations:
* jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
*
*
* Changelog:
* ~~~~~~~~~~
* Version 3.51 (2010-06-09) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
* Added more config options:
* New Feature: Window get the focus by clicking window buttons
* Bugfix: Resizing to regular Size
*
* Version 3.5 (2010-06-09) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
* Added more config options:
* - WindowAnimationSpeed 
*
* Bugfix: iFrames can now change the size correctly
* Bugfix: The buttons now look clean, in all configurations
* Bugfix: window without Maximize button can not be maximized by double-clicking on the header
* Bugfix: When clicking on the buttons appear no more # in the Browser URL 
* Bugfix: Dragging is not longer possible by the content area. Only by Header.
* Bugfix: The content can now be scrolled
* Bugfix: Fixed IE JavaScript crashes
*
* Version 2.0 (2010-06-01) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
* Added more config options:
* - WindowResizable: 
* - WindowMaximize    
* - WindowMinimize    
* - WindowClosable   
* - WindowDraggable  
*
* Date: 2010-06-01
*/

(function($) {
    $.fn.extend({
        //plugin name - Aero Window (like Windows7 Style) 
        AeroWindow: function(options) {
            //Identify clearly this window ----------------------------------------
            WindowID = $(this).attr('id');
            if (($('body').data(WindowID)) == null || options.OpenInNewWindow) {
                var $WindowAllwaysRegistered = false;
                //Register this Window
                $('body').data(WindowID, 1);
            } else {
                //Window exists
                var $WindowAllwaysRegistered = true;
            }
            //If the window is registered, just show it and set focus ---------------     
            if ($WindowAllwaysRegistered == true) {
                var Window = $(this).find(".AeroWindow");
                var WindowContainer = $(this).find(".table-mm-container");
                var WindowContent = $(this).find(".table-mm-content");
                var BTNMin = $(this).find(".win-min-btn");
                var BTNMax = $(this).find(".WinBtnSet.winmax");
                var BTNReg = $(this).find(".WinBtnSet.winreg");
                var BTNClose = $(this).find(".win-close-btn");
                var BTNContainer = $(this).find(".buttons");
                var TitleContainer = $(this).find(".title");
                var WinIcon = $(options.WindowIcon);
                if ($('body').data('WindowLastStatus' + WindowID) == 'minimized') {
                    WinIcon.removeClass("iconBarDivHover");
                    if ($('body').data('WindowBeforeLastStatus' + WindowID) == 'maximized' || $('body').data('WindowBeforeLastStatus' + WindowID) == undefined) {
                        //Max.
                        WindowContainer.css('visibility', 'visible');
                        BTNContainer.css('visibility', 'visible');
                        TitleContainer.css('visibility', 'visible');
                        Window.draggable('enable');
                        Window.resizable('enable');
                        BTNMax.css('display', 'none');
                        BTNReg.css('display', 'block');
                        var resetNeeded = $('body').data('WindowSrcResetNeeded' + WindowID)
                        if (resetNeeded) {
                            WindowContainer.find('iframe').attr('src', options.WindowSrc);
                            TitleContainer.html('<img src="' + $(options.WindowIcon + 'Img').attr('src') + '" width="16px" />' +
                    options.WindowTitle);
                            $('body').data('WindowSrcResetNeeded' + WindowID, false);
                        }
                        WindowContent.animate({
                            width: $(window).width() - 32,
                            height: $(window).height() - 77 - 87 + 60
                        }, {
                            queue: false,
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        //Set new Window Position
                        Window.animate({
                            width: $(window).width(),
                            height: $(window).height() - 87 + 35,
                            top: 0,
                            left: 0
                        }, {
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        $('body').data('WindowBeforeLastStatus' + WindowID, $('body').data('WindowLastStatus' + WindowID));
                        $('body').data('WindowLastStatus' + WindowID, 'maximized');
                    }
                    if ($('body').data('WindowBeforeLastStatus' + WindowID) == 'regular') {
                        BTNMax.css('display', 'block');
                        BTNReg.css('display', 'none');
                        var resetNeeded = $('body').data('WindowSrcResetNeeded' + WindowID)
                        if (resetNeeded) {
                            WindowContainer.find('iframe').attr('src', options.WindowSrc);
                            TitleContainer.html('<img src="' + $(options.WindowIcon + 'Img').attr('src') + '" width="16px" />' +
                    options.WindowTitle);
                            $('body').data('WindowSrcResetNeeded' + WindowID, false);
                        }
                        WindowContainer.css('visibility', 'visible');
                        BTNContainer.css('visibility', 'visible');
                        TitleContainer.css('visibility', 'visible');
                        Window.draggable('enable');
                        Window.resizable('enable');
                        var Width = $('body').data('WindowLastRegularWidth' + WindowID) - 32;
                        var Height = $('body').data('WindowLastRegularHeight' + WindowID) - 72;
                        var Top = $('body').data('WindowLastRegularTop' + WindowID);
                        var Left = $('body').data('WindowLastRegularLeft' + WindowID);

                        WindowContent.animate({
                            width: Width,
                            height: Height + 20
                        }, {
                            queue: false,
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        Window.animate({
                            width: Width + 32,
                            height: Height + 72
                        }, {
                            queue: false,
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });

                        Window.animate({
                            top: Top,
                            left: Left
                        }, {
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        $('body').data('WindowBeforeLastStatus' + WindowID, $('body').data('WindowLastStatus' + WindowID));
                        $('body').data('WindowLastStatus' + WindowID, 'regular');
                    }
                    //Active
                    Window.css('display', 'block');
                    $(".AeroWindow").removeClass('active');
                    if (Window.hasClass('AeroWindow'))
                        Window.addClass('active');
                    if (($('body').data('AeroWindowMaxZIndex')) == null) {
                        $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                    }
                    i = $('body').data('AeroWindowMaxZIndex');
                    i++;
                    Window.css('z-index', i);
                    $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                }
                else {
                    if (Window.css('z-index') != $('body').data('AeroWindowMaxZIndex')) {
                        Window.css('display', 'block');
                        $(".AeroWindow").removeClass('active');
                        if (Window.hasClass('AeroWindow'))
                            Window.addClass('active');
                        if (($('body').data('AeroWindowMaxZIndex')) == null) {
                            $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                        }
                        i = $('body').data('AeroWindowMaxZIndex');
                        i++;
                        Window.css('z-index', i);
                        $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                    }
                    else {
                        //Min.
                        BTNReg.css('display', 'block');
                        BTNMax.css('display', 'none');
                        WindowContainer.css('visibility', 'hidden');
                        BTNContainer.css('visibility', 'hidden');
                        TitleContainer.css('visibility', 'hidden');
                        Window.draggable('disable');
                        Window.resizable('disable');
                        WindowContent.animate({
                            width: 0,
                            height: 0
                        }, {
                            queue: true,
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        //Set new Window Position
                        Window.animate({
                            width: 0,
                            height: 0,
                            top: $(window).height() - 77 - 3 + 40,
                            left: WinIcon.offset().left + 40
                        }, {
                            duration: options.WindowAnimationSpeed,
                            easing: options.WindowAnimation
                        });
                        WinIcon.addClass("iconBarDivHover");
                        Window.css('z-index', -1);
                        $('body').data('WindowLastRegularWidth' + WindowID, Window.css('width').slice(0, -2));
                        $('body').data('WindowLastRegularHeight' + WindowID, Window.css('height').slice(0, -2));
                        $('body').data('WindowLastRegularTop' + WindowID, Window.css('top').slice(0, -2));
                        $('body').data('WindowLastRegularLeft' + WindowID, Window.css('left').slice(0, -2));
                        $('body').data('WindowBeforeLastStatus' + WindowID, $('body').data('WindowLastStatus' + WindowID));
                        $('body').data('WindowLastStatus' + WindowID, 'minimized');
                    }
                }
                return;
            }

            //Settings Window and the default values---------------------------------
            var defaults = {
                WindowTitle: null,
                WindowPositionTop: 60,            /* Possible are pixels or 'center' */
                WindowPositionLeft: 10,            /* Possible are pixels or 'center' */
                WindowWidth: 300,           /* Only pixels */
                WindowHeight: 300,           /* Only pixels */
                WindowMinWidth: 50,           /* Only pixels */
                WindowMinHeight: 10,             /* Only pixels */
                WindowResizable: true,          /* true, false*/
                WindowMaximize: true,          /* true, false*/
                WindowMinimize: true,          /* true, false*/
                WindowClosable: true,          /* true, false*/
                WindowDraggable: true,          /* true, false*/
                WindowStatus: 'regular',     /* 'regular', 'maximized', 'minimized' */
                WindowAnimationSpeed: 500,
                WindowAnimation: 'easeOutElastic',
                WindowIcon: null,
                WindowID: $(this).attr('id'),
                WindowSrc: null,
                RemoveIconOnClosed: true,
                OpenInNewWindow: false
            };

            /*-----------------------------------------------------------------------
            Possible WindowAnimation:
            - easeInQuad
            - easeOutQuad
            - easeInOutQuad
            - easeInCubic
            - easeOutCubic
            - easeInOutCubic
            - easeInQuart
            - easeOutQuart
            - easeInOutQuart
            - easeInQuint
            - easeOutQuint
            - easeInOutQuint
            - easeInSine
            - easeOutSine
            - easeInOutSine
            - easeInExpo
            - easeOutExpo
            - easeInOutExpo
            - easeInCirc
            - easeOutCirc
            - easeInOutCirc
            - easeInElastic
            - easeOutElastic
            - easeInOutElastic
            - easeInBack
            - easeOutBack
            - easeInOutBack
            - easeInBounce
            - easeOutBounce
            - easeInOutBounce      
            -----------------------------------------------------------------------*/

            //Assign current element to variable, in this case is UL element
            var options = $.extend(defaults, options);

            return this.each(function() {
                var o = options;

                //Generate the new Window ---------------------------------------------     
                //var WindowContent = $(this).html();
                var WindowContent = '<iframe src="' + o.WindowSrc + '" width="100%" height="100%" frameborder="0" id="iFrame' + o.WindowID + '"></iframe>';

                //BTN --- 
                if (o.WindowMinimize) {
                    if (o.WindowMaximize || o.WindowClosable) {
                        var WinMinBtn = '<a href="#" class="win-min-btn"></a><div class="win-btn-spacer"></div>';
                    } else {
                        var WinMinBtn = '<a href="#" class="win-min-btn"></a>';
                    }
                } else {
                    var WinMinBtn = '';
                }
                //BTN ---
                if (o.WindowMaximize) {
                    if (o.WindowClosable) {
                        var WinMaxBtn = '<div class="WinBtnSet winmax"><a href="#" class="win-max-btn"></a><div class="win-btn-spacer"></div></div>';
                        var WinRegBtn = '<div class="WinBtnSet winreg"><a href="#" class="win-reg-btn"></a><div class="win-btn-spacer"></div></div>';
                    } else {
                        var WinMaxBtn = '<div class="WinBtnSet winmax"><a href="#" class="win-max-btn"></a></div>';
                        var WinRegBtn = '<div class="WinBtnSet winreg"><a href="#" class="win-reg-btn"></a></div>';
                    }
                } else {
                    var WinMaxBtn = '';
                    var WinRegBtn = '';
                }
                //BTN ---
                if (o.WindowClosable) {
                    var WinCloseBtn = '<a href="#" class="win-close-btn"></a>';
                } else {
                    var WinCloseBtn = '';
                }

                if (o.WindowMinimize || o.WindowMaximize || o.WindowClosable) {
                    var WinBtnLeftedge = '<div class="win-btn-leftedge"></div>';
                    var WinBtnRightedge = '<div class="win-btn-rightedge"></div>';
                } else {
                    var WinBtnLeftedge = '';
                    var WinBtnRightedge = '';
                }
                $(this).html(
          '<div class="AeroWindow">' +
          '  <table cellpadding="0" cellspacing="0" border="0">' +
          '    <tr>' +
          '      <td class="table-tl"></td>' +
          '      <td class="table-tm"></td>' +
          '      <td class="table-tr"></td>' +
          '    </tr>' +
          '    <tr>' +
          '      <td class="table-lm"></td>' +
          '      <td class="table-mm">' +
          '        <div class="title"><img src="' + $(o.WindowIcon + 'Img').attr('src') + '" width="16px" />' +
                    o.WindowTitle + '</div>' +
          '        <div class="buttons">' +
                     WinBtnLeftedge +
                     WinMinBtn +
                     WinMaxBtn +
                     WinRegBtn +
                     WinCloseBtn +
                     WinBtnRightedge +
          '        </div>' +
          '        <div class="table-mm-container" align="left">' +
          '          <div class="table-mm-content" style="width: ' + o.WindowWidth + 'px; height: ' + o.WindowHeight + 'px;">' +
                       WindowContent +
          '          </div>' +
          '        </div>' +
          '      </td>' +
          '      <td class="table-rm"></td>' +
          '    </tr>' +
          '    <tr>' +
          '      <td class="table-bl"></td>' +
          '      <td class="table-bm"></td>' +
          '      <td class="table-br"></td>' +
          '    </tr>' +
          '  </table>' +
          '</div>'
        );

                //Display hidden Containers -------------------------------------------
                $(this).css('display', 'block');

                //Window Objects ------------------------------------------------------
                var Window = $(this).find(".AeroWindow");
                var WindowContainer = $(this).find(".table-mm-container");
                var WindowContent = $(this).find(".table-mm-content");
                var BTNMin = $(this).find(".win-min-btn");
                var BTNMax = $(this).find(".WinBtnSet.winmax");
                var BTNReg = $(this).find(".WinBtnSet.winreg");
                var BTNClose = $(this).find(".win-close-btn");
                var BTNContainer = $(this).find(".buttons");
                var TitleContainer = $(this).find(".title");

                //Initial Configuration -----------------------------------------------
                BTNReg.css('display', 'none');
                FocusWindow(Window);

                //Set Window Position
                if (o.WindowPositionTop == 'center') {
                    o.WindowPositionTop = ($(window).height() / 2) - o.WindowHeight / 2 - 37;
                }
                if (o.WindowPositionLeft == 'center') {
                    o.WindowPositionLeft = ($(window).width() / 2) - o.WindowWidth / 2 - 17;
                }

                switch (o.WindowStatus) {
                    case 'regular':
                        RegularWindow();
                        break;
                    case 'maximized':
                        MaximizeWindow();
                        break;
                    case 'minimized':
                        MinimizeWindow();
                        break;
                    default:
                        break;
                }
                //Window Functions ----------------------------------------------------
                function MaximizeWindow() {
                    WindowContainer.css('visibility', 'visible');
                    BTNContainer.css('visibility', 'visible');
                    TitleContainer.css('visibility', 'visible');
                    Window.draggable('enable');
                    Window.resizable('enable');
                    BTNMax.css('display', 'none');
                    BTNReg.css('display', 'block');
                    WindowContent.animate({
                        width: $(window).width() - 32,
                        height: $(window).height() - 77 - 87 + 60
                    }, {
                        queue: false,
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    //Set new Window Position
                    Window.animate({
                        width: $(window).width(),
                        height: $(window).height() - 87 + 35,
                        top: 0,
                        left: 0
                    }, {
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    $('body').data('WindowBeforeLastStatus' + o.WindowID, o.WindowStatus);
                    o.WindowStatus = 'maximized';
                    $('body').data('WindowLastStatus' + o.WindowID, 'maximized');
                    return (false);
                }
                function MinimizeWindow() {
                    BTNReg.css('display', 'block');
                    BTNMax.css('display', 'none');
                    WindowContainer.css('visibility', 'hidden');
                    BTNContainer.css('visibility', 'hidden');
                    TitleContainer.css('visibility', 'hidden');
                    Window.draggable('disable');
                    Window.resizable('disable');
                    WindowContent.animate({
                        width: 0,
                        height: 0
                    }, {
                        queue: true,
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    //Set new Window Position
                    Window.animate({
                        width: 0,
                        height: 0,
                        top: $(window).height() - 77 - 3 + 40,
                        left: $(o.WindowIcon).offset().left + 40
                    }, {
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    $(o.WindowIcon).addClass("iconBarDivHover");
                    $('body').data('WindowLastRegularWidth' + o.WindowID, Window.css('width').slice(0, -2));
                    $('body').data('WindowLastRegularHeight' + o.WindowID, Window.css('height').slice(0, -2));
                    $('body').data('WindowLastRegularTop' + o.WindowID, Window.css('top').slice(0, -2));
                    $('body').data('WindowLastRegularLeft' + o.WindowID, Window.css('left').slice(0, -2));
                    $('body').data('WindowBeforeLastStatus' + o.WindowID, $('body').data('WindowLastStatus' + o.WindowID));
                    o.WindowStatus = 'minimized';
                    Window.css('z-index', -1);
                    $('body').data('WindowLastStatus' + o.WindowID, 'minimized');
                    return (false);
                }
                function RegularWindow() {
                    BTNMax.css('display', 'block');
                    BTNReg.css('display', 'none');
                    WindowContainer.css('visibility', 'visible');
                    BTNContainer.css('visibility', 'visible');
                    TitleContainer.css('visibility', 'visible');
                    Window.draggable('enable');
                    Window.resizable('enable');
                    WindowContent.animate({
                        width: o.WindowWidth,
                        height: o.WindowHeight + 20
                    }, {
                        queue: false,
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    Window.animate({
                        width: o.WindowWidth + 32,
                        height: o.WindowHeight + 72
                    }, {
                        queue: false,
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    //Set new Window Position
                    //Error handling, if the left position is negative.
                    if ((typeof (o.WindowPositionLeft) == 'string') && (o.WindowPositionLeft.substring(0, 1) == '-')) o.WindowPositionLeft = 0;
                    if (o.WindowPositionLeft == 0 || o.WindowPositionTop == 0) {
                        o.WindowPositionTop += ((Window.css('z-index') - 100) * 30);
                        o.WindowPositionLeft += ((Window.css('z-index') - 100) * 30)
                    }

                    Window.animate({
                        top: o.WindowPositionTop,
                        left: o.WindowPositionLeft
                    }, {
                        duration: o.WindowAnimationSpeed,
                        easing: o.WindowAnimation
                    });
                    $('body').data('WindowBeforeLastStatus' + o.WindowID, o.WindowStatus);
                    o.WindowStatus = 'regular';
                    $('body').data('WindowLastStatus' + o.WindowID, 'regular');
                    return (false);
                }
                function FocusWindow(Window) {
                    $(".AeroWindow").removeClass('active');
                    if (Window.hasClass('AeroWindow'))
                        Window.addClass('active');
                    if (($('body').data('AeroWindowMaxZIndex')) == null) {
                        $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                    }
                    i = $('body').data('AeroWindowMaxZIndex');
                    i++;
                    Window.css('z-index', i);
                    $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                }

                //Attach user events to the Window ------------------------------------
                if (o.WindowMaximize) {
                    $(this).dblclick(function() {
                        switch (o.WindowStatus) {
                            case 'regular':
                                MaximizeWindow();
                                break;
                            case 'maximized':
                                RegularWindow();
                                break;
                            case 'minimized':
                                if ($('body').data('WindowLastStatus' + o.WindowID) == 'maximized') {
                                    MaximizeWindow();
                                }
                                if ($('body').data('WindowLastStatus' + o.WindowID) == 'regular') {
                                    RegularWindow();
                                }
                            default:
                                break;
                        }
                    });
                } 

                //User Interaction - Minimize Button
                BTNMin.click(function() {
                    FocusWindow(Window);
                    MinimizeWindow();
                    return false;
                });
                //User Interaction - Maximize Button
                BTNMax.click(function() {
                    FocusWindow(Window);
                    MaximizeWindow();
                    return false;
                });
                //User Interaction - Regular Button
                BTNReg.click(function() {
                    FocusWindow(Window);
                    RegularWindow();
                    return false;
                });
                //Close Button
                BTNClose.click(function() {
                    //Unregister this Window
                    Window.css('display', 'none');
                    $('body').data('WindowLastRegularWidth' + o.WindowID, Window.css('width').slice(0, -2));
                    $('body').data('WindowLastRegularHeight' + o.WindowID, Window.css('height').slice(0, -2));
                    $('body').data('WindowLastRegularTop' + o.WindowID, Window.css('top').slice(0, -2));
                    $('body').data('WindowLastRegularLeft' + o.WindowID, Window.css('left').slice(0, -2));
                    $('body').data('WindowBeforeLastStatus' + o.WindowID, $('body').data('WindowLastStatus' + o.WindowID));
                    $('body').data('WindowLastStatus' + o.WindowID, 'minimized');
                    $('body').data('WindowSrcResetNeeded' + o.WindowID, true);
                    Window.css('top', 0);
                    Window.css('left', 0);
                    WindowContent.find('iframe').attr('src', '');
                    TitleContainer.html("");
                    if (o.RemoveIconOnClosed) {
                        $(o.WindowIcon).remove();
                    }
                    return (false);
                });

                //Support Dragging ----------------------------------------------------
                if (o.WindowDraggable) {
                    Window.draggable({
                        distance: 3,
                        cancel: ".table-mm-content",
                        containment: "window",
                        start: function() {
                            FocusWindow(Window);
                            $(".AeroWindow").find('#iframeHelper').css({ 'display': 'block' });
                            $(".AeroWindow").removeClass('active');
                            Window.addClass('active');
                            $('body').data('AeroWindowMaxZIndex', $(this).css('z-index'));
                        },
                        drag: function() {
                            WindowTop = -1 * $(this).offset().top;
                            WindowLeft = -1 * $(this).offset().left;
                            $(this).css({ backgroundPosition: WindowLeft + 'px ' + WindowTop + 'px' });
                        },
                        stop: function() {
                            //alert(Window.css('top'));
                            o.WindowPositionTop = Window.css('top');
                            o.WindowPositionLeft = Window.css('left');
                            $(".AeroWindow").find('#iframeHelper').css({ 'display': 'none' });
                        }
                    });
                }

                //Support Focus on Window by Click ------------------------------------
                Window.click(function() {
                    FocusWindow(Window);
                });

                //Support Window Resizing ---------------------------------------------
                if (o.WindowResizable) {
                    Window.resizable({
                        minHeight: o.WindowMinHeight + 52,
                        minWidth: o.WindowMinWidth + 200,
                        alsoResize: WindowContent,
                        start: function() {
                            WindowContainer.css('visibility', 'visible');
                            $(".AeroWindow").find('#iframeHelper').css({ 'display': 'block' });
                            $(".AeroWindow").removeClass('active');
                            Window.addClass('active');
                            if (($('body').data('AeroWindowMaxZIndex')) == null) {
                                $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                            }
                            i = $('body').data('AeroWindowMaxZIndex');
                            i++;
                            Window.css('z-index', i);
                            $('body').data('AeroWindowMaxZIndex', Window.css('z-index'));
                        },
                        stop: function() {
                            o.WindowWidth = WindowContent.width();
                            o.WindowHeight = WindowContent.height();
                            $(".AeroWindow").find('#iframeHelper').css({ 'display': 'none' });
                        }
                    });
                }
            });
        }
    });
})(jQuery);