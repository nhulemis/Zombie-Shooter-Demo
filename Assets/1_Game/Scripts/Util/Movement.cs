using UnityEngine;

namespace _1_Game.Scripts.Util
{
    public static class MovementExtensions
    {
        public static float GetMovementDirectionSign(this Transform transform, Vector3 movement , Transform aimTarget)
        {
            if (aimTarget == null || movement == Vector3.zero)
                return 0; 

            Vector3 toTargetDir = (aimTarget.position - transform.position).normalized;
            Vector3 movementDir = movement.normalized;

            float dot = Vector3.Dot(movementDir, toTargetDir);
            return dot; // Positive = toward target, Negative = away
        }
    }
}