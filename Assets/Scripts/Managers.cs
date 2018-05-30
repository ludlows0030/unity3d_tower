using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {

    public static MissionManager Mission { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Mission = GetComponent<MissionManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Mission);

        StartCoroutine(StartupManagers());

    }

    private IEnumerator StartupManagers()
    {
        foreach(IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in _startSequence)
            {
                if(manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS,numReady,lastReady);
                yield return null;
            }

            Debug.Log("All managers started up");
            Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
        }
    }
}
