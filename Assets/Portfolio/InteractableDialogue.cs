using UnityEngine;


[RequireComponent(typeof(DialogueCaller))]
public class InteractableDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string defaultLayer = "Interactable-Off";
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

    public void OnHighlight() => SetLayerRecursively(gameObject, LayerMask.NameToLayer(highlightLayer));
    public void OffHighlight() => SetLayerRecursively(gameObject, LayerMask.NameToLayer(defaultLayer));

    //This project uses the outlines package, and that thing works through layers.
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
