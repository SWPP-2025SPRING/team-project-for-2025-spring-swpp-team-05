using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterController
{
    void SetLevel(int level);
    IEnumerator StartMonster();
    IEnumerator EndMonster();
    void OnAttack();
    void OnAttacked();
}