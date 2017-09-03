

function freqCapSave() {
    // validate, then post    
    var freqCapDays = $('#txtFreqCapDays').val();
    var freqCapEmails = $('#txtFreqCapEmail').val();
    var freqCapEnabled = $('#chkFreqencyCapEnabled').is(':checked');
    var applicationId = $('#applicationId').val();
    $.ajax({
        url: freqCapSaveAction,
        type: 'POST',
        data: { ApplicationID: applicationId, FrequencyCapDays: freqCapDays, FrequencyCapEmails: freqCapEmails, Enabled: freqCapEnabled },
        success: function (data) {
            //alert(data.Success);
        },
        error: function (xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}

function freqCapReset() {
    // reset and gets the current state from db    
    var applicationId = $('#applicationId').val();
    $.ajax({
        url: freqCapResetAction,
        type: 'GET',
        cache: false,
        data: { ApplicationID: applicationId },
        success: function (data) {
            freqCapReset_Success(data);
        },
        error: function (xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}

function freqCapReset_Success(data) {
    // apply to partial view
    if (data.Enabled == true) {
        $('#chkFreqencyCapEnabled').attr('checked','checked');
    } else {
        $('#chkFreqencyCapEnabled').removeAttr('checked');
    }
    $('#txtFreqCapEmail').val(data.FrequencyCapEmail);
    $('#txtFreqCapDays').val(data.FrequencyCapDays);   
}
