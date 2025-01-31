using Godot;
using Godot.Collections;

public abstract class DebugInfoDecorator : DebugInfo{
    protected DebugInfo _component;

    public DebugInfoDecorator(DebugInfo component){
        _component = component;
    }

    public void SetComponent(DebugInfo component){
        _component = component;
    }

    public bool CanShowDebugMenu()
    {
        return _component.CanShowDebugMenu();
    }

    public void CanShowDebugMenu(bool show)
    {
        _component.CanShowDebugMenu(show);
    }

    public Settings DisplayDebugMenu(Dictionary data){
        return _component.DisplayDebugMenu(data);
    }
}