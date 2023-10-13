using System;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class ShaderRenderer : MonoBehaviour
    {
        // Constant
        private static readonly int[] TEXTURE_RESOLUTION = { 1920 / 4, 1080 / 4};
        
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
            renderTexture = new RenderTexture(TEXTURE_RESOLUTION[0], TEXTURE_RESOLUTION[1], 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
            
            renderShader.SetTexture(0, "output", renderTexture);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            SetupTexture();
            renderShader.SetInt("boids_count", BoidsManager.instance.boidsCount);
            renderShader.SetInts("res", TEXTURE_RESOLUTION);
            renderShader.SetBuffer(0, "boids_data_buffer", BoidsManager.instance.boidsDataBuffer);
            
            int threadGroupSize = Mathf.CeilToInt(BoidsManager.instance.boidsCount / 64.0f);
            renderShader.Dispatch(0, threadGroupSize, 1, 1);
            
            Graphics.Blit(renderTexture, destination);
        }
    }
}