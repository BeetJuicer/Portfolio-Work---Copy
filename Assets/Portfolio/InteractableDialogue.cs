using UnityEngine;


[RequireComponent(typeof(DialogueCaller))]
public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string defaultLayer = "Default";
    [SerializeField] private string highlightLayer = "Interactable-On";
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

    public void OnHighlight() => gameObject.layer = LayerMask.NameToLayer(highlightLayer);
    public void OffHighlight() => gameObject.layer = LayerMask.NameToLayer(defaultLayer);
}
