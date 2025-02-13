namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public abstract class Node
    {
        public enum NodeState { Running, Success, Failure }
        protected NodeState state;
        public abstract NodeState Evaluate();
    }
}