$("#success").hide();
$("#error").hide();

// Displays the system message box based on type 

function ShowsysMessage(type, content) {
    CloseSysMessage();
    if (type == "error") {
        $("#errorContent").html(content);
        $("#error").show(500);

        setTimeout(function() {
            $("#error").hide(500);}, 4000);

    } else if (type == "success") {
        $("#successContent").html(content);
        $("#success").show(500);
        
        setTimeout(function () {
            $("#success").hide(500);}, 4000);
    }
}

// Closes the system message

function CloseSysMessage() {
    $("#success").hide(500);
    $("#error").hide(500);
}

// close click event
$(".closeSysMessage").click(function() {
    CloseSysMessage();
});

// closes the system message box when user is clicked on content
$(".successSysMessage").click(function() {
    CloseSysMessage();
});

// closes the system message box when user is clicked on content
$(".errorSysMessage").click(function() {
    CloseSysMessage();
});