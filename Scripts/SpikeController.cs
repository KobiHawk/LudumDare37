using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour
{
    public Sprite Spikes;
    public bool spikesEngaged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(spikesEngaged)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                player.TakeDamage();
            }
            else if(collision.transform.CompareTag("Enemy"))
            {
                WitchController witch = collision.GetComponent<WitchController>();
                witch.TakeDamage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!spikesEngaged && (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy")))
        {

            spikesEngaged = true;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = Spikes;
        }
    }
}
