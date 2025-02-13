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
        
        public IEnumerable GetAllClips()
        {
            if (_animatorController == null) return new ValueDropdownList<string>(){{"Animator is Null", null}};
            
            var clipList = new ValueDropdownList<string>();
            foreach (var layer in _animatorController.layers)
            {
                foreach (var state in layer.stateMachine.states)
                {
                    Motion motion = state.state.motion;

                    if (motion is AnimationClip clip) // Normal animation state
                    {
                        clipList.Add(new ValueDropdownItem<string>(clip.name, clip.name));
                    }
                    else if (motion is BlendTree blendTree) // Blend Tree detected
                    {
                        GetBlendTreeClips(blendTree, clipList); // Extract Blend Tree clips
                    }
                }
            }
            return clipList;
        }
        
        private void GetBlendTreeClips(BlendTree blendTree, ValueDropdownList<string> clipList)
        {
            foreach (var child in blendTree.children)
            {
                if (child.motion is AnimationClip clip)
                {
                    clipList.Add(new ValueDropdownItem<string>($"[BlendTree] {clip.name}", clip.name));
                }
                else if (child.motion is BlendTree subBlendTree) // Handle nested Blend Trees
                {
                    GetBlendTreeClips(subBlendTree, clipList);
                }
            }
        }
    }
}