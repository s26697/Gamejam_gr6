using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public float speed = 0.1f;
    public string text;
    private string currentText = "";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText(){
        for(int i = 0; i < text.Length; i++){
            currentText = text.Substring(0, i);
            this.GetComponent<TMPro.TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(speed);
        }
    }
}
