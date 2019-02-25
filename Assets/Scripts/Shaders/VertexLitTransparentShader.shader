Shader "Handout/TransparentVertexLit" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (0,1,1,1)
		_Ambient ("Ambient", Range(0,1)) = 0.1
		_Alpha ("Alpha", Range(0,1)) = 0.3
	}
	SubShader {
		ZWrite Off
		ZTest Less
		Cull Off
		BlendOp Add
		Blend One One


		Tags {
			"Queue" = "Transparent"
			"LightMode"="ForwardBase"
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_SHADER_NO_UPGRADE 1

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 diffuseLight : COLOR;
			};

			sampler2D _MainTex;
			float4 _Color;
			float _Ambient;
			float _Alpha;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.uv;
				half3 worldNormal = mul(UNITY_MATRIX_M,float4(v.normal,0));
                float lt = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                lt = lt * (1-_Ambient) + _Ambient;
                o.diffuseLight = float4(lt,lt,lt,1);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				float4 col = _Color;
				col.a = _Alpha;
				return tex2D(_MainTex,i.uv) * i.diffuseLight * col;
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
