using UnityEngine;

[CreateAssetMenu(fileName = "BulletPreset", menuName = "Scriptable Objects/BulletPreset")]
public class SO_BulletPreset : ScriptableObject
{
    public SO_BulletWave    wave;
    public SO_BulletPattern pattern;
    [TextArea] public string description;
}
