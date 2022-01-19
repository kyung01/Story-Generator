Shader "Custom/UnLitTerrainShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;/// id of the coordiante on the sprite shett

				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;


				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				int xInt = v.vertex.x;
				int zInt = v.vertex.z;
				float length = 1.0 / 8.0;
				float xDecimal =( v.vertex.x - xInt)*length;
				float zDecimal = (v.vertex.z - zInt)*length;
				float xIndent = (v.vertex.x - xInt) * length;
				float zIndent = (v.vertex.z - zInt) * length;

				//o.uv = float2(v.vertex.x - xInt, v.vertex.z - zInt);
				o.uv = float2(xDecimal + xIndent , zIndent + zDecimal);
				UNITY_TRANSFER_FOG(o,o.vertex);

				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = i.color;
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}

		ENDCG
	}
	}
}
