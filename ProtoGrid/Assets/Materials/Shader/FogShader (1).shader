// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogShader"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,0)
		_FogHeight("Fog Height", Float) = -10
		_FogDepth("Fog Depth", Float) = 10
		_BaseMap("Base Map", 2D) = "white" {}
		_CloudScale("CloudScale", Float) = 0.12
		_CloudHeight("CloudHeight", Float) = 8
		_CloudSpeed("Cloud Speed", Float) = 0.8
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _FogHeight;
		uniform float _CloudSpeed;
		uniform float _CloudScale;
		uniform float _CloudHeight;
		uniform float _FogDepth;
		uniform sampler2D _BaseMap;
		uniform float4 _BaseMap_ST;
		uniform float4 _Color;


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


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_BaseMap = i.uv_texcoord * _BaseMap_ST.xy + _BaseMap_ST.zw;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult77 = dot( ase_worldNormal , ase_worldlightDir );
			float temp_output_3_0_g3 = ( min( saturate( dotResult77 ) , ase_lightAtten ) - 0.1 );
			float temp_output_82_0 = saturate( ( temp_output_3_0_g3 / fwidth( temp_output_3_0_g3 ) ) );
			UnityGI gi85 = gi;
			float3 diffNorm85 = ase_worldNormal;
			gi85 = UnityGI_Base( data, 1, diffNorm85 );
			float3 indirectDiffuse85 = gi85.indirect.diffuse + diffNorm85 * 0.0001;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float mulTime45 = _Time.y * _CloudSpeed;
			float2 panner44 = ( mulTime45 * float2( 1,1 ) + ase_worldPos.xy);
			float2 break52 = panner44;
			float4 appendResult50 = (float4(break52.x , break52.y , ase_worldPos.z , 0.0));
			float simplePerlin3D29 = snoise( appendResult50.xyz*_CloudScale );
			simplePerlin3D29 = simplePerlin3D29*0.5 + 0.5;
			float temp_output_46_0 = ( _FogHeight + ( simplePerlin3D29 * _CloudHeight ) );
			float clampResult16 = clamp( _FogDepth , 0.0 , ( abs( temp_output_46_0 ) - 1.0 ) );
			float clampResult12 = clamp( ( ( ase_worldPos.y - temp_output_46_0 ) / clampResult16 ) , 0.0 , 1.0 );
			float temp_output_39_0 = ( 1.0 * clampResult12 );
			float4 lerpResult88 = lerp( float4( 0,0,0,0 ) , ( ( tex2D( _BaseMap, uv_BaseMap ) * _Color ) * ( float4( ( temp_output_82_0 + ( ( 1.0 - temp_output_82_0 ) * indirectDiffuse85 ) ) , 0.0 ) * ase_lightColor ) ) , temp_output_39_0);
			c.rgb = lerpResult88.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor28 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPosNorm.xy/ase_grabScreenPosNorm.w);
			float3 ase_worldPos = i.worldPos;
			float mulTime45 = _Time.y * _CloudSpeed;
			float2 panner44 = ( mulTime45 * float2( 1,1 ) + ase_worldPos.xy);
			float2 break52 = panner44;
			float4 appendResult50 = (float4(break52.x , break52.y , ase_worldPos.z , 0.0));
			float simplePerlin3D29 = snoise( appendResult50.xyz*_CloudScale );
			simplePerlin3D29 = simplePerlin3D29*0.5 + 0.5;
			float temp_output_46_0 = ( _FogHeight + ( simplePerlin3D29 * _CloudHeight ) );
			float clampResult16 = clamp( _FogDepth , 0.0 , ( abs( temp_output_46_0 ) - 1.0 ) );
			float clampResult12 = clamp( ( ( ase_worldPos.y - temp_output_46_0 ) / clampResult16 ) , 0.0 , 1.0 );
			float temp_output_39_0 = ( 1.0 * clampResult12 );
			float4 lerpResult23 = lerp( screenColor28 , float4( 0,0,0,0 ) , temp_output_39_0);
			o.Emission = lerpResult23.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
2553;240;1920;677;1280.292;-184.8922;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;55;-3106.725,736.9828;Inherit;False;Property;_CloudSpeed;Cloud Speed;6;0;Create;True;0;0;0;False;0;False;0.8;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;45;-2920.925,741.8178;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;30;-3011.977,545.2481;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;44;-2758.925,494.8178;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;52;-2551.925,562.3178;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;33;-2391.099,729.8898;Inherit;False;Property;_CloudScale;CloudScale;4;0;Create;True;0;0;0;False;0;False;0.12;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;50;-2394.925,557.3178;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;76;-1712.717,971.4626;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;75;-1732.677,1121.243;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;77;-1456.717,1021.463;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2194.925,733.3178;Inherit;False;Property;_CloudHeight;CloudHeight;5;0;Create;True;0;0;0;False;0;False;8;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;29;-2218.002,608.7697;Inherit;False;Simplex3D;True;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;78;-1321.304,1026.367;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;81;-1363.717,1113.464;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-2016.925,619.3178;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2045.503,522.314;Inherit;False;Property;_FogHeight;Fog Height;1;0;Create;True;0;0;0;False;0;False;-10;-7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1846.752,527.8632;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;80;-1145.717,1037.463;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;82;-1004.717,1005.463;Inherit;False;Step Antialiasing;-1;;3;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;15;-1684.342,731.4998;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;7;-1682.1,384.8;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;14;-1618.342,592.6001;Inherit;False;Property;_FogDepth;Fog Depth;2;0;Create;True;0;0;0;False;0;False;10;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-1557.552,731.8632;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;85;-872.717,1140.464;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;83;-802.717,1067.463;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;16;-1395.141,684.6997;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;-1399.742,424.8;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-654.7172,1072.463;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;24;-1042.637,616.5255;Inherit;True;Property;_BaseMap;Base Map;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-492.7169,997.4626;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;90;-476.5116,1133.097;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;1;-1001.759,809.6267;Inherit;False;Property;_Color;Base Color;0;0;Create;False;0;0;0;False;0;False;1,1,1,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-1256.961,430.8;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-317.2155,986.9748;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-671.6372,785.5255;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;12;-1104.541,432.3462;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;27;-677.9451,525.8279;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-153.1099,965.1603;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-884.7356,414.459;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;28;-469.9449,525.8279;Inherit;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;0;False;0;False;Object;-1;False;True;False;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;88;46.1921,636.6318;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;23;-117.8158,391.223;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;269.7174,382.1373;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;FogShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;55;0
WireConnection;44;0;30;0
WireConnection;44;1;45;0
WireConnection;52;0;44;0
WireConnection;50;0;52;0
WireConnection;50;1;52;1
WireConnection;50;2;30;3
WireConnection;77;0;76;0
WireConnection;77;1;75;0
WireConnection;29;0;50;0
WireConnection;29;1;33;0
WireConnection;78;0;77;0
WireConnection;48;0;29;0
WireConnection;48;1;49;0
WireConnection;46;0;10;0
WireConnection;46;1;48;0
WireConnection;80;0;78;0
WireConnection;80;1;81;0
WireConnection;82;2;80;0
WireConnection;15;0;46;0
WireConnection;47;0;15;0
WireConnection;83;0;82;0
WireConnection;16;0;14;0
WireConnection;16;2;47;0
WireConnection;9;0;7;2
WireConnection;9;1;46;0
WireConnection;84;0;83;0
WireConnection;84;1;85;0
WireConnection;86;0;82;0
WireConnection;86;1;84;0
WireConnection;13;0;9;0
WireConnection;13;1;16;0
WireConnection;91;0;86;0
WireConnection;91;1;90;0
WireConnection;26;0;24;0
WireConnection;26;1;1;0
WireConnection;12;0;13;0
WireConnection;92;0;26;0
WireConnection;92;1;91;0
WireConnection;39;1;12;0
WireConnection;28;0;27;0
WireConnection;88;1;92;0
WireConnection;88;2;39;0
WireConnection;23;0;28;0
WireConnection;23;2;39;0
WireConnection;0;2;23;0
WireConnection;0;13;88;0
ASEEND*/
//CHKSM=62E087B76430727F3377E0E870D0C8B427F1A02B