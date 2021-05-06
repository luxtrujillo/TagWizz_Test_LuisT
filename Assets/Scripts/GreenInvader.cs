using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenInvader : Invader
{
    private void OnEnable()
    {
        hp = 1;
        score = 1;
    }
}
