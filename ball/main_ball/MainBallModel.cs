using Godot;

public partial class MainBallModel
{
    private string _pattern;
    private Color _firstColor;
    private Color _secondColor;

    public MainBallModel(string pattern, Color firstColor, Color secondColor){
        _pattern = pattern;
        _firstColor = firstColor;
        _secondColor = secondColor;
    }
    
    public string GetPattern(){
        return _pattern;
    }

    public void SetPattern(string pattern){
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