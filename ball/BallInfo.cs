using Godot;
using System;

public partial class BallInfo
{
    private String _name { get; set; }
    private String _brand { get; set; }
    private String _model { get; set; }
    private BallSize _size { get; set; }
    private BallPhysicsParameters _physicsParameters{ get; set; }
}