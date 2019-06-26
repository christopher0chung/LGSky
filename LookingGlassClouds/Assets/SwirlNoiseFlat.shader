// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SwirlNoiseFlat"
{
	Properties
	{
		_Scale0("Scale0", Float) = 1
		_Scale1("Scale1", Float) = 2
		_Scale2("Scale2", Float) = 4
		_Scale3("Scale3", Float) = 8
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _Scale2;
		uniform float _Scale3;
		uniform float _Scale1;
		uniform float _Scale0;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 break32_g2 = ase_worldPos;
			float temp_output_3_0_g2 = _Scale2;
			float3 appendResult11_g2 = (float3(break32_g2.x , break32_g2.y , ( break32_g2.z / temp_output_3_0_g2 )));
			float simplePerlin3D18_g2 = snoise( ( appendResult11_g2 * temp_output_3_0_g2 ) );
			float temp_output_8_0_g2 = _Scale3;
			float3 appendResult13_g2 = (float3(break32_g2.x , break32_g2.y , ( break32_g2.z / temp_output_8_0_g2 )));
			float simplePerlin3D21_g2 = snoise( ( appendResult13_g2 * temp_output_8_0_g2 ) );
			float blendOpSrc26_g2 = (0.0 + (simplePerlin3D18_g2 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float blendOpDest26_g2 = (0.0 + (simplePerlin3D21_g2 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float temp_output_2_0_g2 = _Scale1;
			float3 appendResult15_g2 = (float3(break32_g2.x , break32_g2.y , ( break32_g2.z / temp_output_2_0_g2 )));
			float simplePerlin3D25_g2 = snoise( ( appendResult15_g2 * temp_output_2_0_g2 ) );
			float blendOpSrc30_g2 = ( saturate( 2.0f*blendOpDest26_g2*blendOpSrc26_g2 + blendOpDest26_g2*blendOpDest26_g2*(1.0f - 2.0f*blendOpSrc26_g2) ));
			float blendOpDest30_g2 = (0.0 + (simplePerlin3D25_g2 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float temp_output_1_0_g2 = _Scale0;
			float3 appendResult19_g2 = (float3(break32_g2.x , break32_g2.y , ( break32_g2.z / temp_output_1_0_g2 )));
			float simplePerlin3D28_g2 = snoise( ( appendResult19_g2 * temp_output_1_0_g2 ) );
			float blendOpSrc31_g2 = ( saturate( 2.0f*blendOpDest30_g2*blendOpSrc30_g2 + blendOpDest30_g2*blendOpDest30_g2*(1.0f - 2.0f*blendOpSrc30_g2) ));
			float blendOpDest31_g2 = (0.0 + (simplePerlin3D28_g2 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float3 temp_cast_0 = (( saturate( 2.0f*blendOpDest31_g2*blendOpSrc31_g2 + blendOpDest31_g2*blendOpDest31_g2*(1.0f - 2.0f*blendOpSrc31_g2) ))).xxx;
			o.Albedo = temp_cast_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
2567;-477;1906;1140;1371.438;373.785;1.145902;True;False
Node;AmplifyShaderEditor.RangedFloatNode;36;-592,352;Float;False;Property;_Scale3;Scale3;3;0;Create;True;0;0;False;0;8;2.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-592,288;Float;False;Property;_Scale2;Scale2;2;0;Create;True;0;0;False;0;4;1.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;32;-608,0;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;34;-592,224;Float;False;Property;_Scale1;Scale1;1;0;Create;True;0;0;False;0;2;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-592,160;Float;False;Property;_Scale0;Scale0;0;0;Create;True;0;0;False;0;1;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;70;-368,96;Float;False;GBG_CloudTexture;-1;;2;8004816f301a4d94690c915c91b817e9;0;5;5;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SwirlNoiseFlat;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;70;5;32;0
WireConnection;70;1;33;0
WireConnection;70;2;34;0
WireConnection;70;3;35;0
WireConnection;70;8;36;0
WireConnection;0;0;70;0
ASEEND*/
//CHKSM=9B719A8771646B751CD8988BD0DBC37934E1EDD2