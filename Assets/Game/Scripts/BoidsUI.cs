using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BoidsUI : MonoBehaviour
    {
        // Internal
        private bool isShown = true;
        
        // References
        private CanvasGroup canvasGroup;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            if (isShown)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            
            isShown = !isShown;
        }
        
        public void OnResetButtonClicked()
        {
            BoidsManager.instance.Reset();
        }
    }
}