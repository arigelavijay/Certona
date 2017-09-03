(function ($) {

    ajaxErrorSetup();
    bindAjaxSpinner();

})(jQuery);

// ==================== AJAX Helper functions ===========================
function ajaxErrorSetup() {
    // **Note: to trap specific error numbers, use:
    //        if (x.status == 403) { ...
    $.ajaxSetup({
        error: function (x, status, error) {
            var msg = "An error has occurred, please refresh the browser.  If the problem persists, please contact support.<br /><br />";
            ShowsysMessage("error", msg);
            bindAjaxSpinner();
            $.post("/Error/LogJavaScriptError", { message: error }); // Log exception via ELMAH
        }
    });
}

// Bind jQuery's ajaxStart and ajaxStop methods to display spinner
function bindAjaxSpinner() {
    $("div#spinnerContainer").bind("ajaxStart.label", function () { $(this).fadeIn("fast"); });
    $("div#spinnerContainer").bind("ajaxStop.label", function () { $(this).fadeOut("fast"); });
}

// Un-Bind jQuery's ajaxStart and ajaxStop methods to display spinner
function unbindAjaxSpinner() {
    $("div#spinnerContainer").unbind(".label");
}
// =======================================================================