using System;
using _1_Game.Scripts.Systems.WeaponSystem.Commands;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Pickup
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PickupItem : MonoBehaviour
    {
        [SerializeReference] private GameObject _pickupItemData;

        private void Start()
        {
            transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (_pickupItemData == null) return;
            transform.DOKill();
            IPickupableObject pickupableObject = _pickupItemData.GetComponent<IPickupableObject>();
            Assert.IsNotNull(pickupableObject, "Class Weapon is not implement IPickupableObject");
            await pickupableObject.Pickup();
            Destroy(gameObject);
        }
    }
}