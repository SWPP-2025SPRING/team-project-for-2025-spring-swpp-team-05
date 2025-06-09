using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoomController
{
    void SetLevel(int level);
    IEnumerator StartRoom();
    IEnumerator EndRoom();
}