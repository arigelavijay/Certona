// ================================================================
// insight-wizard.js - Client side user interface helper functionality for the
// jQuery Form Wizard plugin.  The plugin files can be found in the
// ~/Scripts/wizard directory.
//
// Additional documentation/examples of the jQuery Form Wizard
// Plugin can be found here:  http://www.thecodemine.org/
// ================================================================

//INITIALIZE THE WIZARD PLUGIN AFTER THE PAGE HAS FINISHED LOADING.
var myWizard;

$(function() {

    $('#ruleContent tr:first').html('');

    $('#wizardForm :input').change(function() {
        $('#hidFormDirty').val('1');
        $("#divSaveSuccessfulMessage").attr("style", "display:none;");
    });
});

$(function() {
    $('#wizardForm').submit(function() {
        postMessageToInsight('returnToInsight|');
    });

    // WIZARD PLUGIN SETUP
    /********************************************/
    // Configure the formwizard plugin.
    if (typeof myWizard === 'undefined') {
        myWizard = new CertonaWizard(5);
        myWizard.StepDivs([['create'], ['select_destination'], ['include_recommendations'], ['schedule_campaign'], ['confirm']]);
        myWizard.StepTitles(['Set Basic Remarketing Campaign File Features', 'Choose an E-mail Service Provider for the Remarketing Campaign File', 'Set Recommendation Preferences for the Remarketing Campaign File', 'Set Recommendation Campaign File Schedule Details', 'Save and Activate the Remarketing Campaign File']);
        myWizard.Mode(wizardMode);
        myWizard.WizardVariableName('myWizard');
        myWizard.StepChanged = StepChanged;
        myWizard.StepChanging = StepChanging; // Wire up the function
        myWizard.Display();
    }

    // EdM: Added in order to perform validation PRIOR to changing step

    function StepChanging(args) {
        if (args.CurrentStep === 1) {
            var form = $('#wizardForm');
            if (!form.valid()) {
                return false;
            }
        }
        return true;
    }

    function StepChanged(args) {
        validateFieldsForWizardConfirmation();
        displayWizardSteps();
        if (args.CurrentStep === 5) {
            $('#next').attr('disabled', 'disabled');
            $('#back').removeAttr('disabled');
        } else if (args.CurrentStep === 1) {
            $('#back').attr('disabled', 'disabled');
            $('#next').removeAttr('disabled');
        } else {
            $('#next').removeAttr('disabled');
            $('#back').removeAttr('disabled');
            if (args.CurrentStep === 3) {
                $("#Step3ViewModel_SelectedSchemeId").change();
            }
        }
    }

    displayWizardSteps();

    $('#save').click(function() {
        saveForm();
    });

    // WIZARD PLUGIN SAVE DATA ON EACH STEP
    /********************************************/
    // Wire up an AJAX POST to each Next and Back buttons in the Wizard.  When the user clicks the "Next" or
    // "Back" button, a POST will be made to the controller to save the user's data.  If the POST was successful,
    // the user will advance to the next step.  If the POST encountered an error, then the user cannot
    // advance to the next step.
    var caller = '';
    $('#next, #back').click(function() {
        caller = $(this).attr('id');

        // Make a POST to the controller's Save() method to persist data to the database.
        switch ($(this).attr('value')) {
        case 'Back':
            myWizard.Previous();
            return false;
        case 'Next':
            myWizard.Next();
            return false;
        }
    });
    /********************************************/


    //STEP 1.
    /********************************************/
    $('#Step1ViewModel_CampaignName').blur(function() {
        checkCampaignNameUniqueness();
    });

    var remarketingCampaignId = $('#campaignId');
    displayAbandonmentFilterControls();

    // Add the functionality to insert the images 
    // for the catalog drop down.
    try {
        $('#selectCatalog').msDropDown();
    } catch(e) {
        alert(e.message);
    }

    // Initialize the change event for the Catalog drop down.
    $('#selectCatalog').change(function() {
        // Clear out the existing drop downs.
        $('#selectCurrencyField').empty();

        // Get the selected Catalog ID.
        var selectedCatalogId = $(this).val();

        // If nothing was chosen, exit.
        if (selectedCatalogId === null || selectedCatalogId === '') {
            return;
        }

        // Make a call to the GetCurrencyFields action, fetching all of the Currency Fields
        // for the specified Catalog.
        $.ajax({
            type: 'GET',
            url: 'GetCurrencyFields',
            data: { catalogId: selectedCatalogId },
            dataType: 'json',
            success: function(currencyFields) {
                populateDropdownList('selectCurrencyField', currencyFields, false);
            }
        });
    });
    /// <summary>
    ///     Handle the "change" event of the Abandonment Filter radio buttons on Step 1 of the Remarketing Wizard.
    /// </summary>
    $('input:radio[name="abandonFilter"]').change(function() {
        // Get the ID of the radio button that was clicked.
        var selectedRadioButtonId = $(this).attr('id');
        var selectedAbandonmentId = selectedRadioButtonId[0];
        var selectedAbandonmentFilterId = selectedRadioButtonId[selectedRadioButtonId.length - 1];

        // Determine if the Catalog Details fields (i.e. the "choose a catalog" and "choose currency field" controls) should be displayed.
        toggleCatalogDetails(selectedRadioButtonId);

        // Determine which associated input control to enable and disable the rest.
        $.each($('input:radio[name="abandonFilter"]'), function() {
            // Get The ID of the radio button currently being operated on.
            var currentRadioButtonId = $(this).attr('id');

            // Get:
            // 1.  The Index of the radio button currently being operated on (the last character of the radio button's ID).
            // 2.  The type (i.e. the AbandonmentID) of abandonment filter.
            var abandonmentFilterId = currentRadioButtonId[currentRadioButtonId.length - 1];
            var abandonmentId = currentRadioButtonId[0];

            // If the user chose the "None" radio button (the ID of the radio button should start with the Abandonment_ID followed by an underscore, 
            // contain "_None" or "_none" and end with the Abandonment_Filter_ID that represents "None" for the specified Abandonment_ID), set the 
            // #selectedAbandonmentFilterId to indicate the "None" filter for the chosen Abandonment Type and move on to the next radio button.
            // The "None" option isn't wired up to an input field, so there's no need to enable/disable anything.
            var regEx = '^' + selectedAbandonmentId + '_.*?[nN]one_' + selectedAbandonmentFilterId + '$';
            if (currentRadioButtonId.match(regEx)) {
                $('#selectedAbandonmentFilterId').val(selectedAbandonmentFilterId);
                return;
            }

            // Build the ID of the Telerik input control that corresponds to the radio button currently being operated on.
            var filterInputId = '#filterOptionInput_' + abandonmentId + '_' + abandonmentFilterId + '-input-text';

            // Enable the Telerik input that corresponds to the radio button selected by the user.  Disable all other input.
            // NOTE:  ".enable()" and ".disable()" are methods specific to the Telerik NumberTextBox controls.  Additional information
            // can be found here: http://demos.telerik.com/aspnet-mvc/razor/numerictextbox/clientsideapi
            if ($(filterInputId).data('tTextBox') != null && $(filterInputId).data('tTextBox') != 'undefined') {
                var numericTextBox = $(filterInputId).data('tTextBox');
                if (currentRadioButtonId === selectedRadioButtonId) {
                    var selectedAbandonmentFilterInputFieldValue = $('#selectedAbandonmentFilterInput').val();
                    numericTextBox.enable();

                    if (selectedAbandonmentFilterInputFieldValue == "") {
                        numericTextBox.value(numericTextBox.minValue);
                    } else {
                        numericTextBox.value(selectedAbandonmentFilterInputFieldValue);
                    }

                    $('#selectedAbandonmentFilterId').val(abandonmentFilterId);
                } else {
                    numericTextBox.disable();
                    numericTextBox.value(numericTextBox.minValue);
                }
            }
        });
    });


    //ACURTIS:
    //HANDLES SETTING A HIDDEN FIELD WHENEVER A NEW ABANDONMENT FILTER
    //NUMBER OR CURRENCY INPUT VALUE CHANGES SO THAT THE INPUT AMOUNT
    //CAN BE BOUND TO THE MODEL.
    $('.AbandonmentFilterInput').change(function() {
        var fieldId = $(this).attr('id');
        var inputAmount = $("#" + fieldId).val();
        $('#selectedAbandonmentFilterInput').val(inputAmount);
    });

    // Strategy Management 
    $('#filteringYes').change(function() {
        $('#hidFormDirty').val('1');
        includeFilteringHandler('filteringYes');
    });

    $('#filteringNo').change(function() {
        $('#hidFormDirty').val('1');
        includeFilteringHandler('filteringNo');
    });

    switch ($('#hdnIncludeFiltering').val()) {
    case 'True':
        includeFilteringHandler('filteringYes');
        break;
    case 'False':
        includeFilteringHandler('filteringNo');
        break;
    }
    /********************************************/


    //STEP 2.
    /********************************************/
    $('#Step2ViewModel_ESPCampaignId').blur(function() {
        checkESPCampaignIDUniqueness();
    });

    //Initialize visiblity of the ESP information based on the selected drop down value.
    ToggleDivs('Step2ViewModel_SelectedEmailServiceProviderId', 5, 'divESP_');
    if ($('#Step2ViewModel_SelectedEmailServiceProviderId :selected').val() != "0") {
        $('#divCampaignID').attr("style", "display:");
        $('#divUserName').attr("style", "display:");
        $('#divUserPassword').attr("style", "display:");
    }
    /********************************************/


    //STEP 3.
    /********************************************/
    //Set default value for the include recommendations radio buttons.
    //Also wire up events to hide\show include recommendations divs based on 
    //which radio button was selected.
    $('#includeRecommendationsYes').change(function() {
        includeRecommendationsHandler('includeRecommendationsYes');
    });

    $('#includeRecommendationsNo').change(function() {
        includeRecommendationsHandler('includeRecommendationsNo');
    });

    switch ($('#hdnIncludeRecommendations').val()) {
    case 'True':
        includeRecommendationsHandler('includeRecommendationsYes');
        break;
    case 'False':
        includeRecommendationsHandler('includeRecommendationsNo');
        break;
    }

    //ACURTIS:
    //MAKES AN AJAX CALL TO REBUILD THE RIS IMAGE TAG BASED ON THE
    //INFORMATION ENTERED SO FAR.  IF ALL FIELDS AREN'T COMPLETE
    //A MISSING INFORMATION CONFIRMATION MESSAGE WILL BE RETURNED.
    $("#Step3ViewModel_SelectedSchemeId").change(function() {
        if ($("#Step3ViewModel_SelectedSchemeId").val() > 0) {
            var form = $('#wizardForm');

            // If the form isn't valid, exit the routine without making the POST.
            // .valid() method comes from the jquery.validate script:  http://docs.jquery.com/Plugins/Validation
            if (!form.valid()) {
                return;
            }

            $.ajax(
                {
                    url: "RIS",
                    type: "POST",
                    data: form.serializeObject(),
                    success: function(result) {
                        $('#divRISText').html(result.RISSnippet);
                        $("#divRIS").attr("style", "display:block");
                    },
                    error: function(req, status, error) {
                        //alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
                    }
                });
        } else {
            $('#divRISText').html('');
            $("#divRIS").attr("style", "display:none");
        }
    });
    /********************************************/
});

function saveForm(mode) {
    var form = $('#wizardForm');

    // If the form isn't valid, exit the routine without making the POST.
    // .valid() method comes from the jquery.validate script:  http://docs.jquery.com/Plugins/Validation
    if (!form.valid()) {
        $("#divCampaignNameMustBeUnique").attr("style", "display:none;");
        return false;
    }

    // Call Validate BEFORE posting, if errors then pass back to controller as part of form data
    // In old flow, the validate step was performed AFTER post and save
    validateFieldsForWizardConfirmation();

    if (errors) {
        $('#errors').val('true');
    } else {
        $('#errors').val('');
    }

    $.ajax({
        url: 'SaveIncremental',
        type: 'POST',
        data: form.serializeObject(),
        success: function(data) {
            $("#campaignId").val(data["Id"]);
            resetStatus(data);
            $('#hidFormDirty').val('0');

            // Introspect the return data for the updated Scheme/Variant/Rule IDs
            if (data["FilterSchemeID"] !== null && data["FilterSchemeID"] !== '') {
                // apply these to form               
                $("#filterschemeId").val(data["FilterSchemeID"]);
                $("#variantId").val(data["VariantID"]);
                $("#ruleId").val(data["RuleID"]);
            }

            if (mode == 'rulesEditor') {
                // Continue on to rules editor
                postRulesEditorMessage();
            } else if (mode == 'activate') {
                activateCampaign();
            } else {
                //alert('Save Successful!');
                $("#divSaveSuccessfulMessage").attr("style", "display:inline;");
            }
        },
        error: function(xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
    return false;
}


/// <summary>
/// Determine how to display the wizard steps at the top of the page:
///
/// 1.  The step currently being viewed will have the "currentStep" CSS clas applied.
/// 2.  Any activated steps (i.e. steps that were visited prior to the current step) will have
///     the "activatedStep" CSS class applied.
/// 3.  Any remaining steps will not have a special CSS class applied.
/// 
/// The addVisualization() method is used to build the SPAN and apply the correct CSS.
///</summary>

function displayWizardSteps() {
    var currentStep = myWizard.StepDivs()[myWizard.Step() - 1][0];
    $('#currentStep').val(currentStep);
    if (myWizard.Step() === 5) {
        $('#next').attr('disabled', 'disabled');
        $('#next').removeClass('inputButton');
        $('#next').addClass('inputButtonDisabled');

        $('#back').removeAttr('disabled');
        $('#back').removeClass('inputButtonDisabled');
        $('#back').addClass('inputButton');
    } else if (myWizard.Step() === 1) {
        $('#back').attr('disabled', 'disabled');
        $('#back').removeClass('inputButton');
        $('#back').addClass('inputButtonDisabled');

        $('#next').removeAttr('disabled');
        $('#next').removeClass('inputButtonDisabled');
        $('#next').addClass('inputButton');
    } else {
        $('#next').removeAttr('disabled');
        $('#next').removeClass('inputButtonDisabled');
        $('#next').addClass('inputButton');

        $('#back').removeAttr('disabled');
        $('#back').removeClass('inputButtonDisabled');
        $('#back').addClass('inputButton');
        if (myWizard.Step() === 3) {
            $("#Step3ViewModel_SelectedSchemeId").change();
        }
    }
    updateHelpLink();
}


/// <summary>
/// Display the user's input the various other "screens" (which are really just DIVs) in the wizard on the 
/// last step of the wizard (i.e. the "Confirmation" screen).
/// </summary>
/// <remarks>
/// This method is quite the hack.  I'm sure there is a smarter way to handle this, but it'll get us by
/// for now...
/// </remarks>

function validateFieldsForWizardConfirmation() {
    errors = false;

    //ACURTIS:
    //VALIDATE ALL WIZARD FORM FIELDS AND SET APPROPRIATE 
    //FIELD VALUES OR ERROR TEXT ON CONFIRMATION SCREEN.
    /*****************************************************/

    //STEP 1.
    validateAndSetField($('#Step1ViewModel_CampaignName').val(), 'summary_campaignName');
    validateAndSetField($('input[:name=useAcrossAllApps]:checked').val() === 'True' ? 'Use across entire account' : 'Use only in this application', 'summary_useAcrossAllApps');

    if ($('#Step1ViewModel_SelectedAbandonmentType_AbandonmentID :selected').val() === "0") {
        validateAndSetField($('#Step1ViewModel_SelectedAbandonmentType_AbandonmentID :selected').val(), 'summary_campaignType');
    } else {
        validateAndSetField($('#Step1ViewModel_SelectedAbandonmentType_AbandonmentID :selected').text(), 'summary_campaignType');
    }

    // RULE:  A catalog is required regardless of which abandonment filter was selected.
    if ($('#selectCatalog').val() === '0') {
        validateAndSetField($('#selectCatalog :selected').val(), 'summary_catalogName');
    } else {
        validateAndSetField($('#selectCatalog :selected').text(), 'summary_catalogName');
    }

    // RULE:  If a Currency Abandonment Filter has been selected, then a Currency Field must be selected.  Otherwise, the Currency
    // Field doesn't matter, so the summary will display "[Not Applicable]".
    if ($('input:radio[name="abandonFilter"]:checked').attr('id') != undefined && $('input:radio[name="abandonFilter"]:checked').attr('id').match('[Cc]urrency')) {
        if ($('#selectCurrencyField :selected').val() === '') {
            validateAndSetField($('#selectCurrencyField :selected').val(), 'summary_currencyFieldName');
        } else {
            validateAndSetField($('#selectCurrencyField :selected').text(), 'summary_currencyFieldName');
        }
    } else {
        $('#summary_currencyFieldName').text('[Not Applicable]');
        $('#summary_currencyFieldName').css('color', '#404040');
    }


    //STEP 2.
    if ($('#hdnESPCount').val() === "0") {
        validateAndSetField('0', 'summary_serviceProvider');
        validateAndSetField('0', 'summary_campaignID');
        validateAndSetField('0', 'summary_connectionUserID');
        validateAndSetField('0', 'summary_connectionPassword');
    } else {
        if ($('#Step2ViewModel_SelectedEmailServiceProviderId :selected').val() === "0") {
            validateAndSetField($('#Step2ViewModel_SelectedEmailServiceProviderId :selected').val(), 'summary_serviceProvider');
        } else {
            validateAndSetField($('#Step2ViewModel_SelectedEmailServiceProviderId :selected').text(), 'summary_serviceProvider');
        }
        validateAndSetField($('#Step2ViewModel_ESPCampaignId').val(), 'summary_campaignID');

        validateAndSetField($('#Step2ViewModel_Export_Data_Service_UserName').val(), 'summary_connectionUserID');
        validateAndSetField($('#Step2ViewModel_Export_Data_Service_Password').val(), 'summary_connectionPassword');
        if ($('#Step2ViewModel_SelectedEmailServiceProviderId :selected').val() === exportDataServiceProvider_MyFTP) {
            validateAndSetField($('#Step2ViewModel_Export_Data_Service_HostBaseAddress').val(), 'HostBaseAddress');
            if (trimString($('#Step2ViewModel_Export_Data_Service_HostBaseAddress').val()) !== '') {
                $('#HostBaseAddress').attr("style", "color:#0000FF;");
            }
        }
    }


    //STEP 3.
    if ($('#divNoLocationsConfirmation').is(':visible')) {
        validateAndSetField('No', 'summary_recommendations');
        validateAndSetField('N\A', 'summary_emailScheme');
    } else {
        var recommendations = '';
        if ($('#includeRecommendationsYes').is(':checked')) {
            recommendations = 'Yes';
        }

        if ($('#includeRecommendationsNo').is(':checked')) {
            recommendations = 'No';
        }

        validateAndSetField(recommendations, 'summary_recommendations');

        if (recommendations == 'Yes') {
            if ($('#Step3ViewModel_SelectedSchemeId :selected').val() === "0") {
                validateAndSetField($('#Step3ViewModel_SelectedSchemeId :selected').val(), 'summary_emailScheme');
            } else {
                validateAndSetField($('#Step3ViewModel_SelectedSchemeId :selected').text(), 'summary_emailScheme');
            }
        } else {
            validateAndSetField('N\A', 'summary_emailScheme');
        }
    }


    //STEP 4.
    //HANDLE THE FREQUENCY CAP FIELDS AND TOGGLE THE DIVS 
    //BASED ON WHETHER FREQUENCY CAPPING IS BEING APPLIED.
    /* TEMPORARILY DISABLING UNTIL WE ARE READY TO IMPLEMENT
       FREQUENCY CAPPING ACROSS CAMPAIGNS.
    if ($('#includeFrequencyCapYes').is(':checked')) {
    $("#divSummaryFrequencyCapNo").attr("style", "display:none;");
    $("#divSummaryFrequencyCapYes").attr("style", "display:inline;");
    $('#summary_abandonmentInterval').text($('#Step4ViewModel_SelectedIntervalId :selected').text());
    $('#summary_frequencyCapEmails').text($('#Step4ViewModel_FrequencyCapEmails').val());
    $('#summary_frequencyCapDays').text($('#Step4ViewModel_FrequencyCapDays').val());
    }
    else {
    $("#divSummaryFrequencyCapNo").attr("style", "display:inline;");
    $("#divSummaryFrequencyCapYes").attr("style", "display:none;");
    }
    */

    $('#summary_startDate').text($('#Step4ViewModel_StartDate').val());

    var Step4ViewModel_EndDate = $('#Step4ViewModel_EndDate').val();
    if (Step4ViewModel_EndDate == '') {
        $('#summary_endDate').text('no end date');
    } else {
        $('#summary_endDate').text(Step4ViewModel_EndDate);
    }


    //STEP 5.
    //CONFIGURE SUMMARY ERROR MESSAGE AND SET STYLING FOR ACTIVATE BUTTON.
    if (errors) {
        $("#divNotReadyForActivation").attr("style", "display:inline;");
        $("#divReadyForActivation").attr("style", "display:none;");
        $("#divDeliveryDetailsParent").attr("style", "display:inline;");
        $("#activate").attr("style", "display:none;");
    } else {
        $("#divNotReadyForActivation").attr("style", "display:none;");
        $("#divReadyForActivation").attr("style", "display:inline;");
        $("#divDeliveryDetailsParent").attr("style", "display:inline;");

        var currentStep = myWizard.StepDivs()[myWizard.Step() - 1][0];
        if (currentStep == 'confirm') {
            $("#activate").attr("style", "display:inline;");
        } else {
            $("#activate").attr("style", "display:none;");
        }
    }
    /*****************************************************/


    //---------------------------------------------------------
    // SET THE DELIVERY DESTINATION DETAILS.
    //---------------------------------------------------------
    $('#deliveryDetailsTest').empty();

    if (!$('#noEspsConfigured').is(':visible')) {
        var serviceProviderDetailsId = $('#Step2ViewModel_SelectedEmailServiceProviderId :selected').val();

        if (serviceProviderDetailsId != exportDataServiceProvider_MyFTP) {
            $('#divESP_' + serviceProviderDetailsId).clone(true)
                .appendTo($('#deliveryDetailsTest'));
        } else {
            $('#divESP_' + serviceProviderDetailsId + '_Copy').clone(true).attr("style", "display:inline;")
                .appendTo($('#deliveryDetailsTest'));
            if (errors)
                $("#divDeliveryDetailsParent").attr("style", "display:inline;");
        }
    }
}


//ACURTIS:
//VALIDATION ROUTINE WHICH VERIFIES THE FIELD VALUES AND SETS 
//THE SYLING ACCORDING TO WHETHER OR NOT THERE WAS ANY ERRORS.
var errors = false;

function validateAndSetField(value, summarySpanName) {
    var fieldRequired = $('#requiredForActivationText').val();

    if (trimString(value) == '' || trimString(value) == '0') {
        errors = true;
        $('#' + summarySpanName).attr("style", "color:red;");
        $('#' + summarySpanName).html(fieldRequired);
    } else {
        $('#' + summarySpanName).attr("style", "color:#404040;");
        $('#' + summarySpanName).html(value);
    }
}


/// <summary>
/// Split up the step's ID at the underscore character.  Capitalize the first letter
/// of each word, unless the word is "and", "or" or "the".  Return the adjusted text
/// for use as the text to describe the steps in the Remarketing Wizard.
/// </summary>
/// <param name="id">The ID of the step whose display text is being built.</param>

function buildStepText(id) {
    // Separate the id into "terms" based on the underscore.
    var words = id.split('_');
    var stepText = '';

    // Capitalize the first letter of each term and append it to the stepText.
    for (var i = 0; i < words.length; i++) {
        // Do not capitalize and, or, the
        if (words[i].match('^\and|or|the+$')) {
            continue;
        }

        // Capitalize the first letter then append everything else.
        words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);

        stepText += words[i] + ' ';
    }

    // Trim the end of the string, removing any extra spaces.
    return stepText.replace(/^\s+|\s+$/g, '');
}


//ACURTIS:
//GENERIC FUNCTION TO TOGGLE TWO DIVS BASED ON A DROP DOWN SELECTION.
//THE DIVS NEED TO HAVE A BASE NAME APPENDED WITH A UNIQUE ID THAT
//CORRESPOND TO THE ID SELECTED IN THE DROP DOWN. IF A DEFAULT
//VALUE IS PRESENT IN THE DROP DOWN THE VALUE SHOULD BE 0.  THIS WILL
//HIDE ALL DIVS.

function ToggleDivs(dropDownName, numberOfDropDownItems, divBaseName) {
    var i = 0;

    for (i = 1; i < numberOfDropDownItems + 1; i++) {
        if ($("#" + dropDownName).val() == i) {
            $("#" + divBaseName + i).attr("style", "display:block");
        } else {
            {
                $("#" + divBaseName + i).attr("style", "display:none");
            }
        }
    }
}


/// <summary>
/// Display the appropriate Abandonment Filter DIV for the selected Abandonment Type.  As long as the user
/// has selected an Abandonment Type, show the Catalog dropdown list.
/// </summary>

function displayAbandonmentFilterControls() {
    var abandonmentTypeId = $('#Step1ViewModel_SelectedAbandonmentType_AbandonmentID :selected').val();
    var divToShow = 'divAbandonmentFilters_' + abandonmentTypeId;

    // Loop through the Abandonment Filter divs and display the div that contains the Filter controls
    // for the selected Abandonment Type.
    $.each($('div[id^=divAbandonmentFilters_]'), function() {
        var currentDiv = $(this);

        if (currentDiv.attr('id') === divToShow) {
            $(currentDiv).css('display', 'block');
        } else {
            $(currentDiv).css('display', 'none');
        }
    });

    // If the user chose an Abandonment Type, display the Catalog dropdown list.
    if (abandonmentTypeId === '0') {
        $('#catalogDetails').css('display', 'none');
    } else {
        $('#catalogDetails').css('display', 'block');
    }
}


//ACURTIS:
//TOGGLES THE CAMPAIGN ID FIELD ON\OFF BASED ON WHETHER OR NOT A VALID
//ESP HAS BEEN SELECTED IN THE DROP DOWN.

function ToggleCampaignIDDiv() {
    if (!$('#noEspsConfigured').is(':visible')) {
        if ($("#Step2ViewModel_SelectedEmailServiceProviderId").val() === "0") {
            $("#divCampaignID").attr("style", "display:none;");
            $("#divUserName").attr("style", "display:none;");
            $("#divUserPassword").attr("style", "display:none;");
        } else {
            $("#divCampaignID").attr("style", "display:inline;");
            $("#divUserName").attr("style", "display:inline;");
            $("#divUserPassword").attr("style", "display:inline;");

        }
    }
}


//ACURTIS:
//MAKES A CALL TO DETERMINE WHETHER OR NOT THE ESP CAMPAIGN
//ID IS UNIQUE.

function checkESPCampaignIDUniqueness() {
    var form = $('#wizardForm');

    // If the form isn't valid, exit the routine without making the POST.
    // .valid() method comes from the jquery.validate script:  http://docs.jquery.com/Plugins/Validation
    if (!form.valid()) {
        return;
    }

    var newCampaignId = $("#Step2ViewModel_ESPCampaignId").val();
    ;
    var origCampaignId = $("#hdnOriginalCampaignId").val();

    if (trimString(newCampaignId) != trimString(origCampaignId)) {
        $.ajax(
            {
                url: "CheckESPCampaignIDUniqueness",
                type: "POST",
                data: form.serializeObject(),
                success: function(result) {
                    if (!result.Unique) {
                        $("#divESPCampaignIDRequired").attr("style", "display:inline;");
                        var textBox = $('#Step2ViewModel_ESPCampaignId');
                        textBox.val('');
                        textBox.focus();
                        return false;
                    } else {
                        $("#divESPCampaignIDRequired").attr("style", "display:none;");
                    }
                },
                error: function(req, status, error) {
                    //alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
                }
            });
    } else {
        $("#divESPCampaignIDRequired").attr("style", "display:none;");
    }
}


//ACURTIS:
//MAKES A CALL TO DETERMINE WHETHER OR NOT THE CAMPAIGN
//NAME IS UNIQUE.

function checkCampaignNameUniqueness() {
    var form = $('#wizardForm');

    // If the form isn't valid, exit the routine without making the POST.
    // .valid() method comes from the jquery.validate script:  http://docs.jquery.com/Plugins/Validation
    if (!form.valid()) {
        $("#divCampaignNameMustBeUnique").attr("style", "display:none;"); // hide if necessary as required field validator takes preference
        return;
    }

    var newCampaignName = $("#Step1ViewModel_CampaignName").val();
    var origCampaignName = $("#hdnOriginalCampaignName").val();

    if (trimString(newCampaignName).toLowerCase() != trimString(origCampaignName).toLowerCase()) {
        $.ajax(
            {
                url: "CheckCampaignNameUniqueness",
                type: "POST",
                data: form.serializeObject(),
                success: function(result) {
                    if (!result.Unique) {
                        $("#divCampaignNameMustBeUnique").attr("style", "display:inline;");
                        var textBox = $('#Step1ViewModel_CampaignName');
                        textBox.val('');
                        //textBox.focus();
                        return false;

                    } else {
                        $("#divCampaignNameMustBeUnique").attr("style", "display:none;");
                    }
                },
                error: function(req, status, error) {
                    //alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
                }
            });
    } else {
        $("#divCampaignNameMustBeUnique").attr("style", "display:none;");
    }
}


//ACURTIS:
//TOGGLES THE 'no end date' RADIO BUTTON SELECTION AND THE 
//END DATE FIELD VALUES SO THE USER CAN'T SELECT NO END DATE
//WHILE SELECTING AN ACTUAL END DATE. THIS ALSO PREVENTS THE
//USER FROM SELECTING AN END DATE THAT IS EARLIER THAN THE
//START DATE.

function CampaignEndDateHandler(controlThatRaisedEvent) {
    if (controlThatRaisedEvent == 'radio') {
        $("#Step4ViewModel_EndDate").val('');
        $("#rdoNoEndDate").attr('checked', true);
    } else {
        var startDate = trimString($("#Step4ViewModel_StartDate").val());
        var endDate = trimString($("#Step4ViewModel_EndDate").val());

        if (startDate != '' && endDate != '') {
            if (Date.parse(startDate) >= Date.parse(endDate)) {
                alert('The campaign end date must be later than the start date.');
                $("#Step4ViewModel_EndDate").val('');
                $("#rdoNoEndDate").attr('checked', true);
            }
        }
        if (trimString($("#Step4ViewModel_EndDate").val()) === '') {
            $("#rdoNoEndDate").attr('checked', true);
        } else {
            $("#rdoNoEndDate").attr('checked', false);
        }
    }

    $("#hdnNoEnddate").val($("#rdoNoEndDate").is(':checked').toString());
}


//ACURTIS:
//THIS PREVENTS THE USER FROM SELECTING A START DATE THAT 
//IS EARLIER THAN THE END DATE.

function CampaignStartDateHandler() {
    var startDate = trimString($("#Step4ViewModel_StartDate").val());
    var endDate = trimString($("#Step4ViewModel_EndDate").val());

    if (startDate != '' && endDate != '') {
        if (Date.parse(startDate) >= Date.parse(endDate)) {
            alert('The campaign start date must be earlier than the end date.');
            $("#Step4ViewModel_StartDate").val('');
        }
    }
}


//ACURTIS:
//Handles the switching of the Include Recommendations radio buttons on step 3.

function includeRecommendationsHandler(checkedRadioButtonId) {
    switch (checkedRadioButtonId) {
    case 'includeRecommendationsYes':
        $("#divRecommendationDetails").attr("style", "display:inline");
        break;
    case 'includeRecommendationsNo':
        //Hide the RIS Snippet area.
        $('#divRISText').html('');
        $("#divRIS").attr("style", "display:none");

        //Hide the Email Location section.
        $("#divRecommendationDetails").attr("style", "display:none");

        //Set the Email Location drop down back to the default selection.
        $("#Step3ViewModel_SelectedSchemeId").val('0');
        break;
    }
}


/// <summary>
/// When a user chooses a different Campaign Type, 
/// </summary>
/// <remarks>
/// This function is called in the onchange event of the dropdownlist defined on line 26 of the _Wizard.cshtml partial view 
/// (the "Campaign Type" dropdown list on Step 1 of the wizard).
/// </remarks>

function setAbandonmentFilter() {
    // Get the value of the selected item in the Campaign Type dropdown list.
    var abandonmentId = $('#Step1ViewModel_SelectedAbandonmentType_AbandonmentID :selected').val();

    // If the "Select a Campaign Type:" item is selected, exit.
    if (abandonmentId === '0') {
        return;
    }

    // Get a handle on the value for the Abandonment Filter ID.
    var selectedAbandonmentFilterId = $('#selectedAbandonmentFilterId').val();

    // Get a handle on the AbandonmentId that was selected.
    var selectedAbandonmentId = $('#selectedAbandonmentId').val();
    if (selectedAbandonmentId === '') {
        selectedAbandonmentId = abandonmentId;
    }

    // Get the selector for the radio button that corresponds to the selected Abandonment ID and the selected Abandonment Filter ID.
    // If the selectedAbandonmentFilterId is an invalid value OR the newly selected AbandonmentId doesn't match the current AbandonmentId,
    // choose the "None" filter option for the corresponding Abandonment ID.
    var radioButtonSelector;
    if (null === selectedAbandonmentFilterId || selectedAbandonmentFilterId === undefined || selectedAbandonmentFilterId === '0' || selectedAbandonmentId !== abandonmentId) {
        radioButtonSelector = $('input:radio[name="abandonFilter"][id^=' + abandonmentId + '_filterOption_None]');
    } else {
        radioButtonSelector = $('input:radio[name="abandonFilter"][id$=' + selectedAbandonmentFilterId + ']');
    }

    $('#selectedAbandonmentId').val(abandonmentId);

    // Get a handle on the radio button and raise its change() event.
    var radioButton = $(radioButtonSelector);
    radioButton.attr('checked', 'checked');
    radioButton.change();
}


/// <summary>
/// Determine whether or not to display the Catalog Details controls (i.e. the Catalog and Currency Field dropdowns).
/// if a user has clicked on one of the "Currency" Abandonment Filters, 
/// </summary>

function toggleCatalogDetails(selectedFilterId) {
    // Get a reference to the div that holds the Catalog Currency Field dropdown list.
    var currencyFieldsDiv = $('#currencyFieldSelection');

    // If the user clicked on a "Currency" filter radio button, display the currencyCatalog Details controls.
    // Otherwise, hide the Catalog Details controls.
    if (selectedFilterId.match('\[Cc]urrency')) {
        currencyFieldsDiv.css('display', 'block');
    } else {
        currencyFieldsDiv.css('display', 'none');
    }
}


/// <summary>
/// Add a collection of items to a dropdown list control.
/// </summary>
/// <param name="dropdownId">The ID of the dropdown input conrol that needs to be populated.
/// The ID can be provided with or without the '#' prefix.</param>
/// <param name="selectList">The collection of items that need to be added to the dropdown list.</param>

function populateDropdownList(dropdownId, selectList, includeInitialOption) {
    // Get a reference to the dropdown list and empty it.
    dropdownId = buildIdSelector(dropdownId);
    var dropdownControl = $(dropdownId);
    dropdownControl.empty();

    // If the item is optional then display some a initial 
    // option giving the user the ability to not select anything.
    if (includeInitialOption) {
        if (selectList.length > 1) {
            dropdownControl.append($('<option/>', {
                value: '',
                text: 'This is optional...'
            }));
        }
    }

    // Add each list item to the dropdown list.
    $.each(selectList, function(index, selectListItem) {
        dropdownControl.append($('<option/>', {
            value: selectListItem.Value,
            text: selectListItem.Text,
            selected: selectListItem.Selected
        }));
    });
}


/// <summary>
/// Pre-pend a "#" symbol to an element ID.  This builds a proper ID selector for use with jQuery.
/// </summary>

function buildIdSelector(elementId) {
    return elementId.substr(0, 1) === '#' ? elementId : '#' + elementId;
}


/// <summary>
/// Trim leading and trailing spaces from a string.
/// </summary>

function trimString(input) {
    return input.replace(/^\s+|\s+$/g, '');
}


/// <summary>
/// Used to jump to any step in the wizard.
/// </summary>
/// <remarks>
/// This method is called by the NavigationEdit.cshtml when a user clicks on the navigation links at the top of the screen.
/// </remarks>

function gotoStep(stepId) {
    $('#wizardForm').formwizard('show', stepId);
}


//ACURTIS:
//HANDLES SETTING A HIDDEN FIELD WHENEVER A NEW ABANDONMENT FILTER
//IS SELECTED SO THAT THE SELECTED FILTER CAN BE BOUND TO THE MODEL.

function handleAbandonmentFilterSelection(abandonmentFilterID) {
    $('#selectedAbandonmentFilterId').val(abandonmentFilterID);
}


/// <summary>
/// Extend jQuery with a "serializeObject" method.  This method will serialize all fields on a form, even
/// those that are disabled.
/// </summary>
/// <remarks>
/// This method is called by the click() event on the Next and Back buttons.  Using this method allows us to post the
/// entire form.
/// </remarks>
$.fn.serializeObject = function() {
    // Enable all disabled fields - serialization will ignore any fields that are disabled (whether or not they have values).
    var disabledFields = this.find(':input:disabled').removeAttr('disabled');

    var o = {};
    var a = this.serializeArray();

    // Loop through the serialized array and dump them into the object that will be returned.    
    $.each(a, function() {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    // Disable the fields that were disabled before.
    disabledFields.attr('disabled', 'disabled');

    return o;
};


//ACURTIS:
//MAKES A JSON CALL TO THE CONTROLLER TO ACTIVATE A CAMPAIGN.  
//UPON A SUCCESSFUL RESPONSE IT REDIRECTS THE USER BACK TO 
//THE SUMMARY SCREEN IN INSIGHT.

function activateCampaign() {
    var form = $('#wizardForm');

    // If the form isn't valid, exit the routine without making the POST.
    // .valid() method comes from the jquery.validate script:  http://docs.jquery.com/Plugins/Validation
    if (!form.valid()) {
        return;
    }

    if ($('#hidFormDirty').val() == 1) {
        saveForm('activate');
        return;
    }

    $.ajax({
        url: 'Activate',
        type: 'POST',
        data: form.serializeObject(),
        success: function() {
            postMessageToInsight('returnToInsight|');
        },
        error: function(xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}

//ACURTIS:
//BUILDS THE HELP PARAMETER AND CALLS THE POST MESSAGE
//FUNCTION WHENEVER THE USER TO A DIFFERENT STEP.

function updateHelpLink() {
    var helpLink = 'help|' + $("#currentStep").val();
    postMessageToInsight(helpLink);
}

//ACURTIS:
//POST A MESSAGE TO THE PARENT INSIGHT CONTAINER AND 
//PASS THE DESIRED PARAMETERS.  THIS WILL BE USED TO
//REDIRECT BACK TO THE INSIGHT APPLICATION PAGE, THE HELP
//LINKS, AND OTHER FUNCTIONALITY THAT REQUIRES
//COMMUNICATION BETWEEN INSIGHT & FORESIGHT.

function postMessageToInsight(parameter) {
    var origin = $("#postMessageOrigin").val();
    $.postMessage(parameter, origin);
}

//EDM:
//RESETS STATUS OF FORM AFTER A SAVE
//STYLE IF INACTIVATED

function resetStatus(data) {
    // Clear
    $('#errors').val('');

    if (data.ViewModel["Status_ID"] == "1") {
        $('#status').val('Active');
    } else if (data.ViewModel["Status_ID"] == "2") {
        // If Inactive coming back then check previous state, If previous state == Active, then display message
        if ($('#status').val().toLowerCase() == 'active') {
            // display message 
            $("#reactivateMessage").attr("style", "display:inline;");
            // change button text
            $("#activate").attr("value", $('#reactivateButtonText').val());
        }
        $('#status').val('Inactive');
    } else {
        $('#status').val('Deleted');
    }
}


function exitRemarketingWizard() {
    if ($('#hidFormDirty').val() == 1) {
        if (confirm($("#unsavedChangesText").val())) {
            saveForm();
        }
    }
    postMessageToInsight('returnToInsight|');
}

//////////////////////////////////////////////////////////////////
//
// Functions for Remarketing Strategy/Rules Management
//
//////////////////////////////////////////////////////////////////

function openRulesEditor(expressionId) {

    // first check for validations specific to Strategy Management, prior to calling generic saveForm
    // i.e. Catalog Selected
    if ($("#selectCatalog").val() == '0') {
        alert($('#ruleSelectCatalogText').val());
        return;
    }

    if ($('#hidFormDirty').val() == 1) {
        if (confirm($('#ruleSaveCampaignText').val())) {

            // ideally, this would post message to save campaign and get missing variantId/catalogId/RuleId            
            if (saveForm('rulesEditor') == false) {
                return;
            }
        }
    } else {
        // ok to post
        postRulesEditorMessage();
    }
}

function postRulesEditorMessage() {
    var accountId = $("#accountId").val();
    var campaignId = $("#campaignId").val();
    var applicationId = $('#applicationId').val();
    var catalogId = $("#selectCatalog").val();

    var variantId = $("#variantId").val();
    var ruleId = $("#ruleId").val();

    var expId = '0'; // will be zero when creating a new expression

    //postMessageToInsight('displayRulesEditor|AccountID=' + 'ResonanceRecords' + '|ApplicationID=' + 'ResonanceRecords01' + '|VariantID=' + '25' + '|CatalogID=' + 'ResonanceRecordsc01' + '|RuleID=' + '12' + '|ExpID=' + '0');

    postMessageToInsight('displayRulesEditor|' + campaignId + '|' + accountId + '|' + applicationId + '|' + variantId + '|' + catalogId + '|' + ruleId + '|' + expId);
}


function getRulesHTML() {
    var accountID = $("#accountId").val();
    var applicationID = $("#applicationId").val();
    var catalogID = $("#selectCatalog").val();
    var variantID = $("#variantId").val();

    $.ajax({
        url: 'GetRuleContent',
        type: 'POST',
        data: {
            accountID: accountID,
            applicationID: applicationID,
            catalogID: catalogID,
            variantID: variantID
        },
        dataType: 'json',
        success: function(data) {
            $('#ruleContent').html(data.HTML);
            $('#ruleContent tr:first').html('');
        },
        error: function(xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}


function deleteExpression(accountID, applicationID, variantID, ruleID, expressionID) {

    if (!confirm($("#ruleDeleteText").val())) {
        return;
    }

    // Need the ruleID
    ruleID = $("#ruleId").val();

    $.ajax({
        url: 'DeleteExpression',
        type: 'POST',
        data: {
            accountID: accountID,
            /*
            applicationID: applicationID,
            variantID: variantID,
            ruleID: ruleID,
            */
            expressionID: expressionID
        },
        dataType: 'json',
        success: function(data) {
            // Reload the rules grid
            getRulesHTML();
        },
        error: function(xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}

function showPopupRuleEditor(accountID, applicationID, catalogID, variantID, ruleID, expressionID) {
    //alert('popupruleeditor');
    var campaignID = $("#campaignId").val();
    ruleID = $("#ruleId").val();
    postMessageToInsight('displayRulesEditor|' + campaignID + '|' + accountID + '|' + applicationID + '|' + variantID + '|' + catalogID + '|' + ruleID + '|' + expressionID);
}

function changeRuleExpressionExecutionOrder(accountID, applicationID, variantID, expressionID, direction) {
    //alert('changeRuleExpressionExecutionOrder');
    var ruleID = $("#ruleId").val();

    $.ajax({
        url: 'ChangeRuleExpressionOrder',
        type: 'POST',
        data: {
            accountID: accountID,
            applicationID: applicationID,
            variantID: variantID,
            ruleID: ruleID,
            expressionID: expressionID,
            direction: direction
        },
        dataType: 'json',
        success: function(data) {
            // Reload the rules grid
            getRulesHTML();
        },
        error: function(xhr, status, error) {
            alert('an error occurred!\n\nstatus: ' + status + '\nerror: ' + error.name + '\nerror message: ' + error.message);
        }
    });
}

function expandExpression(variantID, expressionID) {
    $('#expression_' + variantID + '_' + expressionID).attr('style', 'width:600px;');

    $('#expCollapseButton_' + variantID + '_' + expressionID).css('display', 'block');
    $('#expExpandButton_' + variantID + '_' + expressionID).css('display', 'none');
}

function collapseExpression(variantID, expressionID) {
    $('#expression_' + variantID + '_' + expressionID).attr('style', 'width:600px;overflow:hidden;height:14px;');

    $('#expCollapseButton_' + variantID + '_' + expressionID).css('display', 'none');
    $('#expExpandButton_' + variantID + '_' + expressionID).css('display', 'block');
}

// Handles the switching of the Include Filtering radio buttons on step 1.

function includeFilteringHandler(checkedRadioButtonId) {
    switch (checkedRadioButtonId) {
    case 'filteringYes':
        $("#divUseRuleFiltering").attr("style", "display:inline");
        break;
    case 'filteringNo':
        //Hide the Email Location section.
        $("#divUseRuleFiltering").attr("style", "display:none");
        break;
    }
}