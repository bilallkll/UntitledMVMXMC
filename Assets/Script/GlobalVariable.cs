using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GlobalVar")]
public class GlobalVariable : ScriptableObject
{
    public LayerMask groundMask;
    public float drawMarkTreshold;
}
