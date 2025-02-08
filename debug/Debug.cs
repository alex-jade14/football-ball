using Godot;

public partial class Debug : Control
{
    private MainBall _mainBall;
    private ShadowBall _shadowBall;
    private TabContainer _settingsContainer;
    private TabContainer _infoContainer;
    private MarginContainer _resetSimulationContainer;
    private Button _resetSimulationButton;
    private SpinBox _xInitialPositionBox;
    private SpinBox _yInitialPositionBox;
    private SpinBox _zInitialPositionBox;
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
    private ColorPickerButton _firstColorButton;
    private ColorPickerButton _secondColorButton;
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

    private void SetInteraction(TabContainer settingsContainer){
        VBoxContainer interactionContainer = (VBoxContainer) settingsContainer.GetNode("Interacción").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        HBoxContainer positionContainer = (HBoxContainer) interactionContainer.GetNode("PositionContainer");
        Vector3 globalPosition = _mainBall.GetGlobalPosition();
        _xInitialPositionBox = (SpinBox) positionContainer.GetNode("XInitialPositionBox");
        _xInitialPositionBox.SetValueNoSignal(globalPosition.X);
        Callable xInitialPositionValueChanged = new (this, MethodName.XInitialPositionValueChanged);
        _xInitialPositionBox.Connect(SpinBox.SignalName.ValueChanged, xInitialPositionValueChanged);
        
        _yInitialPositionBox = (SpinBox) positionContainer.GetNode("YInitialPositionBox");
        _yInitialPositionBox.SetValueNoSignal(globalPosition.Y);
        SetMinAndMaxRadiusValueForInitialYImpulseComponent();
        Callable yInitialPositionValueChanged = new (this, MethodName.YInitialPositionValueChanged);
        _yInitialPositionBox.Connect(SpinBox.SignalName.ValueChanged, yInitialPositionValueChanged);
        
        _zInitialPositionBox = (SpinBox) positionContainer.GetNode("ZInitialPositionBox");
        _zInitialPositionBox.SetValueNoSignal(globalPosition.Z);
        Callable zInitialPositionValueChanged = new (this, MethodName.ZInitialPositionValueChanged);
        _zInitialPositionBox.Connect(SpinBox.SignalName.ValueChanged, zInitialPositionValueChanged);
        
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
        _impulseFactorBox.SetValueNoSignal(4);
    }

    private void XInitialPositionValueChanged(float value){
        _mainBall.SetGlobalPosition(new Vector3(value, _mainBall.GetGlobalPosition().Y, _mainBall.GetGlobalPosition().Z));
        CameraOptionButtonItemSelected(_cameraOptionButton.GetSelectedId());
        CopyPropertiesToShadowBall();
    }

    private void YInitialPositionValueChanged(float value){
        _mainBall.SetGlobalPosition(new Vector3(_mainBall.GetGlobalPosition().X, value, _mainBall.GetGlobalPosition().Z));
        CameraOptionButtonItemSelected(_cameraOptionButton.GetSelectedId());
        CopyPropertiesToShadowBall();
    }

     private void ZInitialPositionValueChanged(float value){
        _mainBall.SetGlobalPosition(new Vector3(_mainBall.GetGlobalPosition().X, _mainBall.GetGlobalPosition().Y, value));
        CameraOptionButtonItemSelected(_cameraOptionButton.GetSelectedId());
        CopyPropertiesToShadowBall();
    }

    private void SetMinAndMaxRadiusValueForInitialImpulsePosition(){
        _xInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _xInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
    }

     private void SetMinAndMaxRadiusValueForInitialYImpulseComponent(){
        _yInitialPositionBox.SetMin(_mainBall.GetMeasurement().GetRadius());
        _yInitialPositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
    }

    private void SetParameters(TabContainer settingsContainer){
        VBoxContainer parametersContainer = (VBoxContainer) settingsContainer.GetNode("Parámetros").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        SpinBox massBox = (SpinBox) parametersContainer.GetNode("MassBox");
        massBox.SetMin(UnitsConverterHelper.ConvertKilogramToGram(BallParametersRanges.MinMassValue));
        massBox.SetMax(UnitsConverterHelper.ConvertKilogramToGram(BallParametersRanges.MaxMassValue));
        massBox.SetValueNoSignal(_mainBall.GetMeasurement().GetMassInGrams());
        Callable massBoxCallable = new (this, MethodName.MassBoxValueChanged);
        massBox.Connect(SpinBox.SignalName.ValueChanged, massBoxCallable);
        
        SpinBox circumferenceBox = (SpinBox) parametersContainer.GetNode("CircumferenceBox");
        circumferenceBox.SetMin(UnitsConverterHelper.ConvertMetersToCentimeters(BallParametersRanges.MinCircumferenceValue));
        circumferenceBox.SetMax(UnitsConverterHelper.ConvertMetersToCentimeters(BallParametersRanges.MaxCircumferenceValue));
        circumferenceBox.SetValueNoSignal(_mainBall.GetMeasurement().GetCircumferenceInCentimeters());
        Callable circumferenceBoxCollable = new (this, MethodName.CircumferenceBoxValueChanged);
        circumferenceBox.Connect(SpinBox.SignalName.ValueChanged, circumferenceBoxCollable);

        SpinBox coefficientOfRestitutionBox = (SpinBox) parametersContainer.GetNode("CoefficientOfRestitutionBox");
        coefficientOfRestitutionBox.SetMin(BallParametersRanges.MinCoefficientOfRestitutionValue);
        coefficientOfRestitutionBox.SetMax(BallParametersRanges.MaxCoefficientOfRestitutionValue);
        coefficientOfRestitutionBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetCoefficientOfRestitution());
        Callable coefficientOfRestitutionBoxValueChanged = new (this, MethodName.CoefficientOfRestitutionBoxValueChanged);
        coefficientOfRestitutionBox.Connect(SpinBox.SignalName.ValueChanged, coefficientOfRestitutionBoxValueChanged);
        
        SpinBox frictionCoefficientBox = (SpinBox) parametersContainer.GetNode("FrictionCoefficientBox");
        frictionCoefficientBox.SetMin(BallParametersRanges.MinFrictionCoefficientValue);
        frictionCoefficientBox.SetMax(BallParametersRanges.MaxFrictionCoefficientValue);
        frictionCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetFrictionCoefficient());
        Callable frictionCoefficientBoxValueChanged = new (this, MethodName.FrictionCoefficientBoxValueChanged);
        frictionCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, frictionCoefficientBoxValueChanged);

        SpinBox dragCoefficientBox = (SpinBox) parametersContainer.GetNode("DragCoefficientBox");
        dragCoefficientBox.SetMin(BallParametersRanges.MinDragCoefficientValue);
        dragCoefficientBox.SetMax(BallParametersRanges.MaxDragCoefficientValue);
        dragCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetDragCoefficient());
        Callable dragCoefficientBoxValueChanged = new (this, MethodName.DragCoefficientBoxValueChanged);
        dragCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, dragCoefficientBoxValueChanged);

        SpinBox liftCoefficientBox = (SpinBox) parametersContainer.GetNode("LiftCoefficientBox");
        liftCoefficientBox.SetMin(BallParametersRanges.MinLiftCoefficientValue);
        liftCoefficientBox.SetMax(BallParametersRanges.MaxLiftCoefficientValue);
        liftCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetLiftCoefficient());
        Callable liftCoefficientBoxValueChanged = new (this, MethodName.LiftCoefficientBoxValueChanged);
        liftCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, liftCoefficientBoxValueChanged);
        
        SpinBox angularDampingCoefficientBox = (SpinBox) parametersContainer.GetNode("AngularDampingCoefficientBox");
        angularDampingCoefficientBox.SetMin(BallParametersRanges.MinAngularDampingCoefficientValue);
        angularDampingCoefficientBox.SetMax(BallParametersRanges.MaxAngularDampingCoefficientValue);
        angularDampingCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetAngularDampingCoefficient());
        Callable angularDampingCoefficientBoxValueChanged = new (this, MethodName.AngularDampingCoefficientBoxValueChanged);
        angularDampingCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, angularDampingCoefficientBoxValueChanged);
    }

    private void SetAdditional(TabContainer settingsContainer){
        VBoxContainer additionalContainer = (VBoxContainer) settingsContainer.GetNode("Adicional").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        CheckButton airResistanceButton = (CheckButton) additionalContainer.GetNode("AirContainer").GetNode("AirResistanceButton");
        airResistanceButton.SetPressedNoSignal(true);
        Callable airResistanceButtonValueChanged = new (this, MethodName.AirResistanceButtonValueChanged);
        airResistanceButton.Connect(CheckBox.SignalName.Toggled, airResistanceButtonValueChanged);
        
        CheckButton magnusEffectButton = (CheckButton) additionalContainer.GetNode("MagnusContainer").GetNode("MagnusEffectButton");
        magnusEffectButton.SetPressedNoSignal(true);
        Callable magnusEffectButtonValueChanged = new (this, MethodName.MagnusEffectButtonValueChanged);
        magnusEffectButton.Connect(CheckBox.SignalName.Toggled, magnusEffectButtonValueChanged);
        
        SpinBox airDensityBox = (SpinBox) additionalContainer.GetNode("AirDensityBox");
        airDensityBox.SetMin(WorldEnvironmentParametersRanges.MinDensityOfFluidValue);
        airDensityBox.SetMax(WorldEnvironmentParametersRanges.MaxDensityOfFluidValue);
        airDensityBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid());
        Callable airDensityBoxValueChanged = new (this, MethodName.AirDensityBoxValueChanged);
        airDensityBox.Connect(SpinBox.SignalName.ValueChanged, airDensityBoxValueChanged);

       
        CheckButton shadowBallTrajectoryButton = (CheckButton) additionalContainer.GetNode("ShadowBallContainer").GetNode("ShadowBallTrajectoryButton");
        shadowBallTrajectoryButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
        Callable shadowBallTrajectoryButtonValueChanged = new (this, MethodName.ShadowBallTrajectoryButtonValueChanged);
        shadowBallTrajectoryButton.Connect(CheckBox.SignalName.Toggled, shadowBallTrajectoryButtonValueChanged);

        _cameraOptionButton = (OptionButton) additionalContainer.GetNode("CameraOptionButton");
        _cameraOptionButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
        Callable cameraOptionButtonItemSelected = new (this, MethodName.CameraOptionButtonItemSelected);
        _cameraOptionButton.Connect(OptionButton.SignalName.ItemSelected, cameraOptionButtonItemSelected);
    }

    private void SetModel(TabContainer settingsContainer){
        BoxContainer modelContainer = (VBoxContainer) settingsContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        _patternOptionButton = (OptionButton) modelContainer.GetNode("PatternOptionButton");
        _patternOptionButton.Select(0);
        Callable patternOptionItemSelected = new (this, MethodName.PatternOptionItemSelected);
        _patternOptionButton.Connect(OptionButton.SignalName.ItemSelected, patternOptionItemSelected);

        ColorPickerButton firstColorPickerButton = (ColorPickerButton) modelContainer.GetNode("FirstColorPickerButton");
        firstColorPickerButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        Callable firstColorPickerButtonValueChaned = new (this, MethodName.FirstColorPickerButtonValueChaned);
        firstColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, firstColorPickerButtonValueChaned);

        ColorPickerButton secondColorPickerButton = (ColorPickerButton) modelContainer.GetNode("SecondColorPickerButton");
        secondColorPickerButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
        Callable secondColorPickerButtonValueChaned = new (this, MethodName.SecondColorPickerButtonValueChaned);
        secondColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, secondColorPickerButtonValueChaned);
    }

    private void MassBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetMassInGrams(value);
        _mainBall.GetPhysicsParameters().SetNormalForce(_mainBall.GetMeasurement().GetMass());
        _mainBall.GetInfo().UpdateTerminalVelocity();
        CopyPropertiesToShadowBall();
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

    private void CircumferenceBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _mainBall.ScaleCollisionToRadius();
        _mainBall.ScaleMeshToRadius();
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
        SetMinAndMaxRadiusValueForInitialYImpulseComponent();
        _yInitialPositionBox.SetValue(_mainBall.GetMeasurement().GetRadius());
        CopyPropertiesToShadowBall();
        _circumferenceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _diameterLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetDiameterInCentimeters(), 2).ToString()
        );
        _radiusLabel.SetText(
             PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetRadiusInCentimeters(), 2).ToString()
        );
        _inertiaLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetMeasurement().GetInertia(), 4).ToString()
        );
        _crossSectionalAreaLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(
                _mainBall.GetMeasurement().GetCrossSectionalArea(), 4).ToString()
        );
    }

    private void CoefficientOfRestitutionBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetCoefficientOfRestitution(value);
        CopyPropertiesToShadowBall();
        _coefficientOfRestitutionLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    private void FrictionCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetFrictionCoefficient(value);
        CopyPropertiesToShadowBall();
        _frictionCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _frictionForceLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetFrictionForce(), 2).ToString()
        );
    }

    private void DragCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetDragCoefficient(value);
        CopyPropertiesToShadowBall();
        _dragCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _mainBall.GetInfo().UpdateTerminalVelocity();
    }

    private void LiftCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetLiftCoefficient(value);
        CopyPropertiesToShadowBall();
        _liftCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    private void AngularDampingCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetAngularDampingCoefficient(value);
        CopyPropertiesToShadowBall();
        _angularDampingCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
    }

    private void AirResistanceButtonValueChanged(bool onToggled){
        _mainBall.CanApplyAirResistance(onToggled);
        CopyPropertiesToShadowBall();
        _dragLabel.SetText(_mainBall.CanApplyAirResistance() ? "Sí" : "No");
    }

    private void MagnusEffectButtonValueChanged(bool onToggled){
        _mainBall.CanApplyMagnusEffect(onToggled);
        CopyPropertiesToShadowBall();
        _magnusEffectLabel.SetText(_mainBall.CanApplyMagnusEffect() ? "Sí" : "No");
    }

    private void AirDensityBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().GetEnvironment().SetDensityOfFluid(value);
        _mainBall.GetInfo().UpdateTerminalVelocity();
        CopyPropertiesToShadowBall();
        _angularDampingCoefficientLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(value, 2).ToString()
        );
        _terminalVelocityLabel.SetText(
            PrecisionHelper.ValueWithTruncatedDecimals(_mainBall.GetPhysicsParameters().GetTerminalVelocity(), 4).ToString()
        );
    }

    private void ShadowBallTrajectoryButtonValueChanged(bool onToggled){
        _shadowBall.CanShowItsTrajectory(onToggled);
    }

    private void CameraOptionButtonItemSelected(int index){
        switch(index){
            case 0:
                _mainBall.GetSubViewportCameraControl().Hide();
                _mainBall.GetMainCamera().SetCurrent(true);
                SetMainCamera("ball");
                break;
            case 1:
                _mainBall.GetSubViewportCameraControl().Show();
                _mainBall.CanFollowMainCamera(false);
                SetMainCamera("ball_trajectory");
                _mainBall.GetMainCamera().SetCurrent(true);
                break;
            case 2:
                _mainBall.GetSubViewportCameraControl().Hide();
                _mainBall.GetMainCamera().SetCurrent(false);
                break;
            case 3:
                _mainBall.GetSubViewportCameraControl().Show();
                SetMainCamera("trajectory_ball");
                _mainBall.CanFollowMainCamera(true);
                _mainBall.GetMainCamera().SetCurrent(false);
                break;
        }
    }

    private void SetMainCamera(string configuration){
        switch(configuration){
            case "ball":
                _mainBall.GetSubViewportCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 2,
                        _mainBall.GetGlobalPosition().Y + 1.5f,
                        _mainBall.GetGlobalPosition().Z - 3
                    )
                );
                _mainBall.GetMainCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 2,
                        _mainBall.GetGlobalPosition().Y + 1.5f,
                        _mainBall.GetGlobalPosition().Z - 3
                    )
                );
                _mainBall.GetSubViewportCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-15), Mathf.DegToRad(150), 0));
                _mainBall.GetMainCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-15), Mathf.DegToRad(150), 0));
                break;
            case "ball_trajectory":
                _mainBall.GetMainCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 2,
                        _mainBall.GetGlobalPosition().Y + 1.5f,
                        _mainBall.GetGlobalPosition().Z - 3
                    )
                );
                _mainBall.GetSubViewportCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 3,
                        _mainBall.GetGlobalPosition().Y + 1f,
                        _mainBall.GetGlobalPosition().Z
                    )
                );
                _mainBall.GetSubViewportCamera().SetGlobalRotation(new Vector3(0, Mathf.DegToRad(93.7f), 0));
                _mainBall.GetMainCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-15), Mathf.DegToRad(150), 0));
                break;
            case "trajectory_ball":
                _mainBall.GetSubViewportCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 1,
                        _mainBall.GetGlobalPosition().Y + 1,
                        _mainBall.GetGlobalPosition().Z - 1.5f
                    )
                );
                _mainBall.GetMainCamera().SetGlobalPosition(
                    new Vector3(
                        _mainBall.GetGlobalPosition().X + 1,
                        _mainBall.GetGlobalPosition().Y + 1,
                        _mainBall.GetGlobalPosition().Z - 1.5f
                    )
                );
                _mainBall.GetSubViewportCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-25), Mathf.DegToRad(150), 0));
                _mainBall.GetMainCamera().SetGlobalRotation(new Vector3(Mathf.DegToRad(-25), Mathf.DegToRad(150), 0));
                break;
        }
    }

    private void PatternOptionItemSelected(int index){
        _mainBall.GetModel().SetPattern(index == 0 ? "hexagon-pentagon" : "stars");
        _patternLabel.SetText(_patternOptionButton.GetItemText(index));
        _mainBall.ChangeMesh();
        _mainBall.ChangeColorToMesh();
    }

    private void FirstColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetFirstColor(color);
        _mainBall.ChangeColorToMesh();
    }

    private void SecondColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetSecondColor(color);
        _mainBall.ChangeColorToMesh();
    }


    private void CopyPropertiesToShadowBall(){
        _shadowBall = (ShadowBall) _mainBall.ShallowCopy(_shadowBall);
    }

    private void SetStartSimulationButton(){
        _startSimulationButton.Connect(Button.SignalName.Pressed, Callable.From(StartSimulationButtonPressed));
    }

    private void StartSimulationButtonPressed(){
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

    private void SetInteractionInfo(TabContainer infoContainer){
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

    private void UpdateInteractionInfo(){
        Vector3 globalPosition = _mainBall.GetGlobalPosition();
        float currentHeight = PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Y - _mainBall.GetMeasurement().GetRadius(), 1);
        float distanceFromOrigin = new Vector3(globalPosition.X, 0, globalPosition.Z).DistanceTo(new Vector3(0,0,0));
        globalPosition.X =  PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.X, 0);
        globalPosition.Y = Mathf.Abs(PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Y, 2));
        globalPosition.Z = PrecisionHelper.ValueWithTruncatedDecimals(globalPosition.Z, 0);
        _positionLabel.SetText(globalPosition.ToString());
        _distanceLabel.SetText(PrecisionHelper.ValueWithTruncatedDecimals(distanceFromOrigin, 2).ToString());
        _currentHeightLabel.SetText(currentHeight.ToString());
        
        Vector3 linearVelocity = _mainBall.GetLinearVelocity();
        float linearSpeed = PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Length(), 2);
        linearVelocity.X =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.X, 0);
        linearVelocity.Y =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Y, 0);
        linearVelocity.Z =  PrecisionHelper.ValueWithTruncatedDecimals(linearVelocity.Z, 0);
        _linearVelocityLabel.SetText(linearVelocity.ToString());
        _linearSpeedLabel.SetText(linearSpeed.ToString());
        
        Vector3 angularVelocity = _mainBall.GetAngularVelocity();
        float angularSpeed = PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Length(), 2);
        angularVelocity.X =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.X, 0);
        angularVelocity.Y =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Y, 0);
        angularVelocity.Z =  PrecisionHelper.ValueWithTruncatedDecimals(angularVelocity.Z, 0);
        _angularVelocityLabel.SetText(angularVelocity.ToString());
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

    private void SetParametersInfo(TabContainer infoContainer){
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

    private void SetAdditionalInfo(TabContainer infoContainer){
        VBoxContainer additionalContainer = (VBoxContainer) infoContainer.GetNode("Adicional").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        _airDensityLabel = (Label) additionalContainer.GetNode("AirDensityContainer").GetNode("AirDensityLabel");
        _airDensityLabel.SetText(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid().ToString());
        
        _gravityLabel = (Label) additionalContainer.GetNode("GravityContainer").GetNode("GravityLabel");
        _gravityLabel.SetText(PhysicsHelper.Gravity.ToString());
        
        _shadowBallTrajectoryLabel = (Label) additionalContainer.GetNode("ShadowBallTrajectoryContainer").GetNode("ShadowBallTrajectoryLabel");
        _shadowBallTrajectoryLabel.SetText(_shadowBall.CanShowItsTrajectory() ? "Sí" : "No");
    }

    private void SetModelInfo(TabContainer infoContainer){
        VBoxContainer modelContainer = (VBoxContainer) infoContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        _patternLabel = (Label) modelContainer.GetNode("PatternContainer").GetNode("PatternLabel");
        _patternLabel.SetText(_mainBall.GetModel().GetPattern().ToString() == "hexagon-pentagon" ? "Hexágonos y pentágonos" : "Estrellas");
        
        _firstColorButton = (ColorPickerButton) modelContainer.GetNode("FirstColorContainer").GetNode("FirstColorButton");
        _firstColorButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        
        _secondColorButton = (ColorPickerButton) modelContainer.GetNode("SecondColorContainer").GetNode("SecondColorButton");
        _secondColorButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
    }

    private void SetResetSimulationButton(){
        _resetSimulationButton.Connect(Button.SignalName.Pressed, Callable.From(ResetSimulationButtonPressed));
    }

    private void ResetSimulationButtonPressed(){
        _mainBall.SetLinearVelocity(new Vector3(0,0,0));
        _mainBall.SetAngularVelocity(new Vector3(0,0,0));
        _mainBall.SetGlobalPosition(new Vector3(
            (float) _xInitialPositionBox.GetValue(),
            (float) _yInitialPositionBox.GetValue(), 
            (float) _zInitialPositionBox.GetValue()));
        _mainBall.GetMesh().SetGlobalRotation(new Vector3(0,0,0));
        _shadowBall.GetPositionMarker().Hide();
        _shadowBall.GetDrawer().ClearMeshInstances();
        CopyPropertiesToShadowBall();
        _infoContainer.Hide();
        _resetSimulationContainer.Hide();
        _settingsContainer.Show();
        _startSimulationButton.Show();
    }
}
