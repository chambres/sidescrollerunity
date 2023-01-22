using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
public class FlagChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameManager gameManager;

    void endofanim()
    {
        if(gameObject.name == "flag3"){
            gameManager.allowedToMove=false;
            gameManager.winText.SetActive(true);
        }
        else{
            return;
        }
    }
}}
