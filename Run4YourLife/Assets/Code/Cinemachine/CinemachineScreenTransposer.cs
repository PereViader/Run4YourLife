﻿using System;
using UnityEngine;

using Cinemachine;
using Run4YourLife.GameManagement;
using Run4YourLife.Player.Boss;

namespace Run4YourLife.Cinemachine
{
    [Serializable]
    public class CinemachineScreenTransposerData
    {
        [Tooltip("Offset from target to be calculated from")]
        public Vector3 m_offsetFromTarget;

        [Tooltip("Meters displayed on the left side of the screen from the target. Should allways be >= 0")]
        public float m_verticalHeight = 1;

        [Range(0, 1)]
        [Tooltip("Percentual screen X position for target")]
        public float m_screenX = 0.5f;

        [Range(0, 1)]
        [Tooltip("Percentual screen Y position for target")]
        public float m_screenY = 0.5f;

        public CinemachineScreenTransposerData()
        {
        }

        public CinemachineScreenTransposerData(CinemachineScreenTransposerData other)
        {
            this.m_offsetFromTarget = other.m_offsetFromTarget;
            this.m_verticalHeight = other.m_verticalHeight;
            this.m_screenX = other.m_screenX;
            this.m_screenY = other.m_screenY;
        }
    }

    public class CinemachineScreenTransposer : CinemachineComponentBase
    {
        [SerializeField]
        // Used for serialization don't use directly
        private CinemachineScreenTransposerData m_screenTransposerData;

        private CinemachineScreenTransposerData m_cinemachineScreenTransposerData;
        private Transform m_previousTarget;

        public override bool IsValid { get { return enabled && FollowTarget != null; } }
        public CinemachineScreenTransposerData CinemachineScreenTransposerData { get { return m_cinemachineScreenTransposerData; } set { m_cinemachineScreenTransposerData = value; } }
        public override CinemachineCore.Stage Stage { get { return CinemachineCore.Stage.Body; } }

        private void Awake()
        {
            m_cinemachineScreenTransposerData = new CinemachineScreenTransposerData(m_screenTransposerData);
        }

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            if (IsValid)
            {
                UpdateCinemachineScreenTransposerData();
                curState.RawPosition = CalculatePosition();
            }
            m_previousTarget = FollowTarget;
        }

        private void UpdateCinemachineScreenTransposerData()
        {
            if (FollowTarget != null && m_previousTarget != FollowTarget)
            {
                m_cinemachineScreenTransposerData = FollowTarget.GetComponent<BossPathWalker>().CinemachineScreenTransposerData;
            }
        }

        private Vector3 CalculatePosition()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 position = FollowTarget.position + m_cinemachineScreenTransposerData.m_offsetFromTarget;

            float aspectRatio = mainCamera.aspect;
            float xWorldDistance = (m_cinemachineScreenTransposerData.m_verticalHeight * aspectRatio) / 2.0f;
            float screenXPercentage = -m_cinemachineScreenTransposerData.m_screenX * 2 + 1;
            position += FollowTarget.right * xWorldDistance * screenXPercentage;

            float yWorldDistance = (m_cinemachineScreenTransposerData.m_verticalHeight / 2.0f);
            float screenYPercentage = m_cinemachineScreenTransposerData.m_screenY * 2 - 1;
            position += FollowTarget.up * yWorldDistance * screenYPercentage;

            float zWorldDistance = m_cinemachineScreenTransposerData.m_verticalHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2.0f));
            position += zWorldDistance * -FollowTarget.forward;

            return position;
        }
    }

}