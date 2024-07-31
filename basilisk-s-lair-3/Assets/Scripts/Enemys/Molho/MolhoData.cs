using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "molhoData", menuName = "ScriptableObjects/molhoData", order = 1)]
public class MolhoData : ScriptableObject
{
    public List<string> DeadMolhos = new List<string>();
}