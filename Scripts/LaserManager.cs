using UnityEngine;
using System.Collections;

public class LaserManager : MonoBehaviour {

    public LaserController[] lasers;

    public void takeStep()
    {
       
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].takeStep();
        }
    }

    public void destroyLasers()
    {
        for(int i = 0; i < lasers.Length; i++)
        {
            //make an explosion at lasers[i].transform.position
            Destroy(lasers[i]);
        }
    }
}
