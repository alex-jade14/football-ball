using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BallModel
{
    private String _pattern;
    private Color _firstColor;
    private Color _secondColor;

    public BallModel(String pattern, Color firstColor, Color secondColor){
        _pattern = pattern;
        _firstColor = firstColor;
        _secondColor = secondColor;
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
        return _secondColor;
    }

    public void SetSecondColor(Color secondColor){
        _secondColor = secondColor;
    }
}