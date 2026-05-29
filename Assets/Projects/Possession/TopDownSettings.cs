namespace StateMachineCore
{
using UnityEngine;

public class TopDownSettings : MonoBehaviour
{
    private void Awake()
    {
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0, 1, 0);
    }
}

}