using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.GameData
{
    public class UIAnimationConfig : BaseConfig
    {
        public List<UIAnimationData> UIAnimationDatas = new List<UIAnimationData>();

        public IAnimationData GetUIAnim(string animName)
        {
            return UIAnimationDatas.Find(data => data.id == animName)?.animationData;
        }
    }

    public interface IAnimationData
    {
        void DoIn(Object obj);
        UniTask DoOut(Object obj);
        void DOReverse(Object obj, bool force = false);
    }
    
    [Serializable]
    public class UIAnimationData 
    {
        public string id;
        [SerializeReference] public IAnimationData animationData;
    }
    
    [Serializable]
    public class SlideInData : IAnimationData
    {
        public Vector2 from;
        public Vector2 to;
        public float duration;
        public float delay;
        public Ease ease = Ease.OutCubic;
        public async void DoIn(Object obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            var rectTransform = obj as RectTransform;
            rectTransform.DOKill();
            rectTransform.anchoredPosition = from;
            rectTransform.DOAnchorPos(to, duration).SetEase(ease);
        }

        public async UniTask DoOut(Object obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            var rectTransform = obj as RectTransform;
            rectTransform.DOKill();
            //rectTransform.anchoredPosition = from;
            rectTransform.DOAnchorPos(from * -1f, duration/2).SetEase(ease);
            await UniTask.CompletedTask;
        }

        public async void DOReverse(Object obj ,bool force = false)
        {
            var rectTransform = obj as RectTransform;
            if (force)
            {
                rectTransform.DOKill();
                rectTransform.anchoredPosition = from;
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            rectTransform.DOKill();
            rectTransform.DOAnchorPos(from, duration).SetEase(ease);
        }
    }
}