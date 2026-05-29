using CommandPattern;
using UnityEngine;

public class TimeReversalSpriteFX : MonoBehaviour
{
    [SerializeField] private GameObject visuals;

    private void Update()
    {
        visuals.SetActive(TimeManager.Instance.IsReversing);
    }
}
