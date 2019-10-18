using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.StalkerProject.Stalker
{
    public class DataExchange : MonoBehaviour
    {
        //Note: Stalker checks the distance and sends the gameOver signal if distance < 30m
        //And Escaper checks time and sends gameOver signal if time = 0
        //gameOver signal needs to interrupt timer

        //fields
        [SerializeField]
        private Stalker sta;
        [SerializeField]
        private Escaper esc;
        [SerializeField]
        private GPS gps;
        [Tooltip("GPS Output Textfield")]
        [SerializeField]
        Text gpsText;

        PhotonView photonView;
        private bool updateText = false;

        float partnerLong = 0;
        float partnerLat = 0;
        byte[] partnerPic = null;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void NewGame()
        {
            partnerLong = 0;
            partnerLat = 0;
            partnerPic = null;
            gpsText.text = "unknown";
        }

        public float GetPartnerLong()
        {
            return partnerLong;
        }

        public float GetPartnerLat()
        {
            return partnerLat;
        }

        public byte[] GetPic()
        {
            return partnerPic;
        }

        //launch through OnClick of the last button before Canvas 6
        public void ActivateTextUpdates()
        {
            updateText = true;
        }

        public void SendGPS()
        {
            photonView.RPC("ReceiveGPS", RpcTarget.Others, gps.GetLong(), gps.GetLat());
        }

        public void SendPic(byte[] pic)
        {
            photonView.RPC("ReceivePic", RpcTarget.Others, pic);
        }

        public void SendGameBegin()
        {
            photonView.RPC("ReceiveGameBegin", RpcTarget.Others);
        }

        public void SendGameOver()
        {
            photonView.RPC("ReceiveGameOver", RpcTarget.Others);
        }

        [PunRPC]
        void ReceiveGPS(float pLong, float pLat)
        {
            Debug.Log("DataExchange: GPS Received: Lat: " + pLat + " Long: " + pLong);
            partnerLong = pLong;
            partnerLat = pLat;
            if(updateText)
            {
                gpsText.text = "Lat: " + pLat + "\nLong: " + pLong;
            }
        }

        [PunRPC]
        void ReceivePic(byte[] pic)
        {
            if(pic == null)
            {
                Debug.LogWarning("DataExchange: Picture Received: Null");
            }
            else
            {
                Debug.Log("DataExchange: Picture Received: Not null");
                partnerPic = pic;
            }
        }

        [PunRPC]
        void ReceiveGameBegin()
        {
            Debug.Log("DataExchange: Received Game Begin Signal");
            if (esc.escaperActive)
            {
                esc.StartTimer();
            }
            else
            {
                Debug.LogError("DataExchange: ReceiveGameBegin received even though Escaper not active!");
            }
        }

        [PunRPC]
        void ReceiveGameOver()
        {
            if(sta.stalkerActive)
            {
                sta.sGameOverLost();
            }
            else
            {
                if(esc.escaperActive)
                {
                    esc.eGameOverLost();
                }
            }
        }
    }
}