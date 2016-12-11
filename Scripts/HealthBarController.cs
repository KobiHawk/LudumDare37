using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    int currHealth = 10;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[currHealth];
    }

    public void loseHealth()
    {
        currHealth--;
        if(currHealth < 0)
        {
            return;
        }
        spriteRenderer.sprite = sprites[currHealth];
    }
}
