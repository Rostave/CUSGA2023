using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace R0.PP
{
    public static class DepthPerspectiveSwitch
    {
        public static bool EnableRenderFeature = true;
        public static DepthPerspectiveGrabPass DepthPerspectivePass;
    }
    
    [System.Serializable]
    public class DepthPerspectiveSetting
    {
        public string renderTag;
        public Material depthPerspectiveMat; //, distortionMaskMat;
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Vector2Int grabPassSize = new Vector2Int(1920, 1080);
    }
    
    public class DepthPerspectiveGrabPass : ScriptableRenderPass
    {
        protected readonly string _renderTag;
        protected readonly Material _depthPerspectiveMat;
        protected RenderTargetHandle _tempTarget;
        protected CommandBuffer _cmd;
        protected Vector2Int _grabPassSize;
        
        protected readonly int CameraColor = Shader.PropertyToID("_CameraColorTexture");

        public DepthPerspectiveGrabPass(DepthPerspectiveSetting setting)
        {
            _renderTag = setting.renderTag;
            _grabPassSize = setting.grabPassSize;
            renderPassEvent = setting.renderPassEvent;
            _depthPerspectiveMat = setting.depthPerspectiveMat;
            _tempTarget.Init("_Temp");
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(_renderTag);
            cmd.GetTemporaryRT(_tempTarget.id, _grabPassSize.x, _grabPassSize.y);
            var tempIdentifier = _tempTarget.Identifier();
            cmd.Blit(CameraColor, tempIdentifier, _depthPerspectiveMat);
            cmd.Blit(tempIdentifier, CameraColor);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { cmd.ReleaseTemporaryRT(_tempTarget.id); }

    }
    
    public class DepthPerspectiveFeature : ScriptableRendererFeature
    {
        private DepthPerspectiveGrabPass _scriptablePass;
        public DepthPerspectiveSetting settings;

        public override void Create()
        {
            _scriptablePass = new DepthPerspectiveGrabPass(settings);
            DepthPerspectiveSwitch.DepthPerspectivePass = _scriptablePass;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.isSceneViewCamera) return;
            if (!renderingData.postProcessingEnabled) return;
            if (!DepthPerspectiveSwitch.EnableRenderFeature) return;
            renderer.EnqueuePass(_scriptablePass);
        }
    }
}