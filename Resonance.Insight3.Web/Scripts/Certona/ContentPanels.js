function ContentPanels() {
    var settings = {
        contentPanels: [],
        currentType: ''
    };

    this.Add = function(cp) {
        settings.contentPanels.push(cp);
    };

    this.CurrentType = function(type) {
        if (type !== undefined && typeof type === 'string') {
            settings.currentType = type;
        } else {
            return settings.currentType;
        }
    };

    this.HasContentPanel = function(nodeType) {
        var ret = false;
        $.each(settings.contentPanels, function (idx, val) {
            if (val.NodeType() === nodeType) {
                ret = true;
                return false;
            }
        });
        return ret;
    };

    this.GetContentPanel = function (nodeType) {
        var ret = null;
        $.each(settings.contentPanels, function (idx, val) {
            if (val.NodeType() === nodeType) {
                ret = val;
                return false;
            }
        });
        return ret;
    };

    this.GetCurrentContentPanel = function() {
        var ret = null;
        $.each(settings.contentPanels, function (idx, val) {
            if (val.NodeType() === settings.currentType) {
                ret = val;
                return false;
            }
        });
        return ret;
    };
}

function ContentPanel(nodeType) {
    var settings = {
        nodeType: nodeType,
        sections: []
    };

    this.NodeType = function() {
        return settings.nodeType;
    };

    this.Sections = function() {
        return settings.sections;
    };

    this.HasContentPanelSection = function(name) {
        var cpsExists;
        $.each(settings.sections, function (idx, val) {
            if (val.Name() === name) {
                cpsExists = true;
                return false;
            }
        });
        return cpsExists;
    };

    this.GetContentPanelSection = function(name) {
        var cps;
        $.each(settings.sections, function (idx, val) {
            if (val.Name() === name) {
                cps = val;
                return false;
            }
        });
        return cps;
    };
    
    this.GetContentPanelSectionById = function (id) {
        var cps;
        $.each(settings.sections, function (idx, val) {
            if (val.Id() === id) {
                cps = val;
                return false;
            }
        });
        return cps;
    };
}

function ContentPanelSection(options) {
    var settings = {
        name: '',
        id: '',
        isEditing: false,
        nodeId: '',
        catalogId: '',
        nodeType: '',
        viewUrl: '',
        manageUrl: '',
        viewTitle: '',
        manageTitle: ''
    };

    var isDirtyOverride = undefined;
    var dirtyContainer = undefined;
    
    $.extend(settings, options);

    this.SetSettings = function(options) {
        $.extend(settings, options);
    };

    this.GetSettings = function () {
        return settings;
    };

    this.SetIsDirtyOverride = function(f) {
        isDirtyOverride = f;
    };

    this.SetDirtyContainer = function (containerId) {
        dirtyContainer = containerId;
    };

    this.Name = function(name) {
        if (name !== undefined && typeof name === 'string') {
            settings.name = name;
        } else {
            return settings.name;
        }
    };

    this.Id = function (id) {
        if (id !== undefined && typeof id === 'string') {
            settings.id = id;
        } else {
            return settings.id;
        }
    };
    
    this.IsEditing = function(isEditing) {
        if (isEditing !== undefined && typeof isEditing === 'boolean') {
            if (settings.isEditing === true && isEditing === false) {
                // reset when changing mode
                isDirtyOverride = undefined;
                dirtyContainer = undefined;
            }
            settings.isEditing = isEditing;
        } else {
            return settings.isEditing;
        }
    };

    this.NodeID = function(nodeId) {
        if (nodeId !== undefined && typeof nodeId === 'string') {
            settings.nodeId = nodeId;
        } else {
            return settings.nodeId;
        }
    };

    this.CatalogID = function(catalogId) {
        if (catalogId !== undefined && typeof catalogId === 'string') {
            settings.catalogId = catalogId;
        } else {
            return settings.catalogId;
        }
    };

    this.NodeType = function(nodeType) {
        if (nodeType !== undefined && typeof nodeType === 'string') {
            settings.nodeType = nodeType;
        } else {
            return settings.nodeType;
        }
    };

    this.ViewURL = function(viewUrl) {
        if (viewUrl !== undefined && typeof viewUrl === 'string') {
            settings.viewUrl = viewUrl;
        } else {
            return settings.viewUrl;
        }
    };

    this.ManageURL = function(manageUrl) {
        if (manageUrl !== undefined && typeof manageUrl === 'string') {
            settings.manageUrl = manageUrl;
        } else {
            return settings.manageUrl;
        }
    };

    this.ViewTitle = function(viewTitle) {
        if (viewTitle !== undefined && typeof viewTitle === 'string') {
            settings.viewTitle = viewTitle;
        } else {
            return settings.viewTitle;
        }
    };

    this.ManageTitle = function (manageTitle) {
        if (manageTitle !== undefined && typeof manageTitle === 'string') {
            settings.manageTitle = manageTitle;
        } else {
            return settings.manageTitle;
        }
    };

    this.IsDirty = function (modalCreateId) {
        var dirty = false;

        if (typeof isDirtyOverride === 'function') {
            dirty = isDirtyOverride();
            return dirty;
        }

        var inputContainerId = (modalCreateId) ? modalCreateId : settings.name;
        inputContainerId = (dirtyContainer) ? dirtyContainer : inputContainerId;
        $.each($.makeArray($('#' + inputContainerId + ' :input')), function (idx, val) {
            val = $(val);
            if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button' && val.attr('type') !== 'submit') {
                if (val.val() !== val.attr('data-original-value')) {
                    dirty = true;
                    return false;
                }
            }
        });
        return dirty;
    };
    
    this.CanSave = function (modalCreateId) {
        var canSave = true;
        var inputContainerId = (modalCreateId) ? modalCreateId : settings.name;
        if (!this.IsDirty(modalCreateId)) {
            canSave = false;
        } else {
            $.each($.makeArray($('#' + inputContainerId + ' :input')), function (idx, val) {
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
        }
        return canSave;
    };
    
    this.Validate = function (modalCreateId) {
        var isValid = true;
        var inputContainerId = (modalCreateId) ? modalCreateId : settings.name;
        if (this.CanSave(modalCreateId)) {
            $.each($.makeArray($('#' + inputContainerId + ' :input')), function (idx, val) {
                val = $(val);
                if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button') {
                    if (ValidateField(val) == false) {
                        isValid = false;
                    }
                }
            });
        }
        return isValid;
    };

    this.ResetForm = function() {
        $.each($.makeArray($('#' + settings.name + ' :input')), function(idx, val) {
            val = $(val);
            if (val.attr('type') !== 'hidden' && val.attr('type') !== 'button') {
                val.val(val.attr('data-original-value'));
            }
        });
    };

    this.Data = function (modalCreateId) {
        var inputContainerId = (modalCreateId) ? modalCreateId : settings.name;
        var data = $('#' + inputContainerId + ' :input').serialize();
        return data;
    };

    this.Save = function (modalCreateId, createUrl) {
        var postUrl = (createUrl) ? createUrl : settings.manageUrl;
        if (this.Validate(modalCreateId)) {
            $.post(postUrl, this.Data(modalCreateId), function () {
            })
            .done(function (data) {
                saveDoneCallback(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {
                saveFailCallback(errorThrown);
            });
        }
    };

    this.Edit = function() {        
        var data = {
            id: settings.nodeId,
            catalogId: settings.catalogId,
            rnd: Math.random()
        };

        $.get(settings.manageUrl, data, function (returnedData) {
            $('#' + settings.name).addClass('ContentPanelSectionEdit');
            $('#' + settings.name).html(returnedData);
            $('#' + settings.name + '_TitleBar_Span').text(settings.manageTitle);
            settings.isEditing = true;
        });
    };

    this.View = function() {
        var data = {
            id: settings.nodeId,
            catalogId: settings.catalogId,
            rnd: Math.random()
        };

        $.get(settings.viewUrl, data, function(returnedData) {
            $('#' + settings.name).removeClass('ContentPanelSectionEdit');
            $('#' + settings.name).html(returnedData);
            $('#' + settings.name + '_TitleBar_Span').text(settings.viewTitle);
            settings.isEditing = false;
        });
    };
}

function saveContentPanel(cpsSaveConfig) {
    var cps = getContentPanelSection(cpsSaveConfig.contentPanelSectionId);
    if (cps !== null) {
        cps.Save(cpsSaveConfig.modalCreateId, cpsSaveConfig.createUrl);
    }
}

function resetContentPanel(name)
{
    var cps = getContentPanelSection(name);
    if (cps !== null) {
        cps.ResetForm();
    }
}

function saveDoneCallback(data) {
    if (data.Success) {
        var cps = getContentPanelSection(data.Name);
        ShowsysMessage('success', data.Message);
        cps.View();
    } else {
        ShowsysMessage('error', data.Message);
    }
}

function saveFailCallback(errorThrown) {
    ShowsysMessage('error', errorThrown);
}

function cancelEdit(name) {
    var cps = getContentPanelSection(name);
    if (cps !== null) {
        var cont = true;
        if (cps.IsDirty()) {
            if (!confirm('You have unsaved changes.  Are you sure you want to cancel?')) {
                cont = false;
            }
        }
        if (cont) {
            cps.View();
        }
    }
}

var cpsSaveConfig = function (cpsId, sbId, mcId, createUrl) {
    this.contentPanelSectionId = cpsId;
    this.submitButtonId = sbId;
    this.modalCreateId = mcId;
    this.createUrl = createUrl;
};

//function ToggleSaveButton(cpsSaveConfig) {
//    var cps = getContentPanelSection(cpsSaveConfig.contentPanelSectionId);
//    if (cps.CanSave(cpsSaveConfig.modalCreateId)) {
//        $('#' + cpsSaveConfig.submitButtonId).removeAttr('disabled');
//    } else {
//        $('#' + cpsSaveConfig.submitButtonId).attr('disabled', 'disabled');
//    }
//}

function getContentPanelSection(name) {
    var retCps = null;
    var cp = contentPanels.GetCurrentContentPanel();
    if (cp.HasContentPanelSection(name)) {
        retCps = cp.GetContentPanelSection(name);
    }
    return retCps;
}

function ValidateField(val) {
    var validationMessage = '', isValid = true;
    if (val.attr('data-val-required') !== undefined) {
        if (val.val() === '') {
            isValid = false;
            validationMessage = AddValidationMessage(validationMessage, val.attr('data-val-required'));
        }
    }
    if (val.attr('data-val-length-max') !== undefined) {
        if ((val.val().length > parseInt(val.attr('data-val-length-max')))
            ||
            (val.attr('data-val-length-min') !== undefined && val.val().length < parseInt(val.attr('data-val-length-min')))) {
            isValid = false;
            validationMessage = AddValidationMessage(validationMessage, val.attr('data-val-length'));
        }
    }
    ToggleValidationMessage(val, validationMessage);
    
    return isValid;
}

function AddValidationMessage(validationMessage, messageToAdd) {
    return validationMessage + '<br/>' + messageToAdd;
}

function ToggleValidationMessage(obj, msg) {
    if (msg.length > 0) {
        var p;
        if (document.getElementById(obj.attr('id') + '_validation') != undefined) {
            p = $('#' + obj.attr('id') + '_validation');
            p.html(msg);
        } else {
            p = $('<p/>').attr('id', obj.attr('id') + '_validation').css('color', 'red').css('display', 'none').html(msg);
            p.insertAfter(obj);
        }
        obj.css('background-color', 'pink');
        p.slideDown(250);
    } else {
        if (document.getElementById(obj.attr('id') + '_validation') != undefined) {
            var p = $('#' + obj.attr('id') + '_validation');
            p.slideUp(250);
            obj.css('background-color', 'white');
    }
}
}

