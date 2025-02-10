using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

namespace _1_Game.Scripts.Util
{
    public class Runner : MonoBehaviour
    {
        private static Runner instance;
        
        public static Runner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("Runner").AddComponent<Runner>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
        
        private Action<float> lateUpdateAction;

        public static void RunCoroutine(IEnumerator coroutine)
        {
            Instance.StartCoroutine(coroutine);
        }

        public static void LateUpdateSchedule(Action<float> action)
        {
            Instance.RegisterLateUpdate(action);
        }
        
        public static void LateUpdateUnSchedule(Action<float> action)
        {
            Instance.UnRegisterLateUpdate(action);
        }

        private void RegisterLateUpdate(Action<float> action)
        {
            lateUpdateAction += action;
        }
        
        public void UnRegisterLateUpdate(Action<float> action)
        {
            lateUpdateAction -= action;
        }
        
        private void LateUpdate()
        {
            lateUpdateAction?.Invoke(Time.deltaTime);
        }
    }
}