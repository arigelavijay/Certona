function CertonaWizard(Steps) {
    //Static variables
    var titleBarVisited = 'WizardTitleBarVisited';
    var titleBarCurrent = 'WizardTitleBarCurrent';
    var titleBarUnvisited = 'WizardTitleBarUnvisited';

    // Private variables
    var _step = 0;
    var _stepCount = Steps;
    var _stepDivs = new Array();
    var _stepTitles = new Array();
    var _mode = '';
    var _wizardVariableName = '';

    for (var i = 0; i < Steps; i++) {
        _stepDivs[i] = new Array();
        _stepDivs[i][0] = '';
        _stepTitles[i] = '';
    }
    ;

    // Public property functions
    this.Step = function(step) { // step is the logical step
        if (step != null && typeof step === 'number') {
            // Setting the step
            var changingArgs = {
                CurrentStep: _step + 1,
                NextStep: step,
                NextIsLast: step === _stepCount, // Logical
                NextIsFirst: step === 1
            };
            if (this.StepChanging(changingArgs) === false) // allow for validation to stop the change
                return null;

            _step = step - 1;
            this.Display();

            var changedArgs =
            {
                CurrentStep: _step + 1,
                IsLast: _step === _stepCount - 1,
                IsFirst: _step === 0
            };
            this.StepChanged(changedArgs);
        } else {
            // Getting the step
            return _step + 1;
        }
    };

    this.StepCount = function(stepCount) {
        if (stepCount != null && typeof stepCount === 'number') {
            // Setting the step count
            _stepCount = stepCount;
        } else {
            // Gettin g the step count
            return _stepCount;
        }
    };

    this.StepDivs = function(stepDivs) {
        if (stepDivs != null && stepDivs instanceof Array) {
            // Setting the step divs
            _stepDivs = stepDivs;
        } else {
            // Getting the step divs
            return _stepDivs;
        }
    };

    this.StepTitles = function(stepTitles) {
        if (stepTitles != null && stepTitles instanceof Array) {
            _stepTitles = stepTitles;
        } else {
            return _stepTitles;
        }
    };

    this.Mode = function(mode) {
        if (mode != null && typeof mode === 'string') {
            _mode = mode;
        } else {
            return _mode;
        }
    };

    this.WizardVariableName = function(wizardVariableName) {
        if (wizardVariableName != null && typeof wizardVariableName === 'string') {
            _wizardVariableName = wizardVariableName;
        } else {
            return _wizardVariableName;
        }
    };

    //Public event handlers
    this.StepChanging = function(args) { return true; };
    // args.CurrentStep int
    // args.NextStep int
    // args.NextIsLast bool
    // args.NextIsFirst bool

    this.StepChanged = function(args) {
    };
    // args.CurrentStep int
    // args.IsLast bool
    // args.IsFirst bool

    //Public methods
    this.Next = function() {
        if (_step != _stepCount - 1) {
            this.ChangeStep('next');
        }
    };

    this.Previous = function() {
        if (_step != 0) {
            this.ChangeStep('prev');
        }
    };

    this.ChangeStep = function(direction) {
        // Make sure direction is one of the two values we are expecting
        if (direction != 'next' && direction != 'prev') {
            return;
        }

        var changingArgs = new Object();
        changingArgs.CurrentStep = _step + 1; // Logical Current Step
        if (direction === 'next') {
            changingArgs.NextStep = _step + 2; // Logical Next Step
            changingArgs.NextIsLast = _step + 1 === _stepCount - 1; // Zero based 
            changingArgs.NextIsFirst = false; // Zero based
        } else {
            changingArgs.NextStep = _step;
            changingArgs.NextIsLast = false;
            changingArgs.NextIsFirst = _step - 1 === 0;
        }
        // Look into replacing javscript event delegates with
        // the .trigger() and .bind() jQuery methods
        // $(this).trigger('StepChanging', changingArgs);
        this.StepChanging(changingArgs);

        if (direction === 'next') {
            _step += 1;
        } else {
            _step -= 1;
        }

        this.Display();

        var changedArgs =
        {
            CurrentStep: _step + 1,
            IsLast: _step === _stepCount - 1,
            IsFirst: _step === 0
        };
        this.StepChanged(changedArgs);
    };

    this.Display = function() {
        for (var i = 0; i < _stepCount; i++) {
            for (var j = 0; j < _stepDivs[i].length; j++) {
                var div = $('#' + _stepDivs[i][j]);
                var titleBar = document.getElementById(_stepDivs[i][j] + '_TitleBar');
                var img;

                if (titleBar == null) {
                    titleBar = document.createElement('div');
                    titleBar.setAttribute('id', _stepDivs[i][j] + '_TitleBar');
                    div.before(titleBar);

                    titleBar = $('#' + _stepDivs[i][j] + '_TitleBar');

                    var stepSpan = document.createElement('span');
                    stepSpan.innerText = 'Step ' + (i + 1) + ' of ' + _stepCount + ' - ';
                    stepSpan.style.marginLeft = '6px';

                    var titleSpan = document.createElement('span');
                    titleSpan.style.fontWeight = 'bold';
                    titleSpan.innerText = _stepTitles[i];

                    titleBar.append(stepSpan);
                    titleBar.append(titleSpan);

                    var buttonSection = document.createElement('div');
                    buttonSection.setAttribute('class', 'buttonSection');

                    if (i < (_stepCount - 1)) {
                        var nextButton = document.createElement('button');
                        nextButton.innerText = $('#nextButtonText').val();
                        nextButton.setAttribute('id', 'Step' + (i + 1) + 'NextButton');
                        nextButton.setAttribute('class', 'nextButton');
                        nextButton.setAttribute('onclick', _wizardVariableName + '.Next()');
                        buttonSection.appendChild(nextButton);
                    }
                    if (i > 0) {
                        var previousButton = document.createElement('button');
                        previousButton.innerText = $('#previousButtonText').val();
                        previousButton.setAttribute('id', 'Step' + (i + 1) + 'PreviousButton');
                        previousButton.setAttribute('class', 'previousButton');
                        previousButton.setAttribute('onclick', _wizardVariableName + '.Previous()');
                        buttonSection.appendChild(previousButton);
                    }

                    div.append(buttonSection);

                    if (_mode === 'edit') {
                        titleBar.css('cursor', 'pointer');
                        titleBar.attr('onclick', _wizardVariableName + '.Step(' + (i + 1) + ')');

                        img = document.createElement('img');
                        img.setAttribute('id', _stepDivs[i][j] + '_TitleBarImage');
                        titleBar.prepend(img);
                        
                        img = $('#' + _stepDivs[i][j] + '_TitleBarImage');
                    }
                } else {
                    if (_mode === 'edit') {
                        img = $('#' + _stepDivs[i][j] + '_TitleBarImage');
                    }
                }

                titleBar = $('#' + _stepDivs[i][j] + '_TitleBar');

                if (i === _step) {
                    div.attr('class', 'ContentPanelSectionExpanded');
                    titleBar.attr('class', titleBarCurrent);
                    if (_mode === 'edit') {
                        img.attr('src', '/Images/imgExpanded.png');
                    }
                } else {
                    div.attr('class', 'ContentPanelSectionCollapsed');
                    if (_mode === 'edit') {
                        img.attr('src', '/Images/imgCollapsed.png');
                    }
                    if (i < _step) {
                        titleBar.attr('class', titleBarVisited);
                    } else {
                        titleBar.attr('class', titleBarUnvisited);
                    }
                }
            }
        }
    };
}

;