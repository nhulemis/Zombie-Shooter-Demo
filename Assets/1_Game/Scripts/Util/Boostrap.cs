using System;
using _1_Game.Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Scripts.Util
{
    public class Boostrap : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            StartJobs();
        }

        private async void StartJobs()
        {
            await new InitSystemJob().Execute();

            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }
}