using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughBody : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        float alpha = sr.color.a;
        Color tmp = sr.color;

        if (collision.gameObject.tag == "Player")
        {
            while(sr.color.a > 0){
                alpha -= 0.1f;
                tmp.a = alpha;
                sr.color = tmp;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private IEnumerator OnTriggerExit2D(Collider2D collision)
    {
        float alpha = sr.color.a;
        Color tmp = sr.color;

        if (collision.gameObject.tag == "Player")
        {
            while(sr.color.a < 1){
                alpha += 0.1f;
                tmp.a = alpha;
                sr.color = tmp;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
