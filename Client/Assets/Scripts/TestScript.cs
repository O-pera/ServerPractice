using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestScript : MonoBehaviour{
    public bool isStarted = false;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);    
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(isStarted == false) {
                StartCoroutine("CoSendPacket");
                isStarted = true;
            }
            else {
                StopCoroutine("CoSendPacket");
                isStarted = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.A)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    IEnumerator CoSendPacket() {
        C_Enter enterPacket = new C_Enter();
        Managers.Network.Send(enterPacket);
        yield return null;
    }
}