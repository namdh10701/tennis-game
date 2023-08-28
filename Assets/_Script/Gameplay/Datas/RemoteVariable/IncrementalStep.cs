
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IncrementalStep
{
    public List<Step> Steps;
    public IncrementalStep()
    {
        Steps = new List<Step>
        {
            new Step(2, 3),
            new Step(3, 16),
            new Step(4, 36),
            new Step(5, 64),
            new Step(6, 100),
            new Step(7, 144),
            new Step(8, 196),
            new Step(9, 256),
            new Step(10, 324),
            new Step(11, 400)
        };
    }
}

[Serializable]
public class Step
{
    public int Incremental;
    public int TriggerScore;
    public Step(int incremental, int triggerScore)
    {
        Incremental = incremental;
        TriggerScore = triggerScore;
    }
}