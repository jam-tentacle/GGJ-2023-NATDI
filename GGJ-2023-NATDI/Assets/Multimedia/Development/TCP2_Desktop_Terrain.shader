
// Toony Colors Pro+Mobile 2

Shader "Nature/Terrain/Toon"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
		
		//TOONY COLORS RAMP
		[TCP2Gradient] _Ramp ("#RAMPT# Toon Ramp (RGB)", 2D) = "gray" {}
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.01,1)) = 0.1
		
		//BUMP
		_BumpMap ("#NORM# Normal map (RGB)", 2D) = "bump" {}
		
	}
	
	SubShader
	{
		Tags {
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
		}
		LOD 200
		
		CGPROGRAM
		
		#include "Include/TCP2_Include.cginc"
		
		#pragma surface surf ToonyColors vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer addshadow fullforwardshadows
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
		#pragma multi_compile_fog
		#pragma multi_compile __ _NORMALMAP
		#pragma target 3.0
		
		#pragma shader_feature TCP2_DISABLE_WRAPPED_LIGHT
		#pragma shader_feature TCP2_RAMPTEXT
		#pragma shader_feature TCP2_BUMP
		
#include "TerrainSplatmapCommon.cginc"
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 splat_control;
			half weight;
			fixed4 mixedDiffuse;
			SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);

			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
			o.Gloss = mixedDiffuse.a;
			
	#if TCP2_BUMP
			//Normal map
			//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	#endif
		}
		
		ENDCG
		
		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
		UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
	}
	
	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Specular-AddPass"
	Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Specular-Base"
	Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/Diffuse-BaseGen"

	Fallback "Nature/Terrain/Diffuse"
	//CustomEditor "TCP2_MaterialInspector"
}