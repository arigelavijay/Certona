(function($) {

    /************* Account list******************/
    if (document.getElementById('ddlAccount') != undefined) {
        $("#ddlAccount").selectbox();

        $("#sbSelector_" + $.data($('#ddlAccount')[0], 'selectbox').uid).click(function(e) {
            e.stopPropagation();
        });

        $("#ddlAccount").hide();
        $("#ddlAccountList").hide();

        // Account selection
        $("#swapaccount").click(function (e) {
            e.stopPropagation();
            var ddl = document.getElementById('ddlAccount');

            $("#swapaccount").toggle();
            $("#lblAccountID").toggle();
            $("#ddlAccountList").toggle();
            $("#ddlAccountList").click(function(e) {
                e.stopPropagation();
            });

            var sbSelector = $("#sbHolder_" + $.data($('#ddlAccount')[0], 'selectbox').uid);
            sbSelector.on('keyup', function (event) {
                var sbOptions = $("#sbOptions_" + $.data($('#ddlAccount')[0], 'selectbox').uid);
                if (sbOptions.css('display') === 'none' && event.which === 27) {
                    //Escape key was pressed and the selectbox is hidden, we can toggle back to standard view
                    $("#swapaccount").toggle();
                    $("#lblAccountID").toggle();
                    $("#ddlAccountList").toggle();
                }
            });

            if ($("#ddlAccountList").css('display') !== 'hidden') {
                $.selectbox._openSelectbox(ddl);
            }
        });

        // Account change event
        // Loads the navigation panel based on account change
        $('#ddlAccount').change(function () {
            // var accountId = $(this).val(); // this no workie because of the selectbox() plugin
            var accountId = $("#sbSelector_" + $.data($(this)[0], 'selectbox').uid)[0].innerHTML; // Bug #45 - hack to get the true selected value from the plugin markup
            $.get(updateLastAccountIdUrl, { "accountID": accountId, rand: new Date().getTime() }, function (data) {
                $('#content-pane').load(getAccountChangedUrl);

                //Reload Report Tree View Data
                var reportTree = $("#ReportTree").data("kendoTreeView");
                reportTree.dataSource.read();
                
                //Reload Account Tree View Data
                var accountTree = $("#AccountTreeView").data("kendoTreeView");
                accountTree.dataSource.read();

                //Reload Asset Tree View Data
                var assetTree = $("#AssetTreeView").data("kendoTreeView");
                assetTree.dataSource.read();

                //Reload Tool Tree View Data
                var toolTree = $("#ToolTreeView").data("kendoTreeView");
                if (toolTree != undefined) {
                    toolTree.dataSource.read();
                }
                
                $.get(getPanelBarStatesUrl, { rand: new Date().getTime() }, function (panelBarIds) {
                    var panelBar = $('#PanelBar').data('kendoPanelBar');
                    $.each(panelBarIds, function (idx, val) {
                        var selector = $('[data-panel-bar-id=' + val +']');
                        panelBar.select(selector);
                    });
                });

                $('#chkActive').prop('checked', false);
                $('#chkInactive').prop('checked', false);
                $('#chkDeleted').prop('checked', false);

                $.get(getAccountExplorerStatusesUrl, { rand: new Date().getTime() }, function(statuses) {
                    $.each(statuses, function(idx, val) {
                        $('#chk' + val).prop('checked', true);
                    });
                });


                $('#hdnAccountID').val(accountId);
                // 01/16/2013 - Added to fix Bug #25 
                SetAccountIDText(-1);

                $("#swapaccount").show();
                $("#lblAccountID").show();
                $("#ddlAccountList").hide();
            });
        });
    }

    $("#divInfo").html(defaultInfoPanelText);

    // Reolads navigation panel when error occurs
    $('#aClickHere').click(function() {
        $.get(homeNaviagtionPanelUrl, { "accountID": "", rand: new Date().getTime() }, function (data) {
            $('#nav-pane').html(data);
        });
    });
    $("#accountCollapse").hide();
    $("#accountstatus").hide();

    // Account treeview expand all event
    $("#accountExpand").click(function() {
        $("#accountCollapse").show();
        $("#accountExpand").hide();
        var treeview = $("#AccountTreeView").data("kendoTreeView");
        expandCollapseAllAccountNodes = true;
        treeview.expand(".k-item");
        expandCollapseAllAccountNodes = false;
    });

    // Account treeview collapse all event
    $("#accountCollapse").click(function() {
        $("#accountCollapse").hide();
        $("#accountExpand").show();
        var treeview = $("#AccountTreeView").data("kendoTreeView");
        expandCollapseAllAccountNodes = true;
        treeview.collapse(".k-item");
        expandCollapseAllAccountNodes = false;
    });

    // Account explorer settings icon click event
    $('#aconfig').click(function () {
        if ($('#accountstatus').is(':hidden')) {
            $("#aconfig").attr('src', '/Images/Nav_AcctExpl_BoltAct.png');
            $("#aconfig").removeClass("accountconfig");
            $("#aconfig").addClass("accountconfigactive");
        } else {
            $("#aconfig").attr('src', '/Images/Nav_AcctExpl_Bolt.png');
            $("#aconfig").removeClass("accountconfigactive");
            $("#aconfig").addClass("accountconfig");
        }
        $('#accountstatus').slideToggle('500', function() {
            if ($('#accountstatus').is(':hidden')) {
                if (accountStatusChanged) {
                    // Bug # 44,46 - add extra data parameter to prevent caching of call to controller method
                    var treeview = $("#AccountTreeView").data("kendoTreeView");
                    treeview.dataSource.read();

                    accountStatusChanged = false;
                }
                //$("#aconfig").attr('src', '/Images/Nav_AcctExpl_Bolt.png');
                //$("#aconfig").removeClass("accountconfigactive");
                //$('#divAcctSetting').removeClass("setting-iconsel");
            } else {
                //$("#aconfig").attr('src', '/Images/Nav_AcctExpl_BoltAct.png');
                //$("#aconfig").addClass("accountconfigactive");
                //$('#divAcctSetting').addClass("setting-iconsel");
            }
        });
    });
    $("#assetstatus").hide();
    $("#assetCollapse").hide();

    // Asset treeview expand all event
    $("#assetExpand").click(function() {
        $("#assetCollapse").show();
        $("#assetExpand").hide();
        var treeview = $("#AssetTreeView").data("kendoTreeView");
        expandCollapseAllAssetNodes = true;
        treeview.expand(".k-item");
        expandCollapseAllAssetNodes = false;
    });

    // Asset treeview collapse all event
    $("#assetCollapse").click(function() {
        $("#assetCollapse").hide();
        $("#assetExpand").show();
        var treeview = $("#AssetTreeView").data("kendoTreeView");
        expandCollapseAllAssetNodes = true;
        treeview.collapse(".k-item");
        expandCollapseAllAssetNodes = false;
    });
    
    // Asset explorer settings icon click event
    $('#aplus').click(function() {
        $('#aplus').addClass("assetplusactive");
        $('#divAssetSetting').addClass("setting-plusiconsel");
        $('#assetstatus').slideToggle('500', function() {
            if ($('#assetstatus').is(':hidden')) {
                $("#aplus").removeClass("assetplusactive");
                $('#divAssetSetting').removeClass("setting-plusiconsel");
            } else {
                $("#aplus").addClass("assetplusactive");
                $('#divAssetSetting').addClass("setting-plusiconsel");
            }
        });
    }); // for treeview lines
    $(".k-treeview > .k-group").addClass("k-treeview-lines");
})(jQuery);

var accountStatusChanged = false;

function AddApplicationToTree(app) {
    if (app.Name !== '' && app.ApplicationID !== '') {
        var add = false, isClassified = false, addClassValue = false, classificationValue = '', classificationId = null, classNode = null;
        var data = $('#AccountTreeView').data("kendoTreeView").dataSource.data();

        var nodes = data;

        if (data[0].ClassificationID !== '' && data[0].ClassificationID !== undefined && data[0].ClassificationID !== null) {
            isClassified = true;
            for (prop in app) {
                if (prop.indexOf('Classification_ID') > -1) {
                    if (app[prop] == data[0].ClassificationID) {
                        add = true;
                        classificationValue = app[prop.replace('Classification_ID', 'Classification_Value')];
                        classificationId = app[prop];

                        addClassValue = true;

                        $.each(data, function (idx, val) {
                            if (val.DisplayValue === classificationValue) {
                                addClassValue = false;
                                return false;
                            }
                        });

                        if (addClassValue) {
                            classNode = {
                                ApplicationID: null,
                                CatalogID: null,
                                ClassificationID: classificationId,
                                DisplayValue: classificationValue,
                                Expanded: true,
                                NodeID: app[prop.replace('Classification_ID', 'Classification_Value_ID')],
                                NodeType: $.inArray('Classification_Value', nodeTypes),
                                Selected: false,
                                Status: null,

                                ChildNodes: []
                            };
                        }
                        break;
                    }
                }
            }
        } else {
            add = true;
        }

        var beforeOrAfter, selector, node;

        if (add) {
            var applicationNode = {
                ApplicationID: app.ApplicationID,
                CatalogID: null,
                ClassificationID: classificationId,
                DisplayValue: app.Name,
                Expanded: false,
                NodeID: app.ApplicationID,
                NodeType: $.inArray('Application', nodeTypes),
                Selected: false,
                Status: $.inArray('Inactive', nodeStatuses),

                ChildNodes: []
            };

            if (addClassValue) {
                node = classNode;
            } else {
                node = applicationNode;
                if (isClassified) {
                    $.each(nodes, function (idx, val) {
                        if (val.DisplayValue === classificationValue) {
                            nodes = val.ChildNodes;
                            return false;
                        }
                    });
                }
            }

            $.each(nodes, function (idx, val) {
                if (idx === nodes.length - 1) {
                    selector = $('[data-uid=' + val.uid + ']');
                    if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                        beforeOrAfter = 'before';
                    } else {
                        beforeOrAfter = 'after';
                    }
                    return false;
                } else {
                    if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                        beforeOrAfter = 'before';
                        selector = $('[data-uid=' + val.uid + ']');
                        return false;
                    }
                }
            });

            var id = AddToAccountTree(node, selector, beforeOrAfter);

            var highlightSelector = '#AccountTreeView [data-uid=' + id + '] > div > span.k-in';

            if (addClassValue) {
                var aid = AddToAccountTree(applicationNode, $('[data-uid=' + id + ']'), 'append');
                highlightSelector += ',#AccountTreeView [data-uid=' + aid + '] > div > span.k-in';
            }

            $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
            window.setTimeout(function () {
                $(highlightSelector).animate({
                    backgroundColor: 'transparent'
                }, 1500);
            }, 3000);
        }
    } else {
        alert('You must enter an Application Name in order to add!');
    }
}

function AddPackageToTree(pack) {
    var data = $('#AccountTreeView').data("kendoTreeView").dataSource.data();
    var beforeOrAfter, selector;
    var parentNode = findNodeByTypeAndID(data, $.inArray('Application', nodeTypes), pack.ApplicationID);

    var nodes = parentNode.ChildNodes;
    
    if(pack.PackageType === 'WebPage') {
        $.each(nodes, function (idx, val) {
            if(val.DisplayValue === 'Web Pages') {
                nodes = val.ChildNodes;
                return false;
            }
        });
    } else if (pack.PackageType === 'Email') {
        $.each(nodes, function (idx, val) {
            if (val.DisplayValue === 'Emails') {
                nodes = val.ChildNodes;
                return false;
            }
        });
    }

    var node = {
        ApplicationID: parentNode.ApplicationID,
        CatalogID: null,
        ClassificationID: parentNode.ClassificationID,
        DisplayValue: pack.Name,
        Expanded: false,
        NodeID: pack.PackageID,
        NodeType: $.inArray('Package', nodeTypes),
        Selected: true,
        Status: $.inArray('Inactive', nodeStatuses),

        ChildNodes: []
    };

    if (nodes.length > 0) {
        $.each(nodes, function (idx, val) {
            if (idx === nodes.length - 1) {
                selector = $('[data-uid=' + val.uid + ']');
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                } else {
                    beforeOrAfter = 'after';
                }
                return false;
            } else {
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                    selector = $('[data-uid=' + val.uid + ']');
                    return false;
                }
            }
        });
    } else {
        selector = $('[data-uid=' + parentNode.uid + ']');
        beforeOrAfter = 'append';
    }

    var uid = AddToAccountTree(node, selector, beforeOrAfter);

    var highlightSelector = '#AccountTreeView [data-uid=' + uid + '] > div > span.k-in';

    $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
    window.setTimeout(function () {
        $(highlightSelector).animate({
            backgroundColor: 'transparent'
        }, 1500);
    }, 3000);
}

function AddSchemeToTree(scheme) {
    var data = $('#AccountTreeView').data("kendoTreeView").dataSource.data();
    var beforeOrAfter, selector;
    var parentNode = findNodeByTypeAndID(data, $.inArray('Package', nodeTypes), scheme.PackageID);

    var nodes = parentNode.ChildNodes;

    var node = {
        ApplicationID: parentNode.ApplicationID,
        CatalogID: null,
        ClassificationID: parentNode.ClassificationID,
        DisplayValue: scheme.Name,
        Expanded: false,
        NodeID: scheme.PackageID,
        NodeType: $.inArray('Scheme', nodeTypes),
        Selected: true,
        Status: $.inArray('Inactive', nodeStatuses),

        ChildNodes: []
    };

    if (nodes.length > 0) {
        $.each(nodes, function (idx, val) {
            if (idx === nodes.length - 1) {
                selector = $('[data-uid=' + val.uid + ']');
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                } else {
                    beforeOrAfter = 'after';
                }
                return false;
            } else {
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                    selector = $('[data-uid=' + val.uid + ']');
                    return false;
                }
            }
        });
    } else {
        selector = $('[data-uid=' + parentNode.uid + ']');
        beforeOrAfter = 'append';
    }

    var uid = AddToAccountTree(node, selector, beforeOrAfter);

    var highlightSelector = '#AccountTreeView [data-uid=' + uid + '] > div > span.k-in';

    $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
    window.setTimeout(function () {
        $(highlightSelector).animate({
            backgroundColor: 'transparent'
        }, 1500);
    }, 3000);
}

function AddExperienceToTree(exp) {
    var data = $('#AccountTreeView').data("kendoTreeView").dataSource.data();
    var beforeOrAfter, selector;
    var parentNode = findNodeByTypeAndID(data, $.inArray('Scheme', nodeTypes), exp.SchemeID);

    var nodes = parentNode.ChildNodes;

    var node = {
        ApplicationID: parentNode.ApplicationID,
        CatalogID: null,
        ClassificationID: parentNode.ClassificationID,
        DisplayValue: exp.Name,
        Expanded: false,
        NodeID: exp.ExperienceID,
        NodeType: $.inArray('Experience', nodeTypes),
        Selected: true,
        Status: $.inArray('Inactive', nodeStatuses),

        ChildNodes: []
    };

    if (nodes.length > 0) {
        $.each(nodes, function (idx, val) {
            if (idx === nodes.length - 1) {
                selector = $('[data-uid=' + val.uid + ']');
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                } else {
                    beforeOrAfter = 'after';
                }
                return false;
            } else {
                if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                    beforeOrAfter = 'before';
                    selector = $('[data-uid=' + val.uid + ']');
                    return false;
                }
            }
        });
    } else {
        selector = $('[data-uid=' + parentNode.uid + ']');
        beforeOrAfter = 'append';
    }

    var uid = AddToAccountTree(node, selector, beforeOrAfter);

    var highlightSelector = '#AccountTreeView [data-uid=' + uid + '] > div > span.k-in';

    $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
    window.setTimeout(function () {
        $(highlightSelector).animate({
            backgroundColor: 'transparent'
        }, 1500);
    }, 3000);
}

function AddVariantToTree(variant) {
    var data = $('#AccountTreeView').data("kendoTreeView").dataSource.data();
    var beforeOrAfter, selector;
    var parentNode = findNodeByTypeAndID(data, $.inArray('Experience', nodeTypes), variant.ExperienceID);

    var nodes = parentNode.ChildNodes;

    var node = {
        ApplicationID: parentNode.ApplicationID,
        CatalogID: null,
        ClassificationID: parentNode.ClassificationID,
        DisplayValue: variant.Name,
        Expanded: false,
        NodeID: variant.VariantID,
        NodeType: $.inArray('Variant', nodeTypes),
        Selected: true,
        Status: $.inArray('Inactive', nodeStatuses),

        ChildNodes: []
    };

    $.each(nodes, function (idx, val) {
        if (idx === nodes.length - 1) {
            selector = $('[data-uid=' + val.uid + ']');
            if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                beforeOrAfter = 'before';
            } else {
                beforeOrAfter = 'after';
            }
            return false;
        } else {
            if (node.DisplayValue.localeCompare(val.DisplayValue) < 1) {
                beforeOrAfter = 'before';
                selector = $('[data-uid=' + val.uid + ']');
                return false;
            }
        }
    });

    var uid = AddToAccountTree(node, selector, beforeOrAfter);

    var highlightSelector = '#AccountTreeView [data-uid=' + uid + '] > div > span.k-in';

    $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
    window.setTimeout(function () {
        $(highlightSelector).animate({
            backgroundColor: 'transparent'
        }, 1500);
    }, 3000);
}

function AddToAccountTree(node, selector, beforeAfterAppend) {
    var tree = $('#AccountTreeView').data("kendoTreeView");
    if (beforeAfterAppend === 'before') {
        tree.insertBefore(node, selector);
    } else if (beforeAfterAppend === 'after') {
        tree.insertAfter(node, selector);
    } else if (beforeAfterAppend === 'append') {
        tree.append(node, selector);
    }
    var nodeData = findNodeByTypeAndID(tree.dataSource.data(), node.NodeType, node.NodeID);
    if (nodeData !== null && nodeData !== undefined) {
        return nodeData.uid;
    } else {
        return '';
    }
}

function findNodeByTypeAndID(nodes, nodeType, nodeId) {
    var node = null;
    $.each(nodes, function(idx, val) {
        if (val.NodeType == nodeType && val.NodeID == nodeId) {
            node = val;
            return false;
        } else {
            node = findNodeByTypeAndID(val.ChildNodes, nodeType, nodeId);
            if (node !== null && node !== undefined) {
                return false;
            }
        }
    });
    return node;
}

// Panelbar select event
// Will be fired when panelbar header item is selected.
// Changing the selected header item style. 

function select(e) {
    var panelItem = $(e.item).find("> .k-header").text().replace(/\s/g, "");
    if (panelItem.indexOf('Account') > -1) {
        panelItem = 'Account';
    }
    var pElement = $("#PanelBar");

    if (panelItem != "") { // Panel bar header item
        pElement.find('.k-header .Reportsselected').addClass('Reports');
        pElement.find('.k-header .AccountExplorerselected').addClass('AccountExplorer');
        pElement.find('.k-header .AssetExplorerselected').addClass('AssetExplorer');
        pElement.find('.k-header .Toolsselected').addClass('Tools');

        pElement.find('.k-header').removeClass('k-state-selected');

        pElement.find('.Reportsselected').removeClass('Reportsselected');
        pElement.find('.AccountExplorerselected').removeClass('AccountExplorerselected');
        pElement.find('.AssetExplorerselected').removeClass('AssetExplorerselected');
        pElement.find('.Toolsselected').removeClass('Toolsselected');
        pElement.find('.panelbaritemselected').removeClass('panelbaritemselected');

        if ($(e.item).is(".k-state-active")) { // For toggle evect
            //pElement.find(".k-header ." + panelItem).addClass(panelItem + "selected");
            var that = this;
            window.setTimeout(function() {
                that.collapse(e.item);
            }, 1);
        } else {
            pElement.find(".k-header ." + panelItem).addClass(panelItem + "selected");
            $(e.item).find("> .k-header").addClass("panelbaritemselected");

            SetPanelBarStates(e); //window.setTimeout("SetPanelBarHeight()", 300);
        }
    }
}

// Called when pannelbar is expanded
// Changes the item style(to font bold)

function panelBarExpand(e) {
    var panelItem = $(e.item).find("> .k-header").text().replace(/\s/g, "");
    if (panelItem.toLowerCase() == "tips") {
        $(e.item).find("> .k-header").addClass("panelbaritemselected");
        $(e.item).find("> .k-header").css('color', '#cf102d');
        $(e.item).find("> .k-header").css('background-color', '#CECECE');
        $(e.item).find("> .k-header").removeClass("k-state-selected");
        SetPanelBarHeight(true, false);
    } else {
        $(e.item).find("> .k-header").addClass("panelbaritemselected");
        $(e.item).find("> .k-header").css('background-color', '#CECECE');
        
        SetPanelBarHeight(false, false);
    }
    var oldClass = '';
    if (panelItem.indexOf('Account') > -1) {
        oldClass = 'AccountExplorer';
    } else if (panelItem.indexOf('Report') > -1) {
        oldClass = 'Reports';
    } else if (panelItem.indexOf('Asset') > -1) {
        oldClass = 'AssetExplorer';
    }
    $(e.item).find("> .k-header > .k-sprite").removeClass(oldClass);
    $(e.item).find("> .k-header > .k-sprite").addClass(oldClass + 'selected');
    ResizeGreyedOutBackground('expanded');
}

// Called when pannelbar is collapsed

function panelBarCollapse(e) {
    var panelItem = $(e.item).find("> .k-header").text().replace(/\s/g, "");
    if (panelItem.toLowerCase() == "tips") {
    } else {
        $(e.item).find("> .k-header").css('background-color', '#e6e6e6');
    }
}

// Set panelbar states
// Calls jquery post method to set selected panel bar

function SetPanelBarStates(e) {
    var panelBarId = $(e.item).data('panelBarId');
    unbindAjaxSpinner();
    $.post('/Navigation/SetPanelBarStates', { "panelBarId": panelBarId }, function(data) { });
    bindAjaxSpinner();
}

// Account status change event
// Calls jquery post method to set the status and updates the navigational panel

function accountStatusChange(e) {
    $.post('/Home/SetAccountExplorerStatus', { "strStatusValue": e.id }, function(data) {
        //$("#divAccountTreeView").html(data);
    })
        .error(function() {
            alert("Error in SetAccountExplorerStatus, Please try later");
        });
    accountStatusChanged = true;
}

// Account tree view expand event
// Calls jquery post method to set expanded node

function AccountTreeViewExpand(e) {
    var treeview = $('#AccountTreeView').data('kendoTreeView');
    var data = treeview.dataItem(e.node);
    var nodeId = data.NodeID;
    var nodeType = data.NodeType;
    var classificationId = data.ClassificationID;
    if (data != undefined && data.id != undefined) {
        if (data.parentNode() === undefined) {
            // Expand disabled grouping nodes
            // Web Pages, Emails, and Remarketing Campaigns

            treeview.expand('#Web_Pages,#Emails,#Remarketing_Campaigns');
        }
        if (!expandCollapseAllAccountNodes) {
            unbindAjaxSpinner();  // unbind ajax spinner
            $.post('/Navigation/SetAccountExplorerNodeState', { "nodeID": nodeId, "nodeType": nodeType, "classificationID": classificationId, "nodeName": data.DisplayValue, "expand": true }, function (data) { });
            bindAjaxSpinner();  // Rebind ajax spinner
        }
    }
}

// Account tree view collapse event
// Calls jquery post method to set collapsed node

function AccountTreeViewCollapse(e) {
    var treeview = $('#AccountTreeView').data('kendoTreeView');
    var data = treeview.dataItem(e.node);
    var nodeId = data.NodeID;
    var nodeType = data.NodeType;
    var classificationId = data.ClassificationID;

    // Bug #6 - don't collapse these nodes since they are disabled and cannot be expanded by user
    switch (nodeId) {
        case "Web_Pages":
        case "Emails":
        case "Remarketing_Campaigns":
            e.preventDefault();
            break;
        default:
            break;
    }
    
    if (!expandCollapseAllAccountNodes) {
        unbindAjaxSpinner();
        $.post('/Navigation/SetAccountExplorerNodeState', { "nodeID": nodeId, "nodeType": nodeType, "classificationID": classificationId, "nodeName": data.DisplayValue, "expand": false }, function(data) { });
        bindAjaxSpinner();
    }
}

function ReportTreeDataBound(e) {
    var tree = $('#ReportTree').data('kendoTreeView');
    for (var i = 0; i < tree.dataSource._data.length; i++) {
        var node = tree.dataSource._data[i];
        if(node.Selected) {
            var selector = '[data-uid="' + node.uid + '"]';
            tree.select(selector);
            e.node = node;
            SetContent(e);
        }
    }
}

function reportTreeRequestStart(e) {
    $('#ReportTree').css('display', 'none');
}

function reportTreeRequestEnd(e) {
    $('#ReportTree').css('display', 'inline');
}

function AccountTreeDataBound(e) {
    var tree = $('#AccountTreeView').data('kendoTreeView');
    if (e.node != undefined) {
        var node = tree.dataItem(e.node);
        var selector = '[data-uid="' + node.uid + '"]';
        expandCollapseAllAccountNodes = true;

        if (node.DisplayValue == 'Web Pages' || node.DisplayValue == 'Emails') {
            $(selector).attr('id', node.DisplayValue.replace(' ', '_'));
            tree.enable(selector, false);
            tree.expand(selector);
        }
        if (node.Expanded) {
            tree.expand(selector);
        }
        if (node.Selected) {
            tree.select(selector);
            SetContent(e);            
        }
        
        expandCollapseAllAccountNodes = false;

        // wire up hotspot
        var infoPanelText = '';
        switch (node.NodeType) {
            case 3: //Application
                infoPanelText = $('#hidApplicationNodeInfo').val();
                break;
            case 4: //Location
                infoPanelText = $('#hidLocationNodeInfo').val();
                break;
            case 5: //Container
                infoPanelText = $('#hidContainerNodeInfo').val();
                break;
            case 6: //Strategy
                infoPanelText = $('#hidStrategyNodeInfo').val();
                break;
            case 16: //Experience
                infoPanelText = $('#hidExperienceNodeInfo').val();
                break;
        }
        if (infoPanelText != '') {
            $(selector + "> div > span[class=k-in]").addClass('hotspot').attr('data-hotspot', infoPanelText);
        }
    } else {
        if (highlightTreeItem) {
            // get the uid
            var nodeData = tree.dataSource.data();
            var foundNode = findNodeByTypeAndID(nodeData, $.inArray(highlightTreeItem.nodeType, nodeTypes), highlightTreeItem.nodeId);
            highlightTreeItem = undefined; // reset
            if (foundNode) {
                // expand parent - if applicable
                var parentNode = foundNode.parentNode();
                if (parentNode) {
                    tree.expand('[data-uid="' + parentNode.uid + '"]');
                }
                var highlightSelector = '#AccountTreeView [data-uid=' + foundNode.uid + '] > div > span.k-in';
                $(highlightSelector).css('background-color', 'rgb(204, 255, 204)');
                window.setTimeout(function () {
                    $(highlightSelector).animate({
                        backgroundColor: 'transparent'
                    }, 1500, function () { $(highlightSelector).removeAttr('style'); });
                }, 3000);
            }
        }
    }
    
}

function accountTreeRequestStart(e) {
    $('#AccountTreeView').css('display', 'none');
}

function accountTreeRequestEnd(e) {
    $('#AccountTreeView').css('display', 'inline');
}

function AssetTreeDataBound(e) {
    var tree = $('#AssetTreeView').data('kendoTreeView');
    if (e.node != undefined) {
        var node = tree.dataItem(e.node);
        var selector = '[data-uid="' + node.uid + '"]';
        expandCollapseAllAccountNodes = true;
        if (node.Expanded) {
            tree.expand(selector);
        }
        if (node.Selected) {
            tree.select(selector);
            SetContent(e);
        }
        
        expandCollapseAllAccountNodes = false;
    } else {
        for (var i = 0; i < tree.dataSource._data.length; i++) {
            var node = tree.dataSource._data[i];
            if (node.Expanded) {
                tree.expand(selector);
            }
            if (node.Selected) {
                var selector = '[data-uid="' + node.uid + '"]';
                tree.select(selector);
                e.node = node;
                SetContent(e);
            }
        }
    }
}

function assetTreeRequestStart(e) {
    $('#AssetTreeView').css('display', 'none');
}

function assetTreeRequestEnd(e) {
    $('#AssetTreeView').css('display', 'inline');
}

function ToolTreeDataBound(e) {
    var tree = $('#ToolTreeView').data('kendoTreeView');
    for (var i = 0; i < tree.dataSource._data.length; i++) {
        var node = tree.dataSource._data[i];
        if (node.Selected) {
            var selector = '[data-uid="' + node.uid + '"]';
            tree.select(selector);
            e.node = node;
            SetContent(e);
        }
    }
}

function toolTreeRequestStart(e) {
    $('#ToolTreeView').css('display', 'none');
}

function toolTreeRequestEnd(e) {
    $('#ToolTreeView').css('display', 'inline');
}

// Asset tree view expand event
// Calls jquery post method to set expanded node

function AssetTreeViewExpand(e) {
    var data = $('#AssetTreeView').data('kendoTreeView').dataItem(e.node);
    var nodeId = data.NodeID;
    var nodeType = data.NodeType;
    if (!expandCollapseAllAssetNodes) {
        $.post('/Navigation/SetAssetExplorerNodeState', { "nodeID": nodeId, "nodeType": nodeType, "nodeName": data.DisplayValue, "expand": true }, function (data) {
        });
    }
}

// Asset tree view collapse event
// Calls jquery post method to set collapsed node

function AssetTreeViewCollapse(e) {
    var data = $('#AssetTreeView').data('kendoTreeView').dataItem(e.node);
    var nodeId = data.NodeID;
    var nodeType = data.NodeType;
    if (!expandCollapseAllAssetNodes) {
        $.post('/Navigation/SetAssetExplorerNodeState', { "nodeID": nodeId, "nodeType": nodeType, "nodeName": data.DisplayValue, "expand": false }, function (data) {
        });
    }
}

// Help panelbar expand
// When expanded, the Help Panel vertically overlays all the other controls in the Navigation Panel

function HelpExpand(e) {
    $(e.item).find("> .k-header").css('background-color', '#FDEDAC');
    $(e.item).find("> .k-header").css('font-weight', 'bold');
    $(e.item).find("> .k-header").removeClass("k-state-selected");
    $('#mainPanel').hide();
    $('#infoPanel').hide();
}

// Help panelbar collapse
// When collpsed minimize help panel

function HelpCollapse(e) {
    $(e.item).find("> .k-header").css('background-color', '#E6E6E6');
    $(e.item).find("> .k-header").css('font-weight', '');
    $(e.item).find("> .k-header").removeClass("k-state-selected");
    $('#mainPanel').show();
    $('#infoPanel').show();
}

// Set panelbar height
// Sets the panelbar height based on currently selected panelbar(main, info or help)

function SetPanelBarHeight(fromInfoExpand, fromInfoCollapse) {
    var mpwHeight = $('#mainPanelWrapper').height();
    var infoHeight = 0;

    if (fromInfoExpand || fromInfoCollapse) {
        if (fromInfoExpand) {
            infoHeight = $('#divInfo').height();
        }
    } else {
        var infoPanelbar = $("#InfoPanelBar-1").css('display') !== 'none';
        if (infoPanelbar) {
            infoHeight = $('#divInfo').height();
        }
    }

    var eaHeight = (mpwHeight - 200 - infoHeight) + 'px'; // 220 = 66 (for logo at bottom) + (29 * 4 (number of panel bar items)) + 18 (Expand All Line)
    var sHeight = (mpwHeight - 182 - infoHeight) + 'px'; // 182 = 66 (for logo at bottom) + (29 * 4 (number of panel bar items))

    $('#divAccountTreeView,#divAssetTreeView').css('height', eaHeight);
    $('#divReportTreeView').css('height', sHeight);
    
    // Getting the height from the header so the greyed out div is consistent if adjusted by a larger image - DRY principal
    $("#logopanelgrey").css('height', $("#logopanel").height() + $(".border").height() + 1);
}

function MainPanelOverlay() {
    ResizeGreyedOutBackground('');
    
    if ($("#logopanelgrey").css('display') !== 'none') {
        $("#mainPanelGrey").css('display', 'block');
    } else {
        $("#mainPanelGrey").css('display', 'none');
    }
}

//Shows hot spot content, this needs to be called from content panel

function ShowInfo(content) {
    if ($('#InfoPanelBar-1').css('display') == "block") {
        var panelbar = $("#InfoPanelBar").data("kendoPanelBar");
        if (panelbar != null)
            panelbar.expand(".k-item");
        $("#divInfo").html(content);
    }
}

// Hides info panelbar

function HideInfo() {
    if ($('#InfoPanelBar-1').css('display') == "block") {
        $("#divInfo").html('');
    }
}

// Called when info panel bar is collapsed

function InfoCollapse(e) {
    $(e.item).find("> .k-header").removeClass("panelbaritemselected");
    $(e.item).find("> .k-header").removeClass("k-state-selected");
    $(e.item).find("> .k-header").css('color', '#000');
    $(e.item).find("> .k-header").css('background-color', '#E6E6E6');

    SetPanelBarHeight(false, true);
    ResizeGreyedOutBackground('collapsed');
}

function ResizeGreyedOutBackground(action) {
    var spaceUsed = 96;
    var mainPanelWrapper = $("#mainPanelWrapper").height();
    
    if (action === "collapsed") {
        spaceUsed = 96;
    }
    else if (action === "expanded") {
        spaceUsed = 229;
    } else {
        if ($("#divInfo").is(":visible")) {
            spaceUsed = 224;
        } else {
            spaceUsed = 96;
        }
    }
    
    $("#mainPanelGrey").height(mainPanelWrapper - spaceUsed);
}

// 01/16/2013 - Added to fix Bug #24

function SetScrollPosition(divID) {
    if ($.browser.msie) {
        // ie
        var position = $(divID).scrollTop();
        window.setTimeout(function() {
            $(divID).scrollTop(position);
        }, 1);
    }
}

$(function() {
    SetPanelBarHeight(false, false);
});

// Bug#7 - flags to prevent calls to SetAccountExplorerNodeState/SetAssetExplorerNodeState when Expand/Collapse All is selected
var expandCollapseAllAccountNodes = false;
var expandCollapseAllAssetNodes = false;
