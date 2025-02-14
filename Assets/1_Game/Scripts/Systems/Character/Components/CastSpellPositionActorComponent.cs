using System;
using DG.Tweening;
using Script.GameData;

namespace UnityEngine.SceneManagement
{
    public class CastSpellPositionActorComponent : MonoBehaviour
    {
        [SerializeField] private Transform _visualizer;
        
        private Transform _target;
        private SpellDataSet _spell;
        
        private bool _isCasting = false;
        private bool _isSpellFinished = false;
        private Action _callback;

        public void Init(SpellDataSet spell, Transform target, Action callback)
        {
            _spell = spell;
            _target = target;
            _callback = callback;
        }

        private void FixedUpdate()
        {
            if(!_isCasting) return;
            transform.position = _target.position;
        }

        public bool IsSpellFinished()
        {
            return _isSpellFinished;
        }

        public void CastSpell()
        {
            Log.Debug("[CastSpellPositionActorComponent] CastSpell");
            _isCasting = true;
            _visualizer.localScale= Vector3.zero;
            _visualizer.DOScale(Vector3.one, _spell.castTime).OnComplete(() =>
            {
                _isCasting = false;
                _isSpellFinished = true;
                _callback?.Invoke();
                Destroy(gameObject,0.25f);
            });
        }
    }
}