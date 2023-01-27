using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject laser;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] AudioClip explosionSound;
    AudioSource audioSource;


    private Player player;
    // handle to animator component

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource= GetComponent<AudioSource>();
        // null check
        if(player == null)
        {
            Debug.Log("the player is null");
        }
        //assign the component

        if(anim == null)
        {
            Debug.Log("animator is null");
        }

        if(audioSource == null)
        {
            Debug.Log("audio source is null");
        }
        else
        {
            audioSource.clip = explosionSound;
        }



    }
    void Update()
    {
        // move down at 4
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // if bottom of screen, respawn at top with a new random x position
        if(transform.position.y < -4)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 11, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if other is player, damage player n destroy us

        if(other.tag == "Player")
        {
            // damage player
            // null check
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();     
            }
            // trigger anim
            anim.SetTrigger("OnEnemyDeath");
            audioSource.Play();
            speed = 0;
            boxCollider.enabled = false;
            Destroy(gameObject, 2.8f);
        }

        // if other is laser,  laser n destroy us

        if (other.tag == "Laser")
        {            
            Destroy(other.gameObject);

            // add 10 to score
            if(player != null)
            {
                player.AddScore(10);
            }

            // trigger anim
            anim.SetTrigger("OnEnemyDeath");
            audioSource.Play();
            speed = 0;
            boxCollider.enabled = false;
            Destroy(gameObject, 2.8f);
        }


    }
}
