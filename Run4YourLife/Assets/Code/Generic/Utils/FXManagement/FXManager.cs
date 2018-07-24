﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SingletonMonoBehaviour<FXManager>
{
    [SerializeField]
    private GameObjectPool m_gameObjectPool;

    public GameObject InstantiateFromReceiver(FXReceiver receiver, GameObject prefab, bool setAsParent = false)
    {
        Transform parent = setAsParent ? receiver.transform : null;
        return InstantiateFromValues(receiver.transform.position, receiver.transform.rotation, prefab, parent);
    }

    public GameObject InstantiateFromValues(Vector3 position, Quaternion rotation, GameObject prefab, Transform parent = null)
    {
        GameObject instance = m_gameObjectPool.GetAndPosition(prefab, position, rotation, true);

        SimulateChildOf simulateChildOf = instance.GetComponent<SimulateChildOf>();
        if (simulateChildOf == null)
        {
            Debug.LogWarning("Instance of particle <" + prefab.name + ">  needs the component SimulateChildOf on it's root", prefab);
            simulateChildOf = instance.AddComponent<SimulateChildOf>();
        }
        simulateChildOf.Parent = parent;

        return instance;
    }
}
