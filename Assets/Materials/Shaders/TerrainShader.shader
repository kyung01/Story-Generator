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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPosition : NORMAL;
				fixed4 color : COLOR;


				float2 uv : TEXCOORD0;
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
				o.worldPosition = v.vertex;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//o.uv = float2(xDecimal + xIndent , zIndent + zDecimal);
				//UNITY_TRANSFER_FOG(o,o.vertex);

				o.uv = v.uv;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{


				float vX = i.worldPosition.x + .5f;
				float vZ = i.worldPosition.z + 0.5f;
				int xInt = vX;
				int zInt = vZ;
				float length = 1.0 / 8.0;
				float square = 1.0 / 8.0;
				float xDecimal	= (vX - xInt) * square;
				float zDecimal = 1-((vZ - zInt)	* square);
				float xIndent	= (vX - xInt) * length;
				float zIndent	= (vZ - zInt) * length;

				float2 uv  = float2(xDecimal , zDecimal );
				//o.uv = float2(vX - xInt, vZ - zInt);

				// sample the texture
				//fixed4 col = i.color;
				//fixed4 col = float4(uv.x, uv.y, 0, 1);
				fixed4 col01 = tex2D(_MainTex, float2(xDecimal, zDecimal) );
				fixed4 col02 = tex2D(_MainTex, float2(square+xDecimal, + zDecimal) );
				fixed4 col03 = tex2D(_MainTex, float2(square*2 + xDecimal, zDecimal));

				float normalizedPower = i.uv.x + i.uv.y + i.uv1.x;
				fixed4 col =
					col01 * (i.uv.x / normalizedPower)
					+ col02 * (i.uv.y / normalizedPower)
					 +col03 * (i.uv1.x / normalizedPower);

				col = fixed4(col.x, col.y, col.z, 1);
				// col = tex2D(_MainTex, uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}

		ENDCG
	}
	}
}
