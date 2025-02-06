using Godot;
using System;

public partial class Debug : Control
{
    private MainBall _mainBall;
    private ShadowBall _shadowBall;
    private TabContainer _settingsContainer;
    private TabContainer _infoContainer;
    private MarginContainer _resetSimulationContainer;
    private Button _resetSimulationButton;
    private SpinBox _yInitialPositionBox;
    private SpinBox _xInitialImpulseBox;
    private SpinBox _yInitialImpulseBox;
    private SpinBox _zInitialImpulseBox;
    private SpinBox _xInitialImpulsePositionBox;
    private SpinBox _yInitialImpulsePositionBox;
    private SpinBox _zInitialImpulsePositionBox;
    private SpinBox _impulseFactorBox;
    private Button _startSimulationButton;
    private OptionButton _patternOptionButton;
    private OptionButton _cameraOptionButton;
    private Label _positionLabel;
    private Label _distanceLabel;
    private Label _currentHeightLabel;
    private Label _linearVelocityLabel;
    private Label _angularVelocityLabel;
    private Label _linearSpeedLabel;
    private Label _angularSpeedLabel;
    private Label _detectedCollisionLabel;
    private Label _dragLabel;
    private Label _magnusEffectLabel;
    private Label _dragForceLabel;
    private Label _magnusEffectForceLabel;
    private Label _massLabel;
    private Label _inertiaLabel;
    private Label _circumferenceLabel;
    private Label _diameterLabel;
    private Label _radiusLabel;
    private Label _crossSectionalAreaLabel;
    private Label _coefficientOfRestitutionLabel;
    private Label _frictionCoefficientLabel;
    private Label _normalForceLabel;
    private Label _frictionForceLabel;
    private Label _dragCoefficientLabel;
    private Label _liftCoefficientLabel;
    private Label _terminalVelocityLabel;
    private Label _angularDampingCoefficientLabel;
    private Label _airDensityLabel;
    private Label _gravityLabel;
    private Label _shadowBallTrajectoryLabel;
    private Label _patternLabel;
    private ColorPickerButton _firstColorButton;
    private ColorPickerButton _secondColorButton;

    public void Create(MainBall mainBall, ShadowBall shadowBall){
        _mainBall = mainBall;
        _shadowBall = shadowBall;
    }

    public void SetInitialDebugData(){
        _settingsContainer = (TabContainer) GetNode("TabContainer");
        _startSimulationButton = (Button) GetNode("ButtonContainer").GetNode("StartSimulationButton");
        SetInteraction(_settingsContainer);
        SetParameters(_settingsContainer);
        SetAdditional(_settingsContainer);
        SetModel(_settingsContainer);
        SetStartSimulationButton();
        _infoContainer = (TabContainer) GetNode("TabContainer2");
        _resetSimulationContainer = (MarginContainer) GetNode("ButtonContainer2");
        _resetSimulationButton = (Button) GetNode("ButtonContainer2").GetNode("ResetSimulationButton");
        SetInteractionInfo(_infoContainer);
        SetParametersInfo(_infoContainer);
        SetAdditionalInfo(_infoContainer);
        SetModelInfo(_infoContainer);
        SetResetSimulationButton();
    }

    public void SetInteraction(TabContainer settingsContainer){
        VBoxContainer interactionContainer = (VBoxContainer) settingsContainer.GetNode("Interacción").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        HBoxContainer positionContainer = (HBoxContainer) interactionContainer.GetNode("PositionContainer");
        Vector3 globalPosition = _mainBall.GetGlobalPosition();
        SpinBox xInitialPositionBox = (SpinBox) positionContainer.GetNode("XInitialPositionBox");
        _yInitialPositionBox = (SpinBox) positionContainer.GetNode("YInitialPositionBox");
        SpinBox zInitialPositionBox = (SpinBox) positionContainer.GetNode("ZInitialPositionBox");
        xInitialPositionBox.SetValueNoSignal(globalPosition.X);
        _yInitialPositionBox.SetValueNoSignal(globalPosition.Y);
        SetMinRadiusValueForInitialYImpulseComponent();
        zInitialPositionBox.SetValueNoSignal(globalPosition.Z);
        
        HBoxContainer impulseContainer = (HBoxContainer) interactionContainer.GetNode("ImpulseContainer");
        _xInitialImpulseBox = (SpinBox) impulseContainer.GetNode("XInitialImpulseBox");
        _yInitialImpulseBox = (SpinBox) impulseContainer.GetNode("YInitialImpulseBox");
        _zInitialImpulseBox = (SpinBox) impulseContainer.GetNode("ZInitialImpulseBox");
        _xInitialImpulseBox.SetValueNoSignal(-10);
        _yInitialImpulseBox.SetValueNoSignal(5);
        _zInitialImpulseBox.SetValueNoSignal(10);
        
        HBoxContainer impulsePositionContainer = (HBoxContainer) interactionContainer.GetNode("ImpulsePositionContainer");
        _xInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("XInitialImpulsePositionBox");
        _yInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("YInitialImpulsePositionBox");
        _zInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("ZInitialImpulsePositionBox");
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
        _xInitialImpulsePositionBox.SetValueNoSignal(0);
        _yInitialImpulsePositionBox.SetValueNoSignal(-_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetValueNoSignal(_mainBall.GetMeasurement().GetRadius());

        HBoxContainer impulseFactorontainer = (HBoxContainer) interactionContainer.GetNode("ImpulseFactorContainer");
        _impulseFactorBox = (SpinBox) impulseFactorontainer.GetNode("ImpulseFactorBox");
        _impulseFactorBox.SetValueNoSignal(2);
    }

    public void SetMinAndMaxRadiusValueForInitialImpulsePosition(){
        _xInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _xInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
    }

     public void SetMinRadiusValueForInitialYImpulseComponent(){
        _yInitialPositionBox.SetMin(_mainBall.GetMeasurement().GetRadius());
    }

    public void SetParameters(TabContainer settingsContainer){
        VBoxContainer parametersContainer = (VBoxContainer) settingsContainer.GetNode("Parámetros").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        SpinBox massBox = (SpinBox) parametersContainer.GetNode("MassBox");
        massBox.SetMin(UnitsConverterHelper.ConvertKilogramToGram(BallParametersRanges.minMassValue));
        massBox.SetMax(UnitsConverterHelper.ConvertKilogramToGram(BallParametersRanges.maxMassValue));
        massBox.SetValueNoSignal(_mainBall.GetMeasurement().GetMassInGrams());
        Callable massBoxCallable = new (this, MethodName.MassBoxValueChanged);
        massBox.Connect(SpinBox.SignalName.ValueChanged, massBoxCallable);
        
        SpinBox circumferenceBox = (SpinBox) parametersContainer.GetNode("CircumferenceBox");
        circumferenceBox.SetMin(UnitsConverterHelper.ConvertMetersToCentimeters(BallParametersRanges.minCircumferenceValue));
        circumferenceBox.SetMax(UnitsConverterHelper.ConvertMetersToCentimeters(BallParametersRanges.maxCircumferenceValue));
        circumferenceBox.SetValueNoSignal(_mainBall.GetMeasurement().GetCircumferenceInCentimeters());
        Callable circumferenceBoxCollable = new (this, MethodName.CircumferenceBoxValueChanged);
        circumferenceBox.Connect(SpinBox.SignalName.ValueChanged, circumferenceBoxCollable);

        SpinBox coefficientOfRestitutionBox = (SpinBox) parametersContainer.GetNode("CoefficientOfRestitutionBox");
        coefficientOfRestitutionBox.SetMin(BallParametersRanges.minCoefficientOfRestitutionValue);
        coefficientOfRestitutionBox.SetMax(BallParametersRanges.maxCoefficientOfRestitutionValue);
        coefficientOfRestitutionBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetCoefficientOfRestitution());
        Callable coefficientOfRestitutionBoxValueChanged = new (this, MethodName.coefficientOfRestitutionBoxValueChanged);
        coefficientOfRestitutionBox.Connect(SpinBox.SignalName.ValueChanged, coefficientOfRestitutionBoxValueChanged);
        
        SpinBox frictionCoefficientBox = (SpinBox) parametersContainer.GetNode("FrictionCoefficientBox");
        frictionCoefficientBox.SetMin(BallParametersRanges.minFrictionCoefficientValue);
        frictionCoefficientBox.SetMax(BallParametersRanges.maxFrictionCoefficientValue);
        frictionCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetFrictionCoefficient());
        Callable frictionCoefficientBoxValueChanged = new (this, MethodName.frictionCoefficientBoxValueChanged);
        frictionCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, frictionCoefficientBoxValueChanged);

        SpinBox dragCoefficientBox = (SpinBox) parametersContainer.GetNode("DragCoefficientBox");
        dragCoefficientBox.SetMin(BallParametersRanges.minDragCoefficientValue);
        dragCoefficientBox.SetMax(BallParametersRanges.maxDragCoefficientValue);
        dragCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetDragCoefficient());
        Callable dragCoefficientBoxValueChanged = new (this, MethodName.dragCoefficientBoxValueChanged);
        dragCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, dragCoefficientBoxValueChanged);

        SpinBox liftCoefficientBox = (SpinBox) parametersContainer.GetNode("LiftCoefficientBox");
        liftCoefficientBox.SetMin(BallParametersRanges.minLiftCoefficientValue);
        liftCoefficientBox.SetMax(BallParametersRanges.maxLiftCoefficientValue);
        liftCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetLiftCoefficient());
        Callable liftCoefficientBoxValueChanged = new (this, MethodName.liftCoefficientBoxValueChanged);
        liftCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, liftCoefficientBoxValueChanged);
        
        SpinBox angularDampingCoefficientBox = (SpinBox) parametersContainer.GetNode("AngularDampingCoefficientBox");
        angularDampingCoefficientBox.SetMin(BallParametersRanges.minAngularDampingCoefficientValue);
        angularDampingCoefficientBox.SetMax(BallParametersRanges.maxAngularDampingCoefficientValue);
        angularDampingCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetAngularDampingCoefficient());
        Callable angularDampingCoefficientBoxValueChanged = new (this, MethodName.angularDampingCoefficientBoxValueChanged);
        angularDampingCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, angularDampingCoefficientBoxValueChanged);
    }

    public void SetAdditional(TabContainer settingsContainer){
        VBoxContainer additionalContainer = (VBoxContainer) settingsContainer.GetNode("Adicional").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        CheckButton airResistanceButton = (CheckButton) additionalContainer.GetNode("AirContainer").GetNode("AirResistanceButton");
        airResistanceButton.SetPressedNoSignal(true);
        Callable airResistanceButtonValueChanged = new (this, MethodName.airResistanceButtonValueChanged);
        airResistanceButton.Connect(CheckBox.SignalName.Toggled, airResistanceButtonValueChanged);
        
        CheckButton magnusEffectButton = (CheckButton) additionalContainer.GetNode("MagnusContainer").GetNode("MagnusEffectButton");
        magnusEffectButton.SetPressedNoSignal(true);
        Callable magnusEffectButtonValueChanged = new (this, MethodName.magnusEffectButtonValueChanged);
        magnusEffectButton.Connect(CheckBox.SignalName.Toggled, magnusEffectButtonValueChanged);
        
        SpinBox airDensityBox = (SpinBox) additionalContainer.GetNode("AirDensityBox");
        airDensityBox.SetMin(WorldEnvironmentParametersRanges.minDensityOfFluidValue);
        airDensityBox.SetMax(WorldEnvironmentParametersRanges.maxDensityOfFluidValue);
        airDensityBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid());
        Callable airDensityBoxValueChanged = new (this, MethodName.airDensityBoxValueChanged);
        airDensityBox.Connect(SpinBox.SignalName.ValueChanged, airDensityBoxValueChanged);

       
        CheckButton shadowBallTrajectoryButton = (CheckButton) additionalContainer.GetNode("ShadowBallContainer").GetNode("ShadowBallTrajectoryButton");
        shadowBallTrajectoryButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
        Callable shadowBallTrajectoryButtonValueChanged = new (this, MethodName.shadowBallTrajectoryButtonValueChanged);
        shadowBallTrajectoryButton.Connect(CheckBox.SignalName.Toggled, shadowBallTrajectoryButtonValueChanged);

        _cameraOptionButton = (OptionButton) additionalContainer.GetNode("CameraOptionButton");
        _cameraOptionButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
        Callable cameraOptionButtonItemSelected = new (this, MethodName.cameraOptionButtonItemSelected);
        _cameraOptionButton.Connect(OptionButton.SignalName.ItemSelected, cameraOptionButtonItemSelected);
    }

    public void SetModel(TabContainer settingsContainer){
        BoxContainer modelContainer = (VBoxContainer) settingsContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        _patternOptionButton = (OptionButton) modelContainer.GetNode("PatternOptionButton");
        _patternOptionButton.Select(0);
        Callable patternOptionItemSelected = new (this, MethodName.patternOptionItemSelected);
        _patternOptionButton.Connect(OptionButton.SignalName.ItemSelected, patternOptionItemSelected);

        
        ColorPickerButton firstColorPickerButton = (ColorPickerButton) modelContainer.GetNode("FirstColorPickerButton");
        firstColorPickerButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        Callable firstColorPickerButtonValueChaned = new (this, MethodName.firstColorPickerButtonValueChaned);
        firstColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, firstColorPickerButtonValueChaned);

        ColorPickerButton secondColorPickerButton = (ColorPickerButton) modelContainer.GetNode("SecondColorPickerButton");
        secondColorPickerButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
        Callable secondColorPickerButtonValueChaned = new (this, MethodName.secondColorPickerButtonValueChaned);
        secondColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, secondColorPickerButtonValueChaned);
    }

    public void MassBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetMassInGrams(value);
        _mainBall.GetPhysicsParameters().SetNormalForce(_mainBall.GetMeasurement().GetMass());
        _mainBall.GetPhysicsParameters().UpdateTerminalVelocity(
            _mainBall.GetMeasurement().GetCrossSectionalArea(),
            _mainBall.GetMeasurement().GetMass()
        );
        copyPropertiesToShadowBall();
        _massLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _inertiaLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetInertia(), 4).ToString()
        );
        _normalForceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetNormalForce(), 2).ToString()
        );
        _frictionForceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetFrictionForce(), 2).ToString()
        );
        _terminalVelocityLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetTerminalVelocity(), 2).ToString()
        );
    }

    public void CircumferenceBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _mainBall.ScaleCollisionToRadius();
        _mainBall.ScaleMeshToRadius();
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
        SetMinRadiusValueForInitialYImpulseComponent();
        _yInitialPositionBox.SetValue(_mainBall.GetMeasurement().GetRadius());
        copyPropertiesToShadowBall();
        _circumferenceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _diameterLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetDiameterInCentimeters(), 2
            ).ToString()
        );
        _radiusLabel.SetText(
             PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetRadiusInCentimeters(), 2
            ).ToString()
        );
        _inertiaLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetInertia(), 4).ToString()
        );
        _crossSectionalAreaLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetCrossSectionalArea(), 4
            ).ToString()
        );
    }

    public void coefficientOfRestitutionBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetCoefficientOfRestitution(value);
        copyPropertiesToShadowBall();
        _coefficientOfRestitutionLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    public void frictionCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetFrictionCoefficient(value);
        copyPropertiesToShadowBall();
        _frictionCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _frictionForceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetFrictionForce(), 2).ToString()
        );
    }

    public void dragCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetDragCoefficient(value);
        copyPropertiesToShadowBall();
        _dragCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _mainBall.GetPhysicsParameters().UpdateTerminalVelocity(
            _mainBall.GetMeasurement().GetCrossSectionalArea(),
            _mainBall.GetMeasurement().GetMass()
        );
    }

    public void liftCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetLiftCoefficient(value);
        copyPropertiesToShadowBall();
        _liftCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    public void angularDampingCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetAngularDampingCoefficient(value);
        copyPropertiesToShadowBall();
        _angularDampingCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    public void airResistanceButtonValueChanged(bool onToggled){
        _mainBall.CanApplyAirResistance(onToggled);
        copyPropertiesToShadowBall();
        _dragLabel.SetText(_mainBall.CanApplyAirResistance() ? "Sí" : "No");
    }

    public void magnusEffectButtonValueChanged(bool onToggled){
        _mainBall.CanApplyMagnusEffect(onToggled);
        copyPropertiesToShadowBall();
        _magnusEffectLabel.SetText(_mainBall.CanApplyMagnusEffect() ? "Sí" : "No");
    }

    public void airDensityBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().GetEnvironment().SetDensityOfFluid(value);
        _mainBall.GetPhysicsParameters().UpdateTerminalVelocity(
            _mainBall.GetMeasurement().GetCrossSectionalArea(),
            _mainBall.GetMeasurement().GetMass()
        );
        copyPropertiesToShadowBall();
        _angularDampingCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _terminalVelocityLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetTerminalVelocity(), 4).ToString()
        );
    }

    public void shadowBallTrajectoryButtonValueChanged(bool onToggled){
        _shadowBall.CanShowItsTrajectory(onToggled);
    }

    public void cameraOptionButtonItemSelected(int index){
        switch(index){
            case 0:
                _mainBall.GetViewportCameraControl().Hide();
                _mainBall.GetMainCamera().SetCurrent(true);
                break;
            case 1:
                _mainBall.GetViewportCameraControl().Show();
                _mainBall.CanFollowViewportCameraGuide(false);
                _mainBall.GetViewportCamera().SetGlobalPosition(new Vector3(3, 1.5f, 0f));
                _mainBall.GetViewportCamera().SetGlobalRotation(new Vector3(0, Mathf.DegToRad(93.7f), 0));
                GD.Print(_mainBall.GetViewportCamera().GetGlobalPosition());
                _mainBall.GetMainCamera().SetCurrent(true);
                break;
            case 2:
                _mainBall.GetViewportCameraControl().Hide();
                _mainBall.GetMainCamera().SetCurrent(false);
                break;
            case 3:
                _mainBall.GetViewportCameraControl().Show();
                _mainBall.GetViewportCamera().SetGlobalPosition(new Vector3(1, 1, -1.5f));
                _mainBall.GetViewportCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-25), Mathf.DegToRad(150), 0));
                _mainBall.CanFollowViewportCameraGuide(true);
                _mainBall.GetMainCamera().SetCurrent(false);
                break;
        }
    }

    public void patternOptionItemSelected(int index){
        _mainBall.GetModel().SetPattern(index == 0 ? "hexagon-pentagon" : "stars");
        _patternLabel.SetText(_patternOptionButton.GetItemText(index));
        _mainBall.ChangeMesh();
        _mainBall.ChangeColorToMesh();
    }

    public void firstColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetFirstColor(color);
        _mainBall.ChangeColorToMesh();
    }

    public void secondColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetSecondColor(color);
        _mainBall.ChangeColorToMesh();
    }


    public void copyPropertiesToShadowBall(){
        _shadowBall = (ShadowBall) _mainBall.ShallowCopy(_shadowBall);
    }

    public void SetStartSimulationButton(){
        _startSimulationButton.Connect(Button.SignalName.Pressed, Callable.From(StartSimulationButtonPressed));
    }

    public void StartSimulationButtonPressed(){
        _settingsContainer.Hide();
        _startSimulationButton.Hide();
        _infoContainer.Show();
        _resetSimulationContainer.Show();
        _mainBall.ApplyImpulse(
            new Vector3(
                (float) _xInitialImpulseBox.GetValue(),
                (float) _yInitialImpulseBox.GetValue(),
                (float) _zInitialImpulseBox.GetValue()
            ),
            new Vector3(
                (float) _xInitialImpulsePositionBox.GetValue(),
                (float) _yInitialImpulsePositionBox.GetValue(),
                (float) _zInitialImpulsePositionBox.GetValue()
            ) * (float) _impulseFactorBox.GetValue()
        );
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateInteractionInfo();
    }

    public void SetInteractionInfo(TabContainer infoContainer){
        VBoxContainer interactionContainer = (VBoxContainer) infoContainer.GetNode("Interacción").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        _positionLabel = (Label) interactionContainer.GetNode("PositionContainer").GetNode("PositionLabel");
        _distanceLabel = (Label) interactionContainer.GetNode("DistanceContainer").GetNode("DistanceLabel");
        _currentHeightLabel = (Label) interactionContainer.GetNode("CurrentHeightContainer").GetNode("CurrentHeightLabel");
        _linearVelocityLabel = (Label) interactionContainer.GetNode("LinearVelocityContainer").GetNode("LinearVelocityLabel");
        _angularVelocityLabel = (Label) interactionContainer.GetNode("AngularVelocityContainer").GetNode("AngularVelocityLabel");
        _linearSpeedLabel = (Label) interactionContainer.GetNode("LinearSpeedContainer").GetNode("LinearSpeedLabel");
        _angularSpeedLabel = (Label) interactionContainer.GetNode("AngularSpeedContainer").GetNode("AngularSpeedLabel");
        _detectedCollisionLabel = (Label) interactionContainer.GetNode("DetectedCollisionContainer").GetNode("DetectedCollisionLabel");
        _dragLabel = (Label) interactionContainer.GetNode("DragContainer").GetNode("DragLabel");
        _dragLabel.SetText(_mainBall.CanApplyAirResistance() ? "Sí" : "No");
        _magnusEffectLabel = (Label) interactionContainer.GetNode("MagnusEffectContainer").GetNode("MagnusEffectLabel");
        _magnusEffectLabel.SetText(_mainBall.CanApplyMagnusEffect() ? "Sí" : "No");
        _dragForceLabel = (Label) interactionContainer.GetNode("DragForceContainer").GetNode("DragForceLabel");
        _magnusEffectForceLabel = (Label) interactionContainer.GetNode("MagnusEffectForceContainer").GetNode("MagnusEffectForceLabel");
    }

    public void UpdateInteractionInfo(){
        Vector3 globalPosition = _mainBall.GetGlobalPosition();
        globalPosition.X =  PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.X, 0);
        float currentHeight = PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Y - _mainBall.GetMeasurement().GetRadius(), 1);
        globalPosition.Y = Mathf.Abs(PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Y, 2));
        globalPosition.Z = PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Z, 0);
        _positionLabel.SetText(globalPosition.ToString());
        _distanceLabel.SetText(PrecisionHelper.ValueWithTruncatedDecimals(new Vector3(globalPosition.X, 0, globalPosition.Z).DistanceTo(new Vector3(0,0,0)), 2).ToString());
        _currentHeightLabel.SetText(currentHeight.ToString());
        Vector3 linearVelocity = _mainBall.GetLinearVelocity();
        float linearSpeed = PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Length(), 2);
        linearVelocity.X =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.X, 0);
        linearVelocity.Y =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Y, 0);
        linearVelocity.Z =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Z, 0);
        _linearVelocityLabel.SetText(linearVelocity.ToString());
        Vector3 angularVelocity = _mainBall.GetAngularVelocity();
        float angularSpeed = PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Length(), 2);
        angularVelocity.X =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.X, 0);
        angularVelocity.Y =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Y, 0);
        angularVelocity.Z =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Z, 0);
        _angularVelocityLabel.SetText(angularVelocity.ToString());
        _linearSpeedLabel.SetText(linearSpeed.ToString());
        _angularSpeedLabel.SetText(angularSpeed.ToString());
        _detectedCollisionLabel.SetText(_mainBall.IsACollisionDetected() ? "Sí" : "No");
        Vector3 dragForce = _mainBall.GetPhysicsParameters().GetDragForce();
        dragForce.X =  PrecisionHelper.ValueWithTruncatedDecimals(dragForce.X, 0);
        dragForce.Y =  PrecisionHelper.ValueWithTruncatedDecimals(dragForce.Y, 0);
        dragForce.Z =  PrecisionHelper.ValueWithTruncatedDecimals(dragForce.Z, 0);
        _dragForceLabel.SetText(dragForce.ToString());
        Vector3 magnusEffectForce = _mainBall.GetPhysicsParameters().GetMagnusEffectForce();
        magnusEffectForce.X =  PrecisionHelper.ValueWithTruncatedDecimals(magnusEffectForce.X, 0);
        magnusEffectForce.Y =  PrecisionHelper.ValueWithTruncatedDecimals(magnusEffectForce.Y, 0);
        magnusEffectForce.Z =  PrecisionHelper.ValueWithTruncatedDecimals(magnusEffectForce.Z, 0);
        _magnusEffectForceLabel.SetText(magnusEffectForce.ToString());
    }

    public void SetParametersInfo(TabContainer infoContainer){
        VBoxContainer parametersContainer = (VBoxContainer) infoContainer.GetNode("Parámetros").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        _massLabel = (Label) parametersContainer.GetNode("MassContainer").GetNode("MassLabel");
        _massLabel.SetText(_mainBall.GetMeasurement().GetMassInGrams().ToString());
        _inertiaLabel = (Label) parametersContainer.GetNode("InertiaContainer").GetNode("InertiaLabel");
        float inertia = PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetInertia(), 4);
        _inertiaLabel.SetText(inertia.ToString());
        _circumferenceLabel = (Label) parametersContainer.GetNode("CircumferenceContainer").GetNode("CircumferenceLabel");
        _circumferenceLabel.SetText(_mainBall.GetMeasurement().GetCircumferenceInCentimeters().ToString());
        _diameterLabel = (Label) parametersContainer.GetNode("DiameterContainer").GetNode("DiameterLabel");
        float diameter = PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetDiameterInCentimeters(), 2);
        _diameterLabel.SetText(diameter.ToString());
        _radiusLabel = (Label) parametersContainer.GetNode("RadiusContainer").GetNode("RadiusLabel");
        float radius = PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetRadiusInCentimeters(), 2);
        _radiusLabel.SetText(radius.ToString());
        _crossSectionalAreaLabel = (Label) parametersContainer.GetNode("CrossSectionalAreaContainer").GetNode("CrossSectionalAreaLabel");
        float crossSectionalArea = PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetCrossSectionalArea(), 4);
        _crossSectionalAreaLabel.SetText(crossSectionalArea.ToString());
        _coefficientOfRestitutionLabel = (Label) parametersContainer.GetNode("CoefficientOfRestitutionContainer").GetNode("CoefficientOfRestitutionLabel");
        _coefficientOfRestitutionLabel.SetText(_mainBall.GetPhysicsParameters().GetCoefficientOfRestitution().ToString());
        _frictionCoefficientLabel = (Label) parametersContainer.GetNode("FrictionCoefficientContainer").GetNode("FrictionCoefficientLabel");
        _frictionCoefficientLabel.SetText(_mainBall.GetPhysicsParameters().GetFrictionCoefficient().ToString());
        _normalForceLabel = (Label) parametersContainer.GetNode("NormalForceContainer").GetNode("NormalForceLabel");
        _normalForceLabel.SetText(_mainBall.GetPhysicsParameters().GetNormalForce().ToString());
        _frictionForceLabel = (Label) parametersContainer.GetNode("FrictionForceContainer").GetNode("FrictionForceLabel");
        _frictionForceLabel.SetText(_mainBall.GetPhysicsParameters().GetFrictionForce().ToString());
        _dragCoefficientLabel = (Label) parametersContainer.GetNode("DragCoefficientContainer").GetNode("DragCoefficientLabel");
        _dragCoefficientLabel.SetText(_mainBall.GetPhysicsParameters().GetDragCoefficient().ToString());
        _liftCoefficientLabel = (Label) parametersContainer.GetNode("LiftCoefficientContainer").GetNode("LiftCoefficientLabel");
        _liftCoefficientLabel.SetText(_mainBall.GetPhysicsParameters().GetLiftCoefficient().ToString());
        _terminalVelocityLabel = (Label) parametersContainer.GetNode("TerminalVelocityContainer").GetNode("TerminalVelocityLabel");
        float terminalVelocity = PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetTerminalVelocity(), 4);
        _terminalVelocityLabel.SetText(terminalVelocity.ToString());
        _angularDampingCoefficientLabel = (Label) parametersContainer.GetNode("AngularDampingCoefficientContainer").GetNode("AngularDampingCoefficientLabel");
        _angularDampingCoefficientLabel.SetText(_mainBall.GetPhysicsParameters().GetAngularDampingCoefficient().ToString());
    }

    public void SetAdditionalInfo(TabContainer infoContainer){
        VBoxContainer additionalContainer = (VBoxContainer) infoContainer.GetNode("Adicional").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        _airDensityLabel = (Label) additionalContainer.GetNode("AirDensityContainer").GetNode("AirDensityLabel");
        _airDensityLabel.SetText(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid().ToString());
        _gravityLabel = (Label) additionalContainer.GetNode("GravityContainer").GetNode("GravityLabel");
        _gravityLabel.SetText(PhysicsHelper.gravity.ToString());
        _shadowBallTrajectoryLabel = (Label) additionalContainer.GetNode("ShadowBallTrajectoryContainer").GetNode("ShadowBallTrajectoryLabel");
        _shadowBallTrajectoryLabel.SetText(_shadowBall.CanShowItsTrajectory() ? "Sí" : "No");
    }

    public void SetModelInfo(TabContainer infoContainer){
        VBoxContainer modelContainer = (VBoxContainer) infoContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        _patternLabel = (Label) modelContainer.GetNode("PatternContainer").GetNode("PatternLabel");
        _patternLabel.SetText(_mainBall.GetModel().GetPattern().ToString() == "hexagon-pentagon" ? "Hexágonos y pentágonos" : "Estrellas");
        _firstColorButton = (ColorPickerButton) modelContainer.GetNode("FirstColorContainer").GetNode("FirstColorButton");
        _firstColorButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        _secondColorButton = (ColorPickerButton) modelContainer.GetNode("SecondColorContainer").GetNode("SecondColorButton");
        _secondColorButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
    }

    public void SetResetSimulationButton(){
        _resetSimulationButton.Connect(Button.SignalName.Pressed, Callable.From(ResetSimulationButtonPressed));
    }

    public void ResetSimulationButtonPressed(){
        _mainBall.SetLinearVelocity(new Vector3(0,0,0));
        _mainBall.SetAngularVelocity(new Vector3(0,0,0));
        _mainBall.SetGlobalPosition(new Vector3(0, _mainBall.GetMeasurement().GetRadius(), 0));
        _mainBall.GetMesh().SetGlobalRotation(new Vector3(0,0,0));
        _shadowBall._positionMarker.Hide();
        _shadowBall.GetDrawer().ClearMeshInstances();
        copyPropertiesToShadowBall();
        _infoContainer.Hide();
        _resetSimulationContainer.Hide();
        _settingsContainer.Show();
        _startSimulationButton.Show();
    }


}
