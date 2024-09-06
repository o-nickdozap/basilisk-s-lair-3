using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemysManager : MonoBehaviour
{
    public EnemyData _enemyData;

    void ClearList()
    {
        _enemyData.DeadEnemys.Clear();
    }

    private void OnApplicationQuit()
    {
        ClearList();
    }
}
