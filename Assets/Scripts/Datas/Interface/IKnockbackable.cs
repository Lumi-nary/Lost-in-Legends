using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockBack(Vector2 direction, float force);
}
