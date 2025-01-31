using Godot;
using Godot.Collections;

public class DebugDecorator : DebugInfoDecorator{
    public DebugDecorator(DebugInfo comp) : base (comp){}

    public bool CanShowDebugMenu()
    {
        return base.CanShowDebugMenu();
    }

    public void CanShowDebugMenu(bool show)
    {
        base.CanShowDebugMenu(show);
    }

    public Settings DisplayDebugMenu(Dictionary data){
        return base.DisplayDebugMenu(data);
    }

}