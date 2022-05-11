// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogShader"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,0)
		_FogColor("Fog Color", Color) = (1,0.5137255,0.5176471,1)
		_FogHeight("Fog Height", Float) = 0
		_FogDepth("Fog Depth", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _Color;
		uniform float _FogHeight;
		uniform float _FogDepth;
		uniform float4 _FogColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float clampResult16 = clamp( _FogDepth , 0.0 , abs( _FogHeight ) );
			float clampResult12 = clamp( ( ( ase_worldPos.y - _FogHeight ) / clampResult16 ) , 0.0 , 1.0 );
			float4 lerpResult3 = lerp( float4( 0,0,0,1 ) , _Color , clampResult12);
			o.Albedo = lerpResult3.rgb;
			float4 lerpResult23 = lerp( _FogColor , float4( 0,0,0,0 ) , clampResult12);
			o.Emission = lerpResult23.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
91;196;1472;786;1060.189;-134.8295;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;10;-1256.6,523.4002;Inherit;False;Property;_FogHeight;Fog Height;2;0;Create;True;0;0;0;False;0;False;0;-7.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;15;-1058.9,729.4999;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;7;-1129.1,379.8;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;14;-1019.9,586.6002;Inherit;False;Property;_FogDepth;Fog Depth;3;0;Create;True;0;0;0;False;0;False;0;6.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;-892.3002,426.8;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;16;-852.6999,680.6998;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-666.5001,428.8;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;-527.1998,429;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-606.6002,574.3;Inherit;False;Property;_FogColor;Fog Color;1;0;Create;True;0;0;0;False;0;False;1,0.5137255,0.5176471,1;1,0.37,0.37,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-697.1002,197.6001;Inherit;False;Property;_Color;Base Color;0;0;Create;False;0;0;0;False;0;False;1,1,1,0;0.9254902,0.9176471,0.9176471,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-296.8158,410.223;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;3;-339,182;Inherit;True;3;0;COLOR;0,0,0,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;255,179;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;FogShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;10;0
WireConnection;9;0;7;2
WireConnection;9;1;10;0
WireConnection;16;0;14;0
WireConnection;16;2;15;0
WireConnection;13;0;9;0
WireConnection;13;1;16;0
WireConnection;12;0;13;0
WireConnection;23;0;2;0
WireConnection;23;2;12;0
WireConnection;3;1;1;0
WireConnection;3;2;12;0
WireConnection;0;0;3;0
WireConnection;0;2;23;0
ASEEND*/
//CHKSM=685FE20E3E8AAD2D481D9D51FA8CD6D3096BA04E