using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private FPSMovement playerMovement;
    [SerializeField] private SM_FP_Basic stateMachine;

    private void Update()
    {
        if(dialogueManager.dialogueIsPlaying)
        {
            Cursor.lockState = CursorLockMode.None; // Locks cursor to center of screen
            Cursor.visible = true;                   // Hides the cursor
            playerMovement.DisableMovement();
            stateMachine.DisableLook();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Locks cursor to center of screen
            Cursor.visible = false;                   // Hides the cursor
            playerMovement.EnableMovement();
            stateMachine.EnableLook();
        }
    }
}
