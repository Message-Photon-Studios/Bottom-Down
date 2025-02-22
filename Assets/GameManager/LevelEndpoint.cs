using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEndpoint : MonoBehaviour
{
    [SerializeField] string goToLevel;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] GameObject canvas;

    [SerializeField] Sprite[] LoadingSprites;

    [SerializeField] bool startRun = false;

    private UIController uiController;
    bool enableExit = false;

    void OnEnable()
    {
        uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        if(interactAction)
            interactAction.action.performed += ExitLevel;
    }

    void OnDisable()
    {
        if(interactAction)
            interactAction.action.performed -= ExitLevel;
    }

    void ExitLevel (InputAction.CallbackContext ctx)
    {
        if(!enableExit) return;
        if(startRun) GameManager.instance.SetStartRun();
        LevelManager.instance.EndLevel(goToLevel);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
            enableExit = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(false);
            enableExit = false;
        }
    }
}
