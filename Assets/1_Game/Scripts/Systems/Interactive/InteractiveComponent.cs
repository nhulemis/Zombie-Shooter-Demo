using _1_Game.Systems.Character;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Interactive
{

    [RequireComponent(typeof(Rigidbody))]
    public class InteractiveComponent : MonoBehaviour, IInteractive
    {
        public virtual void React()
        {
            Log.Warning("React method is not implemented");
        }

        public virtual void ReactEnd()
        {
            Log.Warning("ReactEnd method is not implemented");
        }

        public virtual void Execute()
        {
            Log.Warning("Execute method is not implemented");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerActor interactive))
            {
                React();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerActor interactive))
            {
                ReactEnd();
            }
        }
    }
}