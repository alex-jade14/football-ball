using Godot;
using System;

public interface IPrototype
{
    GodotObject ShallowCopy();
    GodotObject DeepCopy();
}