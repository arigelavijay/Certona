
function GoContent(elementId, nodeId, nodeType, displayValue, uId, classificationId) {
    var e = {
        sender: {
            element: [{
                id: elementId
            }]
        },
        node: {
            NodeID: nodeId,
            NodeType: nodeType, 
            uid: uId,
            ClassificationID: classificationId,
            DisplayValue: displayValue
        }
    };
    SetContent(e);
}

function GoExperienceContent(nodeId, displayValue) {
    // 16 == Experience
    GoContent('AccountTreeView', nodeId, 16, displayValue, null, null);
}

function GoStrategyContent(nodeId, displayValue) {
    // 6 == Strategy (variant)
    GoContent('AccountTreeView', nodeId, 6, displayValue, null, null);
}

function BindGridSelectAll() {
    $('#chkSelectAll').change(function() {
        if ($(this).is(":checked")) {
            $.each($(':checkbox'), function() {
                if ($(this).data("role") == 'gridSelectCheckBox') {
                    $(this).attr('checked', 'checked');
                }
            });
        } else {
            $.each($(':checkbox'), function() {
                if ($(this).data("role") == 'gridSelectCheckBox') {
                    $(this).removeAttr('checked');
                }
            });
        }
    });
}

function GridSelectCheckbox(id, dataTag) {
    $.each($(':checkbox'), function () {
        if ($(this).data("role") == 'gridSelectCheckBox' && $(this).data(dataTag) == id) {
            $(this).attr('checked', 'checked');
        }
    });
}

function GridRow_Mouseover(id) {
    $(document).on('mouseover', '#' + id + ' > table > tbody tr', function (e) {
        SelectGridRow(this, id, true);
    });
}

function GridRow_Mouseout(id) {
    $(document).on('mouseout', '#' + id + ' > table > tbody tr', function (e) {
        SelectGridRow(this, id, false);
    });
}

function SelectGridRow(obj, gridId, select) {    
    $("#" + gridId + " > table > tbody tr:eq(" + (obj.rowIndex - 1) + ") .k-grid-edit").toggle(select);
    if (select) {
        $(obj).addClass('gridrow-hover');
    } else {
        $(obj).removeClass('gridrow-hover');
    }
}

function HighlightGridRow(gridId, keyName, keyValue, masterRowsOnly) {    
    var grid = $("#" + gridId).data("kendoGrid");
    var matchingRow = undefined;
    var rows;
    if (masterRowsOnly && masterRowsOnly === true) {
        rows = $("#" + gridId + " > table > tbody .k-master-row ");
    } else {
        rows = $("#" + gridId + " > table > tbody tr");
    }    
    $.each(rows, function(idx, val) {
        var obj = grid.dataItem(val);
        if (obj.hasOwnProperty(keyName) && obj[keyName] === keyValue) {            
            matchingRow = val;
            return false;
        }
    });
    
    if (matchingRow) {
        // look into animate() functions    
        $(matchingRow).css('background-color', 'rgb(204, 255, 204)');
        setTimeout(function () {
            if ($(matchingRow).hasClass('k-alt')) {
                $(matchingRow).animate({
                    backgroundColor: 'rgb(248, 248, 248)'
                }, 1500);
            } else {
                $(matchingRow).animate({
                    backgroundColor: 'transparent'
                }, 1500);
            }
        }, 3000);       
    }
}