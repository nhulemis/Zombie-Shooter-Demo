using System;
using _1_Game.Scripts.Util;
using UnityEngine;

namespace _1_Game.Scripts.GamePlay
{
    public class MapActorComponent : MonoBehaviour
    {
        [SerializeField] private string _stageName;
        [SerializeField] public bool IsEndStage;
        public string StageName => _stageName;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_stageName))
            {
                _stageName = gameObject.name;
            }
        }

        private void OnEnable()
        {
            Locator<MapProvider>.Get().SetCurrentStage(this);
        }
    }
}