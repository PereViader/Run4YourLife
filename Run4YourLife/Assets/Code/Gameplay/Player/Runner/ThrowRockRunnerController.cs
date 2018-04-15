﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Input;

namespace Run4YourLife.Player {
    public class ThrowRockRunnerController : MonoBehaviour {

        [SerializeField]
        private GameObject m_rockPrefab;

        [SerializeField]
        private Transform m_rockInstantiationTransform;

        [SerializeField]
        private Transform m_graphics;

        [SerializeField]
        private float force;

        [SerializeField]
        private float angle;

        private RunnerInputStated inputPlayer;

        private void Awake()
        {
            inputPlayer = GetComponent<RunnerInputStated>();
        }

        private void Update()
        {
            if (inputPlayer.GetRockInput())
            {
                ThrowRock();
            }
        }

        private void ThrowRock()
        {
            GameObject rock = Instantiate(m_rockPrefab, m_rockInstantiationTransform.position, Quaternion.identity);
            Rigidbody rigidbody = rock.GetComponent<Rigidbody>();
            rigidbody.velocity = GetThrowVelocity(force, angle);
        }

        private Vector3 GetThrowVelocity(float force, float angle)
        {
            return GetQuaternionforFacingDirection(angle) * Vector3.right * force;
        }

        private Quaternion GetQuaternionforFacingDirection(float angle)
        {
            float facingDirectionAngle = m_graphics.eulerAngles.y == 90 ? angle : 180 - angle;
            return Quaternion.Euler(0, 0, facingDirectionAngle);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 director = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Gizmos.DrawLine(m_rockInstantiationTransform.position, m_rockInstantiationTransform.position + director*5);
        }
    }
}