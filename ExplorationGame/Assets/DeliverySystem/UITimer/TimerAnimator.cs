using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Events;

namespace Races { 

    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(Animator))]
    public class TimerAnimator : MonoBehaviour
    {
        //public MarathonTimerScriptableObject marathonTimerScriptableObject;


        public TextMeshProUGUI timerText;
        public Animator timerAnimator;

        [SerializeField] public float _remainingTime;
        [SerializeField] public bool timeRunner;

        #region animationParameters
        string startCount = "StartCount";
        string hurry = "Hurry";
        string finish = "Finish";
        string entry = "Entry";
        #endregion
        public void Start()
        {
            /*  marathonTimerScriptableObject.timerStartEvent.AddListener(startTime);
              marathonTimerScriptableObject.victoryEvent.AddListener(stopTime);
              marathonTimerScriptableObject.deletedTimerEvent.AddListener(DeleteTimer);*/
            EventManager.AddListener<float>(DeliveryEvents.Started, startTime);
            EventManager.AddListener(DeliveryEvents.Delivered, DeleteTimer);
            timerText = GetComponent<TextMeshProUGUI>();
            timerAnimator = GetComponent<Animator>();
        }

        public void startTime(float remainingTime)
        {
            timerAnimator.SetTrigger(entry);
            timerAnimator.SetTrigger(startCount);
            timerAnimator.SetBool(hurry, false);
            _remainingTime = remainingTime;
            timeRunner = true;
        }

        public void stopTime()
        {
            timeRunner = false;
        }

        public void Update()
        {
            if (!timeRunner)
                return;

            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 1)
            {
                CallDefeat();
            }
            if (_remainingTime < 10)
            {
                timerAnimator.SetBool(hurry, true);
            }
            int tempMin = Mathf.FloorToInt(_remainingTime / 60);
            int tempSeg = Mathf.FloorToInt(_remainingTime % 60);
            timerText.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }

        void CallDefeat()
        {
            DeleteTimer();
            EventManager.Dispatch(DeliveryEvents.Failed);
        }

        void DeleteTimer()
        {
            timeRunner = false;
            timerAnimator.SetTrigger(finish);
        }
    
    }
}
