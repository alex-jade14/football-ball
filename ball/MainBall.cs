using Godot;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase
{
   public EventManager events;

   public MainBall(BallInfo info) : base(info){}
}