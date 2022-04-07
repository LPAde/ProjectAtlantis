using System;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            CheckInputs();
        }

        private void FixedUpdate()
        {
            throw new NotImplementedException();
        }

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                print(GameManager.Instance.RhythmManager.CheckTiming());
            }
        }
    }
}
