using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OpenworldShaderBlackboard : MonoBehaviour
{

    [Header("rim glow")]
    [SerializeField] Color rimColor;
    [SerializeField] float rimRange;

    [Header("noise texture")]
    [SerializeField] Texture psudoNormalTexture;
    [SerializeField] float psudoNormalIntensity;
    [SerializeField] float psudoNormalScale;

    [Header("lighting Ramp")]
    [SerializeField] Texture lightRamp;

    private void OnValidate()
    {
        Shader.SetGlobalColor("_RimGlow", rimColor);
        Shader.SetGlobalFloat("_RimRange", rimRange);
        Shader.SetGlobalFloat("_PsudoNormalEffect", psudoNormalIntensity);
        Shader.SetGlobalFloat("_PsudoNormalScale", psudoNormalScale);
        Shader.SetGlobalTexture("_PsudoNormal", psudoNormalTexture);
        Shader.SetGlobalTexture("_RampTexture", lightRamp);
       // Debug.Log("data..");
    }


}
