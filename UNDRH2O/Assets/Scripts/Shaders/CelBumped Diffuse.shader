//James Gartland, UNDR[H20]

Shader "Cel/BjørnVolp" {
	Properties {
	//Volp Properties
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Float) = 1.4 
	//Bjørn
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Cel

		sampler2D _Ramp;

		// Custom lighting model
		inline half4 LightingCel(SurfaceOutput s, half3 lightDir, half atten) {
			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

			// Calculate lighting from angle between normal and light direction
			half NdotL = saturate(dot(s.Normal, lightDir));
			// New lighting based on texture ramp
			half3 ramp = tex2D(_Ramp, half2(NdotL, 0.5));

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb  * (atten * 2) * ramp;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		half4 _Color;
		half4 _RimColor;
		half _RimPower;

		struct Input {
			half2 uv_MainTex;
			half3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Calculate rim lighting from angle between normal and view direction
			half NdotV = saturate(dot(o.Normal, normalize(IN.viewDir)));
			// New lighting based on texture ramp
			half ramp = tex2D(_Ramp, half2(1.0 - NdotV, 0.5));

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a;
			o.Emission = _RimColor.rgb * pow(ramp, _RimPower);
		}
		ENDCG
	}
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
 
v2f vert(appdata v) {
	// just make a copy of incoming vertex data but scaled according to normal direction
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _OutlineColor;
	return o;
}
ENDCG
 
	SubShader {
		Tags { "Queue" = "Transparent" }
 
		Pass {
			Name "BASE"
			Cull Back
			Blend Zero One
 
			// uncomment this to hide inner details:
			//Offset -8, -8
 
			SetTexture [_OutlineColor] {
				ConstantColor (0,0,0,0)
				Combine constant
			}
		}
 
		// note that a vertex shader is specified here but its using the one above
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
 
			// you can choose what kind of blending mode you want for the outline
			//Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			Blend One OneMinusDstColor // Soft Additive
			//Blend DstColor Zero // Multiplicative
			//Blend DstColor SrcColor // 2x Multiplicative
 
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
 
half4 frag(v2f i) :COLOR {
	return i.color;
}
ENDCG
		}
 
 
	}
	FallBack "Diffuse"
}