// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DepthNodeDebug"
{
	Properties
	{
		_DepthFadeDistance("Depth Fade Distance", Range( 0 , 10)) = 0
		_SurfaceContribution("Surface Contribution", Range( 0 , 1)) = 0
		_VolumeContribution("Volume Contribution", Range( 0 , 1)) = 0
		_FresnelBias("Fresnel Bias", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelPower("Fresnel Power", Float) = 0
		_TotalVolumeContribution("TotalVolumeContribution", Float) = 0
		_ExtrusionMagnitudeScalar("Extrusion Magnitude Scalar", Range( 0.1 , 1)) = 0
		_NoiseScalar("Noise Scalar", Float) = 1
		_SurfaceNoiseScale("Surface Noise Scale", Float) = 0
		_CloudColorTint("Cloud Color Tint", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform float _NoiseScalar;
		uniform float _ExtrusionMagnitudeScalar;
		uniform sampler2D _GrabTexture;
		uniform float _SurfaceNoiseScale;
		uniform float _SurfaceContribution;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeDistance;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _VolumeContribution;
		uniform float _TotalVolumeContribution;
		uniform float4 _CloudColorTint;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 objToWorld61 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float4 appendResult73 = (float4(objToWorld61.x , objToWorld61.y , ( objToWorld61.z * 0.02 ) , 0.0));
			float simplePerlin3D59 = snoise( ( appendResult73 * _NoiseScalar ).xyz );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 normalizeResult53 = normalize( ase_vertexNormal );
			v.vertex.xyz += ( (0.0 + (simplePerlin3D59 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) * _ExtrusionMagnitudeScalar * normalizeResult53 );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color67 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 screenColor11 = tex2D( _GrabTexture, ase_screenPosNorm.xy );
			float3 ase_worldPos = i.worldPos;
			float3 break12_g1 = ase_worldPos;
			float4 appendResult13_g1 = (float4(break12_g1.x , break12_g1.y , ( break12_g1.z * 0.02 ) , 0.0));
			float simplePerlin3D1_g1 = snoise( ( appendResult13_g1 * _SurfaceNoiseScale ).xyz );
			float screenDepth3 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth3 = abs( ( screenDepth3 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV18 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode18 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV18, _FresnelPower ) );
			float dotResult63 = dot( i.viewDir , ase_worldNormal );
			float4 lerpResult62 = lerp( color67 , ( screenColor11 + ( ( ( (0.0 + (simplePerlin3D1_g1 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) * _SurfaceContribution * distanceDepth3 * (0.0 + (fresnelNode18 - 1.0) * (1.0 - 0.0) / (0.0 - 1.0)) ) + ( distanceDepth3 * _VolumeContribution * (0.0 + (fresnelNode18 - 1.0) * (1.0 - 0.0) / (0.0 - 1.0)) ) ) * _TotalVolumeContribution ) ) , (0.0 + (dotResult63 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)));
			o.Emission = ( lerpResult62 * _CloudColorTint ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
2567;-477;1906;1134;4560.296;939.2321;2.529117;True;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-2787.988,399.5692;Float;False;Property;_FresnelScale;Fresnel Scale;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;33;-1992.208,476.364;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-2822.601,92.31567;Float;False;Property;_DepthFadeDistance;Depth Fade Distance;0;0;Create;True;0;0;False;0;0;2.13;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2787.988,488.6023;Float;False;Property;_FresnelPower;Fresnel Power;5;0;Create;True;0;0;False;0;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2787.987,306.4891;Float;False;Property;_FresnelBias;Fresnel Bias;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;61;-1566.34,326.3636;Float;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FresnelNode;18;-2445.292,271.0592;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2564.95,-42.12126;Float;False;Property;_SurfaceContribution;Surface Contribution;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;6;-2479.706,-183.2271;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;16;-2727.283,201.2682;Float;False;Property;_VolumeContribution;Volume Contribution;2;0;Create;True;0;0;False;0;0;0.301;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2722.521,-88.35828;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;3;-2521.119,75.49158;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-2518.921,-278.3071;Float;False;Property;_SurfaceNoiseScale;Surface Noise Scale;9;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-1605.521,524.6417;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;49;-2143.135,181.5072;Float;False;GBG_VolumetricOcclusion;-1;;8;8993da02a2ce21e41aea8fbf691fb978;0;3;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;71;-2146.677,32.08703;Float;False;GBG_SurfaceToWorldSpaceNoiseOcclusion;-1;;1;9aaeb9dd03e05d443a770542a3faa0c1;0;6;9;FLOAT;0;False;15;FLOAT;0;False;4;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1974.425,716.6401;Float;False;Property;_NoiseScalar;Noise Scalar;8;0;Create;True;0;0;False;0;1;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-1319.521,340.6417;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;64;-2038.432,-539.9887;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;65;-2032.519,-686.355;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1649.865,34.43513;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;12;-2116.629,-273.2614;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1952.649,312.079;Float;False;Property;_TotalVolumeContribution;TotalVolumeContribution;6;0;Create;True;0;0;False;0;0;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1148.695,328.1992;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1488.722,34.88433;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;63;-1608.206,-547.3809;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;11;-1860.63,-273.2614;Float;False;Global;_GrabScreen0;Grab Screen 0;1;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;59;-983.7505,322.1238;Float;False;Simplex3D;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;32;-1534.678,824.0273;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1250.822,12.43868;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;66;-1455.927,-545.9034;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;-1461.84,-344.8354;Float;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;53;-1290.467,831.1106;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1623.055,746.834;Float;False;Property;_ExtrusionMagnitudeScalar;Extrusion Magnitude Scalar;7;0;Create;True;0;0;False;0;0;0.165;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;57;-737.3722,326.5591;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;62;-965.0838,-158.5508;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;77;-946.5215,18.64172;Float;False;Property;_CloudColorTint;Cloud Color Tint;10;0;Create;True;0;0;False;0;0,0,0,0;0.8301887,0.7820507,0.747953,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-493.1774,325.3073;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-539.5215,-124.3583;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;31;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Unlit;DepthNodeDebug;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;2;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;33;0
WireConnection;18;1;20;0
WireConnection;18;2;21;0
WireConnection;18;3;22;0
WireConnection;3;0;5;0
WireConnection;74;0;61;3
WireConnection;74;1;72;0
WireConnection;49;1;3;0
WireConnection;49;2;16;0
WireConnection;49;4;18;0
WireConnection;71;9;69;0
WireConnection;71;15;72;0
WireConnection;71;4;6;0
WireConnection;71;5;15;0
WireConnection;71;6;3;0
WireConnection;71;7;18;0
WireConnection;73;0;61;1
WireConnection;73;1;61;2
WireConnection;73;2;74;0
WireConnection;10;0;71;0
WireConnection;10;1;49;0
WireConnection;60;0;73;0
WireConnection;60;1;46;0
WireConnection;26;0;10;0
WireConnection;26;1;29;0
WireConnection;63;0;65;0
WireConnection;63;1;64;0
WireConnection;11;0;12;0
WireConnection;59;0;60;0
WireConnection;27;0;11;0
WireConnection;27;1;26;0
WireConnection;66;0;63;0
WireConnection;53;0;32;0
WireConnection;57;0;59;0
WireConnection;62;0;67;0
WireConnection;62;1;27;0
WireConnection;62;2;66;0
WireConnection;58;0;57;0
WireConnection;58;1;36;0
WireConnection;58;2;53;0
WireConnection;76;0;62;0
WireConnection;76;1;77;0
WireConnection;31;2;76;0
WireConnection;31;11;58;0
ASEEND*/
//CHKSM=74C04A888F67446C8925E9994D2A1E7506805785