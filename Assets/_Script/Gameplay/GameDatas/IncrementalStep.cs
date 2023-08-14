using JetBrains.Annotations;
using System;
using System.Collections.Generic;

[Serializable]
public class IncrementalStep
{
    public List<Step> Steps;
    public IncrementalStep()
    {
        Steps = new List<Step>
        {
            new Step(2, 4),
            new Step(3, 9),
            new Step(4, 15),
            new Step(5, 26),
            new Step(6, 41),
            new Step(7, 61),
            new Step(8, 86),
            new Step(9, 111),
            new Step(8, 141)
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