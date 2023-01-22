using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();




            transform.position = gameManager.target;
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void FunctionToGetRidOfTile()
        {
            Destroy(GameObject.Find("spikestoberemoved"));
        //Debug.Log("Tile removed");
        }

        bool jumpedOn=false;
        void Update()
        {   

            // Tilemap tilemap = GameObject.Find("spikes").GetComponent<Tilemap>();
            // Debug.Log(tilemap);
            // Vector3Int position = new Vector3Int(-26, 31, 0);
            // tilemap.SetTile(position, null);
            if(gameManager.allowedToMove){
                if (Input.GetButton("Horizontal")) 
                {
                    moveInput = Input.GetAxis("Horizontal");
                    Vector3 direction = transform.right * moveInput;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                    animator.SetInteger("playerState", 1); // Turn on run animation
                }
                else
                {
                    if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
                }
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                }
                if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation

                if(facingRight == false && moveInput > 0)
                {
                    Flip();
                }
                else if(facingRight == true && moveInput < 0)
                {
                    Flip();
                }

                if(jumpedOn){
                    rigidbody.AddForce(transform.up * 5f, ForceMode2D.Impulse);
                    jumpedOn = false;
                }

                checkenemykill();

                if(gameManager.coinsCounter == 9 && !removedTiles)
                {
                    FunctionToGetRidOfTile();
                    removedTiles=true;
                }
            }
        }

        bool removedTiles = false;

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        //write a function that checks if i jumped on an enemy
        private void checkenemykill()
        {
            float depth = .9f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, depth, LayerMask.GetMask("enemy"));
            RaycastHit2D hitspikes = Physics2D.Raycast(transform.position, Vector2.down, depth, LayerMask.GetMask("spikes"));
            if (hitspikes)
            {
                deathState = true;
            }

            if (hit)
            {
                Debug.Log(hit.collider.name);
                Debug.Log("hit enemy");

                //rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

                Destroy(hit.collider.gameObject.GetComponent<Collider2D>());

                hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

                hit.collider.gameObject.GetComponent<EnemyAI>().GetComponent<Animator>().SetTrigger("explode");
                tmp = hit.collider.gameObject;
                jumpedOn = true;
                return;
            }
        }

        public Grid grid;
        //public Tilemap tilemap;
        
        

        GameObject tmp;



        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.tag == "Enemy" || other.gameObject.name == "spikes")
            {
                if(other.gameObject == tmp){
                    return;
                }
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
            if(other.gameObject.name == "flag1"){
                other.gameObject.GetComponent<Animator>().SetTrigger("flagup");
                other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameManager.spawn = 2;
            }
            if(other.gameObject.name == "flag2"){
                other.gameObject.GetComponent<Animator>().SetTrigger("flagup");
                other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameManager.spawn = 3;
            }
            if(other.gameObject.name == "flag3"){
                other.gameObject.GetComponent<Animator>().SetTrigger("flagup");
                other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameManager.spawn = 3;
            }

        }

        private void OnDrawGizmos()
    {

    }
    }
}
