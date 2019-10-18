using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.StalkerProject.Stalker
{
    public class Escaper : MonoBehaviourPunCallbacks
    {
        //fields
        [SerializeField]
        DataExchange de;
        [SerializeField]
        private Timer t;
        [SerializeField]
        Snapshot ss;
        [SerializeField]
        Select st;
        [SerializeField]
        Text debugText;

        [SerializeField]
        GameObject canvas5;
        [SerializeField]
        Text searchText;
        [SerializeField]
        GameObject canvas5_1;
        [SerializeField]
        GameObject canvas5_2;
        [SerializeField]
        Image img;
        [SerializeField]
        GameObject canvas5_3;
        [SerializeField]
        Text waitText;
        [SerializeField]
        GameObject canvas6;
        [SerializeField]
        GameObject canvas7;
        [SerializeField]
        GameObject canvas9;
        [SerializeField]
        GameObject canvas9_1;

        [System.NonSerialized]
        public bool escaperActive = false;
        private bool foundPartner = false;
        private bool timerStarted = false;

        string gameVersion = "1";

        public void Connect()
        {
            if(!escaperActive)
            {
                Debug.Log("Escaper: Connect() called.");
                print("Path test: " + Application.dataPath);
                de.NewGame();
                escaperActive = true;
                PhotonNetwork.AutomaticallySyncScene = true;

                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
                UpdateSearchText();
            }
            else
            {
                Debug.LogError("Escaper: Connect() called even though Escaper already active");
            }
        }

        public override void OnConnectedToMaster()
        {
            if(escaperActive)
            {
                Debug.Log("Escaper: OnConnectedToMaster() was called by PUN");
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            escaperActive = false;
            Debug.LogWarningFormat("Escaper: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if(escaperActive)
            {
                Debug.Log("Escaper: OnJoinRandomFailed() was called by PUN.\nReturnCode: " + returnCode + " Message: " + message);
                Invoke("RetryAfterDelay", 2);
            }
            else
            {
                Debug.LogError("Escaper: OnJoinRandomFailed() called, even though Escaper not active");
            }
        }

        private void RetryAfterDelay()
        {
            if(escaperActive)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinedRoom()
        {
            if(escaperActive)
            {
                Debug.Log("Escaper: OnJoinedRoom() called by PUN.");
                foundPartner = true;
                int x = PhotonNetwork.PlayerList.Length;
                Debug.Log("Count in room: " + x);
                Invoke("eSendGPS", 2);
            }
        }

        public void Disconnect()
        {
            if(escaperActive)
            {
                escaperActive = false;
                foundPartner = false;
                timerStarted = false;
                t.End();
                PhotonNetwork.Disconnect();
            }
            else
            {
                Debug.LogError("Escaper: Disconnect() called even though Escaper already not active");
            }
        }

        public void eGameOverWon()
        {
            if(escaperActive)
            {
                Debug.Log("Escaper: eGameOverWon() called!");
                t.End();
                de.SendGameOver();
                foundPartner = false;
                Invoke("eGameOverWonHelper", 2);
                if(canvas5.activeSelf)
                {
                    canvas5.SetActive(false);
                }
                if(canvas5_1.activeSelf)
                {
                    canvas5_1.SetActive(false);
                }
                if(canvas5_2.activeSelf)
                {
                    canvas5_2.SetActive(false);
                }
                if(canvas5_3.activeSelf)
                {
                    canvas5_3.SetActive(false);
                }
                if(canvas6.activeSelf)
                {
                    canvas6.SetActive(false);
                }
                if(canvas7.activeSelf)
                {
                    canvas7.SetActive(false);
                }
                if(canvas9_1.activeSelf)
                {
                    canvas9_1.SetActive(false);
                }
                canvas9.SetActive(true);
            }
        }

        private void eGameOverWonHelper()
        {
            Disconnect();
        }

        public void eGameOverLost()
        {
            if(escaperActive)
            {
                Debug.Log("Escaper: eGameOverLost() called!");
                t.End();
                Disconnect();
                if(canvas5.activeSelf)
                {
                    canvas5.SetActive(false);
                }
                if(canvas5_1.activeSelf)
                {
                    canvas5_1.SetActive(false);
                }
                if(canvas5_2.activeSelf)
                {
                    canvas5_2.SetActive(false);
                }
                if(canvas5_3.activeSelf)
                {
                    canvas5_3.SetActive(false);
                }
                if(canvas6.activeSelf)
                {
                    canvas6.SetActive(false);
                }
                if(canvas7.activeSelf)
                {
                    canvas7.SetActive(false);
                }
                if(canvas9.activeSelf)
                {
                    canvas9.SetActive(false);
                }
                canvas9_1.SetActive(true);
            }
        }

        void UpdateSearchText()
        {
            if(escaperActive)
            {
                if(canvas5.activeSelf)
                {
                    if(!foundPartner)
                    {
                        searchText.text = "Searching Stalker";
                        Invoke("UpdateSearchTextHelper1", 0.7f);
                    }
                    else
                    {
                        canvas5.SetActive(false);
                        canvas5_1.SetActive(true);
                        foundPartner = true;
                    }
                }
                else
                {
                    Invoke("WaitForActive5", 0.7f);
                }
            }
        }

        void WaitForActive5()
        {
            UpdateSearchText();
        }

        void UpdateSearchTextHelper1()
        {
            if(escaperActive)
            {
                if(!foundPartner)
                {
                    searchText.text = "Searching Stalker.";
                    Invoke("UpdateSearchTextHelper2", 0.7f);
                }
                else
                {
                    canvas5.SetActive(false);
                    canvas5_1.SetActive(true);
                    foundPartner = true;
                }
            }
        }

        void UpdateSearchTextHelper2()
        {
            if(escaperActive)
            {
                if(!foundPartner)
                {
                    searchText.text = "Searching Stalker..";
                    Invoke("UpdateSearchTextHelper3", 0.7f);
                }
                else
                {
                    canvas5.SetActive(false);
                    canvas5_1.SetActive(true);
                    foundPartner = true;
                }
            }
        }

        void UpdateSearchTextHelper3()
        {
            if(escaperActive)
            {
                if(!foundPartner)
                {
                    searchText.text = "Searching Stalker...";
                    Invoke("UpdateSearchText", 0.7f);
                }
                else
                {
                    canvas5.SetActive(false);
                    canvas5_1.SetActive(true);
                    foundPartner = true;
                }
            }
        }

        public void StartTimer()
        {
            timerStarted = true;
            t.Begin();
        }

        void UpdateWaitText()
        {
            if(escaperActive)
            {
                if(canvas5_3.activeSelf)
                {
                    if(!timerStarted)
                    {
                        searchText.text = "Waiting for game start";
                        Invoke("UpdateWaitTextHelper1", 0.7f);
                    }
                    else
                    {
                        canvas5_3.SetActive(false);
                        canvas6.SetActive(true);
                        //comment the following out when building with radar
                        //de.ActivateTextUpdates();
                    }
                }
                else
                {
                    Invoke("WaitForActive5_3", 0.7f);
                }
            }
        }

        void WaitForActive5_3()
        {
            UpdateWaitText();
        }

        void UpdateWaitTextHelper1()
        {
            if(escaperActive)
            {
                if(!timerStarted)
                {
                    waitText.text = "Waiting for game start.";
                    Invoke("UpdateWaitTextHelper2", 0.7f);
                }
                else
                {
                    canvas5_3.SetActive(false);
                    canvas6.SetActive(true);
                    //comment the following out when building with radar
                    de.ActivateTextUpdates();
                }
            }
        }

        void UpdateWaitTextHelper2()
        {
            if(escaperActive)
            {
                if(!timerStarted)
                {
                    waitText.text = "Waiting for game start..";
                    Invoke("UpdateWaitTextHelper3", 0.7f);
                }
                else
                {
                    canvas5_3.SetActive(false);
                    canvas6.SetActive(true);
                    //comment the following out when building with radar
                    de.ActivateTextUpdates();
                }
            }
        }

        void UpdateWaitTextHelper3()
        {
            if(escaperActive)
            {
                if(!timerStarted)
                {
                    waitText.text = "Waiting for game start...";
                    Invoke("UpdateWaitText", 0.7f);
                }
                else
                {
                    canvas5_3.SetActive(false);
                    canvas6.SetActive(true);
                    //comment the following out when building with radar
                    de.ActivateTextUpdates();
                }
            }
        }

        void Update()
        {
            if(escaperActive)
            {
                if(foundPartner)
                {
                    if(canvas6.activeSelf)
                    {
                        int roomCount = PhotonNetwork.PlayerList.Length;
                        Debug.Log("Escaper: Update() (was CFP): Count in room: " + roomCount);

                        if(roomCount < 2)
                        {
                            Invoke("CheckForPartnerAfterDelay", 2);
                        }
                    }
                }
            }
        }

        private void CheckForPartnerAfterDelay()
        {
            if(escaperActive)
            {
                if(foundPartner)
                {
                    int roomCount = PhotonNetwork.PlayerList.Length;
                    Debug.Log("Escaper: CheckForPartner(): Count in room: " + roomCount);

                    if(roomCount < 2)
                    {
                        Debug.LogWarning("Escaper: Partner bailed!");
                        Disconnect();
                        if(canvas5.activeSelf)
                        {
                            canvas5.SetActive(false);
                        }
                        if(canvas5_1.activeSelf)
                        {
                            canvas5_1.SetActive(false);
                        }
                        if(canvas5_2.activeSelf)
                        {
                            canvas5_2.SetActive(false);
                        }
                        if(canvas5_3.activeSelf)
                        {
                            canvas5_3.SetActive(false);
                        }
                        if(canvas6.activeSelf)
                        {
                            canvas6.SetActive(false);
                        }
                        canvas7.SetActive(true);
                    }
                }
            }
        }

        public void DoPicFromCam()
        {
            ss.OnButtonClick();
            Invoke("CheckForPic", 1);
        }

        public void DoPicFromGallery()
        {
            st.OnButtonClick();
            Invoke("CheckForPic", 1);
        }

        private void CheckForPic()
        {
            if(escaperActive)
            {
                if(canvas5_1.activeSelf)
                {
                    canvas5_1.SetActive(false);
                }
                if(!canvas5_2.activeSelf)
                {
                    canvas5_2.SetActive(true);
                }

                string path = "/storage/emulated/0/Pictures/Goodies/Escaper.png";
                byte[] pic = File.ReadAllBytes(path);
                if(pic == null)
                {
                    Invoke("CheckForPicHelper", 0.5f);
                }
                else
                {
                    DisplayImage(pic);
                }
            }
        }

        private void CheckForPicHelper()
        {
            CheckForPic();
        }

        private void DisplayImage(byte[] pic)
        {
            if(escaperActive)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(pic);
                int width = tex.width;
                int height = tex.height;
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0, 0));
                img.sprite = spr;
            }
        }

        void eSendGPS()
        {
            if(escaperActive)
            {
                if(foundPartner)
                {
                    de.SendGPS();
                    eSendGPSHelper();
                }
            }
        }

        void eSendGPSHelper()
        {
            Invoke("eSendGPS", 2);
        }

        public void SendPic()
        {
            if(escaperActive)
            {
                if(canvas5_2.activeSelf)
                {
                    canvas5_2.SetActive(false);
                }
                if(!canvas5_3.activeSelf)
                {
                    canvas5_3.SetActive(true);
                }
                UpdateWaitText();
                debugText.text = "We are in SendPic";

                if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    //UpdateWaitText();
                    string path = Application.dataPath + "/Pictures/MyPic.png";
                    byte[] pic = File.ReadAllBytes(path);
                    de.SendPic(pic);
                }
                else
                {
                    if(Application.platform == RuntimePlatform.Android)
                    {
                        //UpdateWaitText();
                        //Replace path with path of our camera output
                        //string path = "/storage/emulated/0/Download/Escaper.png";
                        string path = "/storage/emulated/0/Pictures/Goodies/Escaper.png";
                        byte[] pic = File.ReadAllBytes(path);
                        de.SendPic(pic);
                        debugText.text = "We are deeper";
                    }
                    else
                    {
                        Debug.LogError("Unsupported platform!");
                        Disconnect();
                        Application.Quit(1);
                    }
                }
            }
        }
    }
}