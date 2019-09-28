// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2688,x:33025,y:32527,varname:node_2688,prsc:2|emission-3614-RGB;n:type:ShaderForge.SFN_Tex2d,id:3614,x:32655,y:32569,varname:node_3614,prsc:2,tex:c221c650ebe774b1bb74a9ba572e6085,ntxv:0,isnm:False|UVIN-3348-OUT,TEX-1255-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:1255,x:32235,y:32538,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_1255,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c221c650ebe774b1bb74a9ba572e6085,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:5497,x:32294,y:32717,varname:node_5497,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4193,x:32153,y:32948,varname:node_4193,prsc:2;n:type:ShaderForge.SFN_Add,id:3348,x:32536,y:32772,varname:node_3348,prsc:2|A-5497-UVOUT,B-7885-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2855,x:32153,y:33150,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_2855,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:7885,x:32379,y:33093,varname:node_7885,prsc:2|A-4193-T,B-2855-OUT;proporder:1255-2855;pass:END;sub:END;*/

Shader "Custom/Water" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Speed ("Speed", Float ) = 0.1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_4193 = _Time;
                float2 node_3348 = (i.uv0+(node_4193.g*_Speed));
                float4 node_3614 = tex2D(_MainTex,TRANSFORM_TEX(node_3348, _MainTex));
                float3 emissive = node_3614.rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
