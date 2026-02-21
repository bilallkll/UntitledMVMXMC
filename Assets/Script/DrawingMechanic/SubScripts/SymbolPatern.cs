using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable/SymbolPatern")]
public class SymbolPatern : ScriptableObject
{
    public int[] symbolPatern;
    public bool isThisSymbol = true;
    public int symbolEvent;

}
