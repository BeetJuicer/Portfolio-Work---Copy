namespace StateMachineCore
{
using UnityEngine;

[CreateAssetMenu(fileName = "TD_PlayerData", menuName = "Scriptable Objects/TD_PlayerData")]
public class SO_TD_PlayerData : ScriptableObject
{
    public RollStateData rollStateData;
    public MoveStateData moveStateData;
}



[System.Serializable]
public struct RollStateData
{
    public float duration;
    public float speed;
}

[System.Serializable]
public struct MoveStateData
{
    public float speed;
}
}