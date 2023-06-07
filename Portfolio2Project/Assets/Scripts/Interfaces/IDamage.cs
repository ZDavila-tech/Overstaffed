using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(int dmg);
    void Freeze(float duration);
    void Burn(float duration, float timeBetween);

    void Poison(float duration, float timeBetween);
}
