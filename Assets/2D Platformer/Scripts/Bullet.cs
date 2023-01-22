using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer{
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "wall"){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.tag == "Player"){
            col.gameObject.GetComponent<PlayerController>().deathState = true;
        }
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "wall"){
            Destroy(this.gameObject);
        }
        
    }
}}
