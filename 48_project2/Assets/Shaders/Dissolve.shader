Shader "Custom/DissolveShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		// _MainTex ("Albedo (RGB)", 2D) = "white" {}
 
		//Dissolve properties
		_DissolveTexture("Dissolve Texutre", 2D) = "white" {} 
		_Amount("Amount", Range(0,1)) = 0
	}
 
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Cull Off //Fast way to turn your material double-sided
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{	
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
		
		
				sampler2D _MainTex;
				float4 _MainTex_ST;
		
				struct vertIn{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct vertOut{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};
						
				//Dissolve properties
				sampler2D _DissolveTexture;
				float _Amount;

				fixed4 _Color;

				vertOut vert(vertIn IN){
					vertOut OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
					return OUT;
				}

				fixed4 frag(vertOut IN) : SV_Target{
					
					float dissolve_value = tex2D(_DissolveTexture, IN.uv).r;
					fixed4 pixelColor = tex2D (_MainTex, IN.uv);
					if(dissolve_value - _Amount < 0){
						return pixelColor.a = 0;
					}
					return pixelColor * _Color;
				}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
