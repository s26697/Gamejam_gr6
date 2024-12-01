using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDailog : MonoBehaviour
{
    
    [SerializeField] private bool isInteractable;
    [SerializeField] private float typeDelay;
    [SerializeField] private string[] dialog;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject continueButton;
    
    private int _index;
    private bool _playerIsClose;
    private bool _dialogIsOpen;
    private bool _isTyping;
    private Coroutine _typingCoroutine;

    private void Start()
    {
        
        if(dialogPanel == null) return;
        dialogPanel.SetActive(false);
        _dialogIsOpen = false;
    }

    public void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.E) && _playerIsClose)
        {
            if (_dialogIsOpen)
            {
                ExitDialog();
            }
            else
            {
                OpenDialog();
            }
        }
        else if(_dialogIsOpen && Input.GetKeyDown(KeyCode.Return))
        {
            if (_isTyping)
            {
                StopTyping();
                dialogText.text = dialog[_index];
            }
            else
                NextPhrase();
        }
    }

     private void ExitDialog()
     {
         StopTyping();
         

         dialogPanel.SetActive(false);
         _dialogIsOpen = false;
         _index = 0;
         dialogText.text = "";
    }
     
     private void OpenDialog()
     {
         _dialogIsOpen = true;
         dialogPanel.SetActive(true);
         _typingCoroutine = StartCoroutine(Typing());
         
     }

    public void NextPhrase()
    {
        
        
        StopTyping();
        
        if (_index < dialog.Length - 1)
        {
            _index++;
            dialogText.text = "";
            _typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            ExitDialog();
        }
    }

    private void StopTyping()
    {
        _isTyping = false;
        continueButton.SetActive(true);
        if (_typingCoroutine == null) return;
        
        StopCoroutine(_typingCoroutine);
        _typingCoroutine = null;
    }
    
    private IEnumerator Typing()
    {
        _isTyping = true;
        continueButton.SetActive(false);
        foreach (var letter in dialog[_index])
        {
            if(letter != ' ')
                yield return new WaitForSeconds(typeDelay);
            dialogText.text += letter;
        }
        _isTyping = false;
        continueButton.SetActive(true);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            _playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            _playerIsClose = false;
            ExitDialog();
        }
    }
    
    private void OnDestroy()
    {
        
        if (_typingCoroutine == null) return;
        
        StopCoroutine(_typingCoroutine);
        _typingCoroutine = null;
    }
    
    
}
