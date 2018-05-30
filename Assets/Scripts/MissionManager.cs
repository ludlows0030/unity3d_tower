using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status
    {
        get;
        private set;
    }

    public int curLevel { get; private set; }
    

    public void Startup()
    {
        Debug.Log("Misson manager starting...");

        curLevel = 0;

        status = ManagerStatus.Started;
    }

    public void GoToNext()
    {
        curLevel++;
        SceneManager.LoadScene("Level");
        Debug.Log("Loading " + name);
    }
}
