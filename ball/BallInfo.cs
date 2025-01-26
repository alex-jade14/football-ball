using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class BallInfo
{
    private String _name {
        get{
            return GetName();
        }
        set{
            SetName(value);
        }
    }
    private BallModel _model{
        get{
            return GetModel();
        }
        set{
            SetModel(value);
        }
    }
    private BallSize _size {
        get{
            return GetSize();
        }
        set{
            SetSize(value);
        }
    }
    private BallPhysicsParameters _physicsParameters {
        get{
            return GetPhysicsParameters();
        }
        set{
            SetPhysicsParameters(value);
        }
    }

    public BallInfo(String name, BallModel model, BallSize size, BallPhysicsParameters physicsParameters){
        _name = name;
        _model = model;
        _size = size;
        _physicsParameters = physicsParameters;
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

    public void SetName(BallModel model){
        _model = model;
    }

    public BallSize GetSize(){
        return _size;
    }

    public void SetSize(BallSize size){
        _size = size;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
    }

    public void SetPhysicsParameters(BallPhysicsParameters physicsParameters){
        _physicsParameters = physicsParameters;
    }

}