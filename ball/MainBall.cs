using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase, IPrototype
{
	public EventManager events;
	protected BallModel _model;
	private MeshInstance3D _mesh;
	private bool _isApplyingImpulse;
	private bool _canSimulatePhysics;
	private Camera3D _mainCamera;
	private Control _subViewportCameraControl;
	private Camera3D _subViewportCameraGuide;
	private Camera3D _subViewportCamera;
	private bool _canFollowViewportCameraGuide;
	

	public void Create(String pattern, Color firstColor, Color secondColor, float mass, float circumference, float coefficientOfRestitution,
	float frictionCoefficient, float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
		base.Create(
			mass,
			circumference,
			coefficientOfRestitution,
			frictionCoefficient,
			dragCoefficient,
			liftCoefficient,
			angularDampingCoefficient,
			environment
		);
		_model = new BallModel(
			pattern,
			firstColor,
			secondColor
		);
		events = new EventManager();
	}

	public override void _Ready(){
		base._Ready();
		ScaleMeshToRadius();
		ChangeMesh();
		ChangeColorToMesh();
		_canDetectCollisions = true;
		_mainCamera = (Camera3D) GetNode("MainCamera");
		_subViewportCameraGuide = (Camera3D) GetNode("SubViewportCameraGuide");
		_subViewportCameraControl = (Control) GetNode("Control");
		_subViewportCamera = (Camera3D) _subViewportCameraControl.GetNode("SubViewportContainer").GetNode("SubViewport").GetNode("SubViewportCamera");
	}

	public BallModel GetModel(){
		return _model;
	}

	public void SetModel(BallModel model){
		_model = model;
	}

	public MeshInstance3D GetMesh(){
		return _mesh;
	}

	public void SetMesh(MeshInstance3D mesh){
		_mesh = mesh;
	}

	public Camera3D GetMainCamera(){
		return _mainCamera;
	}

	public Camera3D GetViewportCamera(){
		return _subViewportCamera;
	}

	public Control GetViewportCameraControl(){
		return _subViewportCameraControl;
	}

	public void CanFollowViewportCameraGuide(bool canFollowViewportCameraGuide){
		_canFollowViewportCameraGuide = canFollowViewportCameraGuide;
	}
	
	public GodotObject ShallowCopy(ShadowBall shadowBall){
		shadowBall.SetGlobalPosition(GetGlobalPosition());
		shadowBall.GetMeasurement().SetMass(GetMeasurement().GetMass());
		shadowBall.GetPhysicsParameters().SetNormalForce(GetMeasurement().GetMass());
		shadowBall.GetPhysicsParameters().UpdateTerminalVelocity(GetMeasurement().GetCrossSectionalArea(), GetMeasurement().GetMass());
		shadowBall.GetMeasurement().SetCircumference(GetMeasurement().GetCircumference());
		shadowBall.GetPhysicsParameters().SetCoefficientOfRestitution(GetPhysicsParameters().GetCoefficientOfRestitution());
		shadowBall.GetPhysicsParameters().SetFrictionCoefficient(GetPhysicsParameters().GetFrictionCoefficient());
		shadowBall.GetPhysicsParameters().SetDragCoefficient(GetPhysicsParameters().GetDragCoefficient());
		shadowBall.GetPhysicsParameters().SetLiftCoefficient(GetPhysicsParameters().GetLiftCoefficient());
		shadowBall.GetPhysicsParameters().SetAngularDampingCoefficient(GetPhysicsParameters().GetAngularDampingCoefficient());
		shadowBall.GetPhysicsParameters().SetEnvironment(GetPhysicsParameters().GetEnvironment());
		shadowBall.CanApplyAirResistance(CanApplyAirResistance());
		shadowBall.CanApplyMagnusEffect(CanApplyMagnusEffect());
		shadowBall.ScaleCollisionToRadius();
		shadowBall.SetLinearVelocity(GetLinearVelocity());
		shadowBall.SetLinearVelocity(GetAngularVelocity());
		return shadowBall;
	}

	public GodotObject DeepCopy(ShadowBall shadowBall){
		shadowBall.Create(
			GetMeasurement().GetMassInGrams(),
			GetMeasurement().GetCircumferenceInCentimeters(),
			GetPhysicsParameters().GetCoefficientOfRestitution(),
			GetPhysicsParameters().GetFrictionCoefficient(),
			GetPhysicsParameters().GetDragCoefficient(),
			GetPhysicsParameters().GetLiftCoefficient(),
			GetPhysicsParameters().GetAngularDampingCoefficient(),
			GetPhysicsParameters().GetEnvironment()
		);
		return shadowBall;
	}

	public void ChangeMesh(){
		if(GetModel().GetPattern() == "hexagon-pentagon"){
			_mesh.SetMesh(GD.Load<Mesh>("res://ball/ball_model.res"));
		}
		else{
			_mesh.SetMesh(GD.Load<Mesh>("res://ball/stars_ball_model.res"));
		}
	}

	public void ChangeColorToMesh(){
		StandardMaterial3D firstMaterial = (StandardMaterial3D) _mesh.GetActiveMaterial(0);
		StandardMaterial3D secondMaterial = (StandardMaterial3D) _mesh.GetActiveMaterial(1);
		firstMaterial.SetAlbedo(GetModel().GetFirstColor());
		secondMaterial.SetAlbedo(GetModel().GetSecondColor());
	}

	public override void _PhysicsProcess(double delta)
	{
		SimulatePhysics();
        MoveAndSlide();
		ApplyAngularRotation();
		if(_canFollowViewportCameraGuide){
			_subViewportCamera.SetGlobalPosition(_subViewportCameraGuide.GetGlobalPosition());
		}
	}

	private void ApplyAngularRotation(){
		_mesh.SetGlobalTransform(
			RigidBodyHelper.CalculateAngularRotation(
				GetAngularVelocity(),
				_mesh.GetGlobalTransform()
			)
		);
		ScaleCollisionToRadius();
		ScaleMeshToRadius();
	}

	public override void ApplyImpulse(Vector3 impulse, Vector3 positionWhereImpulseIsApplied){
		base.ApplyImpulse(impulse, positionWhereImpulseIsApplied);
		events.Notify("impulse", new Dictionary{
			{"impulse", impulse},
			{"positionWhereImpulseIsApplied", positionWhereImpulseIsApplied},
		});
	}

	public void ScaleMeshToRadius(){
		_mesh = (MeshInstance3D) GetNode("MeshInstance3D");
        _mesh.SetScale(new Vector3(1,1,1) * GetMeasurement().GetRadius());
	}

	public override void CollisionResponse(){
		base.CollisionResponse();
		if(IsACollisionDetected() && WasOnFloor()){
			events.Notify("updateMarker", new Dictionary {{"hide", true}});
			if(Mathf.Abs(GetLinearVelocity().Y) > 3){
				events.Notify("detectedCollision", new Dictionary{
					{"globalPosition", GetGlobalPosition()},
					{"linearVelocity", GetLinearVelocity()},
					{"angularVelocity", GetAngularVelocity()}
				});
			}
		}
	}
}
