using System.Collections.Generic;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.GamePlay;
using _1_Game.Scripts.Systems.AIBehaviourTree;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using Script.GameData;
using UnityEngine;
using UnityEngine.AI;

namespace _1_Game.Systems.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyActor : CharacterActor
    {
        private Node rootNode;
        private NavMeshAgent agent;
        private Animator animator;
        private Transform player;

        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private int pointsPatrol = 3;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float intervalUpdate = 1f;

        private bool isCastSpell = false;
        private float lastTimeCast = 0f;
        private float lastTimeUpdate = 0f;

        private List<SpellCastData> spellCastDatas = new List<SpellCastData>();

        private struct SpellCastData
        {
            public string SpellId;
            public bool IsReady;
            public float LastTimeCast;
        }

        private GameDataBase GameDataBase => Locator<GameDataBase>.Get();
        private SpellConfig SpellConfig => GameDataBase.Get<SpellConfig>();

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = _animationController.Animator;
            player = FindFirstObjectByType<PlayerActor>().transform;

            RegisterBehaviourTree();

            InitSpellCastData();
            
            Locator<MapProvider>.Get().AddEnemy(gameObject);
        }

        private void RegisterBehaviourTree()
        {
            ConditionNode checkPlayerInRange = new ConditionNode(() =>
            {
                bool isPlayerInRange = Vector3.Distance(transform.position, player.position) < detectionRange;
                if (Locator<DoorObserver>.Get().RxIsDoorOpen.Value)
                {
                    isPlayerInRange = true;
                }
                bool isValidPath = NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas,
                    new NavMeshPath());
                return isPlayerInRange && isValidPath;
            });

            ConditionNode checkPlayerInAttackRange = new ConditionNode(() =>
            {
                bool isPlayerInRange = Vector3.Distance(transform.position, player.position) < Weapon.WeaponDataSet.range;
                
                return isPlayerInRange;
            });

            // Actions
            MoveToPlayerNode.MoveParams moveParams;
            moveParams.Agent = agent;
            moveParams.Player = player;
            moveParams.CharacterDataConfig = CharacterDataConfig;
            moveParams.Character = this;

            MoveToPlayerNode moveToPlayer = new MoveToPlayerNode(moveParams);
            PatrolNode patrol = new PatrolNode(this, agent, PatrolPoints());
            AttackNode attack = new AttackNode(this, player);

            rootNode = new SelectorNode(new List<Node>
            {
                new SequenceNode(new List<Node> { checkPlayerInAttackRange, attack }), // Attack if in range
                new SequenceNode(new List<Node> { checkPlayerInRange, moveToPlayer }), // Chase player
                patrol
            });
        }

        private void InitSpellCastData()
        {
            foreach (var spellId in CharacterDataConfig.SpellIds)
            {
                spellCastDatas.Add(new SpellCastData
                {
                    SpellId = spellId,
                    IsReady = true,
                    LastTimeCast = 0f
                });
            }
        }

        private new string PickSpell()
        {
            //get first spell that is ready
            foreach (var spellData in spellCastDatas)
            {
                if (spellData.IsReady)
                {
                    return spellData.SpellId;
                }
            }

            return string.Empty;
        }

        private void UseSpell(string spellId)
        {
            if (string.IsNullOrEmpty(spellId)) return;
            var spell = SpellConfig.GetSpellDataSet(spellId);
            CastSpell(spell, player);
            UpdateSpellData(spellId);
        }
        
        private void UpdateSpellData()
        {
            for (int i = 0; i < spellCastDatas.Count; i++)
            {
                if(spellCastDatas[i].IsReady) continue;
                if (Time.time - spellCastDatas[i].LastTimeCast > SpellConfig.GetSpellDataSet(spellCastDatas[i].SpellId).cooldown)
                {
                    spellCastDatas[i] = new SpellCastData
                    {
                        SpellId = spellCastDatas[i].SpellId,
                        IsReady = true,
                        LastTimeCast = 0f
                    };
                    Log.Debug($"<color=green>Spell {spellCastDatas[i].SpellId} is ready</color>");
                }
            }
        }

        private void UpdateSpellData(string spellId)
        {
            for (int i = 0; i < spellCastDatas.Count; i++)
            {
                if (spellCastDatas[i].SpellId == spellId)
                {
                    spellCastDatas[i] = new SpellCastData
                    {
                        SpellId = spellId,
                        IsReady = false,
                        LastTimeCast = Time.time
                    };
                    break;
                }
            }
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
            if (CheckAndCastSpell()) return;

            if (Weapon != null)
            {
                Attack(player.position);
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

        void LateUpdate()
        {
            if (IsStunned) return;
            rootNode.Evaluate();
            
            if (Time.time - lastTimeUpdate > intervalUpdate)
            {
                lastTimeUpdate = Time.time;
                UpdateSpellData();
            }
        }

        private bool CheckAndCastSpell()
        {
            if (Time.time - lastTimeCast > CharacterDataConfig.CountDownSpellTime)
            {
                isCastSpell = true;
                lastTimeCast = Time.time;
            }

            string spellId = PickSpell();
            if (isCastSpell && !string.IsNullOrEmpty(spellId))
            {
                isCastSpell = false;
                UseSpell(spellId);
                return true;
            }

            return false;
        }
    }
}