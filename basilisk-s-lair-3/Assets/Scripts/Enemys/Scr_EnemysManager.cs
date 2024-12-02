using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemysManager : MonoBehaviour
{
    public EnemyData _enemyData;

    public static Scr_EnemysManager _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void ClearList()
    {
        _enemyData.DeadEnemys.Clear();
    }

    private void OnApplicationQuit()
    {
        ClearList();
    }
}
