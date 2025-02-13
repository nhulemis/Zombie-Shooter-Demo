using System.Collections.Generic;
using _1_Game.Scripts.Systems.AIBehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace _1_Game.Systems.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Character
    {
        private Node rootNode;
        private NavMeshAgent agent;
        private Animator animator;
        private Transform player;

        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private int pointsPatrol = 3;
        [SerializeField] private float radius = 5f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = _animationController.Animator;
            player = FindFirstObjectByType<Player>().transform;

            ConditionNode checkPlayerInRange = new ConditionNode(() =>
            {
                bool isPlayerInRange = Vector3.Distance(transform.position, player.position) < detectionRange;
                bool isValidPath = NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas,
                    new NavMeshPath());
                return isPlayerInRange && isValidPath;
            });

            ConditionNode checkPlayerInAttackRange = new ConditionNode(() =>
            {
                return Vector3.Distance(transform.position, player.position) < CharacterDataConfig.AttackRange + 0.5f;
            });

            // Actions
            MoveToPlayerNode.MoveParams moveParams;
            moveParams.Agent = agent;
            moveParams.Player = player;
            moveParams.CharacterDataConfig = CharacterDataConfig;
            moveParams.Character = this;

            MoveToPlayerNode moveToPlayer = new MoveToPlayerNode(moveParams);
            PatrolNode patrol = new PatrolNode(this, agent, PatrolPoints());
            AttackNode attack = new AttackNode(this);

            rootNode = new SelectorNode(new List<Node>
            {
                new SequenceNode(new List<Node> { checkPlayerInAttackRange, attack }), // Attack if in range
                new SequenceNode(new List<Node> { checkPlayerInRange, moveToPlayer }), // Chase player
                patrol
            });
        }

        private List<Vector3> PatrolPoints()
        {
            var output = new List<Vector3>(pointsPatrol);

            for (int i = 0; i < pointsPatrol; i++)
            {
                Vector3 randomPoint = GetRandomPointAroundCharacter(radius);
                if (randomPoint != Vector3.zero)
                {
                    output.Add(randomPoint);
                    Debug.DrawRay(randomPoint, Vector3.up * 2, Color.green, 5f); // Visualize point
                }
            }

            return output;
        }
        
        public override void Attack()
        {
            if (Weapon != null)
            {
                _weaponController.Attack(player.position);
            }
        }

        private Vector3 GetRandomPointAroundCharacter(float range)
        {
            Vector3 randomDirection = Random.insideUnitCircle.normalized * range;
            Vector3 point = agent.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

            // Validate the point using NavMesh.SamplePosition
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position; // Return a valid NavMesh position
            }
            return Vector3.zero; // Invalid position
        }

        void Update()
        {
            rootNode.Evaluate();
        }
    }
}