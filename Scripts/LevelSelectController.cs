using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour {

    public static int levelSelectID = 0;

    public void loadLevel(int levelIndex)
    {
        if(levelIndex == 13)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(levelIndex);
        }
    }

    public void loadLevelSelect()
    {
        SceneManager.LoadScene(levelSelectID);
    }
}
