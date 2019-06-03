// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BlackHoleShader"
{
	Properties
	{
		_SuctionScale("Suction Scale", Float) = 0
		_Power("Power", Float) = 0
		_BHColor("BHColor", Color) = (1,0.6933962,0.6933962,0)
		_Scale("Scale", Float) = 0
		_Bias("Bias", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _GrabTexture;
		uniform float _SuctionScale;
		uniform float4 _BHColor;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 unityObjectToClipPos38 = UnityObjectToClipPos( float3( 0,0,0 ) );
			float4 computeScreenPos35 = ComputeScreenPos( unityObjectToClipPos38 );
			float4 break61 = computeScreenPos35;
			float temp_output_60_0 = ( ase_screenPosNorm.x - break61.x );
			float temp_output_62_0 = ( ase_screenPosNorm.y - break61.y );
			float4 normalizeResult67 = normalize( ( ase_screenPosNorm - ( computeScreenPos35 / (computeScreenPos35).w ) ) );
			float4 screenColor2 = tex2D( _GrabTexture, ( ase_screenPosNorm + ( pow( sqrt( ( ( temp_output_60_0 * temp_output_60_0 ) + ( temp_output_62_0 * temp_output_62_0 ) ) ) , _SuctionScale ) * normalizeResult67 ) ).xy );
			o.Emission = ( screenColor2 * _BHColor ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV79 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode79 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV79, _Power ) );
			float clampResult85 = clamp( (1.0 + (fresnelNode79 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			o.Alpha = clampResult85;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
2567;-477;1906;1134;2408.532;393.1018;1.477951;True;False
Node;AmplifyShaderEditor.CommentaryNode;65;-4054.75,748.5845;Float;False;804.4001;281.7;Object origin on the screen;4;55;54;35;38;;1,1,1,1;0;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;38;-4031.06,808.1465;Float;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;69;-3351.604,24.25067;Float;False;995.9998;394.4;Radius from center in UV;7;61;59;58;57;56;60;62;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;64;-3763.264,474.5853;Float;False;525.0642;258.5374;Pixel on the screen;1;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComputeScreenPosHlpNode;35;-3822.058,811.1465;Float;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;61;-3320.441,202.2567;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-3730.448,520.3846;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-3015.541,236.0567;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;60;-3019.44,128.1565;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-2846.149,218.8847;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2844.149,118.8846;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;54;-3628.652,924.585;Float;False;False;False;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2664.149,164.8847;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;55;-3387.651,809.5848;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;66;-3187.081,553.3206;Float;False;794.2;319.7;Direction in UV space away from UV origin;2;67;68;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2341.206,694.6502;Float;False;Property;_SuctionScale;Suction Scale;0;0;Create;True;0;0;False;0;0;-3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;59;-2532.149,164.8847;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;68;-2745.079,684.6205;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalizeNode;67;-2600.779,683.3203;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;77;-2287.61,313.6668;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1784.836,823.2521;Float;False;Property;_Power;Power;1;0;Create;True;0;0;False;0;0;4.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-1761.189,715.3618;Float;False;Property;_Scale;Scale;3;0;Create;True;0;0;False;0;0;7.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-2065.809,345.0495;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1764.144,610.4271;Float;False;Property;_Bias;Bias;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1954.006,185.8502;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FresnelNode;79;-1538.018,613.3832;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;80;-1042.904,637.0305;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;2;-397,41;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-377,231;Float;False;Property;_BHColor;BHColor;2;0;Create;True;0;0;False;0;1,0.6933962,0.6933962,0;1,0.6933962,0.6933962,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;85;-655.6812,699.1044;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-125,29;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;BlackHoleShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;38;0
WireConnection;61;0;35;0
WireConnection;62;0;48;2
WireConnection;62;1;61;1
WireConnection;60;0;48;1
WireConnection;60;1;61;0
WireConnection;57;0;62;0
WireConnection;57;1;62;0
WireConnection;56;0;60;0
WireConnection;56;1;60;0
WireConnection;54;0;35;0
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;55;0;35;0
WireConnection;55;1;54;0
WireConnection;59;0;58;0
WireConnection;68;0;48;0
WireConnection;68;1;55;0
WireConnection;67;0;68;0
WireConnection;77;0;59;0
WireConnection;77;1;72;0
WireConnection;71;0;77;0
WireConnection;71;1;67;0
WireConnection;70;0;48;0
WireConnection;70;1;71;0
WireConnection;79;1;82;0
WireConnection;79;2;83;0
WireConnection;79;3;84;0
WireConnection;80;0;79;0
WireConnection;2;0;70;0
WireConnection;85;0;80;0
WireConnection;6;0;2;0
WireConnection;6;1;7;0
WireConnection;0;2;6;0
WireConnection;0;9;85;0
ASEEND*/
//CHKSM=B963B04D50F6A2B059C890D9BF104067C2B7FF7A