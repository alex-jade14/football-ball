using Godot;
using Godot.Collections;

public interface DebugInfo{
    public bool CanShowDebugMenu();
    public void CanShowDebugMenu(bool show);
    public Settings DisplayDebugMenu(Dictionary data);
}