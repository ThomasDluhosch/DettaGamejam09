Shader "Custom/Jelly"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // Jelly wobble parameters
        _WobbleX ("Wobble X", Float) = 0
        _WobbleZ ("Wobble Z", Float) = 0
        _WobbleSpeed ("Wobble Speed", Float) = 5
        _WobbleIntensity ("Wobble Intensity", Float) = 0.1
        
        // Squash and stretch
        _SquashX ("Squash X", Float) = 1
        _SquashY ("Squash Y", Float) = 1
        
        // Jelly wave effect
        _JellyFrequency ("Jelly Frequency", Float) = 3
        _JellyAmplitude ("Jelly Amplitude", Float) = 0.05
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Jelly"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ UNITY_ETC1_EXTERNAL_ALPHA

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float _WobbleX;
                float _WobbleZ;
                float _WobbleSpeed;
                float _WobbleIntensity;
                float _SquashX;
                float _SquashY;
                float _JellyFrequency;
                float _JellyAmplitude;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                float3 pos = input.positionOS.xyz;
                
                // Get normalized factors for jelly effect (centered at 0)
                float heightFactor = input.uv.y - 0.5; // -0.5 to 0.5
                float widthFactor = input.uv.x - 0.5;  // -0.5 to 0.5
                
                // Apply uniform squash and stretch from center
                pos.x *= _SquashX;
                pos.y *= _SquashY;
                
                // Wobble effect - oscillating scale that preserves shape
                float wobbleTime = _Time.y * _WobbleSpeed;
                float wobbleSin = sin(wobbleTime);
                float wobbleCos = cos(wobbleTime * 1.3);
                
                // Apply wobble as subtle scale oscillation (volume-preserving feel)
                float wobbleScaleX = 1.0 + wobbleSin * _WobbleX * _WobbleIntensity;
                float wobbleScaleY = 1.0 + wobbleCos * _WobbleZ * _WobbleIntensity;
                pos.x *= wobbleScaleX;
                pos.y *= wobbleScaleY;
                
                // Secondary jelly wave effect - ripple from bottom to top
                float normalizedHeight = input.uv.y; // 0 to 1
                float wave = sin(normalizedHeight * _JellyFrequency * 3.14159 + wobbleTime) * _JellyAmplitude;
                float wobbleStrength = (abs(_WobbleX) + abs(_WobbleZ)) * 0.5;
                pos.x += wave * normalizedHeight * wobbleStrength;
                
                // Subtle edge wobble for gelatinous feel
                float edgeWobble = sin(normalizedHeight * _JellyFrequency + _Time.y * _WobbleSpeed * 0.5);
                pos.x += edgeWobble * _JellyAmplitude * normalizedHeight * wobbleStrength * 0.5;
                
                output.positionCS = TransformObjectToHClip(pos);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return texColor * input.color;
            }
            ENDHLSL
        }
        
        // Shadow caster pass for 2D shadows if needed
        Pass
        {
            Name "Sprite Unlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float _WobbleX;
                float _WobbleZ;
                float _WobbleSpeed;
                float _WobbleIntensity;
                float _SquashX;
                float _SquashY;
                float _JellyFrequency;
                float _JellyAmplitude;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                float3 pos = input.positionOS.xyz;
                
                // Get normalized factors for jelly effect (centered at 0)
                float heightFactor = input.uv.y - 0.5; // -0.5 to 0.5
                float widthFactor = input.uv.x - 0.5;  // -0.5 to 0.5
                
                // Apply uniform squash and stretch from center
                pos.x *= _SquashX;
                pos.y *= _SquashY;
                
                // Wobble effect - oscillating scale that preserves shape
                float wobbleTime = _Time.y * _WobbleSpeed;
                float wobbleSin = sin(wobbleTime);
                float wobbleCos = cos(wobbleTime * 1.3);
                
                // Apply wobble as subtle scale oscillation (volume-preserving feel)
                float wobbleScaleX = 1.0 + wobbleSin * _WobbleX * _WobbleIntensity;
                float wobbleScaleY = 1.0 + wobbleCos * _WobbleZ * _WobbleIntensity;
                pos.x *= wobbleScaleX;
                pos.y *= wobbleScaleY;
                
                // Secondary jelly wave effect - ripple from bottom to top
                float normalizedHeight = input.uv.y; // 0 to 1
                float wave = sin(normalizedHeight * _JellyFrequency * 3.14159 + wobbleTime) * _JellyAmplitude;
                float wobbleStrength = (abs(_WobbleX) + abs(_WobbleZ)) * 0.5;
                pos.x += wave * normalizedHeight * wobbleStrength;
                
                // Subtle edge wobble for gelatinous feel
                float edgeWobble = sin(normalizedHeight * _JellyFrequency + _Time.y * _WobbleSpeed * 0.5);
                pos.x += edgeWobble * _JellyAmplitude * normalizedHeight * wobbleStrength * 0.5;
                
                output.positionCS = TransformObjectToHClip(pos);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return texColor * input.color;
            }
            ENDHLSL
        }
    }
    
    Fallback "Sprites/Default"
}
