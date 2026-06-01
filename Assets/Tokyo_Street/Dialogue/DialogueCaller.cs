using UnityEngine;

public class DialogueCaller : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    public void StartDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
