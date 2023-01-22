using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public GameObject playerPrefab;
        public Text coinText;

        private GameObject deathPlayer;

        public GameObject loseText;
        public GameObject winText;

        public Vector3 target;

        [SerializeField]
        public int spawn = 1;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            if(FindObjectsOfType<GameManager>().Length > 1){
                Destroy(gameObject);
            }

            player = GameObject.Find("Player").GetComponent<PlayerController>();
            target = GameObject.Find("spawn1").transform.position;

            loseText.SetActive(false);
            winText.SetActive(false);
        }


        

        void Update()
        {
            coinText.text = coinsCounter.ToString();
            if(player.deathState == true)
            {
                playerGameObject.SetActive(false);
                deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                Destroy(lives[lives.Count-1]);
                lives.RemoveAt(lives.Count-1);
                if(lives.Count == 0){
                    allowedToMove = false;
                    loseText.SetActive(true);
                }
                else{
                    Invoke("ReloadLevel", 3);
                }
            }

            
        }

        public bool allowedToMove = true;

        public List<GameObject> lives;

        


        private void ReloadLevel()
        {   
            
            Destroy(deathPlayer);
            if(spawn == 1){
                Debug.Log("spawn1");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                //GameObject tmp = (GameObject)Instantiate(playerPrefab, GameObject.Find("spawn1").transform.position, Quaternion.identity);
                //tmp.SetActive(true);
                //GameObject.Find("Main Camera").GetComponent<CameraController>().player = tmp.transform;
                //player.transform.position = GameObject.Find("spawn1").transform.position;
                target = GameObject.Find("spawn1").transform.position;
            }
            if(spawn == 2){
                Debug.Log("spawn2");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                //GameObject tmp = (GameObject)Instantiate(playerPrefab, GameObject.Find("spawn1").transform.position, Quaternion.identity);
                //tmp.SetActive(true);
                //GameObject.Find("Main Camera").GetComponent<CameraController>().player = tmp.transform;
                //player.transform.position = GameObject.Find("spawn1").transform.position;
                target = GameObject.Find("spawn2").transform.position;
                
            }
            if(spawn == 3){
                Debug.Log("spawn3");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                //GameObject tmp = (GameObject)Instantiate(playerPrefab, GameObject.Find("spawn1").transform.position, Quaternion.identity);
                //tmp.SetActive(true);
                //GameObject.Find("Main Camera").GetComponent<CameraController>().player = tmp.transform;
                //player.transform.position = GameObject.Find("spawn1").transform.position;
                target = GameObject.Find("spawn3").transform.position;
                
            }
            
            else{
                Debug.Log("spawnNull");
            }


            playerGameObject.transform.position = target;
            playerGameObject.SetActive(true);
        }
    }
}
