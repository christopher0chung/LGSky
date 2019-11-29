// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Kuwahara2"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		
	}

	SubShader
	{
		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			
			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 break65_g69 = ase_screenPosNorm.xy;
				float4 appendResult21 = (float4(_ScreenParams.x , _ScreenParams.y , 0.0 , 0.0));
				float2 break66_g69 = appendResult21.xy;
				float temp_output_6_0_g69 = ( break65_g69.x * break66_g69.x );
				float2 break23_g69 = float2( -1,1 );
				float temp_output_27_0_g69 = ( ( temp_output_6_0_g69 + ( 0 * break23_g69.x ) ) / break66_g69.x );
				float temp_output_7_0_g69 = ( break65_g69.y * break66_g69.y );
				float temp_output_24_0_g69 = ( ( temp_output_7_0_g69 + ( 0 * break23_g69.y ) ) / break66_g69.y );
				float4 appendResult30_g69 = (float4(temp_output_27_0_g69 , temp_output_24_0_g69 , 0.0 , 0.0));
				float4 tex2DNode75_g69 = tex2D( _MainTex, appendResult30_g69.xy );
				float temp_output_1_0_g72 = tex2DNode75_g69.r;
				float temp_output_28_0_g69 = ( ( temp_output_6_0_g69 + ( 1 * break23_g69.x ) ) / break66_g69.x );
				float4 appendResult36_g69 = (float4(temp_output_28_0_g69 , temp_output_24_0_g69 , 0.0 , 0.0));
				float4 tex2DNode79_g69 = tex2D( _MainTex, appendResult36_g69.xy );
				float temp_output_2_0_g72 = tex2DNode79_g69.r;
				float temp_output_29_0_g69 = ( ( temp_output_6_0_g69 + ( 2 * break23_g69.x ) ) / break66_g69.x );
				float4 appendResult42_g69 = (float4(temp_output_29_0_g69 , temp_output_24_0_g69 , 0.0 , 0.0));
				float4 tex2DNode82_g69 = tex2D( _MainTex, appendResult42_g69.xy );
				float temp_output_3_0_g72 = tex2DNode82_g69.r;
				float temp_output_25_0_g69 = ( ( temp_output_7_0_g69 + ( 1 * break23_g69.y ) ) / break66_g69.y );
				float4 appendResult32_g69 = (float4(temp_output_27_0_g69 , temp_output_25_0_g69 , 0.0 , 0.0));
				float4 tex2DNode76_g69 = tex2D( _MainTex, appendResult32_g69.xy );
				float temp_output_4_0_g72 = tex2DNode76_g69.r;
				float4 appendResult38_g69 = (float4(temp_output_28_0_g69 , temp_output_25_0_g69 , 0.0 , 0.0));
				float4 tex2DNode80_g69 = tex2D( _MainTex, appendResult38_g69.xy );
				float temp_output_5_0_g72 = tex2DNode80_g69.r;
				float4 appendResult44_g69 = (float4(temp_output_29_0_g69 , temp_output_25_0_g69 , 0.0 , 0.0));
				float4 tex2DNode83_g69 = tex2D( _MainTex, appendResult44_g69.xy );
				float temp_output_6_0_g72 = tex2DNode83_g69.r;
				float temp_output_26_0_g69 = ( ( temp_output_7_0_g69 + ( 2 * break23_g69.y ) ) / break66_g69.y );
				float4 appendResult34_g69 = (float4(temp_output_27_0_g69 , temp_output_26_0_g69 , 0.0 , 0.0));
				float4 tex2DNode77_g69 = tex2D( _MainTex, appendResult34_g69.xy );
				float temp_output_7_0_g72 = tex2DNode77_g69.r;
				float4 appendResult40_g69 = (float4(temp_output_28_0_g69 , temp_output_26_0_g69 , 0.0 , 0.0));
				float4 tex2DNode78_g69 = tex2D( _MainTex, appendResult40_g69.xy );
				float4 appendResult46_g69 = (float4(temp_output_29_0_g69 , temp_output_26_0_g69 , 0.0 , 0.0));
				float4 tex2DNode81_g69 = tex2D( _MainTex, appendResult46_g69.xy );
				float temp_output_9_0_g72 = tex2DNode81_g69.r;
				float temp_output_1_0_g71 = tex2DNode75_g69.g;
				float temp_output_2_0_g71 = tex2DNode79_g69.g;
				float temp_output_3_0_g71 = tex2DNode82_g69.g;
				float temp_output_4_0_g71 = tex2DNode76_g69.g;
				float temp_output_5_0_g71 = tex2DNode80_g69.g;
				float temp_output_6_0_g71 = tex2DNode83_g69.g;
				float temp_output_7_0_g71 = tex2DNode77_g69.r;
				float temp_output_9_0_g71 = tex2DNode81_g69.g;
				float temp_output_1_0_g70 = tex2DNode75_g69.b;
				float temp_output_2_0_g70 = tex2DNode79_g69.b;
				float temp_output_3_0_g70 = tex2DNode82_g69.b;
				float temp_output_4_0_g70 = tex2DNode76_g69.b;
				float temp_output_5_0_g70 = tex2DNode80_g69.b;
				float temp_output_6_0_g70 = tex2DNode83_g69.b;
				float temp_output_7_0_g70 = tex2DNode77_g69.b;
				float temp_output_9_0_g70 = tex2DNode81_g69.b;
				float temp_output_33_0 = ( ( max( max( max( max( temp_output_1_0_g72 , temp_output_2_0_g72 ) , max( temp_output_3_0_g72 , temp_output_4_0_g72 ) ) , max( max( temp_output_5_0_g72 , temp_output_6_0_g72 ) , max( temp_output_7_0_g72 , tex2DNode78_g69.r ) ) ) , temp_output_9_0_g72 ) - min( temp_output_1_0_g72 , min( min( min( temp_output_2_0_g72 , temp_output_3_0_g72 ) , min( temp_output_4_0_g72 , temp_output_5_0_g72 ) ) , min( min( temp_output_6_0_g72 , temp_output_7_0_g72 ) , min( temp_output_7_0_g72 , temp_output_9_0_g72 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g71 , temp_output_2_0_g71 ) , max( temp_output_3_0_g71 , temp_output_4_0_g71 ) ) , max( max( temp_output_5_0_g71 , temp_output_6_0_g71 ) , max( temp_output_7_0_g71 , tex2DNode78_g69.g ) ) ) , temp_output_9_0_g71 ) - min( temp_output_1_0_g71 , min( min( min( temp_output_2_0_g71 , temp_output_3_0_g71 ) , min( temp_output_4_0_g71 , temp_output_5_0_g71 ) ) , min( min( temp_output_6_0_g71 , temp_output_7_0_g71 ) , min( temp_output_7_0_g71 , temp_output_9_0_g71 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g70 , temp_output_2_0_g70 ) , max( temp_output_3_0_g70 , temp_output_4_0_g70 ) ) , max( max( temp_output_5_0_g70 , temp_output_6_0_g70 ) , max( temp_output_7_0_g70 , tex2DNode78_g69.b ) ) ) , temp_output_9_0_g70 ) - min( temp_output_1_0_g70 , min( min( min( temp_output_2_0_g70 , temp_output_3_0_g70 ) , min( temp_output_4_0_g70 , temp_output_5_0_g70 ) ) , min( min( temp_output_6_0_g70 , temp_output_7_0_g70 ) , min( temp_output_7_0_g70 , temp_output_9_0_g70 ) ) ) ) ) );
				float2 break65_g61 = ase_screenPosNorm.xy;
				float2 break66_g61 = appendResult21.xy;
				float temp_output_6_0_g61 = ( break65_g61.x * break66_g61.x );
				float2 break23_g61 = float2( 1,1 );
				float temp_output_27_0_g61 = ( ( temp_output_6_0_g61 + ( 0 * break23_g61.x ) ) / break66_g61.x );
				float temp_output_7_0_g61 = ( break65_g61.y * break66_g61.y );
				float temp_output_24_0_g61 = ( ( temp_output_7_0_g61 + ( 0 * break23_g61.y ) ) / break66_g61.y );
				float4 appendResult30_g61 = (float4(temp_output_27_0_g61 , temp_output_24_0_g61 , 0.0 , 0.0));
				float4 tex2DNode75_g61 = tex2D( _MainTex, appendResult30_g61.xy );
				float temp_output_1_0_g64 = tex2DNode75_g61.r;
				float temp_output_28_0_g61 = ( ( temp_output_6_0_g61 + ( 1 * break23_g61.x ) ) / break66_g61.x );
				float4 appendResult36_g61 = (float4(temp_output_28_0_g61 , temp_output_24_0_g61 , 0.0 , 0.0));
				float4 tex2DNode79_g61 = tex2D( _MainTex, appendResult36_g61.xy );
				float temp_output_2_0_g64 = tex2DNode79_g61.r;
				float temp_output_29_0_g61 = ( ( temp_output_6_0_g61 + ( 2 * break23_g61.x ) ) / break66_g61.x );
				float4 appendResult42_g61 = (float4(temp_output_29_0_g61 , temp_output_24_0_g61 , 0.0 , 0.0));
				float4 tex2DNode82_g61 = tex2D( _MainTex, appendResult42_g61.xy );
				float temp_output_3_0_g64 = tex2DNode82_g61.r;
				float temp_output_25_0_g61 = ( ( temp_output_7_0_g61 + ( 1 * break23_g61.y ) ) / break66_g61.y );
				float4 appendResult32_g61 = (float4(temp_output_27_0_g61 , temp_output_25_0_g61 , 0.0 , 0.0));
				float4 tex2DNode76_g61 = tex2D( _MainTex, appendResult32_g61.xy );
				float temp_output_4_0_g64 = tex2DNode76_g61.r;
				float4 appendResult38_g61 = (float4(temp_output_28_0_g61 , temp_output_25_0_g61 , 0.0 , 0.0));
				float4 tex2DNode80_g61 = tex2D( _MainTex, appendResult38_g61.xy );
				float temp_output_5_0_g64 = tex2DNode80_g61.r;
				float4 appendResult44_g61 = (float4(temp_output_29_0_g61 , temp_output_25_0_g61 , 0.0 , 0.0));
				float4 tex2DNode83_g61 = tex2D( _MainTex, appendResult44_g61.xy );
				float temp_output_6_0_g64 = tex2DNode83_g61.r;
				float temp_output_26_0_g61 = ( ( temp_output_7_0_g61 + ( 2 * break23_g61.y ) ) / break66_g61.y );
				float4 appendResult34_g61 = (float4(temp_output_27_0_g61 , temp_output_26_0_g61 , 0.0 , 0.0));
				float4 tex2DNode77_g61 = tex2D( _MainTex, appendResult34_g61.xy );
				float temp_output_7_0_g64 = tex2DNode77_g61.r;
				float4 appendResult40_g61 = (float4(temp_output_28_0_g61 , temp_output_26_0_g61 , 0.0 , 0.0));
				float4 tex2DNode78_g61 = tex2D( _MainTex, appendResult40_g61.xy );
				float4 appendResult46_g61 = (float4(temp_output_29_0_g61 , temp_output_26_0_g61 , 0.0 , 0.0));
				float4 tex2DNode81_g61 = tex2D( _MainTex, appendResult46_g61.xy );
				float temp_output_9_0_g64 = tex2DNode81_g61.r;
				float temp_output_1_0_g63 = tex2DNode75_g61.g;
				float temp_output_2_0_g63 = tex2DNode79_g61.g;
				float temp_output_3_0_g63 = tex2DNode82_g61.g;
				float temp_output_4_0_g63 = tex2DNode76_g61.g;
				float temp_output_5_0_g63 = tex2DNode80_g61.g;
				float temp_output_6_0_g63 = tex2DNode83_g61.g;
				float temp_output_7_0_g63 = tex2DNode77_g61.r;
				float temp_output_9_0_g63 = tex2DNode81_g61.g;
				float temp_output_1_0_g62 = tex2DNode75_g61.b;
				float temp_output_2_0_g62 = tex2DNode79_g61.b;
				float temp_output_3_0_g62 = tex2DNode82_g61.b;
				float temp_output_4_0_g62 = tex2DNode76_g61.b;
				float temp_output_5_0_g62 = tex2DNode80_g61.b;
				float temp_output_6_0_g62 = tex2DNode83_g61.b;
				float temp_output_7_0_g62 = tex2DNode77_g61.b;
				float temp_output_9_0_g62 = tex2DNode81_g61.b;
				float temp_output_31_0 = ( ( max( max( max( max( temp_output_1_0_g64 , temp_output_2_0_g64 ) , max( temp_output_3_0_g64 , temp_output_4_0_g64 ) ) , max( max( temp_output_5_0_g64 , temp_output_6_0_g64 ) , max( temp_output_7_0_g64 , tex2DNode78_g61.r ) ) ) , temp_output_9_0_g64 ) - min( temp_output_1_0_g64 , min( min( min( temp_output_2_0_g64 , temp_output_3_0_g64 ) , min( temp_output_4_0_g64 , temp_output_5_0_g64 ) ) , min( min( temp_output_6_0_g64 , temp_output_7_0_g64 ) , min( temp_output_7_0_g64 , temp_output_9_0_g64 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g63 , temp_output_2_0_g63 ) , max( temp_output_3_0_g63 , temp_output_4_0_g63 ) ) , max( max( temp_output_5_0_g63 , temp_output_6_0_g63 ) , max( temp_output_7_0_g63 , tex2DNode78_g61.g ) ) ) , temp_output_9_0_g63 ) - min( temp_output_1_0_g63 , min( min( min( temp_output_2_0_g63 , temp_output_3_0_g63 ) , min( temp_output_4_0_g63 , temp_output_5_0_g63 ) ) , min( min( temp_output_6_0_g63 , temp_output_7_0_g63 ) , min( temp_output_7_0_g63 , temp_output_9_0_g63 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g62 , temp_output_2_0_g62 ) , max( temp_output_3_0_g62 , temp_output_4_0_g62 ) ) , max( max( temp_output_5_0_g62 , temp_output_6_0_g62 ) , max( temp_output_7_0_g62 , tex2DNode78_g61.b ) ) ) , temp_output_9_0_g62 ) - min( temp_output_1_0_g62 , min( min( min( temp_output_2_0_g62 , temp_output_3_0_g62 ) , min( temp_output_4_0_g62 , temp_output_5_0_g62 ) ) , min( min( temp_output_6_0_g62 , temp_output_7_0_g62 ) , min( temp_output_7_0_g62 , temp_output_9_0_g62 ) ) ) ) ) );
				float ifLocalVar10 = 0;
				if( temp_output_33_0 <= temp_output_31_0 )
				ifLocalVar10 = temp_output_33_0;
				else
				ifLocalVar10 = temp_output_31_0;
				float2 break65_g65 = ase_screenPosNorm.xy;
				float2 break66_g65 = appendResult21.xy;
				float temp_output_6_0_g65 = ( break65_g65.x * break66_g65.x );
				float2 break23_g65 = float2( -1,-1 );
				float temp_output_27_0_g65 = ( ( temp_output_6_0_g65 + ( 0 * break23_g65.x ) ) / break66_g65.x );
				float temp_output_7_0_g65 = ( break65_g65.y * break66_g65.y );
				float temp_output_24_0_g65 = ( ( temp_output_7_0_g65 + ( 0 * break23_g65.y ) ) / break66_g65.y );
				float4 appendResult30_g65 = (float4(temp_output_27_0_g65 , temp_output_24_0_g65 , 0.0 , 0.0));
				float4 tex2DNode75_g65 = tex2D( _MainTex, appendResult30_g65.xy );
				float temp_output_1_0_g68 = tex2DNode75_g65.r;
				float temp_output_28_0_g65 = ( ( temp_output_6_0_g65 + ( 1 * break23_g65.x ) ) / break66_g65.x );
				float4 appendResult36_g65 = (float4(temp_output_28_0_g65 , temp_output_24_0_g65 , 0.0 , 0.0));
				float4 tex2DNode79_g65 = tex2D( _MainTex, appendResult36_g65.xy );
				float temp_output_2_0_g68 = tex2DNode79_g65.r;
				float temp_output_29_0_g65 = ( ( temp_output_6_0_g65 + ( 2 * break23_g65.x ) ) / break66_g65.x );
				float4 appendResult42_g65 = (float4(temp_output_29_0_g65 , temp_output_24_0_g65 , 0.0 , 0.0));
				float4 tex2DNode82_g65 = tex2D( _MainTex, appendResult42_g65.xy );
				float temp_output_3_0_g68 = tex2DNode82_g65.r;
				float temp_output_25_0_g65 = ( ( temp_output_7_0_g65 + ( 1 * break23_g65.y ) ) / break66_g65.y );
				float4 appendResult32_g65 = (float4(temp_output_27_0_g65 , temp_output_25_0_g65 , 0.0 , 0.0));
				float4 tex2DNode76_g65 = tex2D( _MainTex, appendResult32_g65.xy );
				float temp_output_4_0_g68 = tex2DNode76_g65.r;
				float4 appendResult38_g65 = (float4(temp_output_28_0_g65 , temp_output_25_0_g65 , 0.0 , 0.0));
				float4 tex2DNode80_g65 = tex2D( _MainTex, appendResult38_g65.xy );
				float temp_output_5_0_g68 = tex2DNode80_g65.r;
				float4 appendResult44_g65 = (float4(temp_output_29_0_g65 , temp_output_25_0_g65 , 0.0 , 0.0));
				float4 tex2DNode83_g65 = tex2D( _MainTex, appendResult44_g65.xy );
				float temp_output_6_0_g68 = tex2DNode83_g65.r;
				float temp_output_26_0_g65 = ( ( temp_output_7_0_g65 + ( 2 * break23_g65.y ) ) / break66_g65.y );
				float4 appendResult34_g65 = (float4(temp_output_27_0_g65 , temp_output_26_0_g65 , 0.0 , 0.0));
				float4 tex2DNode77_g65 = tex2D( _MainTex, appendResult34_g65.xy );
				float temp_output_7_0_g68 = tex2DNode77_g65.r;
				float4 appendResult40_g65 = (float4(temp_output_28_0_g65 , temp_output_26_0_g65 , 0.0 , 0.0));
				float4 tex2DNode78_g65 = tex2D( _MainTex, appendResult40_g65.xy );
				float4 appendResult46_g65 = (float4(temp_output_29_0_g65 , temp_output_26_0_g65 , 0.0 , 0.0));
				float4 tex2DNode81_g65 = tex2D( _MainTex, appendResult46_g65.xy );
				float temp_output_9_0_g68 = tex2DNode81_g65.r;
				float temp_output_1_0_g67 = tex2DNode75_g65.g;
				float temp_output_2_0_g67 = tex2DNode79_g65.g;
				float temp_output_3_0_g67 = tex2DNode82_g65.g;
				float temp_output_4_0_g67 = tex2DNode76_g65.g;
				float temp_output_5_0_g67 = tex2DNode80_g65.g;
				float temp_output_6_0_g67 = tex2DNode83_g65.g;
				float temp_output_7_0_g67 = tex2DNode77_g65.r;
				float temp_output_9_0_g67 = tex2DNode81_g65.g;
				float temp_output_1_0_g66 = tex2DNode75_g65.b;
				float temp_output_2_0_g66 = tex2DNode79_g65.b;
				float temp_output_3_0_g66 = tex2DNode82_g65.b;
				float temp_output_4_0_g66 = tex2DNode76_g65.b;
				float temp_output_5_0_g66 = tex2DNode80_g65.b;
				float temp_output_6_0_g66 = tex2DNode83_g65.b;
				float temp_output_7_0_g66 = tex2DNode77_g65.b;
				float temp_output_9_0_g66 = tex2DNode81_g65.b;
				float temp_output_32_0 = ( ( max( max( max( max( temp_output_1_0_g68 , temp_output_2_0_g68 ) , max( temp_output_3_0_g68 , temp_output_4_0_g68 ) ) , max( max( temp_output_5_0_g68 , temp_output_6_0_g68 ) , max( temp_output_7_0_g68 , tex2DNode78_g65.r ) ) ) , temp_output_9_0_g68 ) - min( temp_output_1_0_g68 , min( min( min( temp_output_2_0_g68 , temp_output_3_0_g68 ) , min( temp_output_4_0_g68 , temp_output_5_0_g68 ) ) , min( min( temp_output_6_0_g68 , temp_output_7_0_g68 ) , min( temp_output_7_0_g68 , temp_output_9_0_g68 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g67 , temp_output_2_0_g67 ) , max( temp_output_3_0_g67 , temp_output_4_0_g67 ) ) , max( max( temp_output_5_0_g67 , temp_output_6_0_g67 ) , max( temp_output_7_0_g67 , tex2DNode78_g65.g ) ) ) , temp_output_9_0_g67 ) - min( temp_output_1_0_g67 , min( min( min( temp_output_2_0_g67 , temp_output_3_0_g67 ) , min( temp_output_4_0_g67 , temp_output_5_0_g67 ) ) , min( min( temp_output_6_0_g67 , temp_output_7_0_g67 ) , min( temp_output_7_0_g67 , temp_output_9_0_g67 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g66 , temp_output_2_0_g66 ) , max( temp_output_3_0_g66 , temp_output_4_0_g66 ) ) , max( max( temp_output_5_0_g66 , temp_output_6_0_g66 ) , max( temp_output_7_0_g66 , tex2DNode78_g65.b ) ) ) , temp_output_9_0_g66 ) - min( temp_output_1_0_g66 , min( min( min( temp_output_2_0_g66 , temp_output_3_0_g66 ) , min( temp_output_4_0_g66 , temp_output_5_0_g66 ) ) , min( min( temp_output_6_0_g66 , temp_output_7_0_g66 ) , min( temp_output_7_0_g66 , temp_output_9_0_g66 ) ) ) ) ) );
				float2 break65_g73 = ase_screenPosNorm.xy;
				float2 break66_g73 = appendResult21.xy;
				float temp_output_6_0_g73 = ( break65_g73.x * break66_g73.x );
				float2 break23_g73 = float2( 1,-1 );
				float temp_output_27_0_g73 = ( ( temp_output_6_0_g73 + ( 0 * break23_g73.x ) ) / break66_g73.x );
				float temp_output_7_0_g73 = ( break65_g73.y * break66_g73.y );
				float temp_output_24_0_g73 = ( ( temp_output_7_0_g73 + ( 0 * break23_g73.y ) ) / break66_g73.y );
				float4 appendResult30_g73 = (float4(temp_output_27_0_g73 , temp_output_24_0_g73 , 0.0 , 0.0));
				float4 tex2DNode75_g73 = tex2D( _MainTex, appendResult30_g73.xy );
				float temp_output_1_0_g76 = tex2DNode75_g73.r;
				float temp_output_28_0_g73 = ( ( temp_output_6_0_g73 + ( 1 * break23_g73.x ) ) / break66_g73.x );
				float4 appendResult36_g73 = (float4(temp_output_28_0_g73 , temp_output_24_0_g73 , 0.0 , 0.0));
				float4 tex2DNode79_g73 = tex2D( _MainTex, appendResult36_g73.xy );
				float temp_output_2_0_g76 = tex2DNode79_g73.r;
				float temp_output_29_0_g73 = ( ( temp_output_6_0_g73 + ( 2 * break23_g73.x ) ) / break66_g73.x );
				float4 appendResult42_g73 = (float4(temp_output_29_0_g73 , temp_output_24_0_g73 , 0.0 , 0.0));
				float4 tex2DNode82_g73 = tex2D( _MainTex, appendResult42_g73.xy );
				float temp_output_3_0_g76 = tex2DNode82_g73.r;
				float temp_output_25_0_g73 = ( ( temp_output_7_0_g73 + ( 1 * break23_g73.y ) ) / break66_g73.y );
				float4 appendResult32_g73 = (float4(temp_output_27_0_g73 , temp_output_25_0_g73 , 0.0 , 0.0));
				float4 tex2DNode76_g73 = tex2D( _MainTex, appendResult32_g73.xy );
				float temp_output_4_0_g76 = tex2DNode76_g73.r;
				float4 appendResult38_g73 = (float4(temp_output_28_0_g73 , temp_output_25_0_g73 , 0.0 , 0.0));
				float4 tex2DNode80_g73 = tex2D( _MainTex, appendResult38_g73.xy );
				float temp_output_5_0_g76 = tex2DNode80_g73.r;
				float4 appendResult44_g73 = (float4(temp_output_29_0_g73 , temp_output_25_0_g73 , 0.0 , 0.0));
				float4 tex2DNode83_g73 = tex2D( _MainTex, appendResult44_g73.xy );
				float temp_output_6_0_g76 = tex2DNode83_g73.r;
				float temp_output_26_0_g73 = ( ( temp_output_7_0_g73 + ( 2 * break23_g73.y ) ) / break66_g73.y );
				float4 appendResult34_g73 = (float4(temp_output_27_0_g73 , temp_output_26_0_g73 , 0.0 , 0.0));
				float4 tex2DNode77_g73 = tex2D( _MainTex, appendResult34_g73.xy );
				float temp_output_7_0_g76 = tex2DNode77_g73.r;
				float4 appendResult40_g73 = (float4(temp_output_28_0_g73 , temp_output_26_0_g73 , 0.0 , 0.0));
				float4 tex2DNode78_g73 = tex2D( _MainTex, appendResult40_g73.xy );
				float4 appendResult46_g73 = (float4(temp_output_29_0_g73 , temp_output_26_0_g73 , 0.0 , 0.0));
				float4 tex2DNode81_g73 = tex2D( _MainTex, appendResult46_g73.xy );
				float temp_output_9_0_g76 = tex2DNode81_g73.r;
				float temp_output_1_0_g75 = tex2DNode75_g73.g;
				float temp_output_2_0_g75 = tex2DNode79_g73.g;
				float temp_output_3_0_g75 = tex2DNode82_g73.g;
				float temp_output_4_0_g75 = tex2DNode76_g73.g;
				float temp_output_5_0_g75 = tex2DNode80_g73.g;
				float temp_output_6_0_g75 = tex2DNode83_g73.g;
				float temp_output_7_0_g75 = tex2DNode77_g73.r;
				float temp_output_9_0_g75 = tex2DNode81_g73.g;
				float temp_output_1_0_g74 = tex2DNode75_g73.b;
				float temp_output_2_0_g74 = tex2DNode79_g73.b;
				float temp_output_3_0_g74 = tex2DNode82_g73.b;
				float temp_output_4_0_g74 = tex2DNode76_g73.b;
				float temp_output_5_0_g74 = tex2DNode80_g73.b;
				float temp_output_6_0_g74 = tex2DNode83_g73.b;
				float temp_output_7_0_g74 = tex2DNode77_g73.b;
				float temp_output_9_0_g74 = tex2DNode81_g73.b;
				float temp_output_30_0 = ( ( max( max( max( max( temp_output_1_0_g76 , temp_output_2_0_g76 ) , max( temp_output_3_0_g76 , temp_output_4_0_g76 ) ) , max( max( temp_output_5_0_g76 , temp_output_6_0_g76 ) , max( temp_output_7_0_g76 , tex2DNode78_g73.r ) ) ) , temp_output_9_0_g76 ) - min( temp_output_1_0_g76 , min( min( min( temp_output_2_0_g76 , temp_output_3_0_g76 ) , min( temp_output_4_0_g76 , temp_output_5_0_g76 ) ) , min( min( temp_output_6_0_g76 , temp_output_7_0_g76 ) , min( temp_output_7_0_g76 , temp_output_9_0_g76 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g75 , temp_output_2_0_g75 ) , max( temp_output_3_0_g75 , temp_output_4_0_g75 ) ) , max( max( temp_output_5_0_g75 , temp_output_6_0_g75 ) , max( temp_output_7_0_g75 , tex2DNode78_g73.g ) ) ) , temp_output_9_0_g75 ) - min( temp_output_1_0_g75 , min( min( min( temp_output_2_0_g75 , temp_output_3_0_g75 ) , min( temp_output_4_0_g75 , temp_output_5_0_g75 ) ) , min( min( temp_output_6_0_g75 , temp_output_7_0_g75 ) , min( temp_output_7_0_g75 , temp_output_9_0_g75 ) ) ) ) ) + ( max( max( max( max( temp_output_1_0_g74 , temp_output_2_0_g74 ) , max( temp_output_3_0_g74 , temp_output_4_0_g74 ) ) , max( max( temp_output_5_0_g74 , temp_output_6_0_g74 ) , max( temp_output_7_0_g74 , tex2DNode78_g73.b ) ) ) , temp_output_9_0_g74 ) - min( temp_output_1_0_g74 , min( min( min( temp_output_2_0_g74 , temp_output_3_0_g74 ) , min( temp_output_4_0_g74 , temp_output_5_0_g74 ) ) , min( min( temp_output_6_0_g74 , temp_output_7_0_g74 ) , min( temp_output_7_0_g74 , temp_output_9_0_g74 ) ) ) ) ) );
				float ifLocalVar12 = 0;
				if( temp_output_32_0 <= temp_output_30_0 )
				ifLocalVar12 = temp_output_32_0;
				else
				ifLocalVar12 = temp_output_30_0;
				float4 temp_output_32_56 = ( ( tex2DNode75_g65 + tex2DNode79_g65 + tex2DNode82_g65 + tex2DNode76_g65 + tex2DNode80_g65 + tex2DNode83_g65 + tex2DNode77_g65 + tex2DNode78_g65 + tex2DNode81_g65 ) / 9.0 );
				float4 ifLocalVar13 = 0;
				if( temp_output_32_0 <= temp_output_30_0 )
				ifLocalVar13 = temp_output_32_56;
				else
				ifLocalVar13 = ( ( tex2DNode75_g73 + tex2DNode79_g73 + tex2DNode82_g73 + tex2DNode76_g73 + tex2DNode80_g73 + tex2DNode83_g73 + tex2DNode77_g73 + tex2DNode78_g73 + tex2DNode81_g73 ) / 9.0 );
				float4 temp_output_33_56 = ( ( tex2DNode75_g69 + tex2DNode79_g69 + tex2DNode82_g69 + tex2DNode76_g69 + tex2DNode80_g69 + tex2DNode83_g69 + tex2DNode77_g69 + tex2DNode78_g69 + tex2DNode81_g69 ) / 9.0 );
				float4 ifLocalVar11 = 0;
				if( temp_output_33_0 <= temp_output_31_0 )
				ifLocalVar11 = temp_output_33_56;
				else
				ifLocalVar11 = ( ( tex2DNode75_g61 + tex2DNode79_g61 + tex2DNode82_g61 + tex2DNode76_g61 + tex2DNode80_g61 + tex2DNode83_g61 + tex2DNode77_g61 + tex2DNode78_g61 + tex2DNode81_g61 ) / 9.0 );
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
Version=17101
1222;35;1299;832;2108.443;353.5879;1.106809;True;False
Node;AmplifyShaderEditor.ScreenParams;20;-1764.331,53.62074;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;34;-1711.453,-338.5574;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;9;-1216,352;Float;False;Constant;_Vector3;Vector 3;0;0;Create;True;0;0;False;0;1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;7;-1216,0;Float;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScreenPosInputsNode;36;-1622.554,-179.8188;Inherit;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-1432.119,89.56598;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;6;-1216,-176;Float;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;8;-1216,176;Float;False;Constant;_Vector2;Vector 2;0;0;Create;True;0;0;False;0;-1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;31;-1008,0;Inherit;False;KuwaharaRegion;-1;;61;d55ee1caa774cb54ea8e2d4f4f0960da;0;4;74;SAMPLER2D;;False;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;33;-1008,-176;Inherit;False;KuwaharaRegion;-1;;69;d55ee1caa774cb54ea8e2d4f4f0960da;0;4;74;SAMPLER2D;;False;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;30;-1008,352;Inherit;False;KuwaharaRegion;-1;;73;d55ee1caa774cb54ea8e2d4f4f0960da;0;4;74;SAMPLER2D;;False;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-1008,176;Inherit;False;KuwaharaRegion;-1;;65;d55ee1caa774cb54ea8e2d4f4f0960da;0;4;74;SAMPLER2D;;False;1;FLOAT2;0,0;False;63;FLOAT2;0,0;False;64;FLOAT2;0,0;False;2;COLOR;56;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;10;-560,-176;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;13;-560,352;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;11;-560,0;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;12;-560,176;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;14;-272,0;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;35;0,0;Float;False;True;2;ASEMaterialInspector;0;2;Kuwahara2;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;0
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;31;74;34;0
WireConnection;31;1;7;0
WireConnection;31;63;36;0
WireConnection;31;64;21;0
WireConnection;33;74;34;0
WireConnection;33;1;6;0
WireConnection;33;63;36;0
WireConnection;33;64;21;0
WireConnection;30;74;34;0
WireConnection;30;1;9;0
WireConnection;30;63;36;0
WireConnection;30;64;21;0
WireConnection;32;74;34;0
WireConnection;32;1;8;0
WireConnection;32;63;36;0
WireConnection;32;64;21;0
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
WireConnection;35;0;14;0
ASEEND*/
//CHKSM=43C348D006D050709685E67903322F9882ACA03D