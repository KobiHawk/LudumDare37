using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static float tileSize = 0.32f;
    public bool shielded = false;
    public LaserManager laserManager;
    public Sprite playerWithShield;
    public Sprite normalPlayer;
    private Animator animator;
    public AudioClip[] clips;
    private AudioSource playerAudio;
    public WitchController witch;
    public bool gameOver = false; // will be set by witch when she dies

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        float xDif = 0;
        float yDif = 0;
        //float xInput = Input.GetAxisRaw("Horizontal");
        //float yInput = Input.GetAxisRaw("Vertical");

        //no diagonal movement
        //if(yInput == -1) // down
        if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            yDif = -tileSize;
        }
        //else if(xInput == -1) // left
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            xDif = -tileSize;
        }
        //else if(yInput == 1) // up
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            yDif = tileSize;
        }
        //else if(xInput == 1) // right
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            xDif = tileSize;
        }
        //Debug.DrawLine(new Vector2(transform.position.x + (float)xDif/2, transform.position.y + (float)yDif/2),
        //    new Vector2(transform.position.x + (float)xDif, transform.position.y + (float)yDif));

        /*
         * Three parts
         * 1. Check to see if tile we want to move to has Ground (it exists in the game world)
         * 2. Check to make sure we want to move at all
         * 3. Check to see if tile we want to move to is a Laser (cant move into lasers)
         */
        if(Physics2D.Linecast(new Vector2(transform.position.x + xDif / 2, transform.position.y + yDif / 2)
            , new Vector2(transform.position.x + xDif, transform.position.y + yDif)
            , 1 << LayerMask.NameToLayer("Ground"))
            && (xDif != 0 || yDif != 0)
            && !Physics2D.Linecast(new Vector2(transform.position.x + xDif / 2, transform.position.y + yDif / 2)
            , new Vector2(transform.position.x + xDif, transform.position.y + yDif)
            , 1 << LayerMask.NameToLayer("Laser")))
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = true; // is set to false after taking damage, we need to reenable it upon moving

            transform.position = new Vector2(transform.position.x + xDif, transform.position.y + yDif);
            playerAudio.PlayOneShot(clips[3]);
            if (laserManager != null)
            {
                laserManager.takeStep();
            }
            if (witch != null)
            {
                witch.TakeStep();
            }
        }

    }

    public void TakeDamage()
    {
        if(!shielded)
        {
            playerAudio.PlayOneShot(clips[2]);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            shielded = false;
            animator.SetTrigger("NotShielded");
            playerAudio.PlayOneShot(clips[0]);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = normalPlayer;
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false; // prevent the player from being hit twice before moving
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Key"))
        {
            playerAudio.PlayOneShot(clips[1]);
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Application.Quit();
            }
        }
        else if(collision.transform.CompareTag("Shield"))
        {
            if(shielded)
            {
                return;
            }
            shielded = true;
            animator.SetTrigger("Shielded");
            playerAudio.PlayOneShot(clips[1]);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = playerWithShield;
            Destroy(collision.gameObject);
        }
        else if(collision.transform.CompareTag("Finish") && gameOver)
        {
            //playEndingCutscene();
            LevelSelectController.levelSelectID = 13;
            SceneManager.LoadScene(13);
        }
    }
}
