using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Systems.Pickup
{
    public interface IPickupableObject
    {
        UniTask Pickup();
        void Drop();
    }
}