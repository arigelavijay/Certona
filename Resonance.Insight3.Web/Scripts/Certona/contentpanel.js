(function($) {
    /****************** User Setting ************************/
    $('#dvUserSetting').hide();
    $('#dvUserHelp').hide();

    $("#aUserSetting").click(function () {
        $('#dvUserSetting').removeClass("hidden");
        $("#aUserSetting").addClass("userSettingActive");

        hideHelpSettings();

        $('#dvUserSetting').slideToggle('500', function () {
            if ($('#dvUserSetting').is(':hidden')) {
                $("#aUserSetting").removeClass("userSettingActive");
            } else {
                $("#aUserSetting").addClass("userSettingActive");
            }
        });
    });

    $("#aUserHelp").click(function () {
        $('#dvUserHelp').removeClass("hidden");
        $("#aUserHelp").addClass("userHelpActive");

        hideUserSettings();

        $('#dvUserHelp').slideToggle('500', function () {
            if ($('#dvUserHelp').is(':hidden')) {
                $("#aUserHelp").removeClass("userHelpActive");
            } else {
                $("#aUserHelp").addClass("userHelpActive");
            }
        });
    });
    
    // confirm dialog sample
    $('#modaldialog').click(function() {
        $.get('/Content/views/confirm.html', function(data) {
            $("#modal-dialog").html(data);
        });

        if (!$("#modal-dialog").data("kendoWindow")) {
            $("#modal-dialog").kendoWindow({
                width: "554px",
                modal: true,
                resizable: true,
                title: 'Saving this change will affect all the instances where the rule is used',
                animation: {
                    open: {
                        effects: { fadeIn: {} },
                        duration: 500,
                        show: true
                    },
                    close: {
                        effects: { fadeOut: {} },
                        duration: 500,
                        hide: true
                    }
                },
                visible: false
            });
            window.setTimeout(function() {
                $("#modal-dialog").data("kendoWindow").center().open();
            }, 100);
        } else {
            $("#modal-dialog").data("kendoWindow").center().open();
        }
    });

    // alert diolog sample
    $('#alertdialog').click(function() {
        $.get('/Content/views/success.html', function(data) {
            $("#alert-dialog").html(data);
        });
        if (!$("#alert-dialog").data("kendoWindow")) {
            $("#alert-dialog").kendoWindow({
                animation: {
                    open: {
                        effects: { fadeIn: {} },
                        duration: 500,
                        show: true
                    },
                    close: {
                        effects: { fadeOut: {} },
                        duration: 500,
                        hide: true
                    }
                },
                width: "454px",
                modal: true,
                resizable: true,
                title: 'Saving this change will affect all.',
                visible: false
            });
            window.setTimeout(function() {
                $("#alert-dialog").data("kendoWindow").center().open();
            }, 100);
        } else {
            $("#alert-dialog").data("kendoWindow").center().open();
        }
        $('#alert-dialog').parent().find('div.k-window-content').css('margin-top', '-15px'); //To set height as per spec
        $('#alert-dialog').parent().find('.k-window-titlebar').css('height', $('#alert-dialog_wnd_title').css('height'));
    });

    $("#successmessage").click(function() {
        ShowsysMessage("success", "Application Details saved successfully.");
    });

    $("#errormessage").click(function() {
        ShowsysMessage("error", "Required fields must be completed.");
    });
})(jQuery);

// To close success window

function hideUserSettings() {
    if ($("#dvUserSetting").is(":visible")) {
        $('#dvUserSetting').removeClass("hidden");
        $("#aUserSetting").addClass("userSettingActive");

        $('#dvUserSetting').slideToggle('500', function () {
            if ($('#dvUserSetting').is(':hidden')) {
                $("#aUserSetting").removeClass("userSettingActive");
            } else {
                $("#aUserSetting").addClass("userSettingActive");
            }
        });
    }
}

function hideHelpSettings() {
    if ($("#dvUserHelp").is(":visible")) {
        $('#dvUserHelp').removeClass("hidden");
        $("#aUserHelp").addClass("userHelpActive");
        $('#dvUserHelp').slideToggle('500', function () {
            if ($('#dvUserHelp').is(':hidden')) {
                $("#aUserHelp").removeClass("userHelpActive");
            } else {
                $("#aUserHelp").addClass("userHelpActive");
            }
        });
    }
}

function closeSuccessWindow() {
    $("#alert-dialog").data("kendoWindow").close();
}

// To close confirm window

function closeConfirmWindow() {
    $("#modal-dialog").data("kendoWindow").close();
}

function displayErrorModal(message, width, title) {
    $("#modal-dialog").html("<span>" + message + "</span>");

    if (!$("#modal-dialog").data("kendoWindow")) {
        $("#modal-dialog").kendoWindow({
            width: width + "px",
            modal: true,
            resizable: true,
            title: title,
            animation: {
                open: {
                    effects: { fadeIn: {} },
                    duration: 500,
                    show: true
                },
                close: {
                    effects: { fadeOut: {} },
                    duration: 500,
                    hide: true
                }
            },
            visible: false
        });
        window.setTimeout(function () {
            $("#modal-dialog").data("kendoWindow").center().open();
        }, 100);
    } else {
        $("#modal-dialog").data("kendoWindow").center().open();
    }

}