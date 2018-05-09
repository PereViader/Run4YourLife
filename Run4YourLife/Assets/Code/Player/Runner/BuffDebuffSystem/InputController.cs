﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.InputManagement;

public class InputController : MonoBehaviour {
    
    private Dictionary<Action, List<InputStatusEffect>> m_inputStatusEffects = new Dictionary<Action, List<InputStatusEffect>>();
    private ControlScheme m_controlScheme;

    private void Awake()
    {
        m_controlScheme = GetComponent<ControlScheme>();
        Debug.Assert(m_controlScheme != null);

        foreach(Action action in m_controlScheme.Actions)
        {
            m_inputStatusEffects.Add(action, new List<InputStatusEffect>());
        }
    }

    public bool Started(Action action)
    {
        Debug.Assert(action.InputSource.InputSourceType == InputSourceType.Button);
        bool started = action.Started();

        foreach(InputStatusEffect inputStatusEffect in m_inputStatusEffects[action])
        {
            switch(inputStatusEffect.inputModifierType)
            {
                case InputModifierType.Override:
                    started = inputStatusEffect.bool_value;
                    break;
                case InputModifierType.Plain:
                    started = started || inputStatusEffect.bool_value;
                    break;
                case InputModifierType.Maximize:
                    Debug.LogError("Button input types are not compatible with maximize modifier");
                    break;
            }
        }
        return started;
    }

    public float Value(Action action)
    {
        Debug.Assert(action.InputSource.InputSourceType == InputSourceType.Axis || action.InputSource.InputSourceType == InputSourceType.Trigger);
        float value = action.Value();

        foreach (InputStatusEffect inputStatusEffect in m_inputStatusEffects[action])
        {
            switch (inputStatusEffect.inputModifierType)
            {
                case InputModifierType.Override:
                    value = inputStatusEffect.float_value;
                    break;
                case InputModifierType.Plain:
                    value = value + inputStatusEffect.float_value;
                    break;
                case InputModifierType.Maximize:
                    value = Mathf.Sign(action.LastValue);
                    break;
            }
        }
        return value;
    }

    public void Add(InputStatusEffect inputStatusEffect)
    {
        Action action = m_controlScheme.GetByName(inputStatusEffect.actionName);
        Debug.Assert(action != null);
        m_inputStatusEffects[action].Add(inputStatusEffect);
    }

    public void Remove(InputStatusEffect inputStatusEffect)
    {
        Action action = m_controlScheme.GetByName(inputStatusEffect.actionName);
        Debug.Assert(action != null);
        m_inputStatusEffects[action].Remove(inputStatusEffect);
    }
}
