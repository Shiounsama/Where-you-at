void GetScreenTexelSize_float(out float2 texelSize)
{
    texelSize = 1.0 / _ScreenParams.xy;
}

void GetScreenTexelSize_half(out half2 texelSize)
{
    texelSize = 1.0 / _ScreenParams.xy;
}

#define MAX_KERNEL 11.0f;

float SampleDepth(float2 UV)
{

       return LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
     //return Linear01Depth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
}

void Outline_float(
    float2 uv,
    float2 texelSize,
    float kernelSize,
    float alpha,
    float shapeRatio,
    float powfactor,
    out float4 laplacian
)
{
    laplacian = float4(0.0f, 0.0f, 0.0f, 0.0f);
    float halfKernelSize = floor(kernelSize);
    float halfKernelSizeSq = kernelSize * kernelSize;
    float centerWeight = 0.0f;


    float2 rotation = float2(cos(alpha), sin(alpha));
    float3 laplacian_normal = float3(0.0f, 0.0f, 0.0f);
    float laplacian_depth = 0.0f;

    [unroll(7)]
    for (float x = -halfKernelSize; x <= halfKernelSize; x++)
    {
        [unroll(7)]
        for (float y = -halfKernelSize; y <= halfKernelSize; y++)
        {
            float2 markerPoint = float2(dot(rotation, float2(x, y)) * shapeRatio, dot(rotation, float2(y, -x)));
            float sqrDist = dot(float2(x,y), float2(x,y));

            if (x == 0 && y == 0)
            {
                continue;
            }
            
            if (sqrDist > halfKernelSizeSq)
            {
                continue;
            }

            float factor = (halfKernelSizeSq - sqrDist) / halfKernelSizeSq;
            
            factor = pow(factor, powfactor);
            
            centerWeight += factor;

            float2 kernelUV = uv + texelSize * float2(x, y);

            laplacian_normal -= SHADERGRAPH_SAMPLE_SCENE_NORMAL(kernelUV) * factor;
            laplacian_depth -= SampleDepth(kernelUV) * factor;
        }
    }

    laplacian_normal += SHADERGRAPH_SAMPLE_SCENE_NORMAL(uv) * centerWeight;
    laplacian_depth += SampleDepth(uv) * centerWeight;
    
    centerWeight = 1 / centerWeight;
    laplacian_normal *= centerWeight;
    laplacian_depth *= centerWeight;

    laplacian = float4(laplacian_normal, laplacian_depth);
}

void Outline_half(
    half2 uv,
    half2 texelSize,
    float kernelSize,
    float alpha,
    float shapeRatio,
    float powfactor,
    out float4 laplacian
)
{
    laplacian = float4(0.0f, 0.0f, 0.0f, 0.0f);
    float halfKernelSize = floor(kernelSize);
    float halfKernelSizeSq = kernelSize * kernelSize;
    float centerWeight = 0.0f;


    float2 rotation = float2(cos(alpha), sin(alpha));
    float3 laplacian_normal = float3(0.0f, 0.0f, 0.0f);
    float laplacian_depth = 0.0f;

    [unroll(20)]
    for (float x = -halfKernelSize; x <= halfKernelSize; x++)
    {
        [unroll(20)]
        for (float y = -halfKernelSize; y <= halfKernelSize; y++)
        {
            float2 markerPoint = float2(dot(rotation, float2(x, y)) * shapeRatio, dot(rotation, float2(y, -x)));
            float sqrDist = dot(float2(x, y), float2(x, y));

            if (x == 0 && y == 0)
            {
                continue;
            }
            
            if (sqrDist > halfKernelSizeSq)
            {
                continue;
            }

            float factor = (halfKernelSizeSq - sqrDist) / halfKernelSizeSq;
            
            factor = pow(factor, powfactor);
            
            centerWeight += factor;

            

            float2 kernelUV = uv + texelSize * float2(x, y);

            laplacian_normal -= SHADERGRAPH_SAMPLE_SCENE_NORMAL(kernelUV) * factor;
            laplacian_depth -= SampleDepth(kernelUV) * factor;
        }
    }

    laplacian_normal += SHADERGRAPH_SAMPLE_SCENE_NORMAL(uv) * centerWeight;
    laplacian_depth += SampleDepth(uv) * centerWeight;
    
    centerWeight = 1 / centerWeight;
    laplacian_normal *= centerWeight;
    laplacian_depth *= centerWeight;

    laplacian = float4(laplacian_normal, laplacian_depth);
    laplacian = float4(1,1,1,1);
}
