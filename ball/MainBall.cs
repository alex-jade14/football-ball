using Godot;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase, IPrototype
{
	public EventManager events;
	private String _name;
	protected BallModel _model;
	

	public override void _Ready(){
		base._Ready();
		ChangeColorToMesh();
	}

	public void create(String name, String pattern, Color firstColor, Color secondColor, Color thirdColor,
	float mass, float circumference, float coefficientOfRestitution, float rotationalCoefficientOfRestitution,
	float frictionCoefficient, float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
		base.create(
			mass,
			circumference,
			coefficientOfRestitution,
			rotationalCoefficientOfRestitution,
			frictionCoefficient,
			dragCoefficient,
			liftCoefficient,
			angularDampingCoefficient,
			environment
		);
		_name = name;
		_model = new BallModel(
			pattern,
			firstColor,
			secondColor,
			thirdColor
		);
		events = new EventManager();
	}

	public String GetName(){
		return _name;
	}

	public void SetName(String name){
		_name = name;
	}

	public BallModel GetModel(){
		return _model;
	}

	public void SetModel(BallModel model){
		_model = model;
	}
	
	public GodotObject ShallowCopy(){
		return (BallBase) this.MemberwiseClone();
	}

	public GodotObject DeepCopy(ShadowBall shadowBall){
		shadowBall.create(
			GetMeasurement().GetMassInGrams(),
			GetMeasurement().GetCircumferenceInCentimeters(),
			GetPhysicsParameters().GetCoefficientOfRestitution(),
			GetPhysicsParameters().GetRotationalCoefficientOfRestitution(),
			GetPhysicsParameters().GetFrictionCoefficient(),
			GetPhysicsParameters().GetDragCoefficient(),
			GetPhysicsParameters().GetLiftCoefficient(),
			GetPhysicsParameters().GetAngularDampingCoefficient(),
			GetPhysicsParameters().GetEnvironment()
		);
		return shadowBall;
	}

	private void ChangeColorToMesh(){
		StandardMaterial3D firstMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(0);
		StandardMaterial3D secondMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(1);
		StandardMaterial3D thirdMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(2);
		firstMaterial.SetAlbedo(GetModel().GetFirstColor());
		secondMaterial.SetAlbedo(GetModel().GetSecondColor());
		thirdMaterial.SetAlbedo(GetModel().GetThirdColor());
	}

	public override void _PhysicsProcess(double delta)
	{
		SimulatePhysics();
        MoveAndSlide();
		ApplyAngularRotation();
	}

	private void ApplyAngularRotation(){
		mesh.SetGlobalTransform(
			RigidBodyHelper.CalculateAngularRotation(
				GetAngularVelocity(),
				mesh.GetGlobalTransform()
			)
		);
	}

	public override void ApplyImpulse(Godot.Vector3 impulse, Godot.Vector3 positionWhereImpulseIsApplied){
		base.ApplyImpulse(impulse, positionWhereImpulseIsApplied);
		events.Notify(new Godot.Collections.Dictionary{
			{"impulse", impulse},
			{"positionWhereImpulseIsApplied", positionWhereImpulseIsApplied}
		});
	}    
	
}
