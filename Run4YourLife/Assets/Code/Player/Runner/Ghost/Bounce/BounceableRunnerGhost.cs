﻿using UnityEngine;

using Run4YourLife.Interactables;

namespace Run4YourLife.Player.Runner.Ghost
{
    [RequireComponent(typeof(Collider))]
    public class BounceableRunnerGhost : BounceableEntityBase
    {
        [SerializeField]
        private float m_bounceForce;

        [SerializeField]
        private RunnerGhostController m_runnerGhostController;

        protected override void Awake()
        {
            base.Awake();
            m_runnerGhostController = GetComponentInParent<RunnerGhostController>();
            Debug.Assert(m_runnerGhostController != null);
        }

        public override Vector3 BounceForce
        {
            get
            {
                return new Vector3()
                {
                    y = m_bounceForce
                };
            }
        }

        public override void BouncedOn()
        {
            base.BouncedOn();
            m_runnerGhostController.ReviveRunner();
        }

        public override bool ShouldBounceByContact(RunnerController runnerCharacterController)
        {
            return runnerCharacterController.Velocity.y < 0;
        }
    }
}