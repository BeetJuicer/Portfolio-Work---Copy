using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject interactUI;

    public UnityEvent onInteract;
    public event Action onEnterTrigger;
    public event Action onExitTrigger;

    private bool targetInTrigger;

    public virtual string InteractLabel => "Press E";

    private void Start()
    {
        interactUI.SetActive(false);
    }
    private void Update()
    {
        if (targetInTrigger && Input.GetKeyDown(interactKey))
        {
            onInteract?.Invoke();
            interactUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, targetLayer)) return;

        targetInTrigger = true;
        if (interactUI != null) interactUI.SetActive(true);
        onEnterTrigger?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, targetLayer)) return;

        targetInTrigger = false;
        if (interactUI != null) interactUI.SetActive(false);
        onExitTrigger?.Invoke();
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}