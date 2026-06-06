using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; private set { instance = value; }  }
     private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private FPSMovement playerMovement;
    [SerializeField] private SM_FP_Basic stateMachine;

    public bool DialogueIsPlaying => dialogueManager.dialogueIsPlaying;

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
