// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UpDownArrowShader"
{
	Properties
	{
		_Arrows("Arrows", Range( -1 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldNormal;
			float3 viewDir;
		};

		uniform float _Arrows;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float ifLocalVar7 = 0;
			if( ase_vertex3Pos.y < 0.0 )
				ifLocalVar7 = ( ase_vertex3Pos.y * ase_vertex3Pos.y * _Arrows * 0.25 );
			float4 appendResult11 = (float4(0.0 , 0.0 , ifLocalVar7 , 0.0));
			v.vertex.xyz += appendResult11.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float dotResult5 = dot( ase_worldNormal , i.viewDir );
			float4 color3 = IsGammaSpace() ? float4(1,0,0,1) : float4(1,0,0,1);
			float4 color4 = IsGammaSpace() ? float4(0.1688471,0,1,1) : float4(0.02421462,0,1,1);
			float4 ifLocalVar6 = 0;
			if( dotResult5 >= 0.0 )
				ifLocalVar6 = color3;
			else
				ifLocalVar6 = color4;
			o.Emission = ifLocalVar6.rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
746;98;979;931;1913.776;386.5876;2.632082;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;8;-863.1069,494.8707;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-822.1475,861.0388;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-940.7738,753.4634;Inherit;False;Property;_Arrows;Arrows;1;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;1;-722.4798,63.11545;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;2;-734.5132,-76.65469;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-645.7738,712.4634;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-500.8033,142.8394;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-499.1523,306.2668;Inherit;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.1688471,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;5;-423.0916,46.76285;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;7;-462.0371,494.872;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;6;-206.9596,45.6703;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;-217.0127,467.9662;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-988.5059,331.9714;Inherit;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;UpDownArrowShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;2
WireConnection;9;1;8;2
WireConnection;9;2;10;0
WireConnection;9;3;12;0
WireConnection;5;0;2;0
WireConnection;5;1;1;0
WireConnection;7;0;8;2
WireConnection;7;4;9;0
WireConnection;6;0;5;0
WireConnection;6;2;3;0
WireConnection;6;3;3;0
WireConnection;6;4;4;0
WireConnection;11;2;7;0
WireConnection;0;2;6;0
WireConnection;0;9;13;0
WireConnection;0;11;11;0
ASEEND*/
//CHKSM=0DF2EC5ADD8525D84D725545818A2C8E61AC48A1