var myWizard;

($(function() {
    if (typeof myWizard === 'undefined') {
        myWizard = new CertonaWizard(4);
    }
    myWizard.StepDivs([['step1'], ['step2'], ['step3'], ['step4']]);
    myWizard.StepTitles(['Step 1 Title', 'Step 2 Title', 'Step 3 Title', 'Step 4 Title']);
    myWizard.Mode(wizardMode);
    myWizard.WizardVariableName('myWizard');
    myWizard.StepChanged = StepChanged; //Fires after the step has changed
    myWizard.StepChanging = StepChanging; //Fired before the step changes
    myWizard.Display();
}));

function StepChanging(args) {
    //Fired before the step changes
    //if (args.CurrentStep === 1) {
    //    var form = $('#wizardForm');
    //    if (!form.valid()) {
    //        return false;
    //    }
    //}
    return true;
}

function StepChanged(args) {
    //Fires after the step has changed
    //validateFieldsForWizardConfirmation();
    //displayWizardSteps();
    //if (args.CurrentStep === 3) {
    //    $("#Step3ViewModel_SelectedSchemeId").change();
    //}
}