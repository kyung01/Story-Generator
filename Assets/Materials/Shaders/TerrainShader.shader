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
	sampler2D _MainTex2;
	
	void surf(Input IN, inout SurfaceOutput o) {
		float vX = IN.worldPos.x + .5f;
		float vZ = IN.worldPos.y + .5f;
		int xInt = vX;
		int zInt = vZ;
		float length = 1.0 / 8.0;
		float square = 1.0 / 8.0;
		float xDecimal = (vX - xInt) * square;
		float zDecimal = 1 - (square-(vZ - zInt)	* square);
		float xIndent = (vX - xInt) * length;
		float zIndent = (vZ - zInt) * length;

		float2 uv = float2(xDecimal, zDecimal);
		//o.uv = float2(vX - xInt, vZ - zInt);

		// sample the texture
		//fixed4 col = i.color;
		//fixed4 col = float4(uv.x, uv.y, 0, 1);

		float smooth01 = tex2D(_MainTex, float2(xDecimal, zDecimal)).w;

		float3 normal01 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal, zDecimal)));
		float3 normal02 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal + square, zDecimal)));
		float3 normal03 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal + square*2, zDecimal)));

		float4 col01 = tex2D(_MainTex, float2(xDecimal, zDecimal));
		float4 col02 = tex2D(_MainTex, float2(square + xDecimal, +zDecimal));
		float4 col03 = tex2D(_MainTex, float2(square * 2 + xDecimal, zDecimal));

		float normalizedPower = IN.uv_MainTex.x + IN.uv_MainTex.y + IN.uv2_MainTex2.x + IN.uv2_MainTex2.y;
		float3 col =
			col01.xyz * (IN.uv_MainTex.x / normalizedPower)
			+ col02.xyz * (IN.uv_MainTex.y / normalizedPower)
			+ col03.xyz * (IN.uv2_MainTex2.x / normalizedPower);
		float3 normal =
			normal01.xyz * (IN.uv_MainTex.x / normalizedPower)
			+ normal02.xyz * (IN.uv_MainTex.y / normalizedPower)
			+ normal03.xyz * (IN.uv2_MainTex2.x / normalizedPower);

		float smooth = smooth01;
		//col = normalize(col);
		//col = float3(min(1,col.x), min(1,col.y), min(col.z,1));

		//col = float4(col.x, col.y, col.z, 1);
		// col = tex2D(_MainTex, uv);
		// apply fog
		//UNITY_APPLY_FOG(i.fogCoord, col);
		//return col;

		o.Albedo = float4(col.x,col.y,col.z,1.0);
		o.Normal = normal;
		o.Specular = smooth;
		//o.Normal = normalize(normal + float3(0, 0, 0));
	}
	ENDCG
	}
		Fallback "Diffuse"
}