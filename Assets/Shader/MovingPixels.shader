Shader "Unlit/MovingPixels"
{
Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        _NumberOfCells("Cell Count", Float) = 5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define PI 3.14159265359

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float4 _Color1;
            float4 _Color2;
            float _NumberOfCells;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float reverseStep(float color, float comparision)
            {
                return color <= comparision;
            }

            float SmoothMinimum(float a, float b, float k)
            {
                float h = max(k - abs(a - b), 0.0) / k;
                return min(a, b) - h * h * h * k * (1.0 / 6.0);
            }

            float SDF_Circle(float2 p, float2 c, float r)
            {
                float d = distance(p, c);
                d = abs(d);
                return max(d - r, 0);
            }

            float SDF_Box(float2 p, float2 r)
            {
                p = abs(p);
                float d = sqrt(max(p.x - r.x, 0) * max(p.x - r.x, 0) + max(p.y - r.y, 0) * max(p.y - r.y, 0));
                d = saturate(d);
                return d;
                // Can be written as 
                // float2 d = abs(p)-r;
                // return length(max(d,0.0)) + min(max(d.x,d.y),0.0);
            }

            float Random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            // float RandomWithMovement(float2 uv)
            // {
            //     return sin(_Time.y * 0.5 + Random(uv) * 10) * 0.5 + 0.5;
            // }

            //Creates patters. Used here for change in height/mask
            float RandomWithMovement(float2 uv)
            {
                return sin(0.5 * _Time.y + sin(dot(uv , float2(1,1))) * 10) * 0.5 + 0.5;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 newUV = i.uv * _NumberOfCells;
                float2 segments = frac(newUV); //Creates smaller fragments of uv
                float2 floored = floor(newUV); //Creates cell id as each pixel in a cell has the same color
                float height = RandomWithMovement(floored); //Height is displaced as per the cell id
                float2 cellID, shadowRegions;
                float distacneToCenter;
                float shadowAggregate = 1;
                for (int j = 0; j < 9; j++) //This loop is responsible for the shadow
                {
                    if (j == 3 || j == 1 || j == 0) //Creates shadow for left, bottom and bottom left corner
                    {
                        cellID = float2(j / 3, j % 3) - 1.; //Spits out cell id for all the surrounding cells
                        shadowRegions = abs(segments - 0.5 - cellID) - 0.5; //This tells how the shadow should look like. It displaces the origin of i.uv

                        float heightOfAdjacentCell = RandomWithMovement(floored + cellID); //We check what is the height of the adjacent cell by adding the cellID to the current cell
                        float differenceBetweenHeights = heightOfAdjacentCell - height; //The difference between current cell and adjacent cell
                        //First we check the length of each pixel co-ordinate we got from shadowRegions from the new origin. We only want positive values as -ve values cannot be visualized.
                        //We divide so that -> more the differenceBetweenHeights less should be the value of distacneToCenter hence a darker color. If the difference is less then distanceToCenter will have a higher value hence lighter color.
                        //We take the max as we cannot divide by 0 or -ve values
                        distacneToCenter = length(max(shadowRegions, 0)) / max(differenceBetweenHeights, 0.01) / 2; 

                        //shadowAggregate is the minimum of all the shadows cast by all the cells nearby. Can be looked as the storage for the loop.
                        shadowAggregate = min(shadowAggregate, distacneToCenter);
                    }
                }
                //lerp between 2 colors based on height/mask and then add the shadow
                float4 col = lerp(_Color1, _Color2, height) + min(shadowAggregate, 0.1) - 0.25;
                return col;
            }
            ENDCG
        }
    }
}
