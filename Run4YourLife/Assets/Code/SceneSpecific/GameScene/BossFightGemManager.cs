﻿using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightGemManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onAllGemsActive;

    [SerializeField]
    private List<GameObject> unactiveGems;

    [SerializeField]
    private Transform activeGemPosition;

    [SerializeField]
    private float m_timeBetweenGems = 2;

    [SerializeField]
    private float m_timeToGoToStand = 0.75f;

    private List<GameObject> activeGems = new List<GameObject>();
    private GameObject activeGem = null;

    public void ActivateGems(Transform gemStand)
    {
        activeGem.GetComponent<GemController>().enabled = false;
        activeGem.SetActive(false);
        unactiveGems.Remove(activeGem);
        activeGems.Add(activeGem);
        
        StartCoroutine(YieldHelper.WaitForSeconds(() => MoveGemToStand(gemStand), m_timeToGoToStand));

        if (unactiveGems.Count == 0)
        {
            m_onAllGemsActive.Invoke();
        }
        else
        {
            StartCoroutine(YieldHelper.WaitForSeconds(()=>ActivateNextGem(), m_timeBetweenGems));
        }      
    }

    public void MoveGemToStand(Transform gemStand)
    {
        activeGem.transform.position = gemStand.position;
        activeGem.SetActive(true);
    }

    public void ActivateNextGem()
    {
        activeGem = unactiveGems[0];
        activeGem.transform.position = activeGemPosition.position;          
        activeGem.SetActive(true);
    }
}
