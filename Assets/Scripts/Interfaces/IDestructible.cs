using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
    void Init();
    void Damage();
    void Death();
}
