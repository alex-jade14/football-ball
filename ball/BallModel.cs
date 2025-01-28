using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BallModel
{
    private String _pattern;
    private Color _firstColor;
    private Color _secondColor;
    private Color _thirdColor;

    public BallModel(String pattern, Color firstColor, Color secondColor, Color thirdColor){
        _pattern = pattern;
        _firstColor = firstColor;
        _secondColor = secondColor;
        _thirdColor = thirdColor;
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

    public Color GetThirdColor(){
        return _thirdColor;
    }

    public void SetThirdColor(Color thirdColor){
        _thirdColor = thirdColor;
    }
}