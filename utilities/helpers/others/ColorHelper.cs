using Godot;
using Godot.Collections;
using System;

public partial class ColorHelper
{   
    public static Color GetColor(Dictionary data){
        return new Color(
            (float) data["r"] / 255,
            (float) data["g"] / 255,
            (float) data["b"] / 255,
            (float) data["a"] / 255
        );
    }
}