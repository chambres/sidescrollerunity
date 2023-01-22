 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 1f; 
        public LayerMask ground;
        public LayerMask wall;

        private Rigidbody2D rigidbody; 
        public Collider2D triggerCollider;
        
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            if(gameObject.name == "sold"){
                InvokeRepeating("shoot", 0.0f, 0.5f*3);
            }
            MuzzleFlash = GameObject.Find("MuzzleFlash").GetComponent<SpriteRenderer>();
            Debug.Log(MuzzleFlash);
        }

        public GameObject b;
        SpriteRenderer MuzzleFlash;

        void shoot(){
            MuzzleFlash.enabled = true;
            GameObject bullet = Instantiate(b, GameObject.Find("BulletSpawner").transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed*2, 0);
            MuzzleFlash.enabled = false;
            //Destroy(bullet, 2);
        }

        void Update()
        {
            rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
        }

        void FixedUpdate()
        {
            if(!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }
        
        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
        }
        void delete(){
            Debug.Log(gameObject.name);
            if(gameObject.name == "sold"){
                CancelInvoke("shoot");
                return;
                
            }
            Destroy(this.gameObject);
        }
    }
}
