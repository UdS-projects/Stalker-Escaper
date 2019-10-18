using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.StalkerProject.Stalker
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        GameObject posManager;
        [SerializeField]
        Escaper esc;

        private bool timerActive = false;
        private int timeLeft = 900;
        private int CDTime = 0;

        private void Start()
        {
            //posManager.SetActive(false);
            timerActive = false;
            timeLeft = 900;
            CDTime = 0;
        }

        public void Begin()
        {
            timerActive = true;
            timeLeft = 900;
            Invoke("TimeHelper1", 60);
            CDTime = 59;
            CountDown();
            posManager.SetActive(true);
            Invoke("WaitForPM", 2);
        }

        //Following method for testing purposes only:
        public void BeginDebug()
        {
            timerActive = true;
            TimeHelper36();
        }

        public void End()
        {
            timerActive = false;
            timeLeft = 900;
            CDTime = 0;
        }

        private void WaitForPM()
        {
            //posManager.SetActive(false);
        }

        private void CountDown()
        {
            if(timerActive)
            {
                print("Timer: timeLeft: " + timeLeft);
                if (CDTime > 0)
                {
                    Invoke("CountDownHelper", 1);
                    CDTime--;
                }
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void CountDownHelper()
        {
            timeLeft--;
            CountDown();
        }

        private void TimeHelper1()
        {
            if(timerActive)
            {
                timeLeft = 840;
                Invoke("TimeHelper2", 60);
                CDTime = 59;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper2()
        {
            if(timerActive)
            {
                timeLeft = 780;
                Invoke("TimeHelper3", 55);
                CDTime = 54;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper3()
        {
            if(timerActive)
            {
                timeLeft = 725;
                Invoke("TimeHelper4", 55);
                CDTime = 54;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper4()
        {
            if(timerActive)
            {
                timeLeft = 670;
                Invoke("TimeHelper5", 50);
                CDTime = 49;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper5()
        {
            if(timerActive)
            {
                timeLeft = 620;
                Invoke("TimeHelper6", 50);
                CDTime = 49;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper6()
        {
            if(timerActive)
            {
                timeLeft = 570;
                Invoke("TimeHelper7", 45);
                CDTime = 44;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper7()
        {
            if(timerActive)
            {
                timeLeft = 525;
                Invoke("TimeHelper8", 45);
                CDTime = 44;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper8()
        {
            if(timerActive)
            {
                timeLeft = 480;
                Invoke("TimeHelper9", 40);
                CDTime = 39;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper9()
        {
            if(timerActive)
            {
                timeLeft = 440;
                Invoke("TimeHelper10", 40);
                CDTime = 39;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper10()
        {
            if(timerActive)
            {
                timeLeft = 400;
                Invoke("TimeHelper11", 35);
                CDTime = 34;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper11()
        {
            if(timerActive)
            {
                timeLeft = 365;
                Invoke("TimeHelper12", 35);
                CDTime = 34;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper12()
        {
            if(timerActive)
            {
                timeLeft = 330;
                Invoke("TimeHelper13", 30);
                CDTime = 29;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper13()
        {
            if(timerActive)
            {
                timeLeft = 300;
                Invoke("TimeHelper14", 30);
                CDTime = 29;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper14()
        {
            if(timerActive)
            {
                timeLeft = 270;
                Invoke("TimeHelper15", 25);
                CDTime = 24;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper15()
        {
            if(timerActive)
            {
                timeLeft = 245;
                Invoke("TimeHelper16", 25);
                CDTime = 24;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper16()
        {
            if(timerActive)
            {
                timeLeft = 220;
                Invoke("TimeHelper17", 20);
                CDTime = 19;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper17()
        {
            if(timerActive)
            {
                timeLeft = 200;
                Invoke("TimeHelper18", 20);
                CDTime = 19;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper18()
        {
            if(timerActive)
            {
                timeLeft = 180;
                Invoke("TimeHelper19", 15);
                CDTime = 14;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper19()
        {
            if(timerActive)
            {
                timeLeft = 165;
                Invoke("TimeHelper20", 15);
                CDTime = 14;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper20()
        {
            if(timerActive)
            {
                timeLeft = 150;
                Invoke("TimeHelper21", 15);
                CDTime = 14;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper21()
        {
            if(timerActive)
            {
                timeLeft = 135;
                Invoke("TimeHelper22", 15);
                CDTime = 14;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper22()
        {
            if(timerActive)
            {
                timeLeft = 120;
                Invoke("TimeHelper23", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper23()
        {
            if(timerActive)
            {
                timeLeft = 110;
                Invoke("TimeHelper24", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper24()
        {
            if(timerActive)
            {
                timeLeft = 100;
                Invoke("TimeHelper25", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper25()
        {
            if(timerActive)
            {
                timeLeft = 90;
                Invoke("TimeHelper26", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper26()
        {
            if(timerActive)
            {
                timeLeft = 80;
                Invoke("TimeHelper27", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper27()
        {
            if(timerActive)
            {
                timeLeft = 70;
                Invoke("TimeHelper28", 10);
                CDTime = 9;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper28()
        {
            if(timerActive)
            {
                timeLeft = 60;
                Invoke("TimeHelper29", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper29()
        {
            if(timerActive)
            {
                timeLeft = 55;
                Invoke("TimeHelper30", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper30()
        {
            if(timerActive)
            {
                timeLeft = 50;
                Invoke("TimeHelper31", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper31()
        {
            if(timerActive)
            {
                timeLeft = 45;
                Invoke("TimeHelper32", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper32()
        {
            if(timerActive)
            {
                timeLeft = 40;
                Invoke("TimeHelper33", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper33()
        {
            if(timerActive)
            {
                timeLeft = 35;
                Invoke("TimeHelper34", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper34()
        {
            if(timerActive)
            {
                timeLeft = 30;
                Invoke("TimeHelper35", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper35()
        {
            if(timerActive)
            {
                timeLeft = 25;
                Invoke("TimeHelper36", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper36()
        {
            if(timerActive)
            {
                timeLeft = 20;
                Invoke("TimeHelper37", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper37()
        {
            if(timerActive)
            {
                timeLeft = 15;
                Invoke("TimeHelper38", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper38()
        {
            if(timerActive)
            {
                timeLeft = 10;
                Invoke("TimeHelper39", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper39()
        {
            if(timerActive)
            {
                timeLeft = 5;
                Invoke("TimeHelper40", 5);
                CDTime = 4;
                CountDown();
                posManager.SetActive(true);
                Invoke("WaitForPM", 2);
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }

        private void TimeHelper40()
        {
            if(timerActive)
            {
                timeLeft = 0;
                if (esc.escaperActive)
                {
                    esc.eGameOverWon();
                }
            }
            else
            {
                timeLeft = 900;
                CDTime = 0;
            }
        }
    }
}