﻿using System.Collections.Generic;

namespace dragoni7
{
    public class EnemyBrain : AbstractBrain
    {
        protected override void Start()
        {
            GenerateBehaviourTree();

            if (_behaviourTreeRoutine == null && BehaviorTree != null)
            {
                _behaviourTreeRoutine = StartCoroutine(RunBehaviourTree());
            }
        }
        protected override void GenerateBehaviourTree()
        {
            ObstacleAvoidanceBehaviour obstacleAvoidanceBehaviour = new ObstacleAvoidanceBehaviour();
            SeekBehaviour seekBehaviour = new SeekBehaviour();

            BehaviorTree =
                new Sequence("Get Obstacles",
                    new DetectObstaclesTask(5f),
                    new Selector("Determine Navigation",
                        new Sequence("Pursue Player",
                            new HasPlayerInRange(10f),
                            new HasTarget(),
                            new Selector("Attack or Pursue",
                                new Sequence("Attack",
                                    new IsTargetWithinRange(4),
                                    new Timer(1, new AttackTask(), true)),
                                new PursueTargetTask(new List<AbstractSteeringBehaviour> { obstacleAvoidanceBehaviour, seekBehaviour }))),
                        new Sequence("Wander",
                            new HasTarget(),
                            //new WanderTask(),
                            new PursueTargetTask(new List<AbstractSteeringBehaviour> { obstacleAvoidanceBehaviour, seekBehaviour })))
                    );
        }
    }
}
