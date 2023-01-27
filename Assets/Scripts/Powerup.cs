using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float speed;

    
    [SerializeField] // ID for Powerups     0 = Triple shot     1 = Speed     2 = Shields
    private int powerUpID;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // move down at a speed of 3
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // destroy it when it leaves the screen

        if(transform.position.y < -4)
        {
            Destroy(this.gameObject);
        }
    }

    // OntriggerCollision
    // only be colectable by the player
    // destroy when collected
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);

            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:                        
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
                
            }

            Destroy(this.gameObject);
        }
    }
}
