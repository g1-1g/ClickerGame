using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSpecTableSO", menuName = "Scriptable Objects/ItemSpecTableSO")]
public class ItemSpecTableSO : ScriptableObject
{
    public ItemSpecData[] Datas;
}