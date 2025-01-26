using Godot;
using System;

public partial class BallModel
{
    private String _pattern {
        get{
            return GetPattern();
        }
        set{
            SetPattern(value);
        }
    }
    private Color _firstColor {
        get{
            return GetFirstColor();
        }
        set{
            SetFirstColor(value);
        }
    }
    private Color _secondColor {
        get{
            return GetSecondColor();
        }
        set{
            SetSecondColor(value);
        }
    }
    private Color _thirdColor {
        get{
            return GetThirdColor();
        }
        set{
            SetThirdColor(value);
        }
    }

    public String GetPattern(){
        return _pattern;
    }

    public void SetPattern(String pattern){
        _pattern = pattern;
    }
    
    public Color GetFirstColor(){
        return _firstColor;
    }

    public void SetFirstColor(Color firstColor){
        _firstColor = firstColor;
    }

    public Color GetSecondColor(){
        return _firstColor;
    }

    public void SetSecondColor(Color secondColor){
        _secondColor = secondColor;
    }

    public Color GetThirdColor(){
        return _firstColor;
    }

    public void SetThirdColor(Color thirdColor){
        _thirdColor = thirdColor;
    }
}