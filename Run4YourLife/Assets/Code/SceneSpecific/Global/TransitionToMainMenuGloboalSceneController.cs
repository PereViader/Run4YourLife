﻿using UnityEngine;
using Run4YourLife.SceneManagement;

using Run4YourLife.Utils;

namespace Run4YourLife.SceneSpecific.Global
{
    public class TransitionToMainMenuGloboalSceneController : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_sceneTransitionRequest;

        private void Start()
        {
            StartCoroutine(YieldHelper.WaitForSeconds(()=> m_sceneTransitionRequest.Execute(), 17.0f));
        }
    }
}