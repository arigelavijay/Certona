var chkAllState = false;
var chkStrategy = [];
var rebindGrid = false;

function initializeExperienceStrategiesGrid() {
    if ($("#rulesShow").data("mode") == "show") {
        toggleRules();
    }
    if ($("#decisionPlanShow").data("mode") == "show") {
        toggleDecisionPlan();
    }

    // Manage grid - Select All checkbox
    $('#chkSelectAll').change(function () {
        if ($(this).is(":checked")) {
            $.each($(':checkbox'), function () {
                if ($(this).data("role") == 'selectStrategy') {
                    $(this).attr('checked', 'checked');
                }
            });
        } else {
            $.each($(':checkbox'), function () {
                if ($(this).data("role") == 'selectStrategy') {
                    $(this).removeAttr('checked');
                }
            });
        }
    });
}

function onStrategyGridDetailExpand(e) {
    // get the strategy id from the master row
    // and load the content for the decision plan (if not already loaded)                 
    var strategyID = e.masterRow[0].cells[1].innerHTML;

    var rulesData = $('#rules-' + strategyID).html();
    $('#variant_rules_strategy_' + strategyID).html(rulesData);

    var decisionData = $('#decision-plan-' + strategyID).html();
    $('#decision_plan_' + strategyID).html(decisionData);
}

function onStrategyGridDataBound(e) {
    this.expandRow(this.tbody.find("tr.k-master-row"));
    bindInfoPanelEvents();  // rebind hot-spot events        

    var rules = $("#rulesShow");
    if (rules.data("mode") == "hide") {
        toggleAllRules("none");
    } else {
        toggleAllRules("block");
    }

    var plan = $("#decisionPlanShow");
    if (plan.data("mode") == "hide") {
        toggleAllDecisionPlans("none");
    } else {
        toggleAllDecisionPlans("block");
    }

    // Manage grid - footer label
    var total = e.sender.dataSource._total;
    $('#paging-label').html("Displaying items 1 - " + total + " of " + total);

    // disables master/detail expand/collapse
    this.element.find('tr.k-master-row').each(function () {
        var row = $(this);
        row.find('.k-hierarchy-cell a').css({ opacity: 0.3, cursor: 'default' }).click(function (e) {
            e.stopImmediatePropagation();
            return false;
        });
    });

    if (rebindGrid) {
        rebindGrid = false;
        resetCheckBoxes(chkAllState, chkStrategy);
    }
    
    var createdVariantId = $('#CreatedVariantId').val();
    if (createdVariantId !== '' && createdVariantId !== 0) {
        //alert('TODO: execute function to highlight treeview and grid for ExperienceId: ' + createdVariantId);
        HighlightGridRow('ExperienceStrategies', 'StrategyID', Number(createdVariantId), true);
    }
}

function toggleRules() {
    var rules = $("#rulesShow");
    var mode = false;
    if (rules.data("mode") == "hide") {
        // show
        rules[0].innerHTML = "Hide Rules";
        rules.data("mode", "show");
        toggleAllRules("block");
        mode = true;
    } else {
        // hide
        rules[0].innerHTML = "Show Rules";
        rules.data("mode", "hide");
        toggleAllRules("none");
    }
    // set
    $.post(setRulesDisplayStateUrl, { showRules: mode });
}

function toggleDecisionPlan() {
    var rules = $("#decisionPlanShow");
    var mode = false;
    if (rules.data("mode") == "hide") {
        rules[0].innerHTML = "Hide Decision Plan";
        rules.data("mode", "show");
        toggleAllDecisionPlans("block");
        mode = true;
    } else {
        rules[0].innerHTML = "Show Decision Plan";
        rules.data("mode", "hide");
        toggleAllDecisionPlans("none");
    }
    // set
    $.post(setDecisionPlanDisplayStateUrl, { showDecisionPlan: mode });
}

function toggleAllRules(mode) {
    $.each($("div[data-container='variant_rules_strategy']"), function () {
        $(this).css("display", mode);
    });
}

function toggleAllDecisionPlans(mode) {
    $.each($("div[data-container='decision_plan']"), function () {
        $(this).css("display", mode);
    });
}

function statusFilter(element) {
    element.kendoDropDownList({
        dataSource: {
            transport: {
                read: statusListURL
            }
        },
        optionLabel: "--Select Value--"
    });
}



// Manage Strategies Grid Functions

function onStrategyActionDatabound(e) {
    $("#updateStrategy").attr("disabled", "disabled");
}

function onStrategyActionChange(e) {
    var selected = e.sender._selectedValue;
    if (selected === "Select an Action") {
        $("#updateStrategy").attr("disabled", "disabled");
    } else {
        $("#updateStrategy").removeAttr("disabled");
    }
}

function onStrategyGridChange(arg) {
    if (event.srcElement.type === "checkbox")
        return;
    
    $.each(this.select(), function () {        
        var id = this.cells[1].innerHTML;
        var checkbox = $('#ExperienceStrategies :checkbox[data-id="' + id + '"]');
        if (checkbox.is(':checked')) {
            checkbox.removeAttr('checked');
        } else {
            checkbox.attr('checked', 'checked');
        }
    });
}

function updateSelectedStrategies() {
    var selected = $('#ExperienceStrategies :checked[data-role="selectStrategy"]');
    if (selected.length === 0)
        return;
    else {
        
        var selectedIDs = '';
        var counter = 0;

        chkAllState = $('#chkSelectAll').is(':checked');
        chkStrategy = [];
        
        var experienceID = $('#experienceID').val();
        $.each(selected, function () {
            counter++;
            chkStrategy.push($(this).attr('data-id'));
            selectedIDs += $(this).attr('data-id') + (counter === selected.length ? '' : ',');
        });
        var statusAction = $("#statusAction").val();

        $.post(variantActionChangeURL, { variantList: selectedIDs, actionStatus: statusAction, experienceID: experienceID }, function (data) {
            ShowsysMessage('success', 'Strategy updates successful');
            
            //rebindStrategiesGrid(data.strategies);
            var grid = $("#ExperienceStrategies").data("kendoGrid");
            grid.dataSource.read();
            
            // find way to accomplish outside
            rebindGrid = true;
            //resetCheckBoxes(chkAllState, chkStrategy);
        });
    }
}

function rebindStrategiesGrid(strategies) {
    var filter = $("#ExperienceStrategies").data("kendoGrid").dataSource._filter;
    var ds = {
        data: strategies,
        sort: { field: "Priority", dir: "asc" },
        schema: {
            model: {
                id: "StrategyID",
            }
        }
    };
    // reapply filter
    if (filter) {
        ds.filter = {};
        ds.filter.field = filter.filters[0].field;
        ds.filter.operator = filter.filters[0].operator;
        ds.filter.value = filter.filters[0].value;
    }    
    var dataSource = new kendo.data.DataSource(ds);
    $("#ExperienceStrategies").data("kendoGrid").setDataSource(dataSource);
}

function resetCheckBoxes(checkBoxAll, checkBoxStrategy) {
    if (checkBoxAll) {
        $("#chkSelectAll").attr('checked', 'checked');
    }
    for (var i = 0; i <= checkBoxStrategy.length - 1; i++) {
        var checkbox = $('#ExperienceStrategies :checkbox[data-id="' + checkBoxStrategy[i] + '"]');
        if (checkbox) {
            checkbox.attr('checked', 'checked');
        }
    }
}

