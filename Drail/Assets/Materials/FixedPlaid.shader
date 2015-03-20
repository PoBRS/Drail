     Shader "Example/ScreenPos" {
    Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _SplatMap ( "Splat Map (RGB + A)" , 2D) = "white" {}
    _Overlay ("Masked Texture", 2D) = "gray" {}
    }
    SubShader {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
    #pragma surface surf Lambert
    struct Input {
    float2 uv_MainTex;
    float2 uv_SplatMap;
    float4 screenPos;
    };
    sampler2D _MainTex;
    sampler2D _SplatMap;
    sampler2D _Overlay;
    void surf ( Input IN , inout SurfaceOutput o) {
    o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
    float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
    screenUV *= float2(8,6);
    half4 splatCol = tex2D(_SplatMap , IN.uv_SplatMap);
    o.Albedo = lerp(O.Albedo, tex2D (_Overlay, screenUV).rgb , splatCol.r);
    }
    ENDCG
    }
    Fallback "Diffuse"