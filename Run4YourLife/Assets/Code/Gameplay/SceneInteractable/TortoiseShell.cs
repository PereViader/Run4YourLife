﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife;

public class TortoiseShell : MonoBehaviour {

    #region Inspector
    [SerializeField]
    private float sinkDelay = 0.5f;
    [SerializeField]
    private float emergeDelay = 0.5f;
    [SerializeField]
    private float acceleration = 10.0f;
    [SerializeField]
    private float sinkMaxDistance = 5.0f;
    #endregion

    #region Variables
    List<Transform> players = new List<Transform>();
    private float currentSpeed = 0.0f;
    private float movementOffset = 0.0f;
    private float actionTime = 0.0f;
    private enum State{ SINKING,EMERGING,NONE };
    private State state = State.NONE;
    private bool triggerLogicActive = true;
    private bool changingStateFlag = false;
    #endregion

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case State.EMERGING:
                {
                    if (movementOffset <= sinkMaxDistance)
                    {
                        actionTime += Time.deltaTime;
                        currentSpeed = Mathf.Pow(acceleration,actionTime) ;
                        Move();
                    }
                    else
                    {
                        triggerLogicActive = true;
                        state = State.NONE;
                    }
                }
                break;
            case State.SINKING:
                if (movementOffset <= sinkMaxDistance)
                {
                    actionTime += Time.deltaTime;
                    currentSpeed = -Mathf.Pow(acceleration, actionTime);
                    Move();
                }
                else
                {
                    if (!changingStateFlag)
                    {
                        StartCoroutine(ChangeStateDelayed(State.EMERGING, emergeDelay));                     
                    }
                }
                break;
            case State.NONE:
                break;
        }
	}

    private void Move()
    {
        MovePlayers(currentSpeed);
        float offset = Mathf.Abs(currentSpeed * Time.deltaTime);
        movementOffset += offset;
        if (movementOffset >= sinkMaxDistance)
        {
            transform.Translate(new Vector3(0, 0, Mathf.Sign(currentSpeed) * (sinkMaxDistance - (movementOffset - offset))));
        }
        else
        {
            transform.Translate(new Vector3(0, 0, currentSpeed * Time.deltaTime));
        }
    }

    private void MovePlayers(float speedValue)
    {
        foreach (Transform transform in players)
        {
            transform.Translate(new Vector3(0, speedValue * Time.deltaTime, 0), Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Runner))
        {
            players.Add(other.transform);
            if (triggerLogicActive)
            {
                triggerLogicActive = false;
                StartCoroutine(ChangeStateDelayed(State.SINKING, sinkDelay));
            }
        }
    }

    IEnumerator ChangeStateDelayed(State s,float delay)
    {
        changingStateFlag = true;
        yield return new WaitForSeconds(delay);
        changingStateFlag = false;
        currentSpeed = 0.0f;
        movementOffset = 0.0f;
        actionTime = 0.0f;
        state = s;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Runner))
        {
            players.Remove(other.transform);
        }
    }
}
