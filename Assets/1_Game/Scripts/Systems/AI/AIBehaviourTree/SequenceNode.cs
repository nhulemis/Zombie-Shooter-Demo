using System.Collections.Generic;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class SequenceNode : Node
    {
        private List<Node> _children;

        public SequenceNode(List<Node> children)
        {
            this._children = children;
        }

        public override NodeState Evaluate()
        {
            foreach (Node node in _children)
            {
                NodeState result = node.Evaluate();
                if (result == NodeState.Failure) return NodeState.Failure;
                if (result == NodeState.Running) return NodeState.Running;
            }

            return NodeState.Success;
        }
    }
}