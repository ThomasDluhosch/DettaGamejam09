Shader "Custom/PlayerEffects"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Space(20)]
        [Header(Damage Effect)]
        _DamageFlashSpeed ("Damage Flash Speed", Range(0, 20)) = 0
        
        [Space(20)]
        [Header(PowerUp Effect)]
        _PowerUpSpeed ("Power Up Color Shift Speed", Range(0, 20)) = 0
        
        [Space(20)]
        [Header(Sprite Settings)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 10
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull [_Cull]
        Lighting Off
        ZWrite Off
        Blend [_SrcBlend] [_DstBlend]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _DamageFlashSpeed;
            float _PowerUpSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                fixed3 originalColor = col.rgb;
                
                float damageActive = step(0.01, _DamageFlashSpeed);
                float damageFlicker = sin(_Time.y * _DamageFlashSpeed * 10.0) * 0.5 + 0.5;
                damageFlicker = step(0.5, damageFlicker);
                float3 damageColor = lerp(originalColor, float3(1, 1, 1), damageFlicker);
                col.rgb = lerp(originalColor, damageColor, damageActive);
                
                float powerUpActive = step(0.01, _PowerUpSpeed);
                float timeShift = _Time.y * _PowerUpSpeed;
                
                float3 color1 = float3(1.0, 0.2, 0.2);  // Red
                float3 color2 = float3(1.0, 1.0, 0.3);  // Yellow
                float3 color3 = float3(1.0, 1.0, 1.0);  // White
                
                float cycle = frac(timeShift * 0.5);
                
                float t1 = saturate(cycle * 3.0);
                float t2 = saturate((cycle - 0.33) * 3.0);
                float t3 = saturate((cycle - 0.66) * 3.0);
                
                float3 blend1 = lerp(color1, color2, t1);
                float3 blend2 = lerp(color2, color3, t2);
                float3 blend3 = lerp(color3, color1, t3);
                
                float weight1 = step(cycle, 0.33);
                float weight2 = step(cycle, 0.66) - weight1;
                float weight3 = 1.0 - step(cycle, 0.66);
                
                float3 powerUpColor = blend1 * weight1 + blend2 * weight2 + blend3 * weight3;
                
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
                float3 shiftedColor = powerUpColor * luminance * 1.5;
                col.rgb = lerp(col.rgb, shiftedColor, powerUpActive);
                
                // Preserve alpha
                #ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
                #endif
                
                return col;
            }
            ENDCG
        }
    }
    
    Fallback "Sprites/Default"
}
