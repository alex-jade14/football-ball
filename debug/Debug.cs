using Godot;
using System;

public partial class Debug : Control
{
    private MainBall _mainBall;
    private ShadowBall _shadowBall;
    private SpinBox _yInitialPositionBox;
    private SpinBox _xInitialImpulseBox;
    private SpinBox _yInitialImpulseBox;
    private SpinBox _zInitialImpulseBox;
    private SpinBox _xInitialImpulsePositionBox;
    private SpinBox _yInitialImpulsePositionBox;
    private SpinBox _zInitialImpulsePositionBox;
    private SpinBox _impulseFactorBox;

    public void Create(MainBall mainBall, ShadowBall shadowBall){
        _mainBall = mainBall;
        _shadowBall = shadowBall;
    }

    public void SetInitialDebugData(){
        TabContainer settingsContainer = (TabContainer) GetNode("TabContainer");
        SetInteraction(settingsContainer);
        SetParameters(settingsContainer);
        SetAdditional(settingsContainer);
        SetModel(settingsContainer);
        SetStartSimulationButton(); 
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

        SpinBox rotationReductionFactorBox = (SpinBox) additionalContainer.GetNode("RotationReductionFactorBox");
        rotationReductionFactorBox.SetValueNoSignal(RollingDynamicsHelper.rotationReductionFactorBox);
        Callable rotationReductionFactorBoxValueChanged = new (this, MethodName.rotationReductionFactorBoxValueChanged);
        rotationReductionFactorBox.Connect(SpinBox.SignalName.ValueChanged, rotationReductionFactorBoxValueChanged);
        
        SpinBox interpolationCoefficientBox = (SpinBox) additionalContainer.GetNode("InterpolationCoefficientBox");
        interpolationCoefficientBox.SetValueNoSignal(RollingDynamicsHelper.interpolationCoefficient);
        Callable interpolationCoefficientBoxValueChanged = new (this, MethodName.interpolationCoefficientBoxValueChanged);
        interpolationCoefficientBox.Connect(SpinBox.SignalName.ValueChanged, interpolationCoefficientBoxValueChanged);
        
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

    public void rotationReductionFactorBoxValueChanged(float value){
        RollingDynamicsHelper.rotationReductionFactorBox = value;
    }

    public void interpolationCoefficientBoxValueChanged(float value){
        RollingDynamicsHelper.interpolationCoefficient = value;
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
}
