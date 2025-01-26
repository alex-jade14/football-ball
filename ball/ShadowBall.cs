using Godot;
using System;

public partial class ShadowBall : BallBase, IObserver
{
    public ShadowBall(BallInfo info) : base(info){}
    public void Update(Object data){
        if ((data as EventManager).state == 1){

        }
    }
}