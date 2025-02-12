using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Script.GameData.AnimationLayerMapping
{
    public class AnimationLayerMappingConfig : BaseConfig
    {
        [SerializeField] private AnimatorController _animatorController;
        
        public IEnumerable GetLayerNames()
        {
            if (_animatorController == null) return new ValueDropdownList<string>(){{"Animator is Null", null}};
            
            var layerList = new ValueDropdownList<string>();
            for (var i = 0; i < _animatorController.layers.Length; i++)
            {
                layerList.Add(_animatorController.layers[i].name, _animatorController.layers[i].name);
            }
            return layerList;
        }
        
        public IEnumerable GetParameterNames()
        {
            if (_animatorController == null) return new ValueDropdownList<string>(){{"Animator is Null", null}};
            
            var parameterList = new ValueDropdownList<string>();
            for (var i = 0; i < _animatorController.parameters.Length; i++)
            {
                parameterList.Add(_animatorController.parameters[i].name, _animatorController.parameters[i].name);
            }
            return parameterList;
        }
    }
}