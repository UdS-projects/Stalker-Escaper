using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Bolt;

namespace com.StalkerProject.Stalker
{
    public class GPS : MonoBehaviour
    {
        private float distance = 0;
        private float longitude = 0;
        private float latitude = 0;
        public Text gpsText;
        public Text gpsDistance;
        //private Vector3 targetPosition;
        //private Vector3 originalPosition;
        [SerializeField]
        private DataExchange de;

        //Start is called before the first frame update
        void Start()
        {
            //if(Input.location.isEnabledByUser)
            StartCoroutine(GetLocation());
        }

        public float GetLong()
        {
            return longitude;
        }

        public float GetLat()
        {
            return latitude;
        }

        public float GetDistance()
        {
            return distance;
        }

        public IEnumerator GetLocation()
        {
            Input.location.Start();
            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                yield return new WaitForSeconds(0.5f);
            }
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            Debug.Log("In GetLocation");
            yield break;
        }

        //calculates distance between two sets of coordinates, taking into account the curvature of the earth.
        public void Calc(float lat1, float lon1, float lat2, float lon2)
        {
            var R = 6378.137; // Radius of earth in KM
            var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
            var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
            float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
              Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
              Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
            var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            distance = (float)R * c * 1000f; //1000f for meters

            //convert distance from double to float
            //float distanceFloat = (float)distance;
            //set the target position of the ufo, this is where we lerp to in the update function
            //targetPosition = originalPosition - new Vector3(0, 0, distanceFloat * 12);
            //distance was multiplied by 12 so I didn't have to walk that far to get the UFO to show up closer
        }

        //Update is called once per frame
        void Update()
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            Calc(latitude, longitude, de.GetPartnerLat(), de.GetPartnerLong());
            Variables.ActiveScene.Set("SelfX", longitude);
            Variables.ActiveScene.Set("SelfY", latitude);
            Variables.ActiveScene.Set("OtherX", de.GetPartnerLong());
            Variables.ActiveScene.Set("OtherY", de.GetPartnerLat());

            gpsText.text = "Lat: " + latitude + "\nLong: " + longitude;
            gpsDistance.text = "D: " + distance;
        }
    }
}