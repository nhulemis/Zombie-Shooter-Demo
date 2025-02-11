using System;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    public class AimingToMouseActorComponent : MonoBehaviour
    {
        public LayerMask AimLayerMask => 1 << LayerMask.NameToLayer("Default");

        private void FixedUpdate()
        {
            if (Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, AimLayerMask))
            {
                transform.position = hit.point;
            }
        }
    }
}