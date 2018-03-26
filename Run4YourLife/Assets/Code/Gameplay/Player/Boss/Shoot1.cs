﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Shoot1 : Shoot
    {
        #region Inspector

        [SerializeField]
        private GameObject instance;

        [SerializeField]
        private float bulletSpeed;

        #endregion

        protected override void OnSuccess()
        {
            audioSource.PlayOneShot(sfx);

            Vector3 director = (crossHair.position - shootInitZone.position).normalized;
            GameObject tempInstance = Instantiate(instance, shootInitZone.position, instance.transform.rotation);
            tempInstance.GetComponent<Rigidbody>().velocity = director * bulletSpeed * Time.deltaTime;
        }
    }
}
