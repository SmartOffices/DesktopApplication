using UnityEngine;

using System.Collections;

using System.Collections.Generic;

using System.Text;
using System;
using NetworksApi.TCP.CLIENT;


public class socketScript : MonoBehaviour
{
    SmartOfficeMetro.SmartOfficeClient unity_client; 
    bool connected;
    void Awake()
    {

        //add a copy of TCPConnection to this game object
        if(Application.isPlaying)
        {
           // unity_client = new SmartOfficeMetro.SmartOfficeClient();
            unity_client = gameObject.AddComponent<SmartOfficeMetro.SmartOfficeClient>();
            connected = true;
            Debug.Log("Connected to server");
        }
        else
        {
            
            unity_client.StopAllCoroutines();
            unity_client = null;
        }
       

    }



    void Start()
    {

    }

    void OnApplicationQuit()
    {
        Debug.Log("Application closed");
        Network.Disconnect();
        MasterServer.UnregisterHost();
        SmartOfficeMetro.SmartOfficeClient.Disconnect();
        unity_client = null;
    }
}
