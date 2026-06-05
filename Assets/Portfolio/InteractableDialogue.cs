using UnityEngine;


[RequireComponent(typeof(DialogueCaller))]
public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private LayerMask defaultLayer;
    [SerializeField] private LayerMask highlightLayer;
    private DialogueCaller dialogue;
    public string InteractLabel = "Hello";
    
    private void Start()
    {
        dialogue = GetComponent<DialogueCaller>();
    }

    public void OnInteract()
    {
        dialogue.StartDialogue();
    }

    public void OnHighlight() => gameObject.layer = highlightLayer;
    public void OffHighlight() => gameObject.layer = defaultLayer;
}
