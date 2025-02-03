using Godot;
using System;

public interface IPrototype
{
    GodotObject ShallowCopy(ShadowBall shadowBall);
    GodotObject DeepCopy(ShadowBall shadowBall);
}