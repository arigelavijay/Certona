(function ($) {

    // panelbar width value from user state
    var panelbarWidth = $("#hdnPanelbarWidth").val();
    if (panelbarWidth !== '') {
        panelbarWidth = parseInt(panelbarWidth);
    } else {
        panelbarWidth = 250;
    }

    AdjustCertonaLogo(panelbarWidth);
    /****************** Splitter bar ************************/
    $("#splitter-pane").kendoSplitter({
        panes: [
            // Uncomment this line and remove the next one when service is modified to return minimum width in pixels instead of perentages. currently it is 100.         
            { collapsible: true, size: panelbarWidth + "px", min: "200px", max: "500px", scrollable: false },
            //{ collapsible: true, size: "250px", min: "200px", max: "500px", scrollable: false },
            { collapsible: false }
        ],
        resize: onResize,
        collapse: onCollapse
    });


})(jQuery);


// Splitter resize event, sets the panelbarWidth for state maintanance
function onResize(e) {
    if (e.sender.resizing != null) {
        var width = e.sender.resizing._resizable.position;
        if (width != undefined) {
            // 01/16/2013 - Added to fix Bug #25
            SetAccountIDText(width);
            unbindAjaxSpinner();
            $.post('/Navigation/SetPanelBarWidth', { "width": width }, function (data) { });
            bindAjaxSpinner();
        }

        AdjustCertonaLogo(width);
    }
    SetPanelBarHeight(false, false);

    // Slight delay to resize greyed div if user is resizing browser
    setTimeout(function () {
        ResizeGreyedOutBackground('');
    }, 25);
}

function AdjustCertonaLogo(width) {
    if (width != null) {
        if (width < 250) {
            // center image so it appears more visually appealing as the maximum container only goes to a specified width
            $('#main > section > aside').css('background-image', 'url(/Images/Console_CertonaLogo-small.png)').css('background-position', 'center bottom');
        } else {
            $('#main > section > aside').css('background-image', 'url(/Images/Console_CertonaLogo.png)').css('background-position', 'left bottom');
        }
    }
}

$(window).resize(function () {
    SetPanelBarHeight(false, false);
});

// Splitter resize event
// Sets the panelbarWidth for state maintanance

function onCollapse(e) {
}


// 01/16/2013 - Added to fix Bug #25 
SetAccountIDText(-1);

function SetAccountIDText(navigationbarWidth) {
    if (navigationbarWidth < 0) {
        var splitter = $("#splitter-pane").data("kendoSplitter");
        navigationbarWidth = parseInt(splitter.size("#nav-pane"), 10);
    }

    if (navigationbarWidth > 0) {
        var contentWidth = navigationbarWidth - 115; // For account label and settings image
        if (navigationbarWidth < 250)
            contentWidth = contentWidth - 10;
        var textLength = parseInt(contentWidth / 10);
        //alert(textLength);
        var accountID = $('#hdnAccountID').val();
        //alert(accountID.length);
        if (accountID !== undefined) {
            if (accountID.length <= textLength) {
                $('#lblAccountID').html(accountID);
            } else {
                var trimmedText = accountID.substring(0, (textLength - 2)) + "..";
                //alert(trimmedText);
                $('#lblAccountID').html(trimmedText);
            }
        }
    }
}


// Called from other panels(navigation panel) to set the content

function SetContent(e) {
    var cont = true;
    var selector;

    if (!bypassUnsavedChangesDialog && !CanLeaveContentPanel()) {
        cont = false;
        unsavedChangesContinueFunction = SetContent;
        unsavedChangesContinueData = e;
    }

    if (cont) {
        selector = '#' + e.sender.element[0].id;

        ClearSelectedNodes(selector);

        var nodeData = $(selector).data('kendoTreeView').dataItem(e.node);
        if (nodeData === undefined) {
            nodeData = e.node;
        }
        var node = {
            'nodeId': nodeData.NodeID,
            'nodeType': nodeTypes[nodeData.NodeType],
            'uid': nodeData.uid,
            'classificationId': nodeData.ClassificationID,
            'display': nodeData.DisplayValue
        };

        if (node.nodeType === 'Asset') {
            node.catalogID = nodeData.CatalogID;
        }

        $(selector).data('kendoTreeView').expand('[data-uid="' + node.uid + '"]');

        // don't force content change if rebinding due to highlight of new item
        if (highlightTreeItem === undefined) {
            History.pushState(node, node.nodeType.replace('_', ' ') + ' - ' + node.display, '?nodeType=' + node.nodeType + '&nodeId=' + node.nodeId);
        }
    } else {
        e.preventDefault();

        var data = History.getState().data;

        selector = '#' + GetSelector(data.nodeType);
        ClearSelectedNodes(selector);
        $(selector).data('kendoTreeView').findByUid(data.uid).children('span:first-child').addClass('k-state-selected');

        PopupUnsavedChangesDialog();
    }
}

function ClearSelectedNodes(selector) {
    var selectedNode;

    if (selector !== '#ReportTree') {
        selectedNode = $('#ReportTree').data('kendoTreeView').select();
        if (selectedNode !== null && selectedNode !== undefined) {
            selectedNode.find('span.k-state-selected').removeClass('k-state-selected').removeClass('k-state-focus');
        }
    }

    if (selector !== '#AccountTreeView') {
        selectedNode = $('#AccountTreeView').data('kendoTreeView').select();
        if (selectedNode !== null && selectedNode !== undefined) {
            selectedNode.find('span.k-state-selected').removeClass('k-state-selected').removeClass('k-state-focus');
        }
    }

    if (selector !== '#AssetTreeView') {
        selectedNode = $('#AssetTreeView').data('kendoTreeView').select();
        if (selectedNode !== null && selectedNode !== undefined) {
            selectedNode.find('span.k-state-selected').removeClass('k-state-selected').removeClass('k-state-focus');
        }
    }

    // Used for Tools Menu
    // if (selector !== '#ToolTreeView') {
    //     selectedNode = $('#ToolTreeView').data('kendoTreeView').select();
    //     if (selectedNode !== null && selectedNode !== undefined) {
    //         selectedNode.find('span.k-state-selected').removeClass('k-state-selected');
    //     }
    // }
}

function isDirty(container) {
    var ret = false;
    var inputArray = $.makeArray($('#' + container + ' :input'));
    $.each(inputArray, function (idx, val) {
        val = $(val);
        if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button' && val.attr('type') !== 'submit') {
            var originalData = val.attr('data-original-value');

            // There is an existing bug with the implementation of dropdown lists on the CreateStrategies partial view
            if (originalData !== undefined && val.val() !== originalData) {
                ret = true;
            }
        }
    });
    return ret;
}

function cancelJqueryDialog(cpsDiv, dialog) {
    // Push dialog divs behind greyed out window
    //$("div[role='dialog']").css('z-index', '4');

    // Set the unsaved changes dialog above greyed out window
    //$("div[aria-describedby='unsaved-changes-dialog']").css('z-index', '20');

    //var cp = contentPanels.GetCurrentContentPanel();
    //var cps = cp.GetContentPanelSection(cpsDiv);
    //if (cps && cps.IsDirty()) {
    //    PopupUnsavedChangesDialog();
    //    unsavedChangesContinueFunction = cancelJqueryDialogContinue;
    //    unsavedChangesContinueData = dialog;
    //} else {
    //    cancelJqueryDialogContinue(dialog);
    //}

    cancelJqueryDialogContinue(dialog);
}

function cancelJqueryDialogContinue(dialog) {
    $(dialog).css('z-index', '100');
    $(dialog).dialog('close');
    bindAjaxSpinner(); //rebind spinner
}


function resetInputValues(container) {
    $.each($.makeArray($('#' + container + ' :input')), function (idx, val) {
        val = $(val);
        if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button' && val.attr('type') !== 'submit') {
            val.val(val.attr('data-original-value'));
        }
    });
}

function cancelEdit(form) {
    var dirtyCheck = this.isDirty(form);
    if (dirtyCheck) {
        alert('TODO: Implement unsaved changes');
    }
}

function cancelEditCPS(cpsId) {
    $("#edit-cps-" + cpsId).trigger('click');
}

function ToggleAddEditCPS(e) {
    ResizeGreyedOutBackground('');
    var cpsDiv = $(e.currentTarget).attr('data-name');
    var cpsDivId = '#' + cpsDiv;
    var nodeId = $(e.currentTarget).attr('data-node-id');
    var cpsId = $(e.currentTarget).attr('data-cps-id');
    var nodeType = $(e.currentTarget).attr('data-node-type');
    var divDisplayUrl = '';
    var divDisplayTitle = '';

    var cp = contentPanels.GetContentPanel(nodeType);
    var cps = cp.GetContentPanelSection(cpsDiv);
    var data = {};

    // ------------------------------------------------------------------
    // Create(Add) operations (Modal dialog support)
    // * Note: if the 'data-create-url' attribute exists then we need to call a seperate function for modal dialog support
    if ($(e.currentTarget).attr('data-create-url')) {
        var showHelp = true; //set to true for Create dialog's
        var createDialogContainer = '#insight-create-dialog';
        divDisplayUrl = $(e.currentTarget).attr('data-create-url');
        divDisplayTitle = $(e.currentTarget).attr('data-create-title');

        var viewInfo = {};
        viewInfo.DataName = cpsDiv;
        viewInfo.NodeId = nodeId;
        viewInfo.NodeType = nodeType;
        viewInfo.updateTargetId = $(e.currentTarget).attr('data-update-target-id');

        data = {
            cps: cps,
            showHelp: showHelp,
            createDialogContainer: createDialogContainer,
            divDisplayUrl: divDisplayUrl,
            divDisplayTitle: divDisplayTitle,
            viewInfo: viewInfo
        };

        if (cps && cps.IsEditing() && cps.IsDirty()) {
            //alert('TODO: Implement unsaved changes');
            PopupUnsavedChangesDialog();
            unsavedChangesContinueFunction = CreateModalCPSContinue;
            unsavedChangesContinueData = data;
            return;
        }

        CreateModalCPSContinue(data);
        return;
    }
    // ------------------------------------------------------------------

    // Manage(Edit) operations
    var editFlag = $('#cps-editmode-flag').val(); // current mode which comes from a hidden varaiable on the partial view (manage/edit=1, view=0)
    if (editFlag === '0')
        editFlag = $(":hidden[data-cps-id='" + cpsId + "']").val(); // should replace this with check above but need to change manage views

    // get url and title (manage vs edit)
    if (editFlag === '1') {
        // if coming from edit mode then navigate to View mode
        divDisplayUrl = $(e.currentTarget).attr('data-view-url');
        divDisplayTitle = $(e.currentTarget).attr('data-view-title');

        // add check form dirty, if so prompt user
        if (cps && cps.IsEditing() && cps.IsDirty()) {
            //alert('TODO: Implement unsaved changes');
            PopupUnsavedChangesDialog();
            unsavedChangesContinueFunction = ToggleAddEditCPSContinue;
            data = {
                url: divDisplayUrl,
                nodeId: nodeId,
                cpsId: cpsId,
                cpsDivId: cpsDivId,
                divDisplayTitle: divDisplayTitle,
                editFlag: editFlag
            };
            unsavedChangesContinueData = data;
            return;
        }
        if (cps) {
            cps.IsEditing(false);
        }
    } else {
        if (cps) {
            cps.IsEditing(true);
        }
        // if coming from view mode then navigate to Edit mode
        divDisplayUrl = $(e.currentTarget).attr('data-manage-url');
        divDisplayTitle = $(e.currentTarget).attr('data-manage-title');
    }

    data = {
        url: divDisplayUrl,
        nodeId: nodeId,
        cpsId: cpsId,
        cpsDivId: cpsDivId,
        divDisplayTitle: divDisplayTitle,
        editFlag: editFlag
    };

    ToggleAddEditCPSContinue(data);
}

function CreateModalCPSContinue(data) {
    if (data.cps) {
        data.cps.IsEditing(true);
    }
    PopupJqueryDialog(data.divDisplayTitle, data.divDisplayUrl, data.viewInfo, data.createDialogContainer, data.showHelp);
}

function ToggleAddEditCPSContinue(data) {
    // use url to populate div with appropriate partial view (this will call a controller method returning a partial view)
    $.get(data.url, { id: data.nodeId, cpsId: data.cpsId, rnd: Math.random() }, function (returnedData) {
        $(data.cpsDivId).html(returnedData);   // populates the div with the partial view returned from the controller

        // Set the title verbiage
        $(data.cpsDivId + '_TitleBar_Span').text(data.divDisplayTitle);

        // Set the background color(css)
        if (data.editFlag === '1') {
            $(data.cpsDivId).removeClass('ContentPanelSectionEdit');
        } else {
            $(data.cpsDivId).addClass('ContentPanelSectionEdit');
        }
    });
}

function toggleSaveButton(container) {
    var canSave = true;

    $.each($.makeArray($('#' + container + ' :input')), function (idx, val) {
        val = $(val);
        if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button') {
            if (val.attr('data-val-required') !== undefined) {
                if (val.val() === '') {
                    canSave = false;
                    return false;
                }
            }
        }
    });

    if (canSave) {
        $('.insight-submit-btn').removeAttr('disabled');
    } else {
        $('.insight-submit-btn').attr('disabled', 'disabled');
    }
}

function ClearDialogs() {
    $("div[role='dialog']").css('z-index', '100');
}

function UnsavedChangesContinue() {
    bypassUnsavedChangesDialog = true;

    $("div[role='dialog']").css('z-index', '100');

    $("#unsaved-changes-dialog").dialog('close');
    unsavedChangesContinueFunction(unsavedChangesContinueData);
}

function CanLeaveContentPanel() {
    var canLeave = true;
    var cp = contentPanels.GetCurrentContentPanel();
    if (cp !== null && cp !== undefined) {
        $.each(cp.Sections(), function (idx, val) {
            if (val.IsEditing() && val.IsDirty()) {
                canLeave = false;
                return false;
            }
        });
    }
    return canLeave;
}

function GetSelector(nodeType) {
    switch (nodeType) {
        case "Application":
        case "Package":
        case "Scheme":
        case "Variant":
        case "Remarketing":
        case "Experience":
            return 'AccountTreeView';
        case "Asset":
        case "Catalog":
        case "Custom_List":
        case "Exclusion_List":
        case "Mapping_Node":
        case "Models":
            return 'AssetTreeView';
        case "Report":
            return 'ReportTree';
        case "Tool":
            return 'ToolTreeView';
    }
}

(function (window, undefined) {
    var History = window.History; // Note: We are using a capital H instead of a lower h
    if (!History.enabled) {
        // History.js is disabled for this browser.
        // This is because we can optionally choose to support HTML4 browsers or not.
        return false;
    }

    // Bind to StateChange Event
    // Note: We are using statechange instead of popstate
    History.Adapter.bind(window, 'statechange', function () {
        var cont = true;

        if (bypassUnsavedChangesDialog) {
            bypassUnsavedChangesDialog = false;
        } else if (!bypassUnsavedChangesDialog && !CanLeaveContentPanel()) {
            cont = false;
            unsavedChangesContinueFunction = $(window).trigger;
            unsavedChangesContinueData = 'statechange';
        }

        var selector;

        if (cont) {
            var data = History.getState().data;

            if (data.hasOwnProperty('nodeId')) {

                var treeview, node;
                var controllerUri = '/Content/GetContentPanel';

                selector = GetSelector(data.nodeType);
                if (selector === 'ReportTree') {
                    controllerUri = '/Reporting/ViewReport';
                }

                if (selector != undefined) {
                    treeview = $('#' + selector).data('kendoTreeView');

                    if (treeview != undefined) {
                        if (data.uid != undefined) {
                            node = treeview.findByUid(data.uid);
                            $.post('/Navigation/SetSelectedNode', { "nodeID": data.nodeId, "nodeType": data.nodeType, "classificationID": data.classificationId }, function () {
                            });
                        } else {
                            $.each(treeview.findByText(data.display), function (idx, val) {
                                if (val.getAttribute('data-id') === data.nodeId && val.getAttribute('data-node-type') === data.nodeType) {
                                    node = treeview.findByUid(val.getAttribute('data-uid'));
                                    return false;
                                }
                            });
                            // secondary check - above check may never work because the text in the tree contains markup
                            if (!node) {
                                var nodeData = treeview.dataSource.data();
                                var foundNode = findNodeByTypeAndID(nodeData, $.inArray(data.nodeType, nodeTypes), data.nodeId);
                                if (foundNode) {
                                    node = treeview.findByUid(foundNode.uid);
                                }
                            }
                        }

                        treeview.select(node);
                    }

                    $("#content-pane").load(controllerUri, { NodeType: data.nodeType, nodeID: data.nodeId, catalogID: data.catalogID, rand: new Date().getTime() }, function () {
                    });
                }
            }
        } else {
            var lastData = History.savedStates[History.savedStates.length - 2].data;

            selector = '#' + GetSelector(lastData.nodeType);

            ClearSelectedNodes(selector);

            $(selector).data('kendoTreeView').findByUid(lastData.uid).addClass('k-state-selected');

            // TODO: Implement unsaved changes
            PopupUnsavedChangesDialog();
        }
    });

    $(function () {
        $('#ReportTree').kendoTreeView({
            dataSource: {
                schema: {
                    model: {
                        id: 'NodeID',
                        hasChildren: false
                    }
                },
                transport: {
                    read: {
                        url: '/Navigation/GetReportTreeItems',
                        cache: false
                    }
                },
                requestStart: reportTreeRequestStart,
                requestEnd: reportTreeRequestEnd
            },
            dataTextField: 'DisplayValue',
            dataBound: ReportTreeDataBound,
            select: SetContent
        });

        $('#AccountTreeView').kendoTreeView({
            dataSource: {
                schema: {
                    model: {
                        id: 'NodeID',
                        hasChildren: function (data) {
                            if (data.ChildNodes.length > 0)
                                return true;
                            else
                                return false;
                        },
                        children: 'ChildNodes'
                    }
                },
                transport: {
                    read: {
                        url: '/Navigation/GetAccountTreeItems',
                        cache: false
                    }
                },
                requestStart: accountTreeRequestStart,
                requestEnd: accountTreeRequestEnd
            },
            dataTextField: 'DisplayValue',
            loadOnDemand: false,
            dataBound: AccountTreeDataBound,
            select: SetContent,
            collapse: AccountTreeViewCollapse,
            expand: AccountTreeViewExpand,
            template: kendo.template($("#treeview-template").html())
        });

        $('#AssetTreeView').kendoTreeView({
            dataSource: {
                schema: {
                    model: {
                        id: 'NodeID',
                        hasChildren: function (data) {
                            if (data.ChildNodes.length > 0)
                                return true;
                            else
                                return false;
                        },
                        children: 'ChildNodes'
                    }
                },
                transport: {
                    read: {
                        url: '/Navigation/GetAssetTreeItems',
                        cache: false
                    }
                },
                requestStart: assetTreeRequestStart,
                requestEnd: assetTreeRequestEnd
            },
            dataTextField: 'DisplayValue',
            loadOnDemand: false,
            dataBound: AssetTreeDataBound,
            select: SetContent,
            collapse: AssetTreeViewCollapse,
            expand: AssetTreeViewExpand
        });

        $('#ToolTreeView').kendoTreeView({
            dataSource: {
                schema: {
                    model: {
                        id: 'NodeID',
                        hasChildren: false
                    }
                },
                transport: {
                    read: {
                        url: '/Navigation/GetToolTreeItems',
                        cache: false
                    }
                },
                requestStart: toolTreeRequestStart,
                requestEnd: toolTreeRequestEnd
            },
            dataTextField: 'DisplayValue',
            dataBound: ToolTreeDataBound,
            select: SetContent
        });

        // Handler for Add/Edit buttons on Content Panel Section header
        $(document).on('click', '.toggleAddEdit', function (e) {
            e.stopPropagation();
            ToggleAddEditCPS(e);
        });

        $(document).on('click', '.ContentPanelSectionTitleBarExpanded,.ContentPanelSectionTitleBarCollapsed', function (e) {
            e.stopPropagation();
            var panelName = $(this).attr('data-name');
            var cpsId = $(this).attr('data-cpsId');
            var titleBar = $('#' + panelName + '_TitleBar');
            var contentPanelSection = $('#' + panelName);

            var image = $('#imgToggleVisibility_' + panelName);

            if (titleBar.hasClass('ContentPanelSectionTitleBarExpanded')) {
                titleBar.removeClass('ContentPanelSectionTitleBarExpanded');
                titleBar.addClass('ContentPanelSectionTitleBarCollapsed');
                contentPanelSection.removeClass('ContentPanelSectionExpanded');
                contentPanelSection.addClass('ContentPanelSectionCollapsed');
                image.attr('src', '/Images/imgCollapsed.png');
            } else {
                titleBar.removeClass('ContentPanelSectionTitleBarCollapsed');
                titleBar.addClass('ContentPanelSectionTitleBarExpanded');
                contentPanelSection.removeClass('ContentPanelSectionCollapsed');
                contentPanelSection.addClass('ContentPanelSectionExpanded');
                image.attr('src', '/Images/imgExpanded.png');
                $(window).resize(); // fire event to redraw trends chart
                //check if expand function exists for content panel section; if it does, call it
                var script = 'if(typeof ' + panelName + '_Expanded === \'function\'){' + panelName + '_Expanded();}';
                eval(script);
            }

            unbindAjaxSpinner(); // unbind ajax spinner
            $.get(setContentSectionStateUrl, { contentPanelSectionID: cpsId, rand: new Date().getTime() }, function (data, textStatus, jqXHR) {
            });
            bindAjaxSpinner(); // Rebind ajax spinner
        });

        $(document).on('keydown', function (evt) {
            if (evt.ctrlKey && evt.shiftKey && evt.altKey && evt.which == 84) {
                $("#content-pane").load('/Test/Index', null, function () {
                });
            }
        });
    });

})(window);