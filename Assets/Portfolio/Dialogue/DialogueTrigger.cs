using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private bool startCutscene;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update() 
    {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) 
        {
            visualCue.SetActive(true);

            //if (InputHandler.InteractInput) 
            {
                //DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void StartCutscene()
    {
        startCutscene = true;
    }
}
