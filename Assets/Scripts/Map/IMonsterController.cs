using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterController
{
    void SetLevel(int level);
    void EndMonster();
    void OnAttack();
}