$(function () {
    bindInfoPanelEvents();
    calculateInnerHeight();
    $(window).resize(function () {
        calculateInnerHeight();
    });
});

function calculateInnerHeight() {
    var height = $(window).height();    // window.innerHeight <= NOT AVAILABLE in IE8
    var screenHeight = height - 79 - $('#header').height(); // Subtract title bar/header from total page height - scrolling will occur in innerContainer DIV
    $("#innerContainer").css({ height: screenHeight });
}

function bindInfoPanelEvents() {
    // Hotspot mouseover
    $(document).on('mouseover', '.hotspot', function () {
        var hotspot = $(this).attr("data-hotspot");
        if (hotspot) {
            ShowInfo(hotspot);
        }
    });
    $(document).on('mouseout', '.hotspot', function () {
        HideInfo();
    });
}
