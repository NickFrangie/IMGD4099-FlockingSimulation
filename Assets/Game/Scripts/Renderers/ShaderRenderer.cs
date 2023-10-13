using System;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class ShaderRenderer : MonoBehaviour
    {
        // Inspector
        [SerializeField] private ComputeShader renderShader;

        // Internal
        private RenderTexture renderTexture;

        private void OnEnable()
        {
        
        }

        private void OnDisable()
        {
            
        }

        private void SetupTexture()
        {
            renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
            
            renderShader.SetTexture(0, "output", renderTexture);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            SetupTexture();
            renderShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
            
            Graphics.Blit(renderTexture, destination);
        }
    }
}