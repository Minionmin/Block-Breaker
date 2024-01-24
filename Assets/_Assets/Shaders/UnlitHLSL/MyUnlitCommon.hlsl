#ifndef MY_UNLIT_COMMON_INCLUDED
// "#ifndef MY_UNLIT_COMMON_INCLUDED" is equivalent to "if !defined(MY_UNLIT_COMMON_INCLUDED)"
#define MY_UNLIT_COMMON_INCLUDED

void TestAlphaClip(float4 colorSample, float4 _Color, float _Cutoff)
{
#ifdef _ALPHA_CUTOUT
    // Short-circuit the fragment function returning immediately after clipped
    // which will neither write to the depth buffer or render target
    // if alpha is less than a threshold. It will be clipped when alpha is LEqual 0
    clip(colorSample.a * _Color.a - _Cutoff);
#endif
}
#endif