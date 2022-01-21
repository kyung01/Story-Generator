Shader "Custom/UnLitTerrainShader"{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	_MainTex2("Texture", 2D) = "white" {}
	_MainTex3("Texture", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf Lambert

		struct Input {

		float3 worldPos;

		float2 uv_MainTex;
		float2 uv2_MainTex2;
		float2 uv3_MainTex3;
	};
	sampler2D _MainTex;
	void surf(Input IN, inout SurfaceOutput o) {
		float vX = IN.worldPos.x + .5f;
		float vZ = IN.worldPos.z + 0.5f;
		int xInt = vX;
		int zInt = vZ;
		float length = 1.0 / 8.0;
		float square = 1.0 / 8.0;
		float xDecimal = (vX - xInt) * square;
		float zDecimal = 1 - ((vZ - zInt)	* square);
		float xIndent = (vX - xInt) * length;
		float zIndent = (vZ - zInt) * length;

		float2 uv = float2(xDecimal, zDecimal);
		//o.uv = float2(vX - xInt, vZ - zInt);

		// sample the texture
		//fixed4 col = i.color;
		//fixed4 col = float4(uv.x, uv.y, 0, 1);
		float4 col01 = tex2D(_MainTex, float2(xDecimal, zDecimal));
		float4 col02 = tex2D(_MainTex, float2(square + xDecimal, +zDecimal));
		float4 col03 = tex2D(_MainTex, float2(square * 2 + xDecimal, zDecimal));

		float normalizedPower = IN.uv_MainTex.x + IN.uv_MainTex.y + IN.uv2_MainTex2.x;
		fixed4 col =
			col01 * (IN.uv_MainTex.x / normalizedPower)
			+ col02 * (IN.uv_MainTex.y / normalizedPower)
			+ col03 * (IN.uv2_MainTex2.x / normalizedPower);

		col = float4(col.x, col.y, col.z, 1);
		// col = tex2D(_MainTex, uv);
		// apply fog
		//UNITY_APPLY_FOG(i.fogCoord, col);
		//return col;

		o.Albedo = col;
	}
	ENDCG
	}
		Fallback "Diffuse"
}