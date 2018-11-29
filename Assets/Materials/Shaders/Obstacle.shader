// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Obstacle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _TargetColor("Target Color", Color) = (1,1,1,1)
        _ObstacleColor("Obstacle Color", Color) = (0,0,0,0)
        _TargetRange("Target Range", Range(0, 360)) = 40
        _Dissolve("Dissolve", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;            
			};
            
			v2f vert (appdata v)
			{
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
                output.uv = v.uv;
                return output;
			}
            
            sampler2D _MainTex;
            float4 _TargetColor;
            float4 _ObstacleColor;
            float _TargetRange;
            float _Dissolve; 
			
			fixed4 frag (v2f i) : SV_Target
			{
                float angle = degrees(atan2(i.uv.y - .5, i.uv.x - .5)) + 180;
                
                float4 color = tex2D(_MainTex, i.uv);
                
                if (angle > _TargetRange) {
                
                    color *= _ObstacleColor;
                    
                } else if (_Dissolve == 0){
                    
                    color *= _TargetColor;
                
                } else {
                    discard;
                }
                
                bool OBS_shouldDissolveCounterClockwise = angle > 360 + _Dissolve * (-180 + _TargetRange * 0.5);
                bool OBS_shouldDissolveClockwise = angle < (180 - _TargetRange * 0.5) * _Dissolve + _TargetRange && angle > _TargetRange;
                
                //bool TAR_shouldDissolveCounterClockwise = angle < _TargetRange && angle > _TargetRange * (1 - _Dissolve * 0.5);
                //bool TAR_shouldDissolveClockwise = angle < _TargetRange * 0.5 * _Dissolve; 
                
                //Dissolve
                if (OBS_shouldDissolveCounterClockwise || OBS_shouldDissolveClockwise ||
                    //TAR_shouldDissolveCounterClockwise || TAR_shouldDissolveClockwise)
                    false)
                {
                    discard;
                }
                
                return color;
			}
			ENDCG
		}
	}
}
