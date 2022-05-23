using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour{
    #region Managers class Essential Instances
    public static Managers Instance { get; private set; } = new Managers();

    private Dictionary<int, ManagerClass> _managerClasses = new Dictionary<int, ManagerClass>();
    private Action _initializeManagers = null;
    private Action _updateManagers = null;

    #endregion


    #region Manager Objects Management Area
    /// <summary>
    /// 여기서 매니저에 대한 선언을 작성한 후 Register에서 AddManager함수를 통해 
    /// Managers의 관리 하에 두어야 작동할 수 있습니다.
    /// </summary>

    public NetworkManager Network { get; private set; } = new NetworkManager();
    public SessionManager Session { get; private set; } = new SessionManager();

    private void Register() {
        AddManager(Network);
        AddManager(Session);
    }

    private void AddManager(ManagerClass manager) {
        _initializeManagers -= manager.Initialize;
        _initializeManagers -= manager.Initialize;

        if(manager.Type == ManagerType.Updatable) {
            _updateManagers -= manager.Update;
            _updateManagers += manager.Update;
        }
    }

    #endregion




    #region ManagerClass Management Functions



    #endregion

    #region Unity LifeCycle Functions

    private void Awake() {
        gameObject.name = "@Managers";

        DontDestroyOnLoad(gameObject);

    }


    void Start() {
        _initializeManagers.Invoke();
    }

    void Update() {
        _updateManagers.Invoke();
    }

    #endregion






}
