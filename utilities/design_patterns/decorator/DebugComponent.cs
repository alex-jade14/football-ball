using System;
using Godot;
using Godot.Collections;

public class DebugComponent : DebugInfo{

    public bool _show;
    public bool CanShowDebugMenu(){
        return _show;
    }

    public void CanShowDebugMenu(bool show){
        _show = show;
    }

    public Settings DisplayDebugMenu(Dictionary data){
        PackedScene settingsScene = (PackedScene) GD.Load("res://settings/settings.tscn");
        Settings settings = (Settings) settingsScene.Instantiate();
        // MarginContainer mainContainer = (MarginContainer) settings.GetNode("MarginContainer");
        // GD.Print(mainContainer.GetSize());
        // mainContainer.SetSize(new Godot.Vector2((float) data["horizontal_size"], (float) data["vertical_size"]));
        // mainContainer.SetPosition(new Godot.Vector2((float) data["horizontal_position"], (float) data["vertical_position"]));
        return settings;
    }
}