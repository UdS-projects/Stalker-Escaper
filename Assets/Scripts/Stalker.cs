using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.StalkerProject.Stalker
{
    public class Stalker : MonoBehaviourPunCallbacks
    {
        //fields
        [SerializeField]
        DataExchange de;
        [SerializeField]
        private Timer t;
        [SerializeField]
        private GPS gps;

        [SerializeField]
        GameObject canvas4;
        [SerializeField]
        Text searchText;
        [SerializeField]
        GameObject canvas4_1;
        [SerializeField]
        Text waitText;
        [SerializeField]
        GameObject canvas4_2;
        [SerializeField]
        GameObject canvas6;
        [SerializeField]
        GameObject canvas7;
        [SerializeField]
        GameObject canvas8;
        [SerializeField]
        GameObject canvas8_1;
        [SerializeField]
        Image img;

        [System.NonSerialized]
        public bool stalkerActive = false;
        private bool foundPartner = false;
        private bool gameBegan = false;

        string gameVersion = "1";
        private byte maxPlayersPerRoom = 2;

        public void Connect()
        {
            if (!stalkerActive)
            {
                Debug.Log("Stalker: Connect() called.");

                de.NewGame();
                stalkerActive = true;
                PhotonNetwork.AutomaticallySyncScene = true;

                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                Debug.LogError("Stalker: Connect() called even though Stalker already active");
            }
        }

        public override void OnConnectedToMaster()
        {
            if(stalkerActive)
            {
                Debug.Log("Stalker: OnConnectedToMaster() was called by PUN");
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            stalkerActive = false;
            Debug.LogWarningFormat("Stalker: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinedRoom()
        {
            if(stalkerActive)
            {
                Debug.Log("Stalker: OnJoinedRoom() called by PUN.");
                UpdateSearchText();
            }
        }

        public void Disconnect()
        {
            if(stalkerActive)
            {
                stalkerActive = false;
                foundPartner = false;
                gameBegan = false;
                t.End();
                PhotonNetwork.Disconnect();
            }
            else
            {
                Debug.LogError("Stalker: Disconnect() called even though Stalker already not active");
            }
        }

        public void sGameOverWon()
        {
            if(stalkerActive)
            {
                Debug.Log("Stalker: sGameOverWon() called!");
                t.End();
                de.SendGameOver();
                foundPartner = false;
                Invoke("sGameOverWonHelper", 2);
                if(canvas4.activeSelf)
                {
                    canvas4.SetActive(false);
                }
                if(canvas4_1.activeSelf)
                {
                    canvas4_1.SetActive(false);
                }
                if(canvas4_2.activeSelf)
                {
                    canvas4_2.SetActive(false);
                }
                if(canvas6.activeSelf)
                {
                    canvas6.SetActive(false);
                }
                if(canvas7.activeSelf)
                {
                    canvas7.SetActive(false);
                }
                if(canvas8_1.activeSelf)
                {
                    canvas8_1.SetActive(false);
                }
                canvas8.SetActive(true);
            }
        }

        private void sGameOverWonHelper()
        {
            Disconnect();
        }

        public void sGameOverLost()
        {
            if(stalkerActive)
            {
                Debug.Log("Stalker: sGameOverLost() called!");
                t.End();
                Disconnect();
                if(canvas4.activeSelf)
                {
                    canvas4.SetActive(false);
                }
                if(canvas4_1.activeSelf)
                {
                    canvas4_1.SetActive(false);
                }
                if(canvas4_2.activeSelf)
                {
                    canvas4_2.SetActive(false);
                }
                if(canvas6.activeSelf)
                {
                    canvas6.SetActive(false);
                }
                if(canvas7.activeSelf)
                {
                    canvas7.SetActive(false);
                }
                if(canvas8.activeSelf)
                {
                    canvas8.SetActive(false);
                }
                canvas8_1.SetActive(true);
            }
        }

        private void UpdateSearchText()
        {
            if(stalkerActive)
            {
                if(canvas4.activeSelf)
                {
                    int roomCount = PhotonNetwork.PlayerList.Length;
                    Debug.Log("Stalker: UST: Count in room: " + roomCount);
                    if(roomCount > 1)
                    {
                        canvas4.SetActive(false);
                        canvas4_1.SetActive(true);
                        foundPartner = true;
                        UpdateWaitText();
                    }
                    else
                    {
                        searchText.text = "Searching Escaper";
                        Invoke("UpdateSearchTextHelper1", 0.7f);
                    }
                }
                else
                {
                    Invoke("WaitForActive4", 0.7f);
                }
            }
        }

        private void WaitForActive4()
        {
            UpdateSearchText();
        }

        private void UpdateSearchTextHelper1()
        {
            if(stalkerActive)
            {
                int roomCount = PhotonNetwork.PlayerList.Length;
                Debug.Log("Stalker: USTH1: Count in room: " + roomCount);
                if(roomCount > 1)
                {
                    canvas4.SetActive(false);
                    canvas4_1.SetActive(true);
                    foundPartner = true;
                    UpdateWaitText();
                }
                else
                {
                    searchText.text = "Searching Escaper.";
                    Invoke("UpdateSearchTextHelper2", 0.7f);
                }
            }
        }

        private void UpdateSearchTextHelper2()
        {
            if(stalkerActive)
            {
                int roomCount = PhotonNetwork.PlayerList.Length;
                Debug.Log("Stalker: USTH2: Count in room: " + roomCount);
                if(roomCount > 1)
                {
                    canvas4.SetActive(false);
                    canvas4_1.SetActive(true);
                    foundPartner = true;
                    UpdateWaitText();
                }
                else
                {
                    searchText.text = "Searching Escaper..";
                    Invoke("UpdateSearchTextHelper3", 0.7f);
                }
            }
        }

        private void UpdateSearchTextHelper3()
        {
            if(stalkerActive)
            {
                int roomCount = PhotonNetwork.PlayerList.Length;
                Debug.Log("Stalker: USTH3: Count in room: " + roomCount);
                if(roomCount > 1)
                {
                    canvas4.SetActive(false);
                    canvas4_1.SetActive(true);
                    foundPartner = true;
                    UpdateWaitText();
                }
                else
                {
                    searchText.text = "Searching Escaper...";
                    Invoke("UpdateSearchText", 0.7f);
                }
            }
        }
        
        void Update()
        {
            if(stalkerActive)
            {
                if(foundPartner)
                {
                    if(canvas6.activeSelf)
                    {
                        int roomCount = PhotonNetwork.PlayerList.Length;
                        Debug.Log("Stalker: Update() (was CFP): Count in room: " + roomCount);

                        if(roomCount < 2)
                        {
                            Invoke("CheckForPartnerAfterDelay", 2);
                        }

                        if(gameBegan)
                        {
                            if(gps.GetDistance() < 30)
                            {
                                sGameOverWon();
                            }
                        }
                    }
                }
            }
        }

        private void CheckForPartnerAfterDelay()
        {
            if(stalkerActive)
            {
                if(foundPartner)
                {
                    int roomCount = PhotonNetwork.PlayerList.Length;
                    Debug.Log("Stalker: CFPAfterDelay(): Count in room: " + roomCount);

                    if(roomCount < 2)
                    {
                        Debug.LogWarning("Stalker: Partner bailed!");
                        Disconnect();
                        if(canvas4.activeSelf)
                        {
                            canvas4.SetActive(false);
                        }
                        if(canvas4_1.activeSelf)
                        {
                            canvas4_1.SetActive(false);
                        }
                        if(canvas4_2.activeSelf)
                        {
                            canvas4_2.SetActive(false);
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

        private void UpdateWaitText()
        {
            if(stalkerActive)
            {
                if(canvas4_1.activeSelf)
                {
                    if(de.GetPic() != null)
                    {
                        Debug.Log("Stalker: UWT: Non-null pic!");
                        canvas4_1.SetActive(false);
                        canvas4_2.SetActive(true);
                        DisplayImage(de.GetPic());
                        sSendGPS();
                    }
                    else
                    {
                        Debug.Log("Stalker: UWT: No pic.");
                        waitText.text = "Waiting for picture";
                        Invoke("UpdateWaitTextHelper1", 0.7f);
                    }
                }
                else
                {
                    Invoke("WaitForActive4_1", 0.7f);
                }
            }
        }

        private void WaitForActive4_1()
        {
            UpdateWaitText();
        }

        private void UpdateWaitTextHelper1()
        {
            if(stalkerActive)
            {
                if(de.GetPic() != null)
                {
                    Debug.Log("Stalker: UWTH1: Non-null pic!");
                    canvas4_1.SetActive(false);
                    canvas4_2.SetActive(true);
                    DisplayImage(de.GetPic());
                    sSendGPS();
                }
                else
                {
                    Debug.Log("Stalker: UWTH1: No pic.");
                    waitText.text = "Waiting for picture.";
                    Invoke("UpdateWaitTextHelper2", 0.7f);
                }
            }
        }

        private void UpdateWaitTextHelper2()
        {
            if(stalkerActive)
            {
                if(de.GetPic() != null)
                {
                    Debug.Log("Stalker: UWTH2: Non-null pic!");
                    canvas4_1.SetActive(false);
                    canvas4_2.SetActive(true);
                    DisplayImage(de.GetPic());
                    sSendGPS();
                }
                else
                {
                    Debug.Log("Stalker: UWTH2: No pic.");
                    waitText.text = "Waiting for picture..";
                    Invoke("UpdateWaitTextHelper3", 0.7f);
                }
            }
        }

        private void UpdateWaitTextHelper3()
        {
            if(stalkerActive)
            {
                if(de.GetPic() != null)
                {
                    Debug.Log("Stalker: UWTH3: Non-null pic!");
                    canvas4_1.SetActive(false);
                    canvas4_2.SetActive(true);
                    DisplayImage(de.GetPic());
                    sSendGPS();
                }
                else
                {
                    Debug.Log("Stalker: UWTH3: No pic.");
                    waitText.text = "Waiting for picture...";
                    Invoke("UpdateWaitText", 0.7f);
                }
            }
        }

        private void DisplayImage(byte[] pic)
        {
            if(stalkerActive)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(pic);
                int width = tex.width;
                int height = tex.height;
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0, 0));
                img.sprite = spr;
            }
        }

        public void sSendGameBegin()
        {
            if(stalkerActive)
            {
                if(!gameBegan)
                {
                    de.SendGameBegin();
                    gameBegan = true;
                    t.Begin();
                    if(canvas4_2.activeSelf)
                    {
                        canvas4_2.SetActive(false);
                    }
                    canvas6.SetActive(true);
                    //comment the following out when building with radar
                    //de.ActivateTextUpdates();
                }
                else
                {
                    Debug.LogError("Stalker: sSendGameBegin() called even though game already began!");
                }
            }
        }

        private void sSendGPS()
        {
            if(stalkerActive)
            {
                if(foundPartner)
                {
                    de.SendGPS();
                    sSendGPSHelper();
                }
            }
        }

        private void sSendGPSHelper()
        {
            Invoke("sSendGPS", 2);
        }
    }
}