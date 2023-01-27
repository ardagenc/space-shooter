using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int lives;
    private float canFire = 0f;
    private SpawnManager spawnManager;    

    private bool isTripleShotActive = false;
    [SerializeField] private GameObject tripleShotPrefab;

    private bool isSpeedBoostActive = false;
    [SerializeField] private GameObject speedBoostPrefab;
    [SerializeField] private float boostedSpeed;
    

    [SerializeField] private bool isShieldActive = false;

    // variable referance to the shield visualizer
    [SerializeField] private GameObject shieldObject;

    [SerializeField] private int score = 0;
    private UIManager uiManager;

    [SerializeField] private GameObject rightEngine, leftEngine;

    [SerializeField] private AudioClip laserSound;
    private AudioSource audioSource;

    // Mobile Controller 
    [SerializeField] private GameObject mobileController;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Button fireButton;
    
    
    
    void Start()
    {
#if UNITY_ANDROID
        mobileController.SetActive(true);
#endif

        transform.position = new Vector3(0, 0, 0);
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();

        if (spawnManager == null)
        {
            Debug.Log("spawn manager is null");
        }

        if(uiManager == null)
        {
            Debug.Log("ui manager is null");
        }

        if(audioSource == null)
        {
            Debug.Log("audio source is null");
        }
        else
        {
            audioSource.clip = laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        CalculateMovement();

#if UNITY_ANDROID

        if (Time.time > canFire)
        {
            fireButton.enabled = true;
        }
        else
        {
            fireButton.enabled = false;
        }

#else
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            FireLaser();
        }
#endif



    }

    void CalculateMovement()
    {
#if UNITY_ANDROID

        Vector3 joystickDirection = new Vector3(joystick.Horizontal,joystick.Vertical,0);

        if (isSpeedBoostActive == false)
        {
            transform.Translate(joystickDirection * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(joystickDirection * boostedSpeed * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

#else
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Horizontal movement
        // transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        // Vertical movement
        // transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        // More optimized code
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); 

        if(isSpeedBoostActive == false)
        {
            transform.Translate( direction * speed * Time.deltaTime );
        }
        else
        {
            transform.Translate(direction * boostedSpeed * Time.deltaTime );
        }

        // if player position on the y is greater than 0
        // y position = 0
        // else if position on the y is less than -4f
        // y pos = -4

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        // y position stays between -4 and 0 with Mathf.Clamp method
        // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 0), 0);

        // if player on the x > 11
        // x pos = -11
        // else if player on the x is less than -11
        // x pos = 11

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
#endif
    }
    
    public void FireLaser()
    {
        // hit space > spawn gameObject
        Vector3 laserOffset = new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z);
        canFire = Time.time + fireRate;

        // Instantiate triple shot
        if(isTripleShotActive == true)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, laserOffset, Quaternion.identity);
        }

        // play the laser audio clip

        audioSource.Play();
        


    }

    public void Damage()
    {
        // if shield is active, do nothing
        // deactivate shield
        shieldObject.SetActive(false);
        // return;
        if (isShieldActive == true)
        {
            // disable visualizer
            isShieldActive = false;
            return;
        }

        lives -= 1;

        if(lives == 2)
        {
            rightEngine.SetActive(true);
        }
        if(lives == 1)
        {
            leftEngine.SetActive(true);
        }


        uiManager.UpdateLives(lives);

        //check if dead
        //destroy
        if(lives < 1)
            
        {
            // Communicate with Spawn Manager
            spawnManager.OnPlayerDeath();
            // Let them know to stop spawning
            Destroy(this.gameObject);

            
        }
    }

    public void TripleShotActive()
    {
        isTripleShotActive= true;

        // start the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDown());
    }

    // IEnumerator tripleshotpowerdownroutine
    // wait five sec
    // set triple shot false
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);

        isTripleShotActive= false;
    }

    public void SpeedBoostActive()
    {
        isSpeedBoostActive= true;

        StartCoroutine(SpeedBoostDown());
    }

    IEnumerator SpeedBoostDown()
    {
        yield return new WaitForSeconds(5.0f);

        isSpeedBoostActive= false;
    }

    public void ShieldActive()
    {
        isShieldActive= true;

        //enable visualizer
        shieldObject.SetActive(true);
    }

    // method to add 10 to the score
    // communicate with the UI to update the score

    public void AddScore(int points)
    {
        score += points;
        uiManager.UpdateScore(score);
    }

}
