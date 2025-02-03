using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase, IPrototype
{
	public EventManager events;
	protected BallModel _model;
	

	public override void _Ready(){
		base._Ready();
		ChangeColorToMesh();
	}

	public void Create(String pattern, Color firstColor, Color secondColor, Color thirdColor,
	float mass, float circumference, float coefficientOfRestitution, float rotationalCoefficientOfRestitution,
	float frictionCoefficient, float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
		base.Create(
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
		_model = new BallModel(
			pattern,
			firstColor,
			secondColor,
			thirdColor
		);
		events = new EventManager();
	}

	public BallModel GetModel(){
		return _model;
	}

	public void SetModel(BallModel model){
		_model = model;
	}
	
	public GodotObject ShallowCopy(ShadowBall shadowBall){
		shadowBall.SetGlobalPosition(GetGlobalPosition());
		shadowBall.GetMeasurement().SetMass(GetMeasurement().GetMass());
		shadowBall.GetPhysicsParameters().SetNormalForce(GetMeasurement().GetMass());
		shadowBall.GetPhysicsParameters().UpdateTerminalVelocity(GetMeasurement().GetCrossSectionalArea(), GetMeasurement().GetMass());
		shadowBall.GetMeasurement().SetCircumference(GetMeasurement().GetCircumference());
		shadowBall.GetPhysicsParameters().SetCoefficientOfRestitution(GetPhysicsParameters().GetCoefficientOfRestitution());
		shadowBall.GetPhysicsParameters().SetCoefficientOfRestitution(GetPhysicsParameters().GetCoefficientOfRestitution());
		shadowBall.GetPhysicsParameters().SetRotationalCoefficientOfRestitution(GetPhysicsParameters().GetRotationalCoefficientOfRestitution());
		shadowBall.GetPhysicsParameters().SetRotationalCoefficientOfRestitution(GetPhysicsParameters().GetRotationalCoefficientOfRestitution());
		shadowBall.GetPhysicsParameters().SetFrictionCoefficient(GetPhysicsParameters().GetFrictionCoefficient());
		shadowBall.GetPhysicsParameters().SetDragCoefficient(GetPhysicsParameters().GetDragCoefficient());
		shadowBall.GetPhysicsParameters().SetLiftCoefficient(GetPhysicsParameters().GetLiftCoefficient());
		shadowBall.GetPhysicsParameters().SetAngularDampingCoefficient(GetPhysicsParameters().GetAngularDampingCoefficient());
		shadowBall.GetPhysicsParameters().SetEnvironment(GetPhysicsParameters().GetEnvironment());
		shadowBall.CanApplyAirResistance(CanApplyAirResistance());
		shadowBall.CanApplyMagnusEffect(CanApplyMagnusEffect());
		shadowBall.ScaleMeshAndCollisionToRadius();
		return shadowBall;
	}

	public GodotObject DeepCopy(ShadowBall shadowBall){
		shadowBall.Create(
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

	public void ChangeColorToMesh(){
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
		ScaleMeshAndCollisionToRadius();
	}

	public override void ApplyImpulse(Godot.Vector3 impulse, Godot.Vector3 positionWhereImpulseIsApplied){
		base.ApplyImpulse(impulse, positionWhereImpulseIsApplied);
		events.Notify(new Godot.Collections.Dictionary{
			{"impulse", impulse},
			{"positionWhereImpulseIsApplied", positionWhereImpulseIsApplied}
		});
	}	
}
