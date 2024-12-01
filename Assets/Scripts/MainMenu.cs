using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _creditsview;
    [SerializeField] private GameObject _instructionview;

    private void Awake() {
        _mainView.SetActive(true);
        _creditsview.SetActive(false);
        _instructionview.SetActive(false);
    }

    
    public void PlayClicked() {
        _mainView.SetActive(false);
        LoadLevel("NapisyStartowe");
    }

    public void ExitClicked() {
                Application.Quit();
    }
    public void BackClicked() {
        _mainView.SetActive(true);
        _creditsview.SetActive(false);
        _instructionview.SetActive(false);
    }
    public void CreditsClicked() {
        _mainView.SetActive(false);
        _creditsview.SetActive(true);
        _instructionview.SetActive(false);
    }
    public void InstructionsClicked() {
        _mainView.SetActive(false);
        _creditsview.SetActive(false);
        _instructionview.SetActive(true);
    }
    public void LoadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    
}