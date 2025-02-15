using UnityEngine;
using UnityEngine.UI;

namespace _1_Game.Scripts.Systems.Door
{
    public class OpenDoorActorComponent : MonoBehaviour
    {
        [SerializeField] Slider _slider;
        
        public void OnOpenDoor(float progress)
        {
            _slider.value = progress;
        }
    }
}