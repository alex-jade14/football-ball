using Godot;
using System;

public partial class BallSize
{
    private String _name {
        get{
            return GetName();
        }
        set{
            SetName(value);
        }
    }
    private BallMeasurement _measurement {
        get{
            return GetMeasurement();
        }
        set{
            SetMeasurement(value);
        }
    }
    private bool _isCustom {
        get{
            return IsCustom();
        }
        set{
            IsCustom(value);
        }
    }

    public BallSize(String name, BallMeasurement measurement, bool isCustom){
        _name = name;
        _measurement = measurement;
        _isCustom = isCustom;
    }

    public String GetName(){
        return _name;
    }

    public void SetName(String name){
        _name = name;
    }

    public BallMeasurement GetMeasurement(){
        return _measurement;
    }

    public void SetMeasurement(BallMeasurement measurement){
        _measurement = measurement;
    }

    public bool IsCustom(){
        return _isCustom;
    }

    public void IsCustom(bool isCustom){
        _isCustom = isCustom;
    }
}