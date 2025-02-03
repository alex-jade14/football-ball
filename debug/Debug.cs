using Godot;
using System;

public partial class Debug : Control
{
    private MainBall _mainBall;
    private ShadowBall _shadowBall;
    private TabContainer _settingsContainer;
    private TabContainer _infoContainer;
    private SpinBox _yInitialPositionBox;
    private SpinBox _xInitialImpulseBox;
    private SpinBox _yInitialImpulseBox;
    private SpinBox _zInitialImpulseBox;
    private SpinBox _xInitialImpulsePositionBox;
    private SpinBox _yInitialImpulsePositionBox;
    private SpinBox _zInitialImpulsePositionBox;
    private SpinBox _impulseFactorBox;
    private Button _startSimulationButton;
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
    private Label _crossSectionalArea;
    private Label _coefficientOfRestitution;
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
    private ColorPickerButton _thirdColorButton;

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
        SetInteractionInfo(_infoContainer);
        SetParametersInfo(_infoContainer);
        SetAdditionalInfo(_infoContainer);
        SetModelInfo(_infoContainer);
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
        massBox.SetValueNoSignal(_mainBall.GetMeasurement().GetMassInGrams());
        Callable massBoxCallable = new (this, MethodName.MassBoxValueChanged);
        massBox.Connect(SpinBox.SignalName.ValueChanged, massBoxCallable);
        SpinBox circumferenceBox = (SpinBox) parametersContainer.GetNode("CircumferenceBox");
        circumferenceBox.SetValueNoSignal(_mainBall.GetMeasurement().GetCircumferenceInCentimeters());
        Callable circumferenceBoxCollable = new (this, MethodName.CircumferenceBoxValueChanged);
        circumferenceBox.Connect(SpinBox.SignalName.ValueChanged, circumferenceBoxCollable);

        SpinBox coefficientOfRestitutionBox = (SpinBox) parametersContainer.GetNode("CoefficientOfRestitutionBox");
        coefficientOfRestitutionBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetCoefficientOfRestitution());
        Callable coefficientBoxValueChanged = new (this, MethodName.coefficientBoxValueChanged);
        coefficientOfRestitutionBox.Connect(SpinBox.SignalName.ValueChanged, coefficientBoxValueChanged);
        
        SpinBox rotationalCoefficientOfRestitutionBox = (SpinBox) parametersContainer.GetNode("RotationalCoefficientOfRestitutionBox");
        rotationalCoefficientOfRestitutionBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetRotationalCoefficientOfRestitution());
        Callable rotationalCoefficientBoxValueChanged = new (this, MethodName.rotationalCoefficientBoxValueChanged);
        rotationalCoefficientOfRestitutionBox.Connect(SpinBox.SignalName.ValueChanged, rotationalCoefficientBoxValueChanged);
        
        SpinBox frictionCoefficientBox = (SpinBox) parametersContainer.GetNode("FrictionCoefficientBox");
        frictionCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetFrictionCoefficient());
        Callable frictionCoefficientBoxValueChanged = new (this, MethodName.frictionCoefficientBoxValueChanged);
        frictionCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, frictionCoefficientBoxValueChanged);

        SpinBox dragCoefficientBox = (SpinBox) parametersContainer.GetNode("DragCoefficientBox");
        dragCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetDragCoefficient());
        Callable dragCoefficientBoxValueChanged = new (this, MethodName.dragCoefficientBoxValueChanged);
        dragCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, dragCoefficientBoxValueChanged);


        SpinBox liftCoefficientBox = (SpinBox) parametersContainer.GetNode("LiftCoefficientBox");
        liftCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetLiftCoefficient());
        Callable liftCoefficientBoxValueChanged = new (this, MethodName.liftCoefficientBoxValueChanged);
        liftCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, liftCoefficientBoxValueChanged);
        
        SpinBox angularDampingCoefficientBox = (SpinBox) parametersContainer.GetNode("AngularDampingCoefficientBox");
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
        airDensityBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid());
        Callable airDensityBoxValueChanged = new (this, MethodName.airDensityBoxValueChanged);
        airDensityBox.Connect(SpinBox.SignalName.ValueChanged, airDensityBoxValueChanged);

       
        CheckButton shadowBallTrajectoryButton = (CheckButton) additionalContainer.GetNode("ShadowBallContainer").GetNode("ShadowBallTrajectoryButton");
        shadowBallTrajectoryButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
        Callable shadowBallTrajectoryButtonValueChanged = new (this, MethodName.shadowBallTrajectoryButtonValueChanged);
        airResistanceButton.Connect(CheckBox.SignalName.Toggled, shadowBallTrajectoryButtonValueChanged);
    }

    public void SetModel(TabContainer settingsContainer){
        BoxContainer modelContainer = (VBoxContainer) settingsContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        OptionButton patternOptionButton = (OptionButton) modelContainer.GetNode("PatternOptionButton");
        patternOptionButton.Select(0);

        
        ColorPickerButton firstColorPickerButton = (ColorPickerButton) modelContainer.GetNode("FirstColorPickerButton");
        firstColorPickerButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        Callable firstColorPickerButtonValueChaned = new (this, MethodName.firstColorPickerButtonValueChaned);
        firstColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, firstColorPickerButtonValueChaned);

        ColorPickerButton secondColorPickerButton = (ColorPickerButton) modelContainer.GetNode("SecondColorPickerButton");
        secondColorPickerButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
        Callable secondColorPickerButtonValueChaned = new (this, MethodName.secondColorPickerButtonValueChaned);
        secondColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, secondColorPickerButtonValueChaned);

        ColorPickerButton thirdColorPickerButton = (ColorPickerButton) modelContainer.GetNode("ThirdColorPickerButton");
        thirdColorPickerButton.SetPickColor(_mainBall.GetModel().GetThirdColor());
        Callable thirdColorPickerButtonValueChaned = new (this, MethodName.thirdColorPickerButtonValueChaned);
        thirdColorPickerButton.Connect(ColorPickerButton.SignalName.ColorChanged, thirdColorPickerButtonValueChaned);
    }

    public void MassBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetMassInGrams(value);
        _mainBall.GetPhysicsParameters().SetNormalForce(_mainBall.GetMeasurement().GetMass());
        _mainBall.GetPhysicsParameters().UpdateTerminalVelocity(
            _mainBall.GetMeasurement().GetCrossSectionalArea(),
            _mainBall.GetMeasurement().GetMass()
        );
        copyPropertiesToShadowBall();
    }

    public void CircumferenceBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _shadowBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _mainBall.ScaleMeshAndCollisionToRadius();
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
        SetMinRadiusValueForInitialYImpulseComponent();
        _yInitialPositionBox.SetValue(_mainBall.GetMeasurement().GetRadius());
        copyPropertiesToShadowBall();
    }

    public void coefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetCoefficientOfRestitution(value);
        copyPropertiesToShadowBall();
    }

    public void rotationalCoefficientBoxValueChanged(float value){
        GD.Print("Hola");
        _mainBall.GetPhysicsParameters().SetRotationalCoefficientOfRestitution(value);
        copyPropertiesToShadowBall();
    }

    public void frictionCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetFrictionCoefficient(value);
        copyPropertiesToShadowBall();
    }

    public void dragCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetDragCoefficient(value);
        copyPropertiesToShadowBall();
    }

    public void liftCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetLiftCoefficient(value);
        copyPropertiesToShadowBall();
    }

    public void angularDampingCoefficientBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().SetAngularDampingCoefficient(value);
        copyPropertiesToShadowBall();
    }

    public void airResistanceButtonValueChanged(bool onToggled){
        _mainBall.CanApplyAirResistance(onToggled);
        copyPropertiesToShadowBall();
    }

    public void magnusEffectButtonValueChanged(bool onToggled){
        _mainBall.CanApplyMagnusEffect(onToggled);
        copyPropertiesToShadowBall();
    }

    public void airDensityBoxValueChanged(float value){
        _mainBall.GetPhysicsParameters().GetEnvironment().SetDensityOfFluid(value);
        copyPropertiesToShadowBall();
    }

    public void shadowBallTrajectoryButtonValueChanged(bool onToggled){
        _shadowBall.CanShowItsTrajectory(onToggled);
    }

    public void firstColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetFirstColor(color);
        _mainBall.ChangeColorToMesh();
    }

    public void secondColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetSecondColor(color);
        _mainBall.ChangeColorToMesh();
    }

    public void thirdColorPickerButtonValueChaned(Color color){
        _mainBall.GetModel().SetSecondColor(color);
        _mainBall.ChangeColorToMesh();
    }


    public void copyPropertiesToShadowBall(){
        _shadowBall = (ShadowBall) _mainBall.ShallowCopy(_shadowBall);
    }

    public void SetStartSimulationButton(){
        Button startSimulationButton = (Button) GetNode("ButtonContainer").GetNode("StartSimulationButton");
        startSimulationButton.Connect(Button.SignalName.Pressed, Callable.From(StartSimulationButtonPressed), (uint)GodotObject.ConnectFlags.OneShot);
    }

    public void StartSimulationButtonPressed(){
        _settingsContainer.Hide();
        _startSimulationButton.Hide();
        _infoContainer.Show();
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
        float distance = (float) Math.Truncate(globalPosition.Length() * 100) / 100;
        globalPosition.X =  (float) Mathf.Abs(Math.Truncate(globalPosition.X));
        float currentHeight = (float) Math.Truncate((globalPosition.Y - _mainBall.GetMeasurement().GetRadius()) * 10) / 10;
        globalPosition.Y =  (float) Mathf.Abs(Math.Truncate(globalPosition.Y) * 100) / 100;
        globalPosition.Z =  (float) Math.Truncate(globalPosition.Z);
        _positionLabel.SetText(globalPosition.ToString());
        _distanceLabel.SetText(distance.ToString());
        _currentHeightLabel.SetText(currentHeight.ToString());
        Vector3 linearVelocity = _mainBall.GetLinearVelocity();
        float linearSpeed = (float) Math.Truncate(linearVelocity.Length() * 100) / 100;
        linearVelocity.X =  (float) Math.Truncate(linearVelocity.X);
        linearVelocity.Y =  (float) Math.Truncate(linearVelocity.Y);
        linearVelocity.Z =  (float) Math.Truncate(linearVelocity.Z);
        _linearVelocityLabel.SetText(linearVelocity.ToString());
        Vector3 angularVelocity = _mainBall.GetAngularVelocity();
        float angularSpeed = (float) Math.Truncate(angularVelocity.Length() * 100) / 100;
        angularVelocity.X =  (float) Math.Truncate(angularVelocity.X);
        angularVelocity.Y =  (float) Math.Truncate(angularVelocity.Y);
        angularVelocity.Z =  (float) Math.Truncate(angularVelocity.Z);
        _angularVelocityLabel.SetText(angularVelocity.ToString());
        _linearSpeedLabel.SetText(linearSpeed.ToString());
        _angularSpeedLabel.SetText(angularSpeed.ToString());
        _detectedCollisionLabel.SetText(_mainBall.IsACollisionDetected() ? "Sí" : "No");
        Vector3 dragForce = _mainBall.GetPhysicsParameters().GetDragForce();
        dragForce.X =  (float) Math.Truncate(dragForce.X);
        dragForce.Y =  (float) Math.Truncate(dragForce.Y);
        dragForce.Z =  (float) Math.Truncate(dragForce.Z);
        _dragForceLabel.SetText(dragForce.ToString());
        Vector3 magnusEffectForce = _mainBall.GetPhysicsParameters().GetMagnusEffectForce();
        magnusEffectForce.X =  (float) Math.Truncate(magnusEffectForce.X);
        magnusEffectForce.Y =  (float) Math.Truncate(magnusEffectForce.Y);
        magnusEffectForce.Z =  (float) Math.Truncate(magnusEffectForce.Z);
        _magnusEffectForceLabel.SetText(magnusEffectForce.ToString());
    }

    public void SetParametersInfo(TabContainer infoContainer){
        VBoxContainer parametersContainer = (VBoxContainer) infoContainer.GetNode("Parámetros").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        _massLabel = (Label) parametersContainer.GetNode("MassContainer").GetNode("MassLabel");
        _massLabel.SetText(_mainBall.GetMeasurement().GetMassInGrams().ToString());
        _inertiaLabel = (Label) parametersContainer.GetNode("InertiaContainer").GetNode("InertiaLabel");
        float inertia = (float) Math.Truncate(_mainBall.GetMeasurement().GetInertia() * 10000) / 10000;
        _inertiaLabel.SetText(inertia.ToString());
        _circumferenceLabel = (Label) parametersContainer.GetNode("CircumferenceContainer").GetNode("CircumferenceLabel");
        _circumferenceLabel.SetText(_mainBall.GetMeasurement().GetCircumferenceInCentimeters().ToString());
        _diameterLabel = (Label) parametersContainer.GetNode("DiameterContainer").GetNode("DiameterLabel");
        float diameter = (float) Math.Truncate(_mainBall.GetMeasurement().GetDiameterInCentimeters());
        _diameterLabel.SetText(diameter.ToString());
        _radiusLabel = (Label) parametersContainer.GetNode("RadiusContainer").GetNode("RadiusLabel");
        float radius = (float) Math.Truncate(_mainBall.GetMeasurement().GetRadiusInCentimeters());
        _radiusLabel.SetText(radius.ToString());
        _crossSectionalArea = (Label) parametersContainer.GetNode("CrossSectionalAreaContainer").GetNode("CrossSectionalAreaLabel");
        float crossSectionalArea = (float) Math.Truncate(_mainBall.GetMeasurement().GetCrossSectionalArea() * 10000) / 10000;
        _crossSectionalArea.SetText(crossSectionalArea.ToString());
        _coefficientOfRestitution = (Label) parametersContainer.GetNode("CoefficientOfRestitutionContainer").GetNode("CoefficientOfRestitutionLabel");
        _coefficientOfRestitution.SetText(_mainBall.GetPhysicsParameters().GetCoefficientOfRestitution().ToString());
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
        float terminalVelocity = (float) Math.Truncate(_mainBall.GetPhysicsParameters().GetTerminalVelocity() * 10000) / 10000;
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
        _thirdColorButton = (ColorPickerButton) modelContainer.GetNode("ThirdColorContainer").GetNode("ThirdColorButton");
        _thirdColorButton.SetPickColor(_mainBall.GetModel().GetThirdColor());
    }
}
