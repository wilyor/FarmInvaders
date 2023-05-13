using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBar : ProgressBarManager
{

    public override void InitializeBar(float maxVal)
    {
        maxValue = maxVal;
        SetValue(0);
    }

}
