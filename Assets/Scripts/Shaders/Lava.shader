Shader "Custom/LavaShader" {
	Properties{
		_MainTex( "Texture", 2D ) = "white" {}
		//_Occlusion("Occlusion", 2D) = "white" {}
		//_OcclusionStrength("OcclusionStrength", Range(0, 2)) = 1
		_TimeScale( "TimeScale", float) = 0.25
		_Color( "Color", Color ) = ( 1,1,1,1 )
		_Ambient( "Ambient", Range( 0, 1 ) ) = 0.1
		_TimeOffset( "TimeOffset", Range( 0, 1 ) ) = 0
		_Amplitude( "Amplitude", Range( 0, 3 ) ) = 0.035
		_Speed( "Speed", Range( 0, 10 ) ) = 2.8
		_SwaySpeed( "SwaySpeed", Range( 0, 10 ) ) = 0.45
		_SwayAmplitude( "SwayAmplitude", Range( 0, 5 ) ) = 0.015
		_Zoom ( "Zoom", Range( -10, 0.99 ) ) = 0
	}
	SubShader{
		ZWrite Off
		ZTest Less
		Cull Off
		BlendOp Add
		Blend One OneMinusSrcAlpha

		Tags {
			"Queue" = "Transparent"
			"Lightmode" = "ForwardBase"
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma glsl
			#define UNITY_SHADER_NO_UPGRADE 1


			#include "UnityCG.cginc"

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
			//sampler2D _Occlusion;
			//float _OcclusionStrength;
			float _TimeScale;
			float4 _Color;
			float _Ambient;
			float _TimeOffset;
			float _Amplitude;
			float _Speed;
			float _SwaySpeed;
			float _SwayAmplitude;
			float _Zoom;

			v2f vert(appdata v) {
				v2f o;

				// Getting Brightness from Occlusion Map
				/*#if !defined( SHADER_API_OPENGL )
				fixed4 occlusion = tex2Dlod( _Occlusion, float4( v.uv.xy, 0, 0 ) );
				float occlusionBrightness = 1 - ( occlusion.r + occlusion.g + occlusion.b ) / 3;
				#endif*/

				// Wave movement
				//v.vertex.y += _Amplitude * _OcclusionStrength * occlusionBrightness * sin(v.vertex[0] + (_Time.y - _TimeOffset) * _Speed);
				v.vertex.y += _Amplitude * sin( v.vertex[0] + ( _Time.y - _TimeOffset ) * _Speed );

				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				// Cycle the UVs:
				o.uv = v.uv + float2( _Time.x * _TimeScale, _Time.x * _TimeScale );

				// The following lines are equivalent: 
				// (transform the normal to world space - only direction, no position change!)
				half3 worldNormal = mul(UNITY_MATRIX_M, float4(v.normal, 0));

				// Copy and manipulate the UV's
				o.uv[0] = ( v.uv[0] + sin( _Time.y * _SwaySpeed ) * _SwayAmplitude ) * ( 1 - _Zoom );
				o.uv[1] = ( v.uv[1] + cos( _Time.y * _SwaySpeed ) * _SwayAmplitude ) * ( 1 - _Zoom );

				// dot product between normal and light direction for
				// standard diffuse ( Lambert ) lighting:
				float lt = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				// scale the range from [ 0, 1 ] to [ _Ambient, 1 ], to take ambient light ( =base value ) into account:
				lt = lt * ( 1 - _Ambient ) + _Ambient;
				o.diffuseLight = float4( lt, lt, lt, 1 );

				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * i.diffuseLight * _Color;
				return col;
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
