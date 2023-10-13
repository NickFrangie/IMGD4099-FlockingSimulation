using System;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Handles the which renderer is displaying boids.
    /// </summary>
    public class BoidsDisplay : MonoBehaviour
    {
        [Header("Mode")]
        [SerializeField] internal bool isShaderMode;
        
        [Header("Renderers")]
        [SerializeField] private ShaderRenderer shaderRenderer;
        [SerializeField] private GameObjectRenderer gameObjectsRenderer;


        private void Start()
        {
            SetDisplay();
        }

        private void SetDisplay()
        {
            shaderRenderer.enabled = isShaderMode;
            gameObjectsRenderer.gameObject.SetActive(!isShaderMode);
        }
        
        public void ToggleDisplay()
        {
            isShaderMode = !isShaderMode;
            SetDisplay();
        }
    }
}