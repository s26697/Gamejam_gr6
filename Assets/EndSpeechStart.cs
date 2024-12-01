using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EndSpeechStart : MonoBehaviour
{
    private PlayableDirector timeline;
    public TimelineAsset timelineAsset;
    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Awake(){
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D otherd){
        Debug.Log("Collision detected!");
        if(otherd.gameObject.tag == "Player"){
            timeline.playableAsset = timelineAsset; // Przypisanie TimelineAsset
                timeline.Play();                       // Odtwarzanie Timeline
                Debug.Log("Timeline started!");
                gameObject.SetActive(false);          // Dezaktywacja obiektu
        }
    }
}
