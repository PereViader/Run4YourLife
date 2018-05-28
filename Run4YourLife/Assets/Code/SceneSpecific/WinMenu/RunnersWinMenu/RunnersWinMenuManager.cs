﻿using TMPro;
using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;
using System;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class RunnersWinMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject runnerSlotPrefab;

        [SerializeField]
        private GameObject[] spawnPoints;

        [SerializeField]
        private float winnerScale = 5.0f;

        [SerializeField]
        private float othersScale = 3.0f;

        [SerializeField]
        private string winnerAnimation = "dance";

        [SerializeField]
        private string othersAnimation = "idle";

        private RunnerPrefabManager runnerPrefabManager;

        void Awake()
        {
            runnerPrefabManager = GetComponent<RunnerPrefabManager>();         
        }

        private void Start()
        {
            if(PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                SetUpTestPlayers(3);
            }

            if(!GlobalDataContainer.Instance.Data.ContainsKey(GlobalDataContainerKeys.Score))
            {
                SetUpTestScores();
            }

            SpawnRunners();
            ClearScoreData();
        }

        private void ClearScoreData()
        {
            GlobalDataContainer.Instance.Data.Remove(GlobalDataContainerKeys.Score);
        }

        private void SetUpTestPlayers(uint numRunners)
        {
            Debug.Log("Setting up test players");

            for(uint i = 0; i < numRunners; ++i)
            {
                PlayerManager.Instance.AddPlayer(new PlayerHandle()
                {
                    CharacterType = (CharacterType)i,
                    InputDevice = InputDeviceManager.Instance.InputDevices[(int)i],
                    IsBoss = false
                });
            }
        }

        private void SetUpTestScores()
        {
            Debug.Log("Setting up test scores");
            Dictionary<PlayerHandle, float> scores = new Dictionary<PlayerHandle,float>();
            foreach(PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                scores[playerHandle] = UnityEngine.Random.Range(0.0f, 50.0f);
            }
            GlobalDataContainer.Instance.Data[GlobalDataContainerKeys.Score] = scores;
        }

        private void SpawnRunners()
        {
            Dictionary<PlayerHandle,int> scores = GlobalDataContainer.Instance.Data[GlobalDataContainerKeys.Score] as Dictionary<PlayerHandle,int>;
            Debug.Assert(scores != null);
            List<KeyValuePair<float, PlayerHandle>> points = new List<KeyValuePair<float, PlayerHandle>>();

            foreach(PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                int score;
                scores.TryGetValue(playerHandle,out score);
                points.Add(new KeyValuePair<float, PlayerHandle>(score, playerHandle));
            }

            points.Sort((a, b) => b.Key.CompareTo(a.Key));

            bool isWinner = true;

            for(int i = 0; i < points.Count; ++i)
            {
                GameObject runnerSlot = Instantiate(runnerSlotPrefab, spawnPoints[i].transform, false);
                GameObject runner = Instantiate(runnerPrefabManager.Get(points[i].Value.CharacterType), runnerSlot.transform, false);

                float scale = othersScale;

                if(isWinner)
                {
                    isWinner = false;
                    scale = winnerScale;
                    runner.GetComponent<Animator>().Play(winnerAnimation);
                }
                else
                {
                    runner.GetComponent<Animator>().Play(othersAnimation);
                }

                runnerSlot.transform.localScale = scale * Vector3.one;

                runnerSlot.GetComponentInChildren<TextMeshPro>().text = ((int)Mathf.Round(points[i].Key)).ToString();
            }
        }
    }
}