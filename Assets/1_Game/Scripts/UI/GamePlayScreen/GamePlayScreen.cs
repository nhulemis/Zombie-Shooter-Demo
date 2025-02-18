using System;
using System.Collections;
using _1_Game.Scripts.GamePlay;
using _1_Game.Scripts.Systems;
using _1_Game.Scripts.Systems.WeaponSystem.Commands;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character.Command;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Systems.UI;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GamePlayScreen : UIBase
    {
        [SerializeField, ValueDropdown("tmpTexts")]
        private TMP_Text _txtGrenade;

        [SerializeField, ValueDropdown("tmpTexts")]
        private TMP_Text _txtAmmo;

        [SerializeField, ValueDropdown("tmpTexts")]
        private TMP_Text _txtKeys;
        
        [SerializeField, ValueDropdown("tmpTexts")]
        private TMP_Text _txtStage;

        [SerializeField] private Slider _healthSlider;
        private IEnumerable tmpTexts => transform.GetComponentsInChildren<TMP_Text>();
        public InventorySystem InventorySystem => Locator<InventorySystem>.Get();

        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
            RegisterListeners();
        }

        private async void RegisterListeners()
        {
            InventorySystem.Inventory.ObserveAdd().Subscribe(itemChanged =>
            {
                OnPropertyChanged(itemChanged.Key, itemChanged.Value);
            }).AddTo(this);
            InventorySystem.Inventory.ObserveReplace().Subscribe(itemChanged =>
            {
                OnPropertyChanged(itemChanged.Key, itemChanged.NewValue);
            }).AddTo(this);

            await UniTask.WaitUntil(() => Locator<MapProvider>.Get().PlayerActor != null);
            var player = Locator<MapProvider>.Get().PlayerActor;
            player.RxHealth.Subscribe(health => { _healthSlider.DOValue(player.ProgressHP, 0.5f); }).AddTo(this);
            Locator<MapProvider>.Get().OnStageChange.Subscribe(stage=>_txtStage.text = $"stage: {stage}").AddTo(this);
        }

        private void OnPropertyChanged(Type itemChangedKey, int itemChangedValue)
        {
            if (itemChangedKey == typeof(GrenadeItem))
            {
                AnimAddValue(_txtGrenade, itemChangedValue);
            }
            else if (itemChangedKey == typeof(AmmoItem))
            {
                AnimAddValue(_txtAmmo, itemChangedValue);
            }
            else if (itemChangedKey == typeof(KeyItem))
            {
                AnimAddValue(_txtKeys, itemChangedValue);
            }
        }

        private void AnimAddValue(TMP_Text tmpText, int value)
        {
            tmpText.text = value.ToString();
            tmpText.transform.localScale = Vector3.one * 1.5f;
            tmpText.transform.DOScale(Vector3.one, 0.5f);
        }

        public void OnUseGrenadeClick()
        {
            new PreGrenadeCommand().Execute().Forget();
        }

        public void SwapWeapon()
        {
            new SwitchWeaponCommand().Execute().Forget();
        }
    }
}