Shader "Custom/ TerrainRaim Shader"
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
	float2 uv : TEXCOORD0;
	float4 color : COLOR;
	float2 uv1 : TEXCOORD1;
	float2 uv2 : TEXCOORD2;
   };

   struct v2f
   {
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
	float4 vertex : SV_POSITION;
	float4 color : COLOR;
	float2 uv1 : TEXCOORD1;
	float2 uv2 : TEXCOORD2;
   };

   sampler2D _MainTex;
   float4 _MainTex_ST;

	v2f vert(appdata v)
	 {
	v2f o;
	o.color = fixed4(1, 0, 0, 1);
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	o.uv1 = v.uv1;
	o.uv2 = v.uv2;
	UNITY_TRANSFER_FOG(o,o.vertex);
	return o;
   }

	fixed4 frag(v2f i) : SV_Target
	{
		// return fixed(1,0,0,1);
				 // sam	```ple the texture
	  //	 fixed4 col = tex2D(_MainTex, i.uv);
			  // apply fog
		  //	UNITY_APPLY_FOG(i.fogCoord, col);
			  return fixed4(1,1,1,1);
			  }
		   ENDCG
		  }
	}
}
