using UnityEngine;
using System.Collections;

public class SpikeManager : MonoBehaviour {

    public SpikeController[] spikes;
    public Sprite spikesDisengaged;

	public void retractSpikes()
    {
        for(int i = 0; i < spikes.Length; i++)
        {
            SpriteRenderer renderer = spikes[i].GetComponent<SpriteRenderer>();
            renderer.sprite = spikesDisengaged;
            spikes[i].spikesEngaged = false;
            Destroy(spikes[i]);
        }
    }
}
