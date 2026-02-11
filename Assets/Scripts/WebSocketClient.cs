using UnityEngine;
using NativeWebSocket;
using UnityEngine.Events;
using System;
// using System.Diagnostics;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class WebSocketClient : MonoBehaviour
{
    private WebSocket websocket;

    [Header("ESP32 WebSocket Server")]
    public string serverIP = "10.204.0.55"; 
    public int serverPort = 8081;

    async void Start()
    {
        websocket = new WebSocket($"ws://{serverIP}:{serverPort}");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connected to ESP32 WebSocket server");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("WebSocket Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("WebSocket closed");
        };

        websocket.OnMessage += (bytes) =>
        {
            string msg = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received from ESP32: " + msg);

            if (msg == "PAUSE") {
                TogglePause(); 
            }
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    private async void OnDestroy()
    {
        if (websocket != null)
            await websocket.Close();
    }

    // ---------------------------------------------------------
    // PUBLIC METHOD YOU CALL WHEN THE PLAYER CRASHES
    // ---------------------------------------------------------
    public async void SendCrash()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("CRASH");
            Debug.Log("Sent: CRASH");
        }
        else
        {
            Debug.LogWarning("WebSocket not connected");
        }
    }
    
    private void TogglePause()
    {
        var gm = FindAnyObjectByType<GameManager>();
        if (gm == null) return;

        if (gm.isPaused)
            gm.ResumeGame();
        else
            gm.PauseGame();
}

    
    
}


