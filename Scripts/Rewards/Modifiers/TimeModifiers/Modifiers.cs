using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public static class Modifiers
{
    public static Dictionary<string, Func<TimeModifier>> Ids = new()
    {
        {"DefaultSpeedModifier", ()=> new SpeedTimeModifier("DefaultSpeedModifier", 1.5f, NumericAction.Multiply)},
        {"StackModifier", ()=> new StackSizeTimeModifier("StackModifier", new IntModifier(1000, NumericAction.Add))},
        {"InfiniteTorchModifier", ()=> new TorchTimeModifier("InfiniteTorchModifier",new FloatModifier(100, NumericAction.Multiply))},
    };

    public static TimeModifier GetModifier(string id)
    {
        return Ids[id]();
    }
}
