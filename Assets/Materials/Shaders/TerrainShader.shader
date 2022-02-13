Shader "Custom/UnLitTerrainShader"{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_MainTex2("Texture", 2D) = "white" {}
	_MainTex3("Texture", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows

#pragma target 3.0
		struct Input {

		float3 worldPos;

		float2 uv_MainTex;
		float2 uv2_MainTex2;
		float2 uv3_MainTex3;
		};
	sampler2D _MainTex;
	sampler2D _MainTex2;
	
	void surf(Input IN, inout SurfaceOutputStandard  o) {
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
		float3 normal02 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal + square * 1, zDecimal)));
		float3 normal03 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal + square * 2, zDecimal)));
		float3 normal04 = UnpackNormal(tex2D(_MainTex2, float2(xDecimal + square * 3, zDecimal)));

		float4 col01 = tex2D(_MainTex, float2(xDecimal, zDecimal));
		float4 col02 = tex2D(_MainTex, float2(square * 1 + xDecimal, +zDecimal));
		float4 col03 = tex2D(_MainTex, float2(square * 2 + xDecimal, zDecimal));
		float4 col04 = tex2D(_MainTex, float2(square * 3 + xDecimal, zDecimal));

		float normalizedPower = IN.uv_MainTex.x + IN.uv_MainTex.y + IN.uv2_MainTex2.x + IN.uv2_MainTex2.y;
		float power01 = (IN.uv_MainTex.x / normalizedPower);
		float power02 = (IN.uv_MainTex.y / normalizedPower);
		float power03 = (IN.uv2_MainTex2.x / normalizedPower);
		float power04 = (IN.uv2_MainTex2.y / normalizedPower);

		power01 = pow(power01,3);
		power02 = pow(power02,3);
		power03 = pow(power03, 3);
		power04 = pow(power04, 3);
		float powerNormal = power01 + power02 + power03 + power04;
		power01 /= powerNormal;
		power02 /= powerNormal;
		power03 /= powerNormal;
		power04 /= powerNormal;


		float3 col =
			col01.xyz * power01
			+ col02.xyz * power02
			+ col03.xyz * power03
			+ col04.xyz * power04;
		float3 smooth =
			col01.w * power01
			+ col02.w * power02
			+ col03.w * power03
			+ col04.w * power04;
		float3 normal =
			normal01.xyz * (IN.uv_MainTex.x / normalizedPower)
			+ normal02.xyz * (IN.uv_MainTex.y / normalizedPower)
			+ normal03.xyz * (IN.uv2_MainTex2.x / normalizedPower)
			+ normal04.xyz * (IN.uv2_MainTex2.y / normalizedPower);

		//float smooth = smooth01;
		//col = normalize(col);
		//col = float3(min(1,col.x), min(1,col.y), min(col.z,1));

		//col = float4(col.x, col.y, col.z, 1);
		// col = tex2D(_MainTex, uv);
		// apply fog
		//UNITY_APPLY_FOG(i.fogCoord, col);
		//return col;

		o.Albedo = float4(col.x, col.y, col.z, 1.0);
		//o.Albedo = float4(1, 1, 1, 1);
		o.Normal = normal;
		//o.Specular = 1;
		//o.Metallic =1- smooth;
		//o.Normal = normalize(normal + float3(0, 0, 0));
	}
	ENDCG
	}
		Fallback "Diffuse"
}