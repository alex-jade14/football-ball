using Godot;
using System;

public partial class Debug : Control
{
    private MainBall _mainBall;
    private ShadowBall _shadowBall;
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
        SpinBox yInitialPositionBox = (SpinBox) positionContainer.GetNode("YInitialPositionBox");
        SpinBox zInitialPositionBox = (SpinBox) positionContainer.GetNode("ZInitialPositionBox");
        xInitialPositionBox.SetValueNoSignal(globalPosition.X);
        yInitialPositionBox.SetValueNoSignal(globalPosition.Y);
        zInitialPositionBox.SetValueNoSignal(globalPosition.Z);
        
        HBoxContainer impulseContainer = (HBoxContainer) interactionContainer.GetNode("ImpulseContainer");
        _xInitialImpulseBox = (SpinBox) impulseContainer.GetNode("XInitialImpulseBox");
        _yInitialImpulseBox = (SpinBox) impulseContainer.GetNode("YInitialImpulseBox");
        _zInitialImpulseBox = (SpinBox) impulseContainer.GetNode("ZInitialImpulseBox");
        _xInitialImpulseBox.SetValueNoSignal(-10);
        _yInitialImpulseBox.SetValueNoSignal(0);
        _zInitialImpulseBox.SetValueNoSignal(10);
        
        HBoxContainer impulsePositionContainer = (HBoxContainer) interactionContainer.GetNode("ImpulsePositionContainer");
        _xInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("XInitialImpulsePositionBox");
        _yInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("YInitialImpulsePositionBox");
        _zInitialImpulsePositionBox = (SpinBox) impulsePositionContainer.GetNode("ZInitialImpulsePositionBox");
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
        _xInitialImpulsePositionBox.SetValueNoSignal(0);
        _yInitialImpulsePositionBox.SetValueNoSignal(0);
        _zInitialImpulsePositionBox.SetValueNoSignal(0);

        HBoxContainer impulseFactorontainer = (HBoxContainer) interactionContainer.GetNode("ImpulseFactorContainer");
        _impulseFactorBox = (SpinBox) impulseFactorontainer.GetNode("ImpulseFactorBox");
        _impulseFactorBox.SetValueNoSignal(1);
    }

    public void SetMinAndMaxRadiusValueForInitialImpulsePosition(){
        _xInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMin(-_mainBall.GetMeasurement().GetRadius());
        _xInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _yInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
        _zInitialImpulsePositionBox.SetMax(_mainBall.GetMeasurement().GetRadius());
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
        SpinBox rotationalCoefficientOfRestitutionBox = (SpinBox) parametersContainer.GetNode("RotationalCoefficientOfRestitutionBox");
        rotationalCoefficientOfRestitutionBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetRotationalCoefficientOfRestitution());
        SpinBox frictionCoefficientBox = (SpinBox) parametersContainer.GetNode("FrictionCoefficientBox");
        frictionCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetFrictionCoefficient());
        SpinBox dragCoefficientBox = (SpinBox) parametersContainer.GetNode("DragCoefficientBox");
        dragCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetDragCoefficient());
        SpinBox liftCoefficientBox = (SpinBox) parametersContainer.GetNode("LiftCoefficientBox");
        liftCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetLiftCoefficient());
        SpinBox angularDampingCoefficientBox = (SpinBox) parametersContainer.GetNode("AngularDampingCoefficientBox");
        angularDampingCoefficientBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetAngularDampingCoefficient());
    }

    public void SetAdditional(TabContainer settingsContainer){
        VBoxContainer additionalContainer = (VBoxContainer) settingsContainer.GetNode("Adicional").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        CheckButton airResistanceButton = (CheckButton) additionalContainer.GetNode("AirContainer").GetNode("AirResistanceButton");
        airResistanceButton.SetPressedNoSignal(true);
        CheckButton magnusEffectButton = (CheckButton) additionalContainer.GetNode("MagnusContainer").GetNode("MagnusEffectButton");
        magnusEffectButton.SetPressedNoSignal(true);
        
        SpinBox airDensityBox = (SpinBox) additionalContainer.GetNode("AirDensityBox");
        airDensityBox.SetValueNoSignal(_mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid());
        
        SpinBox rotationReductionFactorBox = (SpinBox) additionalContainer.GetNode("RotationReductionFactorBox");
        rotationReductionFactorBox.SetValueNoSignal(RollingDynamicsHelper.rotationReductionFactorBox);
        SpinBox interpolationCoefficientBox = (SpinBox) additionalContainer.GetNode("InterpolationCoefficientBox");
        interpolationCoefficientBox.SetValueNoSignal(RollingDynamicsHelper.interpolationCoefficient);
        
        CheckButton shadowBallTrajectoryButton = (CheckButton) additionalContainer.GetNode("ShadowBallContainer").GetNode("ShadowBallTrajectoryButton");
        shadowBallTrajectoryButton.SetPressedNoSignal(_shadowBall.CanShowItsTrajectory());
    }

    public void SetModel(TabContainer settingsContainer){
        BoxContainer modelContainer = (VBoxContainer) settingsContainer.GetNode("Modelo").GetNode("MarginContainer").GetNode("ScrollContainer").GetNode("MarginContainer").GetNode("VBoxContainer");
        
        OptionButton patternOptionButton = (OptionButton) modelContainer.GetNode("PatternOptionButton");
        patternOptionButton.Select(0);
        ColorPickerButton firstColorPickerButton = (ColorPickerButton) modelContainer.GetNode("FirstColorPickerButton");
        firstColorPickerButton.SetPickColor(_mainBall.GetModel().GetFirstColor());
        ColorPickerButton secondColorPickerButton = (ColorPickerButton) modelContainer.GetNode("SecondColorPickerButton");
        secondColorPickerButton.SetPickColor(_mainBall.GetModel().GetSecondColor());
        ColorPickerButton thirdColorPickerButton = (ColorPickerButton) modelContainer.GetNode("ThirdColorPickerButton");
        thirdColorPickerButton.SetPickColor(_mainBall.GetModel().GetThirdColor());
    }

    public void MassBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetMassInGrams(value);
        _shadowBall.GetMeasurement().SetMassInGrams(value);
    }

    public void CircumferenceBoxValueChanged(float value){
        _mainBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _shadowBall.GetMeasurement().SetCircumferenceInCentemeters(value);
        _mainBall.ScaleMeshAndCollisionToRadius();
        _shadowBall.ScaleMeshAndCollisionToRadius();
        SetMinAndMaxRadiusValueForInitialImpulsePosition();
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
