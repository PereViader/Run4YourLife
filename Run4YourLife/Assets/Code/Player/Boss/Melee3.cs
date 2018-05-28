﻿using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    public class Melee3 : Melee
    {
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
        }

        protected override void OnSuccess()
        {
            animator.SetTrigger("Mele");
            AudioManager.Instance.PlayFX(AudioManager.Sfx.BossMelee);
        }
    }
}