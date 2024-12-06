using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCScript : MonoBehaviour
{
    [SerializeField] GameObject infoCanvas;
    [SerializeField] GameObject speechAlert;
    [SerializeField] GameObject mapIcon;

    //List holding all the possible texts for the npc.
    [SerializeField] TMP_Text textUi;
    [SerializeField] TMP_Text nameUi;

    //Pointer towards current text
    private int currentText;

    [SerializeField] GameObject nextPrompt;

    [SerializeField] private InputActionReference interact;

    private bool isInside;

    Dialogue dialogue;

    void Start()
    {
        nameUi.text = name;
        if(mapIcon) mapIcon.SetActive(true);
    }
    private void OnEnable() {
        currentText = 0;
        interact.action.performed += NextText;
    }

    private void OnDisable() {
        interact.action.performed -= NextText;
    }

    private void NextText(InputAction.CallbackContext ctx) {

        if(isInside)
        {
            if(dialogue == null)
            {
                dialogue = NpcManager.instance.GetDialogue(name);
            }

            if (infoCanvas.activeSelf)
            {   
                currentText ++;

                if(currentText == dialogue.texts.Length) {
                    dialogue = NpcManager.instance.GetDialogue(name);
                    currentText = 0;
                    EnableText(false);
                    DataPersistenceManager.instance.SaveGame();
                }

                textUi.text = dialogue.texts[currentText];

            } else 
            {
                EnableText(true);
                currentText = 0;
            }
        }
    }

    private void EnableText(bool input) {
        infoCanvas.SetActive(input);
        speechAlert.SetActive(!input);

        if(input == true)
        {
            if(dialogue == null) dialogue = NpcManager.instance.GetDialogue(name);
            textUi.text = dialogue.texts[currentText];
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            EnableText(true);
            isInside = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnableText(false);
            isInside = false;
            currentText = 0;
        }
    }
}
