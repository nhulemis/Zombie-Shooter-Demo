namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class ConditionNode : Node
    {
        private System.Func<bool> _condition;
        public ConditionNode(System.Func<bool> condition) { this._condition = condition; }

        public override NodeState Evaluate()
        {
            return _condition.Invoke() ? NodeState.Success : NodeState.Failure;
        }
    }
}