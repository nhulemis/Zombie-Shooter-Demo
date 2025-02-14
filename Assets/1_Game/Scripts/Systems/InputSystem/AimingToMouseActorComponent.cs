using System;
using System.Collections.Generic;
using _1_Game.Systems.Character;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    public class AutoAimingActorComponent : MonoBehaviour
    {
        public LayerMask AimLayerMask => 1 << LayerMask.NameToLayer("Enemy");
        public bool IsAiming => _targets.Count > 0;

        private CharacterActor _playerActor;
        
        private float _scanInterval = 0.5f;
        private float _lastScanTime = 0;
        
        List<Transform> _targets = new List<Transform>();

        private void FixedUpdate()
        {
            // if (Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, AimLayerMask))
            // {
            //     transform.position = hit.point;
            // }
            if(Time.time - _lastScanTime > _scanInterval)
            {
                _lastScanTime = Time.time;
                ScanEnemies();
            }

            AimBot();
        }

        private void AimBot()
        {
            if (_targets.Count > 0)
            {
                var closestTarget = _targets[0];
                foreach (var target in _targets)
                {
                    if(target == null) continue;
                    float distanceToClosest = closestTarget ==null ? Mathf.Infinity : Vector3.Distance(_playerActor.transform.position, closestTarget.position);
                    float distanceToCurrent = Vector3.Distance(_playerActor.transform.position, target.position);
                    if (distanceToCurrent < distanceToClosest)
                    {
                        closestTarget = target;
                    }
                }
                if(closestTarget != null)
                    transform.position = closestTarget.position;
            }
        }

        private void ScanEnemies()
        {
            _targets.Clear();
            Collider[] results = new Collider[15];
            var size = Physics.OverlapSphereNonAlloc(_playerActor.transform.position, 6, results, AimLayerMask);
            foreach (var collider in results)
            {
                if(collider == null) continue;
                if (collider.TryGetComponent(out CharacterActor characterActor))
                {
                    if (characterActor != _playerActor)
                    {
                        _targets.Add(characterActor.transform);
                    }
                }
            }
        }

        public void Init(CharacterActor playerActor)
        {
            Log.Debug("AutoAimingActorComponent Init");
            _playerActor = playerActor;
        }
    }
}