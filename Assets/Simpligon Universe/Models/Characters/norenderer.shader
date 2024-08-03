Shader "Custom/norenderer"
{
    SubShader {
        Tags { "Queue" = "Geometry" }
        Pass {
            // Objeyi görünmez yap
            ColorMask 0
            // ZTest, gölgenin doğru yerleştirilmesi için gereklidir
            ZWrite On
        }
    }
    Fallback "Diffuse"
}
