using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {

    public PlayerController player;
    public LaserManager laserManager;
    public SpikeManager spikeManager;
    public HealthBarController healthBar;
    public int health = 10;
    private bool delay; // only moves every other step

	// Use this for initialization
	void Start () {
        new Random();
	}

    public void TakeDamage()
    {
        health--;
        healthBar.loseHealth();
        if(health <= 0)
        {
            laserManager.destroyLasers();
            spikeManager.retractSpikes();

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Attack");
            for(int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i]);
            }
            player.gameOver = true;

            //explosion here too maybe?
            Destroy(this.gameObject);
        }
    }

    public void TakeStep()
    {
        if(delay)
        {
            delay = false;
            return;
        }
        delay = true;

        float randomNumber = Random.value;
        float xDif = 0;
        float yDif = 0;

        if (transform.position.y > player.transform.position.y)
        {
            //down
            Debug.Log("Moving Down.");
            yDif = -PlayerController.tileSize;
        }
        else if (transform.position.y < player.transform.position.y)
        {
            //up
            Debug.Log("Moving Up.");
            yDif = PlayerController.tileSize;
        }

        if (transform.position.x > player.transform.position.x)
        {
            //left
            Debug.Log("Moving Left");
            xDif = -PlayerController.tileSize;
        }
        else if (transform.position.x < player.transform.position.x)
        {
            //right
            Debug.Log("Moving Right");
            xDif = PlayerController.tileSize;
        }

        if(randomNumber >= 0.5)
        {
            //check vertical first
            if (Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + yDif / 2)
            , new Vector2(transform.position.x, transform.position.y + yDif)
            , 1 << LayerMask.NameToLayer("Ground"))
            && !Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + yDif / 2)
            , new Vector2(transform.position.x, transform.position.y + yDif)
            , 1 << LayerMask.NameToLayer("Laser")))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + yDif);
                xDif = 0; // we moved, so we don't want to move anymore
            }

            yDif = 0;
        }
        else
        {
            //check horizontal first
            if (Physics2D.Linecast(new Vector2(transform.position.x + xDif / 2, transform.position.y)
            , new Vector2(transform.position.x + xDif, transform.position.y)
            , 1 << LayerMask.NameToLayer("Ground"))
            && !Physics2D.Linecast(new Vector2(transform.position.x + xDif / 2, transform.position.y)
            , new Vector2(transform.position.x + xDif, transform.position.y)
            , 1 << LayerMask.NameToLayer("Laser")))
            {
                transform.position = new Vector2(transform.position.x + xDif, transform.position.y);
                yDif = 0; // we moved, so we don't want to move anymore
            }

            xDif = 0;
        }

        //whichever direction we checked first has already tried to move
        //if it was unable, xDif/yDif will still have a value
        transform.position = new Vector2(transform.position.x + xDif, transform.position.y + yDif);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.TakeDamage();
        }
    }
}
