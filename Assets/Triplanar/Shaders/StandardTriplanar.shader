// Standard shader with triplanar mapping
// https://github.com/keijiro/StandardTriplanar

Shader "Standard Triplanar"
{
    Properties
    {
        _Color("", Color) = (1, 1, 1, 1)
        _Color2("", Color) = (1, 1, 1, 1)
        
        _MainTex("", 2D) = "white" {}
        _MainTex2("", 2D) = "white" {}

        _Glossiness("", Range(0, 1)) = 0.5
        [Gamma] _Metallic("", Range(0, 1)) = 0

        _BumpScale("", Float) = 1
        _BumpMap("", 2D) = "bump" {}

        _OcclusionStrength("", Range(0, 1)) = 1
        _OcclusionMap("", 2D) = "white" {}

        _MapScale("", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        
        CGPROGRAM
        
        #pragma surface surf Standard vertex:vert fullforwardshadows addshadow

        #pragma shader_feature _NORMALMAP
        #pragma shader_feature _OCCLUSIONMAP

        #pragma target 3.0

        half4 _Color;
        half4 _Color2;
        sampler2D _MainTex;
        sampler2D _MainTex2;

        half _Glossiness;
        half _Metallic;

        half _BumpScale;
        sampler2D _BumpMap;

        half _OcclusionStrength;
        sampler2D _OcclusionMap;

        half _MapScale;

        struct Input
        {
            float3 localCoord;
            float3 localNormal;
        };

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            data.localCoord = v.vertex.xyz;
            data.localNormal = v.normal.xyz;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Blending factor of triplanar mapping
            float pc = abs(IN.localNormal.y);
            
            float3 bf = normalize(abs(IN.localNormal));
            bf /= dot(bf, (float3)1);

            // Triplanar mapping
            float2 tx = IN.localCoord.yz * _MapScale;
            float2 ty = IN.localCoord.zx * _MapScale;
            float2 tz = IN.localCoord.xy * _MapScale;
            
            
            // Base color
            half4 cx1 = lerp(tex2D(_MainTex, tx),tex2D(_MainTex2, tx), pc)* bf.x;
            half4 cy1 = lerp(tex2D(_MainTex, ty),tex2D(_MainTex2, ty), pc)* bf.y;
            half4 cz1 = lerp(tex2D(_MainTex, tz),tex2D(_MainTex2, tz), pc)* bf.z;
            
            half4 cx2 = lerp(tex2D(_MainTex, tx*2),tex2D(_MainTex2, tx*2), pc)* bf.x;
            half4 cy2 = lerp(tex2D(_MainTex, ty*2),tex2D(_MainTex2, ty*2), pc)* bf.y;
            half4 cz2 = lerp(tex2D(_MainTex, tz*2),tex2D(_MainTex2, tz*2), pc)* bf.z;
            
            half4 cx = (cx1+cx2)/2;
            half4 cy = (cy1+cy2)/2;
            half4 cz = (cz1+cz2)/2;
            
            half4 color = (cx + cy + cz) * lerp(_Color,_Color2,pc);
            
            color.r*=(1+sin(IN.localCoord.x/10))*0.5*0.8 + (1+sin(IN.localCoord.x))*0.5*0.2;
            color.b*=(1+sin(IN.localCoord.z/10))*0.5*0.8 + (1+sin(IN.localCoord.z))*0.5*0.2;
            color.g*=(1+sin(IN.localCoord.y))/2;
            o.Albedo = color.rgb;
            o.Alpha = color.a;

    

            // Misc parameters
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
    CustomEditor "StandardTriplanarInspector"
}
