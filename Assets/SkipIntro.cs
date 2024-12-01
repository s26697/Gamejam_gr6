using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkipIntro : MonoBehaviour
{
    PlayerInputActions inputActions;
    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.SkipIntro.Skip.performed += Skip;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.SkipIntro.Skip.performed -= Skip;
    }

    void Skip(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Level 1");
    }
}
