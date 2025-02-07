using Godot;
using Godot.Collections;

public partial class MainBall : BallBase, IPrototype
{
	public EventManager Events;
	protected MainBallModel _model;
	private MeshInstance3D _mesh;
	private bool _isApplyingImpulse;
	private bool _canSimulatePhysics;
	private Control _subViewportCameraControl;
	private Camera3D _mainCamera;
	private Camera3D _subViewportCamera;
	private bool _canFollowMainCamera;
	
	public void Create(string pattern, Color firstColor, Color secondColor, float mass, float circumference, float coefficientOfRestitution,
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
		_model = new MainBallModel(
			pattern,
			firstColor,
			secondColor
		);
		Events = new EventManager();
	}

	public override void _Ready(){
		base._Ready();
		ScaleMeshToRadius();
		ChangeMesh();
		ChangeColorToMesh();
		CanDetectCollisions = true;
		GetCameraNodes();
	}

	public MainBallModel GetModel(){
		return _model;
	}

	public MeshInstance3D GetMesh(){
		return _mesh;
	}

	public Camera3D GetMainCamera(){
		return _mainCamera;
	}

	public Camera3D GetSubViewportCamera(){
		return _subViewportCamera;
	}

	public Control GetSubViewportCameraControl(){
		return _subViewportCameraControl;
	}

	public void SetModel(MainBallModel model){
		_model = model;
	}

	public void SetMesh(MeshInstance3D mesh){
		_mesh = mesh;
	}

	public void CanFollowMainCamera(bool canFollowMainCamera){
		_canFollowMainCamera = canFollowMainCamera;
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
			_mesh.SetMesh(GD.Load<Mesh>("res://ball/main_ball/hexagon_pentagon_ball_model.res"));
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

	private void GetCameraNodes(){
		_mainCamera = (Camera3D) GetNode("MainCamera");
		_subViewportCameraControl = (Control) GetNode("Control");
		_subViewportCamera = (Camera3D) _subViewportCameraControl.GetNode("SubViewportContainer").GetNode("SubViewport").GetNode("SubViewportCamera");
	}

	public override void _PhysicsProcess(double delta)
	{
		SimulatePhysics();
        MoveAndSlide();
		ApplyAngularRotation();
		if(_canFollowMainCamera){
			_subViewportCamera.SetGlobalPosition(_mainCamera.GetGlobalPosition());
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
		// Events.Notify("impulse", new Dictionary{
		// 	{"impulse", impulse},
		// 	{"positionWhereImpulseIsApplied", positionWhereImpulseIsApplied},
		// });
	}

	public void ScaleMeshToRadius(){
		_mesh = (MeshInstance3D) GetNode("MeshInstance3D");
        _mesh.SetScale(new Vector3(1,1,1) * GetMeasurement().GetRadius());
	}

	public override void CollisionResponse(){
		base.CollisionResponse();
		if(IsACollisionDetected() && WasOnFloor()){
			Events.Notify("updateMarker", new Dictionary {{"hide", true}});
			if(Mathf.Abs(GetLinearVelocity().Y) > 3){
				Events.Notify("detectedCollision", new Dictionary{
					{"globalPosition", GetGlobalPosition()},
					{"linearVelocity", GetLinearVelocity()},
					{"angularVelocity", GetAngularVelocity()}
				});
			}
		}
	}
}
