using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
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
