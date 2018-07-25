﻿using UnityEngine;

using Run4YourLife;
using Run4YourLife.Interactables;


namespace Run4YourLife.Player.Runner
{
    [RequireComponent(typeof(RunnerController))]
    public class RunnerBounceController : MonoBehaviour
    {

        [SerializeField]
        private float m_bounceJumpButtonSphereCastRatius;

        [SerializeField]
        private float m_verticalOffset;

        [SerializeField]
        private Vector3 m_colliderSize;
        private RunnerController m_runnerCharacterController;

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IBounceable bounceable = other.GetComponent<IBounceable>();
            if (bounceable != null && bounceable.ShouldBounceByContact(m_runnerCharacterController))
            {
                ExecuteBounce(bounceable);
            }
        }

        public bool ExecuteBounceIfPossible()
        {
            Vector3 position = transform.position + new Vector3(0, m_verticalOffset);
            Collider[] colliders = Physics.OverlapBox(position, m_colliderSize / 2.0f, Quaternion.identity, Layers.RunnerInteractable);
            foreach (Collider collider in colliders)
            {
                IBounceable bounceable = collider.GetComponent<IBounceable>();
                if (bounceable != null)
                {
                    ExecuteBounce(bounceable);
                    return true;
                }
            }

            return false;
        }

        private void ExecuteBounce(IBounceable bounceable)
        {
            transform.position = bounceable.GetStartingBouncePosition(m_runnerCharacterController);
            m_runnerCharacterController.Bounce(bounceable.BounceForce);
            bounceable.BouncedOn();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(0, m_verticalOffset), m_colliderSize);
        }
    }
}