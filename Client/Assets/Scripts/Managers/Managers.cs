using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour{
    #region Singleton
    private Managers _instance;
    public Managers Instance { get { return _instance;  } }
    #endregion

    private void Awake() {
        if(_instance != null) { 
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static NetworkManager Network { get; private set; } = new NetworkManager();

    private void Start() {
        Network.Start();
    }

    private void Update() {
        Network.Update();
    }

    private void OnApplicationQuit() {
        Network.OnApplicationQuit();
    }
}
