using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private float finalHeight;
        private Resolution _res;

        private void Start()
        {
            // Prevent the resolution hurting the credits.
            _res = Screen.currentResolution;
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
        }

        void Update()
        {
            transform.position += new Vector3(0,Time.deltaTime * scrollSpeed,0);

            if (!(transform.position.y > finalHeight) && !Input.GetKeyDown(KeyCode.Escape)) 
                return;
            
            Screen.SetResolution(_res.width,_res.height,FullScreenMode.FullScreenWindow);
            SceneManager.LoadScene(0);
        }
    }
}
