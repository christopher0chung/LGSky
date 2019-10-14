// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RadialTransparency"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = -0.06
		_RadialScalar("Radial Scalar", Float) = 0
		_SpeedScalar("Speed Scalar", Float) = 0
		[HDR]_BaseColor("Base Color", Color) = (0,0,0,0)
		_BandMin("Band Min", Float) = 0
		_BandMax("BandMax", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BaseColor;
		uniform float _RadialScalar;
		uniform float _SpeedScalar;
		uniform float _BandMin;
		uniform float _BandMax;
		uniform float _Cutoff = -0.06;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _BaseColor.rgb;
			o.Alpha = 1;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime12 = _Time.y * _SpeedScalar;
			clip( (_BandMin + (sin( ( ( atan2( ase_vertex3Pos.x , ase_vertex3Pos.z ) * _RadialScalar ) + mulTime12 ) ) - -1.0) * (_BandMax - _BandMin) / (1.0 - -1.0)) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
819;98;979;931;1836.954;-47.99152;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;1;-1668.701,207.6;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ATan2OpNode;2;-1389.833,233.1506;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1424.678,470.8354;Inherit;False;Property;_RadialScalar;Radial Scalar;1;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1445.954,654.9915;Inherit;False;Property;_SpeedScalar;Speed Scalar;2;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1210.279,242.0349;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-1220.954,576.9915;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-988.9536,255.9915;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;5;-753.4777,242.235;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-782.8781,376.6351;Inherit;False;Property;_BandMin;Band Min;4;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-788.6777,453.4353;Inherit;False;Property;_BandMax;BandMax;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-513.478,293.4352;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-507.386,-87.63067;Inherit;False;Property;_BaseColor;Base Color;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2,0.8705882,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Unlit;RadialTransparency;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;-0.06;True;True;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;1
WireConnection;2;1;1;3
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;12;0;13;0
WireConnection;10;0;3;0
WireConnection;10;1;12;0
WireConnection;5;0;10;0
WireConnection;6;0;5;0
WireConnection;6;3;7;0
WireConnection;6;4;8;0
WireConnection;0;2;9;0
WireConnection;0;10;6;0
ASEEND*/
//CHKSM=6835EBBB77C2AA28E4D95B6FE950EAEB47F40D80