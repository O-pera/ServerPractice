using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Manager Interfaces
public enum ManagerType {
    Network,

}
[Serializable]
public abstract class Manager { }
public interface IManagerStart { public void Start(); }
public interface IManagerUpdate { public void Update(); }
public interface IManagerOnApplicationQuit { public void OnApplicationQuit(); }
#endregion

public class Managers : MonoBehaviour {
    #region Singleton
    private Managers _instance;
    public Managers Instance { get { return _instance; } }
    #endregion

    #region Manager Defines

    public static NetworkManager Network { get; private set; } = new NetworkManager();

    private void Register() {
        _managers.Add((ushort)ManagerType.Network, Network);
        Register<NetworkManager>(Network);
    }

    #endregion

    #region Managers Declarations
    private Dictionary<ushort, Manager> _managers = new Dictionary<ushort, Manager>();
    private Action ManagerStarts            = null;
    private Action ManagerUpdates           = null;
    private Action ManagerOnApplicationQuit = null;


    private void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Register();
    }
    private void Start() {
        ManagerStarts.Invoke();
    }
    private void Update() {
        ManagerUpdates.Invoke();
    }
    private void OnApplicationQuit() {
        ManagerStarts = null;
        ManagerUpdates = null;
        ManagerOnApplicationQuit.Invoke();
        ManagerOnApplicationQuit = null;
    }

    private void Register<T>(T manager) {
        IManagerStart start = (IManagerStart)manager;
        IManagerUpdate update = (IManagerUpdate)manager;
        IManagerOnApplicationQuit quit = (IManagerOnApplicationQuit)manager;
        if(start != null) 
            ManagerStarts -= start.Start; ManagerStarts += start.Start;
        if(update != null) 
            ManagerUpdates -= update.Update; ManagerUpdates += update.Update;
        if(quit != null) 
            ManagerOnApplicationQuit -= quit.OnApplicationQuit; ManagerOnApplicationQuit += quit.OnApplicationQuit;
    }

    #endregion
}
