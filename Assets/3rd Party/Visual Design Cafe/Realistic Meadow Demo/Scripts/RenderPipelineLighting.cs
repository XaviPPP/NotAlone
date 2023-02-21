using UnityEngine;
#if UNITY_EDITOR
using VisualDesignCafe.ShaderX.Editor.Drops;
#endif

[ExecuteInEditMode]
public class RenderPipelineLighting : MonoBehaviour
{
    [SerializeField]
    private GameObject _standardLighting;

    [SerializeField]
    private Material _standardSky;

    [SerializeField]
    private GameObject _universalLighting;

    [SerializeField]
    private Material _universalSky;

    [SerializeField]
    private GameObject _highDefinitionLighting;

    [SerializeField]
    private Material _highDefinitionSky;

    private void OnValidate()
    {
        Awake();
    }

    private void Awake()
    {
#if UNITY_EDITOR
        var renderPipeline = new RenderPipeline();
        switch( renderPipeline.Type )
        {
            case RenderPipelineType.Standard:
                _standardLighting.SetActive( true );
                _universalLighting.SetActive( false );
                _highDefinitionLighting.SetActive( false );
                RenderSettings.skybox = _standardSky;
                break;
            case RenderPipelineType.Universal:
                _standardLighting.SetActive( false );
                _universalLighting.SetActive( true );
                _highDefinitionLighting.SetActive( false );
                RenderSettings.skybox = _universalSky;
                break;
            case RenderPipelineType.HighDefinition:
                _standardLighting.SetActive( false );
                _universalLighting.SetActive( false );
                _highDefinitionLighting.SetActive( true );
                RenderSettings.skybox = _highDefinitionSky;
                break;
        }
#endif
    }
}