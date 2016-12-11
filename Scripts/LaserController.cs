using UnityEngine;
using System.Collections;

public enum Direction
{
    DOWN,
    LEFT,
    UP,
    RIGHT
}

public class LaserController : MonoBehaviour {

    public Sprite[] sprites;
    public int initialStep;
    public int resetTo;
    private int currentStep;
    public Direction direction;
    public GameObject laserBeam;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = sprites[initialStep];
        currentStep = initialStep;
        
        switch(direction)
        {
            case Direction.RIGHT:
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                break;

            case Direction.DOWN:
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
                break;

            //UP and LEFT are the defaults for their respective laser sprites
        }
	}
	
    public void takeStep()
    {
        currentStep++;
        GetComponent<SpriteRenderer>().sprite = sprites[currentStep];
        if (currentStep == sprites.Length - 1)
        {
            currentStep = resetTo;

            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        float offSetX = transform.position.x;
        float offSetY = transform.position.y;
        int theStep = currentStep;
        GameObject[] spritesPlaced = new GameObject[10];
        float xDif = 0;
        float yDif = 0;
        
        switch(direction)
        {
            case Direction.DOWN:
                yDif = -1 * PlayerController.tileSize;
                break;
            case Direction.UP:
                yDif = 1 * PlayerController.tileSize;
                break;
            case Direction.LEFT:
                xDif = -1 * PlayerController.tileSize;
                break;
            case Direction.RIGHT:
                xDif = 1 * PlayerController.tileSize;
                break;
        }

        //fire laser
        int i = 0;
        while ((Physics2D.Linecast(new Vector2(transform.position.x + xDif / 2, transform.position.y + yDif / 2)
            , new Vector2(transform.position.x + xDif, transform.position.y + yDif)
            , 1 << LayerMask.NameToLayer("Ground"))))
        {
            spritesPlaced[i] = (GameObject)Instantiate(laserBeam, new Vector2(transform.position.x + xDif, transform.position.y + yDif), Quaternion.identity);
            if (direction == Direction.UP || direction == Direction.DOWN)
            { 
                spritesPlaced[i].transform.Rotate(Vector3.forward * -90);
            }

            transform.position = new Vector2(transform.position.x + xDif, transform.position.y + yDif);
            i++;
        }
        transform.position = new Vector2(offSetX, offSetY);

        yield return new WaitUntil(() => currentStep > theStep);

        for(i = 0; i < spritesPlaced.Length; i++)
        {
            if(spritesPlaced[i] != null)
            {
                Destroy(spritesPlaced[i]);
            }
        }
    }
}
