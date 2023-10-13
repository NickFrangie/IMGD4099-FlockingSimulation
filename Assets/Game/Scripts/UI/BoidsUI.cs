using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BoidsUI : MonoBehaviour
    {
        // Inspector
        [Header("References")]
        [SerializeField] private BoidsDisplay boidsDisplay;
        [SerializeField] private Button toggleButton;
        
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Toggle();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
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
        
        public void OnToggleButtonClicked()
        {
            boidsDisplay.ToggleDisplay();
            toggleButton.GetComponentInChildren<TMP_Text>().text = "Display Mode: " +
                                                                   (boidsDisplay.isShaderMode
                                                                       ? "Shader"
                                                                       : "Game Objects");
            toggleButton.GetComponent<Image>().color = boidsDisplay.isShaderMode
                ? Color.green
                : Color.red;
        }
    }
}