// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Radial_BandingAndVertex"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_RadialScalar1("Radial Scalar", Float) = 0
		_SpeedScalar1("Speed Scalar", Float) = 0
		[HDR]_BaseColor1("Base Color", Color) = (0,0,0,0)
		_BandMin1("Band Min", Float) = 0
		_BandMax1("BandMax", Float) = 0
		_VertexMultiplier("Vertex Multiplier", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _VertexMultiplier;
		uniform float4 _BaseColor1;
		uniform float _RadialScalar1;
		uniform float _SpeedScalar1;
		uniform float _BandMin1;
		uniform float _BandMax1;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime23 = _Time.y * 8.0;
			float4 appendResult18 = (float4(0.0 , ( sin( mulTime23 ) * 0.2 ) , 0.0 , 0.0));
			v.vertex.xyz += ( appendResult18 * _VertexMultiplier ).xyz;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _BaseColor1.rgb;
			o.Alpha = 1;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime6 = _Time.y * _SpeedScalar1;
			clip( (_BandMin1 + (sin( ( ( distance( ase_vertex3Pos , float3( 0,0,0 ) ) * _RadialScalar1 ) + mulTime6 ) ) - -1.0) * (_BandMax1 - _BandMin1) / (1.0 - -1.0)) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
746;98;979;931;1504.332;433.3442;2.505472;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;1;-1549.836,207.2363;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1220.575,934.8632;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1305.813,470.4718;Inherit;False;Property;_RadialScalar1;Radial Scalar;1;0;Create;True;0;0;False;0;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1327.089,654.6279;Inherit;False;Property;_SpeedScalar1;Speed Scalar;2;0;Create;True;0;0;False;0;0;-20.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;13;-1212.785,120.6298;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-999.9462,939.5495;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1091.414,241.6712;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1102.089,576.6279;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;22;-732.4282,921.3792;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-870.0885,255.6278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-525.9238,866.097;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-323.1419,727.1996;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-669.8126,453.0717;Inherit;False;Property;_BandMax1;BandMax;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-664.0131,376.2714;Inherit;False;Property;_BandMin1;Band Min;4;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;8;-634.6127,241.8713;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-512.1653,618.9539;Inherit;False;Property;_VertexMultiplier;Vertex Multiplier;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;2;-1270.968,232.7869;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-161.3993,493.6797;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;11;-394.613,293.0715;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-388.521,-87.9944;Inherit;False;Property;_BaseColor1;Base Color;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Unlit;Radial_BandingAndVertex;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;1;0
WireConnection;23;0;24;0
WireConnection;5;0;13;0
WireConnection;5;1;3;0
WireConnection;6;0;4;0
WireConnection;22;0;23;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;19;0;22;0
WireConnection;18;1;19;0
WireConnection;8;0;7;0
WireConnection;2;0;1;1
WireConnection;2;1;1;2
WireConnection;25;0;18;0
WireConnection;25;1;26;0
WireConnection;11;0;8;0
WireConnection;11;3;9;0
WireConnection;11;4;10;0
WireConnection;0;2;12;0
WireConnection;0;10;11;0
WireConnection;0;11;25;0
ASEEND*/
//CHKSM=B33ACB16766D3F5BB3EB6BDBEA07A526B0BE858D