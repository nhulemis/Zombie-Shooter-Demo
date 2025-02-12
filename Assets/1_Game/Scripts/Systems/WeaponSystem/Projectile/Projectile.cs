using System;
using _1_Game.Systems.Character;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem.Projectile
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] GameObject _explosionPrefab;
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.TryGetComponent(out Player damageable))
            {
                return;
            }
            Explode();
        }
        
        protected void Explode()
        {
            Log.Debug("Projectile explode");
            var explosion = Instantiate(_explosionPrefab, transform.position + Vector3.up, Quaternion.identity);
            Destroy(explosion, 1f);
            Destroy(gameObject);
        }
    }
}