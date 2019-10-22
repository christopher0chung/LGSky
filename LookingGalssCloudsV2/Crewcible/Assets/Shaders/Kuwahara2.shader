// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Kuwahara2"
{
	Properties
	{
		
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 screenPos = i.ase_texcoord;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 break65_g49 = ase_grabScreenPosNorm.xy;
				float4 appendResult21 = (float4(_ScreenParams.x , _ScreenParams.y , 0.0 , 0.0));
				float2 break66_g49 = appendResult21.xy;
				float temp_output_6_0_g49 = ( break65_g49.x * break66_g49.x );
				float2 break23_g49 = float2( -1,1 );
				float temp_output_29_0_g49 = ( ( temp_output_6_0_g49 + ( 2 * break23_g49.x ) ) / break66_g49.x );
				float temp_output_7_0_g49 = ( break65_g49.y * break66_g49.y );
				float temp_output_24_0_g49 = ( ( temp_output_7_0_g49 + ( 0 * break23_g49.y ) ) / break66_g49.y );
				float4 appendResult42_g49 = (float4(temp_output_29_0_g49 , temp_output_24_0_g49 , 0.0 , 0.0));
				float4 screenColor43_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult42_g49.xy);
				float temp_output_1_0_g50 = screenColor43_g49.r;
				float temp_output_28_0_g49 = ( ( temp_output_6_0_g49 + ( 1 * break23_g49.x ) ) / break66_g49.x );
				float4 appendResult36_g49 = (float4(temp_output_28_0_g49 , temp_output_24_0_g49 , 0.0 , 0.0));
				float4 screenColor37_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult36_g49.xy);
				float temp_output_2_0_g50 = screenColor37_g49.r;
				float temp_output_27_0_g49 = ( ( temp_output_6_0_g49 + ( 0 * break23_g49.x ) ) / break66_g49.x );
				float4 appendResult30_g49 = (float4(temp_output_27_0_g49 , temp_output_24_0_g49 , 0.0 , 0.0));
				float4 screenColor31_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult30_g49.xy);
				float temp_output_3_0_g50 = screenColor31_g49.r;
				float temp_output_25_0_g49 = ( ( temp_output_7_0_g49 + ( 1 * break23_g49.y ) ) / break66_g49.y );
				float4 appendResult32_g49 = (float4(temp_output_27_0_g49 , temp_output_25_0_g49 , 0.0 , 0.0));
				float4 screenColor33_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult32_g49.xy);
				float temp_output_4_0_g50 = screenColor33_g49.r;
				float4 appendResult38_g49 = (float4(temp_output_28_0_g49 , temp_output_25_0_g49 , 0.0 , 0.0));
				float4 screenColor39_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult38_g49.xy);
				float temp_output_5_0_g50 = screenColor39_g49.r;
				float4 appendResult44_g49 = (float4(temp_output_29_0_g49 , temp_output_25_0_g49 , 0.0 , 0.0));
				float4 screenColor45_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult44_g49.xy);
				float temp_output_6_0_g50 = screenColor45_g49.r;
				float temp_output_26_0_g49 = ( ( temp_output_7_0_g49 + ( 2 * break23_g49.y ) ) / break66_g49.y );
				float4 appendResult34_g49 = (float4(temp_output_27_0_g49 , temp_output_26_0_g49 , 0.0 , 0.0));
				float4 screenColor35_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult34_g49.xy);
				float temp_output_7_0_g50 = screenColor35_g49.r;
				float4 appendResult40_g49 = (float4(temp_output_28_0_g49 , temp_output_26_0_g49 , 0.0 , 0.0));
				float4 screenColor41_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult40_g49.xy);
				float4 appendResult46_g49 = (float4(temp_output_29_0_g49 , temp_output_26_0_g49 , 0.0 , 0.0));
				float4 screenColor47_g49 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult46_g49.xy);
				float temp_output_9_0_g50 = screenColor47_g49.r;
				float temp_output_1_0_g51 = screenColor43_g49.g;
				float temp_output_2_0_g51 = screenColor37_g49.g;
				float temp_output_3_0_g51 = screenColor31_g49.g;
				float temp_output_4_0_g51 = screenColor45_g49.g;
				float temp_output_5_0_g51 = screenColor39_g49.g;
				float temp_output_6_0_g51 = screenColor33_g49.g;
				float temp_output_7_0_g51 = screenColor35_g49.g;
				float temp_output_9_0_g51 = screenColor47_g49.g;
				float temp_output_1_0_g52 = screenColor43_g49.b;
				float temp_output_2_0_g52 = screenColor37_g49.g;
				float temp_output_3_0_g52 = screenColor31_g49.b;
				float temp_output_4_0_g52 = screenColor45_g49.b;
				float temp_output_5_0_g52 = screenColor39_g49.b;
				float temp_output_6_0_g52 = screenColor33_g49.b;
				float temp_output_7_0_g52 = screenColor35_g49.b;
				float temp_output_9_0_g52 = screenColor47_g49.b;
				float temp_output_33_0 = ( ( max( max( max( max( temp_output_1_0_g50 , temp_output_2_0_g50 ) , max( temp_output_3_0_g50 , temp_output_4_0_g50 ) ) , max( max( temp_output_5_0_g50 , temp_output_6_0_g50 ) , max( temp_output_7_0_g50 , screenColor41_g49.r ) ) ) , temp_output_9_0_g50 ) - min( temp_output_1_0_g50 , min( min( min( temp_output_2_0_g50 , temp_output_3_0_g50 ) , min( temp_output_4_0_g50 , temp_output_5_0_g50 ) ) , min( min( temp_output_6_0_g50 , temp_output_7_0_g50 ) , min( temp_output_7_0_g50 , temp_output_9_0_g50 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g51 , temp_output_2_0_g51 ) , max( temp_output_3_0_g51 , temp_output_4_0_g51 ) ) , max( max( temp_output_5_0_g51 , temp_output_6_0_g51 ) , max( temp_output_7_0_g51 , screenColor41_g49.g ) ) ) , temp_output_9_0_g51 ) - min( temp_output_1_0_g51 , min( min( min( temp_output_2_0_g51 , temp_output_3_0_g51 ) , min( temp_output_4_0_g51 , temp_output_5_0_g51 ) ) , min( min( temp_output_6_0_g51 , temp_output_7_0_g51 ) , min( temp_output_7_0_g51 , temp_output_9_0_g51 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g52 , temp_output_2_0_g52 ) , max( temp_output_3_0_g52 , temp_output_4_0_g52 ) ) , max( max( temp_output_5_0_g52 , temp_output_6_0_g52 ) , max( temp_output_7_0_g52 , screenColor41_g49.b ) ) ) , temp_output_9_0_g52 ) - min( temp_output_1_0_g52 , min( min( min( temp_output_2_0_g52 , temp_output_3_0_g52 ) , min( temp_output_4_0_g52 , temp_output_5_0_g52 ) ) , min( min( temp_output_6_0_g52 , temp_output_7_0_g52 ) , min( temp_output_7_0_g52 , temp_output_9_0_g52 ) ) ) ) ) );
				float2 break65_g37 = ase_grabScreenPosNorm.xy;
				float2 break66_g37 = appendResult21.xy;
				float temp_output_6_0_g37 = ( break65_g37.x * break66_g37.x );
				float2 break23_g37 = float2( 1,1 );
				float temp_output_29_0_g37 = ( ( temp_output_6_0_g37 + ( 2 * break23_g37.x ) ) / break66_g37.x );
				float temp_output_7_0_g37 = ( break65_g37.y * break66_g37.y );
				float temp_output_24_0_g37 = ( ( temp_output_7_0_g37 + ( 0 * break23_g37.y ) ) / break66_g37.y );
				float4 appendResult42_g37 = (float4(temp_output_29_0_g37 , temp_output_24_0_g37 , 0.0 , 0.0));
				float4 screenColor43_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult42_g37.xy);
				float temp_output_1_0_g38 = screenColor43_g37.r;
				float temp_output_28_0_g37 = ( ( temp_output_6_0_g37 + ( 1 * break23_g37.x ) ) / break66_g37.x );
				float4 appendResult36_g37 = (float4(temp_output_28_0_g37 , temp_output_24_0_g37 , 0.0 , 0.0));
				float4 screenColor37_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult36_g37.xy);
				float temp_output_2_0_g38 = screenColor37_g37.r;
				float temp_output_27_0_g37 = ( ( temp_output_6_0_g37 + ( 0 * break23_g37.x ) ) / break66_g37.x );
				float4 appendResult30_g37 = (float4(temp_output_27_0_g37 , temp_output_24_0_g37 , 0.0 , 0.0));
				float4 screenColor31_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult30_g37.xy);
				float temp_output_3_0_g38 = screenColor31_g37.r;
				float temp_output_25_0_g37 = ( ( temp_output_7_0_g37 + ( 1 * break23_g37.y ) ) / break66_g37.y );
				float4 appendResult32_g37 = (float4(temp_output_27_0_g37 , temp_output_25_0_g37 , 0.0 , 0.0));
				float4 screenColor33_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult32_g37.xy);
				float temp_output_4_0_g38 = screenColor33_g37.r;
				float4 appendResult38_g37 = (float4(temp_output_28_0_g37 , temp_output_25_0_g37 , 0.0 , 0.0));
				float4 screenColor39_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult38_g37.xy);
				float temp_output_5_0_g38 = screenColor39_g37.r;
				float4 appendResult44_g37 = (float4(temp_output_29_0_g37 , temp_output_25_0_g37 , 0.0 , 0.0));
				float4 screenColor45_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult44_g37.xy);
				float temp_output_6_0_g38 = screenColor45_g37.r;
				float temp_output_26_0_g37 = ( ( temp_output_7_0_g37 + ( 2 * break23_g37.y ) ) / break66_g37.y );
				float4 appendResult34_g37 = (float4(temp_output_27_0_g37 , temp_output_26_0_g37 , 0.0 , 0.0));
				float4 screenColor35_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult34_g37.xy);
				float temp_output_7_0_g38 = screenColor35_g37.r;
				float4 appendResult40_g37 = (float4(temp_output_28_0_g37 , temp_output_26_0_g37 , 0.0 , 0.0));
				float4 screenColor41_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult40_g37.xy);
				float4 appendResult46_g37 = (float4(temp_output_29_0_g37 , temp_output_26_0_g37 , 0.0 , 0.0));
				float4 screenColor47_g37 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult46_g37.xy);
				float temp_output_9_0_g38 = screenColor47_g37.r;
				float temp_output_1_0_g39 = screenColor43_g37.g;
				float temp_output_2_0_g39 = screenColor37_g37.g;
				float temp_output_3_0_g39 = screenColor31_g37.g;
				float temp_output_4_0_g39 = screenColor45_g37.g;
				float temp_output_5_0_g39 = screenColor39_g37.g;
				float temp_output_6_0_g39 = screenColor33_g37.g;
				float temp_output_7_0_g39 = screenColor35_g37.g;
				float temp_output_9_0_g39 = screenColor47_g37.g;
				float temp_output_1_0_g40 = screenColor43_g37.b;
				float temp_output_2_0_g40 = screenColor37_g37.g;
				float temp_output_3_0_g40 = screenColor31_g37.b;
				float temp_output_4_0_g40 = screenColor45_g37.b;
				float temp_output_5_0_g40 = screenColor39_g37.b;
				float temp_output_6_0_g40 = screenColor33_g37.b;
				float temp_output_7_0_g40 = screenColor35_g37.b;
				float temp_output_9_0_g40 = screenColor47_g37.b;
				float temp_output_31_0 = ( ( max( max( max( max( temp_output_1_0_g38 , temp_output_2_0_g38 ) , max( temp_output_3_0_g38 , temp_output_4_0_g38 ) ) , max( max( temp_output_5_0_g38 , temp_output_6_0_g38 ) , max( temp_output_7_0_g38 , screenColor41_g37.r ) ) ) , temp_output_9_0_g38 ) - min( temp_output_1_0_g38 , min( min( min( temp_output_2_0_g38 , temp_output_3_0_g38 ) , min( temp_output_4_0_g38 , temp_output_5_0_g38 ) ) , min( min( temp_output_6_0_g38 , temp_output_7_0_g38 ) , min( temp_output_7_0_g38 , temp_output_9_0_g38 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g39 , temp_output_2_0_g39 ) , max( temp_output_3_0_g39 , temp_output_4_0_g39 ) ) , max( max( temp_output_5_0_g39 , temp_output_6_0_g39 ) , max( temp_output_7_0_g39 , screenColor41_g37.g ) ) ) , temp_output_9_0_g39 ) - min( temp_output_1_0_g39 , min( min( min( temp_output_2_0_g39 , temp_output_3_0_g39 ) , min( temp_output_4_0_g39 , temp_output_5_0_g39 ) ) , min( min( temp_output_6_0_g39 , temp_output_7_0_g39 ) , min( temp_output_7_0_g39 , temp_output_9_0_g39 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g40 , temp_output_2_0_g40 ) , max( temp_output_3_0_g40 , temp_output_4_0_g40 ) ) , max( max( temp_output_5_0_g40 , temp_output_6_0_g40 ) , max( temp_output_7_0_g40 , screenColor41_g37.b ) ) ) , temp_output_9_0_g40 ) - min( temp_output_1_0_g40 , min( min( min( temp_output_2_0_g40 , temp_output_3_0_g40 ) , min( temp_output_4_0_g40 , temp_output_5_0_g40 ) ) , min( min( temp_output_6_0_g40 , temp_output_7_0_g40 ) , min( temp_output_7_0_g40 , temp_output_9_0_g40 ) ) ) ) ) );
				float ifLocalVar10 = 0;
				if( temp_output_33_0 <= temp_output_31_0 )
				ifLocalVar10 = temp_output_33_0;
				else
				ifLocalVar10 = temp_output_31_0;
				float2 break65_g45 = ase_grabScreenPosNorm.xy;
				float2 break66_g45 = appendResult21.xy;
				float temp_output_6_0_g45 = ( break65_g45.x * break66_g45.x );
				float2 break23_g45 = float2( -1,-1 );
				float temp_output_29_0_g45 = ( ( temp_output_6_0_g45 + ( 2 * break23_g45.x ) ) / break66_g45.x );
				float temp_output_7_0_g45 = ( break65_g45.y * break66_g45.y );
				float temp_output_24_0_g45 = ( ( temp_output_7_0_g45 + ( 0 * break23_g45.y ) ) / break66_g45.y );
				float4 appendResult42_g45 = (float4(temp_output_29_0_g45 , temp_output_24_0_g45 , 0.0 , 0.0));
				float4 screenColor43_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult42_g45.xy);
				float temp_output_1_0_g46 = screenColor43_g45.r;
				float temp_output_28_0_g45 = ( ( temp_output_6_0_g45 + ( 1 * break23_g45.x ) ) / break66_g45.x );
				float4 appendResult36_g45 = (float4(temp_output_28_0_g45 , temp_output_24_0_g45 , 0.0 , 0.0));
				float4 screenColor37_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult36_g45.xy);
				float temp_output_2_0_g46 = screenColor37_g45.r;
				float temp_output_27_0_g45 = ( ( temp_output_6_0_g45 + ( 0 * break23_g45.x ) ) / break66_g45.x );
				float4 appendResult30_g45 = (float4(temp_output_27_0_g45 , temp_output_24_0_g45 , 0.0 , 0.0));
				float4 screenColor31_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult30_g45.xy);
				float temp_output_3_0_g46 = screenColor31_g45.r;
				float temp_output_25_0_g45 = ( ( temp_output_7_0_g45 + ( 1 * break23_g45.y ) ) / break66_g45.y );
				float4 appendResult32_g45 = (float4(temp_output_27_0_g45 , temp_output_25_0_g45 , 0.0 , 0.0));
				float4 screenColor33_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult32_g45.xy);
				float temp_output_4_0_g46 = screenColor33_g45.r;
				float4 appendResult38_g45 = (float4(temp_output_28_0_g45 , temp_output_25_0_g45 , 0.0 , 0.0));
				float4 screenColor39_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult38_g45.xy);
				float temp_output_5_0_g46 = screenColor39_g45.r;
				float4 appendResult44_g45 = (float4(temp_output_29_0_g45 , temp_output_25_0_g45 , 0.0 , 0.0));
				float4 screenColor45_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult44_g45.xy);
				float temp_output_6_0_g46 = screenColor45_g45.r;
				float temp_output_26_0_g45 = ( ( temp_output_7_0_g45 + ( 2 * break23_g45.y ) ) / break66_g45.y );
				float4 appendResult34_g45 = (float4(temp_output_27_0_g45 , temp_output_26_0_g45 , 0.0 , 0.0));
				float4 screenColor35_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult34_g45.xy);
				float temp_output_7_0_g46 = screenColor35_g45.r;
				float4 appendResult40_g45 = (float4(temp_output_28_0_g45 , temp_output_26_0_g45 , 0.0 , 0.0));
				float4 screenColor41_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult40_g45.xy);
				float4 appendResult46_g45 = (float4(temp_output_29_0_g45 , temp_output_26_0_g45 , 0.0 , 0.0));
				float4 screenColor47_g45 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult46_g45.xy);
				float temp_output_9_0_g46 = screenColor47_g45.r;
				float temp_output_1_0_g47 = screenColor43_g45.g;
				float temp_output_2_0_g47 = screenColor37_g45.g;
				float temp_output_3_0_g47 = screenColor31_g45.g;
				float temp_output_4_0_g47 = screenColor45_g45.g;
				float temp_output_5_0_g47 = screenColor39_g45.g;
				float temp_output_6_0_g47 = screenColor33_g45.g;
				float temp_output_7_0_g47 = screenColor35_g45.g;
				float temp_output_9_0_g47 = screenColor47_g45.g;
				float temp_output_1_0_g48 = screenColor43_g45.b;
				float temp_output_2_0_g48 = screenColor37_g45.g;
				float temp_output_3_0_g48 = screenColor31_g45.b;
				float temp_output_4_0_g48 = screenColor45_g45.b;
				float temp_output_5_0_g48 = screenColor39_g45.b;
				float temp_output_6_0_g48 = screenColor33_g45.b;
				float temp_output_7_0_g48 = screenColor35_g45.b;
				float temp_output_9_0_g48 = screenColor47_g45.b;
				float temp_output_32_0 = ( ( max( max( max( max( temp_output_1_0_g46 , temp_output_2_0_g46 ) , max( temp_output_3_0_g46 , temp_output_4_0_g46 ) ) , max( max( temp_output_5_0_g46 , temp_output_6_0_g46 ) , max( temp_output_7_0_g46 , screenColor41_g45.r ) ) ) , temp_output_9_0_g46 ) - min( temp_output_1_0_g46 , min( min( min( temp_output_2_0_g46 , temp_output_3_0_g46 ) , min( temp_output_4_0_g46 , temp_output_5_0_g46 ) ) , min( min( temp_output_6_0_g46 , temp_output_7_0_g46 ) , min( temp_output_7_0_g46 , temp_output_9_0_g46 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g47 , temp_output_2_0_g47 ) , max( temp_output_3_0_g47 , temp_output_4_0_g47 ) ) , max( max( temp_output_5_0_g47 , temp_output_6_0_g47 ) , max( temp_output_7_0_g47 , screenColor41_g45.g ) ) ) , temp_output_9_0_g47 ) - min( temp_output_1_0_g47 , min( min( min( temp_output_2_0_g47 , temp_output_3_0_g47 ) , min( temp_output_4_0_g47 , temp_output_5_0_g47 ) ) , min( min( temp_output_6_0_g47 , temp_output_7_0_g47 ) , min( temp_output_7_0_g47 , temp_output_9_0_g47 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g48 , temp_output_2_0_g48 ) , max( temp_output_3_0_g48 , temp_output_4_0_g48 ) ) , max( max( temp_output_5_0_g48 , temp_output_6_0_g48 ) , max( temp_output_7_0_g48 , screenColor41_g45.b ) ) ) , temp_output_9_0_g48 ) - min( temp_output_1_0_g48 , min( min( min( temp_output_2_0_g48 , temp_output_3_0_g48 ) , min( temp_output_4_0_g48 , temp_output_5_0_g48 ) ) , min( min( temp_output_6_0_g48 , temp_output_7_0_g48 ) , min( temp_output_7_0_g48 , temp_output_9_0_g48 ) ) ) ) ) );
				float2 break65_g41 = ase_grabScreenPosNorm.xy;
				float2 break66_g41 = appendResult21.xy;
				float temp_output_6_0_g41 = ( break65_g41.x * break66_g41.x );
				float2 break23_g41 = float2( 1,-1 );
				float temp_output_29_0_g41 = ( ( temp_output_6_0_g41 + ( 2 * break23_g41.x ) ) / break66_g41.x );
				float temp_output_7_0_g41 = ( break65_g41.y * break66_g41.y );
				float temp_output_24_0_g41 = ( ( temp_output_7_0_g41 + ( 0 * break23_g41.y ) ) / break66_g41.y );
				float4 appendResult42_g41 = (float4(temp_output_29_0_g41 , temp_output_24_0_g41 , 0.0 , 0.0));
				float4 screenColor43_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult42_g41.xy);
				float temp_output_1_0_g42 = screenColor43_g41.r;
				float temp_output_28_0_g41 = ( ( temp_output_6_0_g41 + ( 1 * break23_g41.x ) ) / break66_g41.x );
				float4 appendResult36_g41 = (float4(temp_output_28_0_g41 , temp_output_24_0_g41 , 0.0 , 0.0));
				float4 screenColor37_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult36_g41.xy);
				float temp_output_2_0_g42 = screenColor37_g41.r;
				float temp_output_27_0_g41 = ( ( temp_output_6_0_g41 + ( 0 * break23_g41.x ) ) / break66_g41.x );
				float4 appendResult30_g41 = (float4(temp_output_27_0_g41 , temp_output_24_0_g41 , 0.0 , 0.0));
				float4 screenColor31_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult30_g41.xy);
				float temp_output_3_0_g42 = screenColor31_g41.r;
				float temp_output_25_0_g41 = ( ( temp_output_7_0_g41 + ( 1 * break23_g41.y ) ) / break66_g41.y );
				float4 appendResult32_g41 = (float4(temp_output_27_0_g41 , temp_output_25_0_g41 , 0.0 , 0.0));
				float4 screenColor33_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult32_g41.xy);
				float temp_output_4_0_g42 = screenColor33_g41.r;
				float4 appendResult38_g41 = (float4(temp_output_28_0_g41 , temp_output_25_0_g41 , 0.0 , 0.0));
				float4 screenColor39_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult38_g41.xy);
				float temp_output_5_0_g42 = screenColor39_g41.r;
				float4 appendResult44_g41 = (float4(temp_output_29_0_g41 , temp_output_25_0_g41 , 0.0 , 0.0));
				float4 screenColor45_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult44_g41.xy);
				float temp_output_6_0_g42 = screenColor45_g41.r;
				float temp_output_26_0_g41 = ( ( temp_output_7_0_g41 + ( 2 * break23_g41.y ) ) / break66_g41.y );
				float4 appendResult34_g41 = (float4(temp_output_27_0_g41 , temp_output_26_0_g41 , 0.0 , 0.0));
				float4 screenColor35_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult34_g41.xy);
				float temp_output_7_0_g42 = screenColor35_g41.r;
				float4 appendResult40_g41 = (float4(temp_output_28_0_g41 , temp_output_26_0_g41 , 0.0 , 0.0));
				float4 screenColor41_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult40_g41.xy);
				float4 appendResult46_g41 = (float4(temp_output_29_0_g41 , temp_output_26_0_g41 , 0.0 , 0.0));
				float4 screenColor47_g41 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,appendResult46_g41.xy);
				float temp_output_9_0_g42 = screenColor47_g41.r;
				float temp_output_1_0_g43 = screenColor43_g41.g;
				float temp_output_2_0_g43 = screenColor37_g41.g;
				float temp_output_3_0_g43 = screenColor31_g41.g;
				float temp_output_4_0_g43 = screenColor45_g41.g;
				float temp_output_5_0_g43 = screenColor39_g41.g;
				float temp_output_6_0_g43 = screenColor33_g41.g;
				float temp_output_7_0_g43 = screenColor35_g41.g;
				float temp_output_9_0_g43 = screenColor47_g41.g;
				float temp_output_1_0_g44 = screenColor43_g41.b;
				float temp_output_2_0_g44 = screenColor37_g41.g;
				float temp_output_3_0_g44 = screenColor31_g41.b;
				float temp_output_4_0_g44 = screenColor45_g41.b;
				float temp_output_5_0_g44 = screenColor39_g41.b;
				float temp_output_6_0_g44 = screenColor33_g41.b;
				float temp_output_7_0_g44 = screenColor35_g41.b;
				float temp_output_9_0_g44 = screenColor47_g41.b;
				float temp_output_30_0 = ( ( max( max( max( max( temp_output_1_0_g42 , temp_output_2_0_g42 ) , max( temp_output_3_0_g42 , temp_output_4_0_g42 ) ) , max( max( temp_output_5_0_g42 , temp_output_6_0_g42 ) , max( temp_output_7_0_g42 , screenColor41_g41.r ) ) ) , temp_output_9_0_g42 ) - min( temp_output_1_0_g42 , min( min( min( temp_output_2_0_g42 , temp_output_3_0_g42 ) , min( temp_output_4_0_g42 , temp_output_5_0_g42 ) ) , min( min( temp_output_6_0_g42 , temp_output_7_0_g42 ) , min( temp_output_7_0_g42 , temp_output_9_0_g42 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g43 , temp_output_2_0_g43 ) , max( temp_output_3_0_g43 , temp_output_4_0_g43 ) ) , max( max( temp_output_5_0_g43 , temp_output_6_0_g43 ) , max( temp_output_7_0_g43 , screenColor41_g41.g ) ) ) , temp_output_9_0_g43 ) - min( temp_output_1_0_g43 , min( min( min( temp_output_2_0_g43 , temp_output_3_0_g43 ) , min( temp_output_4_0_g43 , temp_output_5_0_g43 ) ) , min( min( temp_output_6_0_g43 , temp_output_7_0_g43 ) , min( temp_output_7_0_g43 , temp_output_9_0_g43 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g44 , temp_output_2_0_g44 ) , max( temp_output_3_0_g44 , temp_output_4_0_g44 ) ) , max( max( temp_output_5_0_g44 , temp_output_6_0_g44 ) , max( temp_output_7_0_g44 , screenColor41_g41.b ) ) ) , temp_output_9_0_g44 ) - min( temp_output_1_0_g44 , min( min( min( temp_output_2_0_g44 , temp_output_3_0_g44 ) , min( temp_output_4_0_g44 , temp_output_5_0_g44 ) ) , min( min( temp_output_6_0_g44 , temp_output_7_0_g44 ) , min( temp_output_7_0_g44 , temp_output_9_0_g44 ) ) ) ) ) );
				float ifLocalVar12 = 0;
				if( temp_output_32_0 <= temp_output_30_0 )
				ifLocalVar12 = temp_output_32_0;
				else
				ifLocalVar12 = temp_output_30_0;
				float4 temp_output_32_56 = ( ( screenColor31_g45 + screenColor37_g45 + screenColor43_g45 + screenColor33_g45 + screenColor39_g45 + screenColor45_g45 + screenColor35_g45 + screenColor41_g45 + screenColor47_g45 ) / 9.0 );
				float4 ifLocalVar13 = 0;
				if( temp_output_32_0 <= temp_output_30_0 )
				ifLocalVar13 = temp_output_32_56;
				else
				ifLocalVar13 = ( ( screenColor31_g41 + screenColor37_g41 + screenColor43_g41 + screenColor33_g41 + screenColor39_g41 + screenColor45_g41 + screenColor35_g41 + screenColor41_g41 + screenColor47_g41 ) / 9.0 );
				float4 temp_output_33_56 = ( ( screenColor31_g49 + screenColor37_g49 + screenColor43_g49 + screenColor33_g49 + screenColor39_g49 + screenColor45_g49 + screenColor35_g49 + screenColor41_g49 + screenColor47_g49 ) / 9.0 );
				float4 ifLocalVar11 = 0;
				if( temp_output_33_0 <= temp_output_31_0 )
				ifLocalVar11 = temp_output_33_56;
				else
				ifLocalVar11 = ( ( screenColor31_g37 + screenColor37_g37 + screenColor43_g37 + screenColor33_g37 + screenColor39_g37 + screenColor45_g37 + screenColor35_g37 + screenColor41_g37 + screenColor47_g37 ) / 9.0 );
				float4 ifLocalVar14 = 0;
				if( ifLocalVar10 <= ifLocalVar12 )
				ifLocalVar14 = ifLocalVar11;
				else
				ifLocalVar14 = ifLocalVar13;
				
				
				finalColor = ifLocalVar14;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17000
677;83;1100;515;1907.742;353.061;1.933872;True;False
Node;AmplifyShaderEditor.ScreenParams;20;-1764.331,53.62074;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;19;-1509.479,-180.0229;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;6;-1216,-176;Float;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;21;-1432.119,89.56598;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;8;-1216,176;Float;False;Constant;_Vector2;Vector 2;0;0;Create;True;0;0;False;0;-1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;7;-1216,0;Float;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;9;-1216,352;Float;False;Constant;_Vector3;Vector 3;0;0;Create;True;0;0;False;0;1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;33;-1008,-176;Float;False;KuwaharaRegion;-1;;49;d55ee1caa774cb54ea8e2d4f4f0960da;0;3;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-1008,176;Float;False;KuwaharaRegion;-1;;45;d55ee1caa774cb54ea8e2d4f4f0960da;0;3;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;30;-1008,352;Float;False;KuwaharaRegion;-1;;41;d55ee1caa774cb54ea8e2d4f4f0960da;0;3;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;31;-1008,0;Float;False;KuwaharaRegion;-1;;37;d55ee1caa774cb54ea8e2d4f4f0960da;0;3;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;10;-560,-176;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;13;-560,352;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;11;-560,0;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;12;-560,176;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;14;-272,0;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;1;Kuwahara2;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;33;1;6;0
WireConnection;33;63;19;0
WireConnection;33;64;21;0
WireConnection;32;1;8;0
WireConnection;32;63;19;0
WireConnection;32;64;21;0
WireConnection;30;1;9;0
WireConnection;30;63;19;0
WireConnection;30;64;21;0
WireConnection;31;1;7;0
WireConnection;31;63;19;0
WireConnection;31;64;21;0
WireConnection;10;0;33;0
WireConnection;10;1;31;0
WireConnection;10;2;31;0
WireConnection;10;3;33;0
WireConnection;10;4;33;0
WireConnection;13;0;32;0
WireConnection;13;1;30;0
WireConnection;13;2;30;56
WireConnection;13;3;32;56
WireConnection;13;4;32;56
WireConnection;11;0;33;0
WireConnection;11;1;31;0
WireConnection;11;2;31;56
WireConnection;11;3;33;56
WireConnection;11;4;33;56
WireConnection;12;0;32;0
WireConnection;12;1;30;0
WireConnection;12;2;30;0
WireConnection;12;3;32;0
WireConnection;12;4;32;0
WireConnection;14;0;10;0
WireConnection;14;1;12;0
WireConnection;14;2;13;0
WireConnection;14;3;11;0
WireConnection;14;4;11;0
WireConnection;1;0;14;0
ASEEND*/
//CHKSM=A7109785A22DDC3B37059CE11B6825445E7CE599