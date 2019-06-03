// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CloudV00"
{
	Properties
	{
		_AlbedoColor("Albedo Color", Color) = (0.5,0.5,1,0)
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		[HDR]_SubsurfaceScatteringColor("Subsurface Scattering Color", Color) = (1.498039,0.8470588,0.8470588,0)
		_Metalness("Metalness", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 5.0
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _AlbedoColor;
		uniform float4 _SubsurfaceScatteringColor;
		uniform float _Metalness;
		uniform float _Smoothness;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _AlbedoColor.rgb;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = i.worldNormal;
			float dotResult130 = dot( ase_worldlightDir , ase_worldNormal );
			float temp_output_134_0 = (0.0 + (dotResult130 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float4 lerpResult136 = lerp( _SubsurfaceScatteringColor , float4( 0,0,0,0 ) , temp_output_134_0);
			o.Emission = lerpResult136.rgb;
			o.Metallic = _Metalness;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
2567;-477;1906;1140;1978.872;701.0665;1.442519;False;False
Node;AmplifyShaderEditor.WorldNormalVector;152;-1686.04,135.5946;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;133;-1860.72,-225.3955;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;130;-1375.641,-25.72224;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;134;-1228.616,-25.72202;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;137;-1050.235,-207.156;Float;False;Property;_SubsurfaceScatteringColor;Subsurface Scattering Color;6;1;[HDR];Create;True;0;0;False;0;1.498039,0.8470588,0.8470588,0;0.8603976,0.6543561,0.4865075,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-3444.407,1179.907;Float;False;Property;_VertexOffsetTimeScalarO2;Vertex Offset Time Scalar - O2;16;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-3761.161,568.938;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;3;-3978.004,396.9653;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;149;-3519.828,466.938;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-962.2854,-10.05554;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;119;-2756.25,-617.2155;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;4;-3756.206,397.2653;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;40;-3028.507,1269.108;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-3319.506,1264.108;Float;False;Property;_VertexOffsetNoiseScalarO2;Vertex Offset Noise Scalar - O2;14;0;Create;True;0;0;False;0;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;43;-3355.407,1104.907;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-2979.509,953.5067;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-4054.162,643.9379;Float;False;Property;_ScrollSpeed;ScrollSpeed;13;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;146;-3965.161,568.938;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;150;-3306.828,959.938;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-2807.108,953.3069;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-1613.399,393.6847;Float;False;Property;_SubSurfStrenth;SubSurfStrenth;7;0;Create;True;0;0;False;0;0;0.404;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2703.246,1080.303;Float;False;Property;_VertexOffsetAmplitudeO2;Vertex Offset Amplitude - O2;15;0;Create;True;0;0;False;0;0;2.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2389.946,951.6018;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-2210.544,946.4022;Float;False;Octave2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-2115.464,-476.5134;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;132;-1679.328,-25.72222;Float;False;PerturbNormal;-1;;1;c8b64dd82fb09f542943a895dffb6c06;1,26,0;1;6;FLOAT3;0,0,0;False;4;FLOAT3;9;FLOAT;28;FLOAT;29;FLOAT;30
Node;AmplifyShaderEditor.TFHCRemapNode;143;-1234.399,241.6847;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;116;-3061.98,-385.0015;Float;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-1520.217,301.8962;Float;False;36;Octave2;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;115;-3283.36,-390.3815;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;114;-3060.67,-615.6914;Float;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;118;-3799.097,-360.0195;Float;False;Constant;_Vector0;Vector 0;10;0;Create;True;0;0;False;0;0.1,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-2624.207,947.2068;Float;False;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;46;-3467.739,891.4595;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-3882.098,-616.6754;Float;False;128;MajorOctaveOffset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-2985.699,416.6651;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;151;-380.4471,431.9362;Float;False;3;0;FLOAT;8;False;1;FLOAT;0;False;2;FLOAT;40;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-429.3117,39.32716;Float;False;Property;_Metalness;Metalness;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;136;-712.8268,-89.59312;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-428.1256,115.2471;Float;False;Property;_Smoothness;Smoothness;9;0;Create;True;0;0;False;0;0;0.629;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-3452.67,-378.6915;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-3034.697,732.2653;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2813.298,416.4652;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-2223.234,409.5606;Float;False;Octave1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-2630.397,410.3651;Float;False;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-1876.967,-30.54265;Float;False;126;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;89;-3290.051,-621.0704;Float;True;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;85;-2437.964,-472.8464;Float;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3450.598,643.0644;Float;False;Property;_VertexOffetTimeScalarO1;Vertex Offet Time Scalar - O1;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-714.3585,292.6379;Float;False;20;Octave1;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-2634.934,856.6008;Float;False;MajorOctaveOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-715.3585,375.6379;Float;False;36;Octave2;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-481.3585,297.6379;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3325.697,727.2652;Float;False;Property;_VertexOffsetNoiseScaleO1;Vertex Offset Noise Scale - O1;10;0;Create;True;0;0;False;0;0;0.896;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3151.407,1104.907;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2709.436,543.4606;Float;False;Property;_VertexOffsetAmplitudeO1;Vertex Offset Amplitude - O1;11;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-3157.597,568.0645;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-336.6857,-43.19585;Float;False;126;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;113;-393.1582,-543.0026;Float;False;Property;_AlbedoColor;Albedo Color;0;0;Create;True;0;0;False;0;0.5,0.5,1,0;0.5074314,0.5074314,0.5943396,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;7;-267,277;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2396.136,414.7603;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-3361.597,568.0645;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;CloudV00;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0;True;False;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;130;0;133;0
WireConnection;130;1;152;0
WireConnection;134;0;130;0
WireConnection;148;0;146;0
WireConnection;148;1;147;0
WireConnection;149;0;4;3
WireConnection;149;1;148;0
WireConnection;139;0;134;0
WireConnection;139;1;143;0
WireConnection;119;0;114;0
WireConnection;119;1;116;0
WireConnection;4;0;3;0
WireConnection;40;0;41;0
WireConnection;38;0;46;0
WireConnection;38;1;150;0
WireConnection;38;2;44;0
WireConnection;150;0;149;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;35;0;34;0
WireConnection;35;1;37;0
WireConnection;36;0;35;0
WireConnection;126;0;85;0
WireConnection;132;6;131;0
WireConnection;143;0;142;0
WireConnection;143;4;144;0
WireConnection;116;0;115;0
WireConnection;115;0;117;0
WireConnection;114;0;89;0
WireConnection;34;0;39;0
WireConnection;46;0;4;1
WireConnection;5;0;4;1
WireConnection;5;1;149;0
WireConnection;5;2;9;0
WireConnection;136;0;137;0
WireConnection;136;2;134;0
WireConnection;117;0;129;0
WireConnection;117;1;118;0
WireConnection;19;0;10;0
WireConnection;8;0;5;0
WireConnection;8;1;19;0
WireConnection;20;0;31;0
WireConnection;1;0;8;0
WireConnection;89;0;129;0
WireConnection;85;0;119;0
WireConnection;85;1;119;0
WireConnection;128;0;39;0
WireConnection;50;0;48;0
WireConnection;50;1;49;0
WireConnection;44;0;43;0
WireConnection;44;1;42;0
WireConnection;9;0;6;0
WireConnection;9;1;11;0
WireConnection;7;1;50;0
WireConnection;31;0;1;0
WireConnection;31;1;32;0
WireConnection;0;0;113;0
WireConnection;0;2;136;0
WireConnection;0;3;96;0
WireConnection;0;4;97;0
ASEEND*/
//CHKSM=C27F2DC79E788444D6140EEFB6ACCEB59DE601D7