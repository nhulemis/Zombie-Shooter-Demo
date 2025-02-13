using System.Collections.Generic;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class SelectorNode : Node
    {
        private List<Node> _children;
        public SelectorNode(List<Node> children) { this._children = children; }
    
        public override NodeState Evaluate()
        {
            foreach (Node node in _children)
            {
                NodeState result = node.Evaluate();
                if (result == NodeState.Success) return NodeState.Success;
                if (result == NodeState.Running) return NodeState.Running;
            }
            return NodeState.Failure;
        }
    }

}