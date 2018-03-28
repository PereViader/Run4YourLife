﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class EasyMoveHorizontalPhaseManager : GamePhaseManager
    {
        #region Editor variables

        [SerializeField]
        private GameObject m_phase1to2Bridge;

        [SerializeField]
        private GameObject m_phase2StartTrigger;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private CheckPointManager m_checkPointManager;

        #endregion

        #region Member variables

        private PlayerSpawner m_playerSpawner;
        private GameplayPlayerManager m_gameplayPlayerManager;

        #endregion

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);

            RegisterPhase(GamePhase.EasyMoveHorizontal);
        }

        #endregion

        #region Regular Execution

        public override void StartPhase()
        {
            StartPhaseCommon();
        }

        void StartPhaseCommon()
        {
            GameObject boss = m_gameplayPlayerManager.Boss;
            Debug.Assert(boss != null);

            m_virtualCamera.Follow = boss.transform;
            m_virtualCamera.LookAt = boss.transform;
            m_virtualCamera.gameObject.SetActive(true);

            m_checkPointManager.gameObject.SetActive(true);
            m_phase1to2Bridge.SetActive(true);
            m_phase2StartTrigger.SetActive(true);
        }

        public override void EndPhase()
        {
            EndPhaseCommon();
        }

        void EndPhaseCommon()
        {
            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            m_virtualCamera.gameObject.SetActive(false);

            m_checkPointManager.gameObject.SetActive(false);
        }

        #endregion

        #region Debug Execution

        public override void DebugStartPhase()
        {
            m_playerSpawner.InstantiatePlayers();
            StartPhaseCommon();
        }

        public override void DebugEndPhase()
        {
            m_gameplayPlayerManager.DebugDestroyAllPlayersAndClear();
            EndPhaseCommon();
        }

        #endregion
    }
}