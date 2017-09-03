
function HideModalOverlay() {
    $(".greyedoutmodal").css('display', 'none');
    $("#swapaccount").css('display', 'block');
}

function ShowModalOverlay() {
    $(".greyedoutmodal").css('display', 'block');

    var separator = $("div[role='separator']");
    $("#separator-greyed-out").css('top', $("#logopanel").height() + 5).css('left', separator.position().left).css('height', '100%').css('width', separator.width() + 2);
}

function HideParentModalOverlay(parentModelId) {
    // To include header also we need to check on ui-dialog
    $(parentModelId).closest('.ui-dialog').find('.greyedoutparentmodal').css('display', 'none');
}

function ShowParentModalOverlay(parentModelId) {
    $(parentModelId).closest('.ui-dialog').find('.greyedoutparentmodal').css('display', 'block');
}

function PopupJqueryDialog(title, url, viewInfo, dialogContainer, showHelp) {
    ShowModalOverlay();

    var modalHelpFunction = '';
    $(dialogContainer).css('display', 'block');

    // set defaults (if not passed in)
    title = typeof title !== 'undefined' ? title : '';
    url = typeof url !== 'undefined' ? url : blankDefaultUrl;

    // Add  help(?) functionality to dialog
    if (showHelp) {
        modalHelpFunction = function (e, i) {
            var titleHtml = "<span class=\"help-title-bar\" onclick=\"help('" + url + "');\">?</span>";

            // Determine if this is the initial modal request or not & append a new div if it is
            if ($(this).parent().find('.ui-dialog-titlebar').html().indexOf("helpTitleBar") < 0) {
                $(this).parent().find(".ui-dialog-titlebar").append("<div id='helpTitleBar'></div>");
            }

            // Removes focus from close button
            $('.ui-dialog :button').blur();

            // Populate div HTML with currently requested partial view URL
            $("#helpTitleBar").html(titleHtml);
        };
    }

    unbindAjaxSpinner();

    // get content for dialog window
    if (viewInfo) {
        $.get(url, viewInfo, function (data) {
            $(dialogContainer).html(data);
        });
    } else {
        $.get(url, function (data) {
            $(dialogContainer).html(data);
        });
    }


    $(dialogContainer).dialog({
        closeText: "x",
        position: 'center',
        close: function () {
            HideModalOverlay();
            $(dialogContainer).html(''); // remove existing dialog content
            bindAjaxSpinner();
        },
        title: title,
        minWidth: 650,
        open: modalHelpFunction,
        dialogClass: 'dialogClass',
        modal: true
    });
}

var modelZIndex = 3000;
function PopupJqueryMultiDialog(title, url, viewInfo, dialogContainer, showHelp, parentModelId, evt, minWidth, maxHeight) {
    ShowParentModalOverlay(parentModelId);

    // get content for dialog window
    if (viewInfo) {
        $.get(url, viewInfo, function (data) {
            $(dialogContainer).html(data);
        });
    } else {
        $.get(url, function (data) {
            $(dialogContainer).html(data);
        });
    }

    $(dialogContainer).css('display', 'block');
    $(dialogContainer).dialog({
        closeText: "x",
        position: 'center',
        close: function () {
            modelZIndex = modelZIndex - 1;
            HideParentModalOverlay(parentModelId);
            $(dialogContainer).html(''); // remove existing dialog content
            //bindAjaxSpinner();
        },
        title: title,
        minWidth: minWidth,
        maxHeight: maxHeight,
        dialogClass: 'dialogClass',
        modal: true,
        zindex: 1001,
    });

    modelZIndex = modelZIndex + 1;
    $(dialogContainer).closest('.ui-front').css('z-index', modelZIndex);
    $(dialogContainer).dialog('option', 'position', [(window.innerWidth / 2) - ($(dialogContainer).outerWidth() / 2), evt.clientY]);
}

function PopupUnsavedChangesDialog() {
    var overlayState = $(".greyedoutmodal").css('display');
    ShowModalOverlay();

    var dialogContainer = '#unsaved-changes-dialog';
    $(dialogContainer).css('display', 'block');

    unbindAjaxSpinner();

    $.get(unsavedChangesDialogUrl, function (data) {
        $(dialogContainer).html(data);
    });

    bindAjaxSpinner();

    $(dialogContainer).dialog({
        closeText: "x",
        position: 'center',
        close: function () {
            if (overlayState === 'none')
                HideModalOverlay();

            ClearDialogs();
            $(dialogContainer).html(''); // remove existing dialog content
        },
        title: 'Unsaved Changes',
        minWidth: 650,
        modal: true,
        dialogClass: 'dialogClass',
    });
}


function help(url) {
    //alert("Content partial view url: " + url);
    alert("Once the help architecture is complete this will display a help page.  This is an internal message only.");
}

