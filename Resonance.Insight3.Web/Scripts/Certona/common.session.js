(function ($) {
    
    /***************** Session Manager *********************/
    // SessionManager Module
    var sessionTimeoutSeconds = 1800, // 30 * 60 = 30min
        countdownSeconds = 300, // 5 * 60 = 5min
        secondsBeforePrompt = sessionTimeoutSeconds - countdownSeconds,
        displayCountdownIntervalId,
        promptToExtendSessionTimeoutId,
        originalTitle = document.title,
        count = countdownSeconds,
        extendSessionUrl = '/Session/Extend',
        sessionExpired = false;

    // Opens session expired window
    function endSession() {
        CloseWindow();
        sessionExpired = true;
        window.setTimeout(function () {
            OpenWindow("500", "250", "Session/Timeout", "Your session is expired please login again");
        }, 50);

        _Redirect(sessionTimeoutLoginUrl);
    }

    // If user selects close icon on session expire window then user is redirected to login screen
    function onClose() {
        if (sessionExpired) {
            _Redirect("Authentication/Login");
        }
    }

    // redirects to login screen
    function _Redirect(url) {
        // IE8 and lower fix
        if (navigator.userAgent.match(/MSIE\s(?!9.0)/)) {
            var referLink = document.createElement('a');
            referLink.href = url;
            document.body.appendChild(referLink);
            referLink.click();
        }

            // All other browsers
        else {
            window.location.href = url;
        }
    }

    // Called when user selects continue on session warning
    function ActionContinue() {
        CloseWindow();
        refreshSession();
    }

    // User is redirected to relogin screen when session is expired
    function ActionRelogin() {
        var account = $("#Account").val();
        var userName = $("#UserName").val();
        var password = $("#Password").val();
        $.ajax({
            url: '/Session/Timeout',
            type: 'Post',
            data: { "Account": account, "UserName": userName, "Password": password },
            success: function (result) {
                if (result == 'Success') {
                    sessionExpired = false;
                    CloseWindow();
                    refreshSession();
                } else {
                    $("#timeoutmessagecontainer").removeClass('hidden');
                    $("#messagecontent").html(result);
                }
            }
        });
    }

    // display timer on session timeout warning message
    function displayCountdown() {
        var countdown = function () {
            var cd = new Date(count * 1000),
                minutes = cd.getUTCMinutes(),
                seconds = cd.getUTCSeconds(),
                minutesDisplay = minutes === 1 ? '1 minute ' : minutes === 0 ? '' : minutes + ' minutes ',
                secondsDisplay = seconds === 1 ? '1 second' : seconds + ' seconds',
                cdDisplay = minutesDisplay + secondsDisplay;

            var kendoWindow = $("#session-dialog").data("kendoWindow");
            var windowContent = kendoWindow.content();
            //alert($('#btnContinue'));
            $('#sm-countdown').html(cdDisplay);

            $('#aContinue').unbind('click', ActionContinue);
            $('#aContinue').bind('click', ActionContinue);

            $('#relogin').unbind('click', ActionRelogin);
            $('#relogin').bind('click', ActionRelogin);

            if (count === 0) {
                endSession();
            }
            count--;
        };
        countdown();
        displayCountdownIntervalId = window.setInterval(countdown, 1000);
    }

    // kendow window open
    function OpenWindow(width, height, url, title) {
        if (!$("#session-dialog").data("kendoWindow")) {
            $("#session-dialog").kendoWindow({
                content: { url: url },
                draggable: true,
                iframe: false,
                modal: true,
                resizable: true,
                width: width + "px",
                title: title,
                animation: false,
                close: onClose,
                visible: false,
                activate: function () {
                    if (sessionExpired) {
                        $('#Password').focus();
                    }
                }
            }).data("kendoWindow").center().open();
            window.setTimeout(function () {
                $("#session-dialog").data("kendoWindow").center().open();
            }, 50);
        } else {
            $("#session-dialog").data("kendoWindow").title(title);
            $("#session-dialog").data("kendoWindow").refresh(url);
            window.setTimeout(function () {
                $("#session-dialog").data("kendoWindow").center().open();
            }, 50);
        }
    }

    // kendow window close
    function CloseWindow() {
        $("#session-dialog").data("kendoWindow").close();
    }

    // session timeout warning display
    function promptToExtendSession() {
        OpenWindow("500", "100", sessionWarningUrl, "Session timeout warning");
        count = countdownSeconds;
        displayCountdown();
    }

    // refreshes session calling dummy method on server
    function refreshSession() {
        sessionExpired = false;
        window.clearInterval(displayCountdownIntervalId);
        var img = new Image(1, 1);
        img.src = extendSessionUrl;
        window.clearTimeout(promptToExtendSessionTimeoutId);
        startSessionManager();
    }

    // Restart the session manager process(If user doing something)
    function restartSession() {
        sessionExpired = false;
        window.clearInterval(displayCountdownIntervalId);
        window.clearTimeout(promptToExtendSessionTimeoutId);
        startSessionManager();
    }

    // start session manager process
    function startSessionManager() {
        sessionExpired = false;
        promptToExtendSessionTimeoutId =
            window.setTimeout(promptToExtendSession, secondsBeforePrompt * 1000);
    }

    startSessionManager();

    // Whenever an input changes, extend the session,
    // since we know the user is interacting with the site.
    $(':input').change(function () {
        restartSession();
    });

    $(window).bind('click', null, function (e) {
        restartSession();
    });
    
})(jQuery);