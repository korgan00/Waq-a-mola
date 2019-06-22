Shader "Custom/QuantumField"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Custom fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
			float3 pos : SV_POSITION;
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_base v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.pos = v.vertex;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = ((IN.pos.yyy * 3) - 2) * 0.2;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

		float4 LightingCustom(SurfaceOutput s, float3 lightDir, float atten) {
			float4 col;
			
			col.rgb = (dot(s.Normal, lightDir) * atten * _LightColor0.rgb) + s.Albedo;
			//in case we want to make the shader transparent in the future - irrelevant right now
			col.a = 1;

			return col;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
