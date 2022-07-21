#region MIT license
// 
// MIT license
//
// Copyright (c) 2013 Corey Murtagh
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
#endregion
#region Attributions
//
// Contents of the LibMp3Lame and NativeMethods classes and associated enumerations 
// are directly based on the lame.h available at:
//                https://sourceforge.net/p/lame/svn/6430/tree/trunk/lame/include/lame.h
//
// Source lines and comments included where useful/possible.
#endregion
using System;
using System.Text;
using System.Runtime.InteropServices;

#if X64
using size_t = System.UInt64;
#else
using size_t = System.UInt32;
#endif

namespace LameDLLWrap
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Ansi)]
	internal delegate void delReportFunction(string fmt, IntPtr args);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Ansi)]
	internal delegate void delGenreCallback(
		int index,
		string genre,
		IntPtr cookie
	);

	internal static class NativeMethods
	{
		public static IntPtr lame_init() => Environment.Is64BitProcess ? NativeMethods64.lame_init() : NativeMethods32.lame_init();
		public static int lame_close(IntPtr context) => Environment.Is64BitProcess ? NativeMethods64.lame_close(context) : NativeMethods32.lame_close(context);
		public static size_t lame_get_lametag_frame(IntPtr context, byte[] buffer, size_t size)
			 => Environment.Is64BitProcess ? NativeMethods64.lame_get_lametag_frame(context, buffer, size) : NativeMethods32.lame_get_lametag_frame(context, buffer, size);

		#region LAME information

		public static IntPtr get_lame_version()
			=> Environment.Is64BitProcess ? NativeMethods64.get_lame_version() : NativeMethods32.get_lame_version();
		public static string get_lame_short_version()
			=> Environment.Is64BitProcess ? NativeMethods64.get_lame_short_version() : NativeMethods32.get_lame_short_version();
		public static string get_lame_very_short_version()
			=> Environment.Is64BitProcess ? NativeMethods64.get_lame_very_short_version() : NativeMethods32.get_lame_very_short_version();
		public static string get_psy_version()
			=> Environment.Is64BitProcess ? NativeMethods64.get_psy_version() : NativeMethods32.get_psy_version();
		public static string get_lame_url()
			=> Environment.Is64BitProcess ? NativeMethods64.get_lame_url() : NativeMethods32.get_lame_url();
		public static string get_lame_os_bitness()
			=> Environment.Is64BitProcess ? NativeMethods64.get_lame_os_bitness() : NativeMethods32.get_lame_os_bitness();
		public static void get_lame_version_numerical(LAMEVersion ver)
		{ if (Environment.Is64BitProcess) NativeMethods64.get_lame_version_numerical(ver); else NativeMethods32.get_lame_version_numerical(ver); }
		#endregion

		#region Input Stream Description

		public static int lame_set_num_samples(IntPtr context, ulong num_samples)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_num_samples(context, num_samples) : NativeMethods32.lame_set_num_samples(context, num_samples);
		public static ulong lame_get_num_samples(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_num_samples(context) : NativeMethods32.lame_get_num_samples(context);
		public static int lame_set_in_samplerate(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_in_samplerate(context, value) : NativeMethods32.lame_set_in_samplerate(context, value);
		public static int lame_get_in_samplerate(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_in_samplerate(context) : NativeMethods32.lame_get_in_samplerate(context);
		public static int lame_set_num_channels(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_num_channels(context, value) : NativeMethods32.lame_set_num_channels(context, value);
		public static int lame_get_num_channels(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_num_channels(context) : NativeMethods32.lame_get_num_channels(context);
		public static int lame_set_scale(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_scale(context, value) : NativeMethods32.lame_set_scale(context, value);
		public static float lame_get_scale(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_scale(context) : NativeMethods32.lame_get_scale(context);
		public static int lame_set_scale_left(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_scale_left(context, value) : NativeMethods32.lame_set_scale_left(context, value);
		public static float lame_get_scale_left(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_scale_left(context) : NativeMethods32.lame_get_scale_left(context);
		public static int lame_set_scale_right(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_scale_right(context, value) : NativeMethods32.lame_set_scale_right(context, value);
		public static float lame_get_scale_right(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_scale_right(context) : NativeMethods32.lame_get_scale_right(context);
		public static int lame_set_out_samplerate(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_out_samplerate(context, value) : NativeMethods32.lame_set_out_samplerate(context, value);
		public static int lame_get_out_samplerate(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_out_samplerate(context) : NativeMethods32.lame_get_out_samplerate(context);

		#endregion

		#region General control parameters

		public static int lame_set_analysis(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_analysis(context, value) : NativeMethods32.lame_set_analysis(context, value);
		public static bool lame_get_analysis(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_analysis(context) : NativeMethods32.lame_get_analysis(context);
		public static int lame_set_bWriteVbrTag(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_bWriteVbrTag(context, value) : NativeMethods32.lame_set_bWriteVbrTag(context, value);
		public static bool lame_get_bWriteVbrTag(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_bWriteVbrTag(context) : NativeMethods32.lame_get_bWriteVbrTag(context);
		public static int lame_set_decode_only(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_decode_only(context, value) : NativeMethods32.lame_set_decode_only(context, value);
		public static bool lame_get_decode_only(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_decode_only(context) : NativeMethods32.lame_get_decode_only(context);
		public static int lame_set_quality(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_quality(context, value) : NativeMethods32.lame_set_quality(context, value);
		public static int lame_get_quality(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_quality(context) : NativeMethods32.lame_get_quality(context);
		public static int lame_set_mode(IntPtr context, MPEGMode value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_mode(context, value) : NativeMethods32.lame_set_mode(context, value);
		public static MPEGMode lame_get_mode(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_mode(context) : NativeMethods32.lame_get_mode(context);
		public static int lame_set_force_ms(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_force_ms(context, value) : NativeMethods32.lame_set_force_ms(context, value);
		public static bool lame_get_force_ms(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_force_ms(context) : NativeMethods32.lame_get_force_ms(context);
		public static int lame_set_free_format(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_free_format(context, value) : NativeMethods32.lame_set_free_format(context, value);
		public static bool lame_get_free_format(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_free_format(context) : NativeMethods32.lame_get_free_format(context);
		public static int lame_set_findReplayGain(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_findReplayGain(context, value) : NativeMethods32.lame_set_findReplayGain(context, value);
		public static bool lame_get_findReplayGain(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_findReplayGain(context) : NativeMethods32.lame_get_findReplayGain(context);
		public static int lame_set_decode_on_the_fly(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_decode_on_the_fly(context, value) : NativeMethods32.lame_set_decode_on_the_fly(context, value);
		public static bool lame_get_decode_on_the_fly(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_decode_on_the_fly(context) : NativeMethods32.lame_get_decode_on_the_fly(context);
		public static int lame_set_nogap_total(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_nogap_total(context, value) : NativeMethods32.lame_set_nogap_total(context, value);
		public static int lame_get_nogap_total(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_nogap_total(context) : NativeMethods32.lame_get_nogap_total(context);
		public static int lame_set_nogap_currentindex(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_nogap_currentindex(context, value) : NativeMethods32.lame_set_nogap_currentindex(context, value);
		public static int lame_get_nogap_currentindex(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_nogap_currentindex(context) : NativeMethods32.lame_get_nogap_currentindex(context);
		public static int lame_set_brate(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_brate(context, value) : NativeMethods32.lame_set_brate(context, value);
		public static int lame_get_brate(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_brate(context) : NativeMethods32.lame_get_brate(context);
		public static int lame_set_compression_ratio(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_compression_ratio(context, value) : NativeMethods32.lame_set_compression_ratio(context, value);
		public static float lame_get_compression_ratio(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_compression_ratio(context) : NativeMethods32.lame_get_compression_ratio(context);
		public static int lame_set_preset(IntPtr context, LAMEPreset value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_preset(context, value) : NativeMethods32.lame_set_preset(context, value);
		public static int lame_set_asm_optimizations(IntPtr context, ASMOptimizations opt, bool val)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_asm_optimizations(context, opt, val) : NativeMethods32.lame_set_asm_optimizations(context, opt, val);

		#endregion

		#region Frame parameters

		public static int lame_set_copyright(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_copyright(context, value) : NativeMethods32.lame_set_copyright(context, value);
		public static bool lame_get_copyright(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_copyright(context) : NativeMethods32.lame_get_copyright(context);
		public static int lame_set_original(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_original(context, value) : NativeMethods32.lame_set_original(context, value);
		public static bool lame_get_original(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_original(context) : NativeMethods32.lame_get_original(context);
		public static int lame_set_error_protection(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_error_protection(context, value) : NativeMethods32.lame_set_error_protection(context, value);
		public static bool lame_get_error_protection(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_error_protection(context) : NativeMethods32.lame_get_error_protection(context);
		public static int lame_set_extension(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_extension(context, value) : NativeMethods32.lame_set_extension(context, value);
		public static bool lame_get_extension(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_extension(context) : NativeMethods32.lame_get_extension(context);
		public static int lame_set_strict_ISO(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_strict_ISO(context, value) : NativeMethods32.lame_set_strict_ISO(context, value);
		public static bool lame_get_strict_ISO(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_strict_ISO(context) : NativeMethods32.lame_get_strict_ISO(context);

		#endregion

		#region Quantization/Noise Shaping

		public static int lame_set_disable_reservoir(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_disable_reservoir(context, value) : NativeMethods32.lame_set_disable_reservoir(context, value);
		public static bool lame_get_disable_reservoir(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_disable_reservoir(context) : NativeMethods32.lame_get_disable_reservoir(context);
		public static int lame_set_quant_comp(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_quant_comp(context, value) : NativeMethods32.lame_set_quant_comp(context, value);
		public static int lame_get_quant_comp(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_quant_comp(context) : NativeMethods32.lame_get_quant_comp(context);
		public static int lame_set_quant_comp_short(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_quant_comp_short(context, value) : NativeMethods32.lame_set_quant_comp_short(context, value);
		public static int lame_get_quant_comp_short(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_quant_comp_short(context) : NativeMethods32.lame_get_quant_comp_short(context);
		public static int lame_set_experimentalX(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_experimentalX(context, value) : NativeMethods32.lame_set_experimentalX(context, value);
		public static int lame_get_experimentalX(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_experimentalX(context) : NativeMethods32.lame_get_experimentalX(context);
		public static int lame_set_experimentalY(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_experimentalY(context, value) : NativeMethods32.lame_set_experimentalY(context, value);
		public static int lame_get_experimentalY(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_experimentalY(context) : NativeMethods32.lame_get_experimentalY(context);
		public static int lame_set_experimentalZ(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_experimentalZ(context, value) : NativeMethods32.lame_set_experimentalZ(context, value);
		public static int lame_get_experimentalZ(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_experimentalZ(context) : NativeMethods32.lame_get_experimentalZ(context);
		public static int lame_set_exp_nspsytune(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_exp_nspsytune(context, value) : NativeMethods32.lame_set_exp_nspsytune(context, value);
		public static int lame_get_exp_nspsytune(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_exp_nspsytune(context) : NativeMethods32.lame_get_exp_nspsytune(context);
		public static int lame_set_msfix(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_msfix(context, value) : NativeMethods32.lame_set_msfix(context, value);
		public static int lame_get_msfix(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_msfix(context) : NativeMethods32.lame_get_msfix(context);

		#endregion

		#region VBR control

		public static int lame_set_VBR(IntPtr context, VBRMode value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR(context, value) : NativeMethods32.lame_set_VBR(context, value);
		public static VBRMode lame_get_VBR(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR(context) : NativeMethods32.lame_get_VBR(context);
		public static int lame_set_VBR_q(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_q(context, value) : NativeMethods32.lame_set_VBR_q(context, value);
		public static int lame_get_VBR_q(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_q(context) : NativeMethods32.lame_get_VBR_q(context);
		public static int lame_set_VBR_quality(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_quality(context, value) : NativeMethods32.lame_set_VBR_quality(context, value);
		public static float lame_get_VBR_quality(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_quality(context) : NativeMethods32.lame_get_VBR_quality(context);
		public static int lame_set_VBR_mean_bitrate_kbps(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_mean_bitrate_kbps(context, value) : NativeMethods32.lame_set_VBR_mean_bitrate_kbps(context, value);
		public static int lame_get_VBR_mean_bitrate_kbps(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_mean_bitrate_kbps(context) : NativeMethods32.lame_get_VBR_mean_bitrate_kbps(context);
		public static int lame_set_VBR_min_bitrate_kbps(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_min_bitrate_kbps(context, value) : NativeMethods32.lame_set_VBR_min_bitrate_kbps(context, value);
		public static int lame_get_VBR_min_bitrate_kbps(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_min_bitrate_kbps(context) : NativeMethods32.lame_get_VBR_min_bitrate_kbps(context);
		public static int lame_set_VBR_max_bitrate_kbps(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_max_bitrate_kbps(context, value) : NativeMethods32.lame_set_VBR_max_bitrate_kbps(context, value);
		public static int lame_get_VBR_max_bitrate_kbps(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_max_bitrate_kbps(context) : NativeMethods32.lame_get_VBR_max_bitrate_kbps(context);
		public static int lame_set_VBR_hard_min(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_VBR_hard_min(context, value) : NativeMethods32.lame_set_VBR_hard_min(context, value);
		public static bool lame_get_VBR_hard_min(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_VBR_hard_min(context) : NativeMethods32.lame_get_VBR_hard_min(context);

		#endregion

		#region Filtering control

		public static int lame_set_lowpassfreq(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_lowpassfreq(context, value) : NativeMethods32.lame_set_lowpassfreq(context, value);
		public static int lame_get_lowpassfreq(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_lowpassfreq(context) : NativeMethods32.lame_get_lowpassfreq(context);
		public static int lame_set_lowpasswidth(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_lowpasswidth(context, value) : NativeMethods32.lame_set_lowpasswidth(context, value);
		public static int lame_get_lowpasswidth(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_lowpasswidth(context) : NativeMethods32.lame_get_lowpasswidth(context);
		public static int lame_set_highpassfreq(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_highpassfreq(context, value) : NativeMethods32.lame_set_highpassfreq(context, value);
		public static int lame_get_highpassfreq(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_highpassfreq(context) : NativeMethods32.lame_get_highpassfreq(context);
		public static int lame_set_highpasswidth(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_highpasswidth(context, value) : NativeMethods32.lame_set_highpasswidth(context, value);
		public static int lame_get_highpasswidth(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_highpasswidth(context) : NativeMethods32.lame_get_highpasswidth(context);

		#endregion

		#region Psychoacoustics and other advanced settings

		public static int lame_set_ATHonly(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_ATHonly(context, value) : NativeMethods32.lame_set_ATHonly(context, value);
		public static bool lame_get_ATHonly(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_ATHonly(context) : NativeMethods32.lame_get_ATHonly(context);
		public static int lame_set_ATHshort(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_ATHshort(context, value) : NativeMethods32.lame_set_ATHshort(context, value);
		public static bool lame_get_ATHshort(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_ATHshort(context) : NativeMethods32.lame_get_ATHshort(context);
		public static int lame_set_noATH(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_noATH(context, value) : NativeMethods32.lame_set_noATH(context, value);
		public static bool lame_get_noATH(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_noATH(context) : NativeMethods32.lame_get_noATH(context);
		public static int lame_set_ATHtype(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_ATHtype(context, value) : NativeMethods32.lame_set_ATHtype(context, value);
		public static int lame_get_ATHtype(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_ATHtype(context) : NativeMethods32.lame_get_ATHtype(context);
		public static int lame_set_ATHlower(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_ATHlower(context, value) : NativeMethods32.lame_set_ATHlower(context, value);
		public static float lame_get_ATHlower(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_ATHlower(context) : NativeMethods32.lame_get_ATHlower(context);
		public static int lame_set_athaa_type(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_athaa_type(context, value) : NativeMethods32.lame_set_athaa_type(context, value);
		public static int lame_get_athaa_type(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_athaa_type(context) : NativeMethods32.lame_get_athaa_type(context);
		public static int lame_set_athaa_sensitivity(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_athaa_sensitivity(context, value) : NativeMethods32.lame_set_athaa_sensitivity(context, value);
		public static float lame_get_athaa_sensitivity(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_athaa_sensitivity(context) : NativeMethods32.lame_get_athaa_sensitivity(context);
		public static int lame_set_allow_diff_short(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_allow_diff_short(context, value) : NativeMethods32.lame_set_allow_diff_short(context, value);
		public static bool lame_get_allow_diff_short(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_allow_diff_short(context) : NativeMethods32.lame_get_allow_diff_short(context);
		public static int lame_set_useTemporal(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_useTemporal(context, value) : NativeMethods32.lame_set_useTemporal(context, value);
		public static bool lame_get_useTemporal(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_useTemporal(context) : NativeMethods32.lame_get_useTemporal(context);
		public static int lame_set_interChRatio(IntPtr context, float value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_interChRatio(context, value) : NativeMethods32.lame_set_interChRatio(context, value);
		public static float lame_get_interChRatio(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_interChRatio(context) : NativeMethods32.lame_get_interChRatio(context);
		public static int lame_set_no_short_blocks(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_no_short_blocks(context, value) : NativeMethods32.lame_set_no_short_blocks(context, value);
		public static bool lame_get_no_short_blocks(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_no_short_blocks(context) : NativeMethods32.lame_get_no_short_blocks(context);
		public static int lame_set_force_short_blocks(IntPtr context, bool value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_force_short_blocks(context, value) : NativeMethods32.lame_set_force_short_blocks(context, value);
		public static bool lame_get_force_short_blocks(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_force_short_blocks(context) : NativeMethods32.lame_get_force_short_blocks(context);
		public static int lame_set_emphasis(IntPtr context, int value)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_emphasis(context, value) : NativeMethods32.lame_set_emphasis(context, value);
		public static int lame_get_emphasis(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_emphasis(context) : NativeMethods32.lame_get_emphasis(context);
		#endregion

		#region Internal state variables, read only

		public static MPEGVersion lame_get_version(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_version(context) : NativeMethods32.lame_get_version(context);
		public static int lame_get_encoder_delay(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_encoder_delay(context) : NativeMethods32.lame_get_encoder_delay(context);
		public static int lame_get_encoder_padding(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_encoder_padding(context) : NativeMethods32.lame_get_encoder_padding(context);
		public static int lame_get_framesize(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_framesize(context) : NativeMethods32.lame_get_framesize(context);
		public static int lame_get_mf_samples_to_encode(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_mf_samples_to_encode(context) : NativeMethods32.lame_get_mf_samples_to_encode(context);
		public static int lame_get_size_mp3buffer(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_size_mp3buffer(context) : NativeMethods32.lame_get_size_mp3buffer(context);
		public static int lame_get_frameNum(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_frameNum(context) : NativeMethods32.lame_get_frameNum(context);
		public static int lame_get_totalframes(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_totalframes(context) : NativeMethods32.lame_get_totalframes(context);
		public static int lame_get_RadioGain(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_RadioGain(context) : NativeMethods32.lame_get_RadioGain(context);
		public static int lame_get_AudiophileGain(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_AudiophileGain(context) : NativeMethods32.lame_get_AudiophileGain(context);
		public static float lame_get_PeakSample(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_PeakSample(context) : NativeMethods32.lame_get_PeakSample(context);
		public static int lame_get_noclipGainChange(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_noclipGainChange(context) : NativeMethods32.lame_get_noclipGainChange(context);
		public static float lame_get_noclipScale(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_noclipScale(context) : NativeMethods32.lame_get_noclipScale(context);

		#endregion

		#region Processing

		public static int lame_init_params(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_init_params(context) : NativeMethods32.lame_init_params(context);

		public static int lame_encode_buffer(IntPtr context, short[] buffer_l, short[] buffer_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_interleaved(IntPtr context, short[] pcm, int num_samples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_interleaved(context, pcm, num_samples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_interleaved(context, pcm, num_samples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_float(IntPtr context, float[] pcm_l, float[] pcm_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_float(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_float(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_ieee_float(IntPtr context, float[] pcm_l, float[] pcm_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_ieee_float(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_ieee_float(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_interleaved_ieee_float(IntPtr context, float[] pcm, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_interleaved_ieee_float(context, pcm, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_interleaved_ieee_float(context, pcm, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_ieee_double(IntPtr context, double[] pcm_l, double[] pcm_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_ieee_double(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_ieee_double(context, pcm_l, pcm_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_interleaved_ieee_double(IntPtr context, double[] pcm, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_interleaved_ieee_double(context, pcm, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_interleaved_ieee_double(context, pcm, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_long(IntPtr context, long[] buffer_l, long[] buffer_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_long(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_long(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_long2(IntPtr context, long[] buffer_l, long[] buffer_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_long2(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_long2(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_buffer_int(IntPtr context, int[] buffer_l, int[] buffer_r, int nSamples, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_buffer_int(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_buffer_int(context, buffer_l, buffer_r, nSamples, mp3buf, mp3buf_size);

		public static int lame_encode_flush(IntPtr context, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_flush(context, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_flush(context, mp3buf, mp3buf_size);

		public static int lame_encode_flush_nogap(IntPtr context, byte[] mp3buf, int mp3buf_size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_encode_flush_nogap(context, mp3buf, mp3buf_size) : NativeMethods32.lame_encode_flush_nogap(context, mp3buf, mp3buf_size);

		public static int lame_init_bitstream(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_init_bitstream(context) : NativeMethods32.lame_init_bitstream(context);

		#endregion

		#region Reporting callbacks

		public static int lame_set_errorf(IntPtr context, delReportFunction fn)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_errorf(context, fn) : NativeMethods32.lame_set_errorf(context, fn);
		public static int lame_set_debugf(IntPtr context, delReportFunction fn)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_debugf(context, fn) : NativeMethods32.lame_set_debugf(context, fn);
		public static int lame_set_msgf(IntPtr context, delReportFunction fn)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_set_msgf(context, fn) : NativeMethods32.lame_set_msgf(context, fn);

		public static void lame_print_config(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.lame_init(); else NativeMethods32.lame_init(); }
		public static void lame_print_internals(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.lame_init(); else NativeMethods32.lame_init(); }


		#endregion

		#region 'printf' support for reporting functions

		public static int _vsnprintf_s(StringBuilder str, int sizeOfBuffer, int count, string format, IntPtr va_args)
			=> Environment.Is64BitProcess ? NativeMethods64._vsnprintf_s(str, sizeOfBuffer, count, format, va_args) : NativeMethods32._vsnprintf_s(str, sizeOfBuffer, count, format, va_args);

		public static string printf(string format, IntPtr va_args)
			=> Environment.Is64BitProcess ? NativeMethods64.printf(format, va_args) : NativeMethods32.printf(format, va_args);
		#endregion

		#region Decoding

		public static IntPtr hip_decode_init()
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode_init() : NativeMethods32.hip_decode_init();

		public static int hip_decode_exit(IntPtr decContext)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode_exit(decContext) : NativeMethods32.hip_decode_exit(decContext);

		public static void hip_set_errorf(IntPtr decContext, delReportFunction f)
		{ if (Environment.Is64BitProcess) NativeMethods64.hip_set_errorf(decContext, f); else NativeMethods32.hip_set_errorf(decContext, f); }

		public static void hip_set_debugf(IntPtr decContext, delReportFunction f)
		{ if (Environment.Is64BitProcess) NativeMethods64.hip_set_debugf(decContext, f); else NativeMethods32.hip_set_debugf(decContext, f); }

		public static void hip_set_msgf(IntPtr decContext, delReportFunction f)
		{ if (Environment.Is64BitProcess) NativeMethods64.hip_set_msgf(decContext, f); else NativeMethods32.hip_set_msgf(decContext, f); }

		public static int hip_decode(IntPtr decContext, byte[] mp3buf, int len, short[] pcm_l, short[] pcm_r)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode(decContext, mp3buf, len, pcm_l, pcm_r) : NativeMethods32.hip_decode(decContext, mp3buf, len, pcm_l, pcm_r);

		public static int hip_decode_headers(IntPtr decContext, byte[] mp3buf, int len, short[] pcm_l, short[] pcm_r, mp3data mp3data)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode_headers(decContext, mp3buf, len, pcm_l, pcm_r, mp3data) : NativeMethods32.hip_decode_headers(decContext, mp3buf, len, pcm_l, pcm_r, mp3data);

		public static int hip_decode1(IntPtr decContext, byte[] mp3buf, int len, short[] pcm_l, short[] pcm_r)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode1(decContext, mp3buf, len, pcm_l, pcm_r) : NativeMethods32.hip_decode1(decContext, mp3buf, len, pcm_l, pcm_r);

		public static int hip_decode1_headers(IntPtr decContext, byte[] mp3buf, int len, short[] pcm_l, short[] pcm_r, mp3data mp3data)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode1_headers(decContext, mp3buf, len, pcm_l, pcm_r, mp3data) : NativeMethods32.hip_decode1_headers(decContext, mp3buf, len, pcm_l, pcm_r, mp3data);

		public static int hip_decode(IntPtr decContext, byte[] mp3buf, int len, short[] pcm_l, short[] pcm_r, mp3data mp3data, out int enc_delay, out int enc_padding)
			=> Environment.Is64BitProcess ? NativeMethods64.hip_decode(decContext, mp3buf, len, pcm_l, pcm_r, mp3data, out enc_delay, out enc_padding) : NativeMethods32.hip_decode(decContext, mp3buf, len, pcm_l, pcm_r, mp3data, out enc_delay, out enc_padding);

		#endregion

		#region ID3 tag support

		public static void id3tag_genre_list(delGenreCallback handler, IntPtr cookie)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_genre_list(handler, cookie); else NativeMethods32.id3tag_genre_list(handler, cookie); }
		public static void id3tag_init(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_init(context); else NativeMethods32.id3tag_init(context); }
		public static void id3tag_add_v2(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_add_v2(context); else NativeMethods32.id3tag_add_v2(context); }
		public static void id3tag_v1_only(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_v1_only(context); else NativeMethods32.id3tag_v1_only(context); }


		public static void id3tag_v2_only(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_v2_only(context); else NativeMethods32.id3tag_v2_only(context); }
		public static void id3tag_space_v1(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_space_v1(context); else NativeMethods32.id3tag_space_v1(context); }
		public static void id3tag_pad_v2(IntPtr context)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_pad_v2(context); else NativeMethods32.id3tag_pad_v2(context); }
		public static void id3tag_set_pad(IntPtr context, int n)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_set_pad(context, n); else NativeMethods32.id3tag_set_pad(context, n); }
		public static void id3tag_set_title(IntPtr context, string title)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_set_title(context, title); else NativeMethods32.id3tag_set_title(context, title); }
		public static void id3tag_set_artist(IntPtr context, string artist)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_set_artist(context, artist); else NativeMethods32.id3tag_set_artist(context, artist); }
		public static void id3tag_set_album(IntPtr context, string album)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_set_album(context, album); else NativeMethods32.id3tag_set_album(context, album); }
		public static void id3tag_set_year(IntPtr context, string year)
		{ if (Environment.Is64BitProcess) NativeMethods64.id3tag_set_year(context, year); else NativeMethods32.id3tag_set_year(context, year); }
		public static int id3tag_set_comment(IntPtr context, string comment)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_comment(context, comment) : NativeMethods32.id3tag_set_comment(context, comment);
		public static int id3tag_set_comment_utf16(IntPtr context, string lang, byte[] description, byte[] text)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_comment_utf16(context, lang, description, text) : NativeMethods32.id3tag_set_comment_utf16(context, lang, description, text);
		public static int id3tag_set_track(IntPtr context, string track)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_track(context, track) : NativeMethods32.id3tag_set_track(context, track);
		public static int id3tag_set_genre(IntPtr context, string genre)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_genre(context, genre) : NativeMethods32.id3tag_set_genre(context, genre);
		public static int id3tag_set_fieldvalue(IntPtr context, string value)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_fieldvalue(context, value) : NativeMethods32.id3tag_set_fieldvalue(context, value);
		public static int id3tag_set_fieldvalue_utf16(IntPtr context, byte[] value)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_fieldvalue_utf16(context, value) : NativeMethods32.id3tag_set_fieldvalue_utf16(context, value);
		public static int id3tag_set_albumart(IntPtr context, byte[] image, int size)
			=> Environment.Is64BitProcess ? NativeMethods64.id3tag_set_albumart(context, image, size) : NativeMethods32.id3tag_set_albumart(context, image, size);
		public static int lame_get_id3v1_tag(IntPtr context, byte[] buffer, int size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_id3v1_tag(context, buffer, size) : NativeMethods32.lame_get_id3v1_tag(context, buffer, size);
		public static int lame_get_id3v2_tag(IntPtr context, byte[] buffer, int size)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_id3v2_tag(context, buffer, size) : NativeMethods32.lame_get_id3v2_tag(context, buffer, size);
		public static void lame_set_write_id3tag_automatic(IntPtr context, bool value)
		{ if (Environment.Is64BitProcess) NativeMethods64.lame_set_write_id3tag_automatic(context, value); else NativeMethods32.lame_set_write_id3tag_automatic(context, value); }
		public static bool lame_get_write_id3tag_automatic(IntPtr context)
			=> Environment.Is64BitProcess ? NativeMethods64.lame_get_write_id3tag_automatic(context) : NativeMethods32.lame_get_write_id3tag_automatic(context);
		#endregion

#pragma warning restore IDE1006 // Naming Styles
	}
	internal static class NativeMethods32
	{
		const string libname = @"libmp3lame.32.dll";

		static NativeMethods32()
		{
			if (!System.IO.File.Exists(libname))
			{
				try
				{
					System.IO.File.WriteAllBytes(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libname), NAudio.Lame.Properties.Resources.libmp3lame_32);
				}
				catch (Exception ex)
				{
					throw new DllNotFoundException($"Could not load {libname}", ex);
				}
			}
		}


#pragma warning disable IDE1006 // Naming Styles

		public static string BoundDLL { get; } = libname;

		#region Startup/Shutdown

		/*
		 * REQUIRED:
		 * initialize the encoder.  sets default for all encoder parameters,
		 * returns NULL if some malloc()'s failed
		 * otherwise returns pointer to structure needed for all future
		 * API calls.
		 */
		// lame_global_flags * CDECL lame_init(void);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lame_init();

		/*
		 * REQUIRED:
		 * final call to free all remaining buffers
		 */
		// int  CDECL lame_close (lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_close(IntPtr context);


		/*
		 * OPTIONAL:
		 * lame_get_lametag_frame copies the final LAME-tag into 'buffer'.
		 * The function returns the number of bytes copied into buffer, or
		 * the required buffer size, if the provided buffer is too small.
		 * Function failed, if the return value is larger than 'size'!
		 * Make sure lame_encode flush has been called before calling this function.
		 * NOTE:
		 * if VBR  tags are turned off by the user, or turned off by LAME,
		 * this call does nothing and returns 0.
		 * NOTE:
		 * LAME inserted an empty frame in the beginning of mp3 audio data,
		 * which you have to replace by the final LAME-tag frame after encoding.
		 * In case there is no ID3v2 tag, usually this frame will be the very first
		 * data in your mp3 file. If you put some other leading data into your
		 * file, you'll have to do some bookkeeping about where to write this buffer.
		 */
		// int size_t CDECL lame_get_lametag_frame(const lame_global_flags *, unsigned char* buffer, size_t size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern size_t lame_get_lametag_frame(
			IntPtr context,
			[In, Out] byte[] buffer,
			[In] size_t size
			);

		#endregion

		#region LAME information
		/*
		 * OPTIONAL:
		 * get the version number, in a string. of the form:
		 * "3.63 (beta)" or just "3.63".
		 */
		// const char*  CDECL get_lame_version       ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr get_lame_version();

		// const char*  CDECL get_lame_short_version ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_short_version();

		// const char*  CDECL get_lame_very_short_version ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_very_short_version();

		// const char*  CDECL get_psy_version        ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_psy_version();

		// const char*  CDECL get_lame_url           ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_url();

		// const char*  CDECL get_lame_os_bitness    ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_os_bitness();

		// void CDECL get_lame_version_numerical(lame_version_t *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void get_lame_version_numerical([Out]LAMEVersion ver);
		#endregion

		#region Input Stream Description
		/* number of samples.  default = 2^32-1   */
		// int CDECL lame_set_num_samples(lame_global_flags *, unsigned long);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_num_samples(IntPtr context, UInt64 num_samples);
		// unsigned long CDECL lame_get_num_samples(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt64 lame_get_num_samples(IntPtr context);

		/* input sample rate in Hz.  default = 44100hz */
		// int CDECL lame_set_in_samplerate(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_in_samplerate(IntPtr context, int value);
		// int CDECL lame_get_in_samplerate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_in_samplerate(IntPtr context);

		/* number of channels in input stream. default=2  */
		//int CDECL lame_set_num_channels(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_num_channels(IntPtr context, int value);
		//int CDECL lame_get_num_channels(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_num_channels(IntPtr context);

		/*
		  scale the input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		//int CDECL lame_set_scale(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale(IntPtr context, float value);
		//float CDECL lame_get_scale(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale(IntPtr context);

		/*
		  scale the channel 0 (left) input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_scale_left(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale_left(IntPtr context, float value);
		// float CDECL lame_get_scale_left(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale_left(IntPtr context);

		/*
		  scale the channel 1 (right) input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_scale_right(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale_right(IntPtr context, float value);
		// float CDECL lame_get_scale_right(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale_right(IntPtr context);

		/*
		  output sample rate in Hz.  default = 0, which means LAME picks best value
		  based on the amount of compression.  MPEG only allows:
		  MPEG1    32, 44.1,   48khz
		  MPEG2    16, 22.05,  24
		  MPEG2.5   8, 11.025, 12
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_out_samplerate(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_out_samplerate(IntPtr context, int value);
		// int CDECL lame_get_out_samplerate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_out_samplerate(IntPtr context);

		#endregion

		#region General control parameters

		/* 1=cause LAME to collect data for an MP3 frame analyzer. default=0 */
		// int CDECL lame_set_analysis(lame_global_flags *, int);
		// int CDECL lame_get_analysis(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_analysis(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_analysis(IntPtr context);

		/*
		  1 = write a Xing VBR header frame.
		  default = 1
		  this variable must have been added by a Hungarian notation Windows programmer :-)
		*/
		// int CDECL lame_set_bWriteVbrTag(lame_global_flags *, int);
		// int CDECL lame_get_bWriteVbrTag(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_bWriteVbrTag(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool lame_get_bWriteVbrTag(IntPtr context);

		/* 1=decode only.  use lame/mpglib to convert mp3/ogg to wav.  default=0 */
		// int CDECL lame_set_decode_only(lame_global_flags *, int);
		// int CDECL lame_get_decode_only(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_decode_only(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool lame_get_decode_only(IntPtr context);

		/*
		  internal algorithm selection.  True quality is determined by the bitrate
		  but this variable will effect quality by selecting expensive or cheap algorithms.
		  quality=0..9.  0=best (very slow).  9=worst.
		  recommended:  2     near-best quality, not too slow
						5     good quality, fast
						7     ok quality, really fast
		*/
		// int CDECL lame_set_quality(lame_global_flags *, int);
		// int CDECL lame_get_quality(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quality(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quality(IntPtr context);

		/*
		  mode = 0,1,2,3 = stereo, jstereo, dual channel (not supported), mono
		  default: lame picks based on compression ration and input channels
		*/
		// int CDECL lame_set_mode(lame_global_flags *, MPEG_mode);
		// MPEG_mode CDECL lame_get_mode(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_mode(IntPtr context, MPEGMode value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern MPEGMode lame_get_mode(IntPtr context);

		/*
		  force_ms.  Force M/S for all frames.  For testing only.
		  default = 0 (disabled)
		*/
		// int CDECL lame_set_force_ms(lame_global_flags *, int);
		// int CDECL lame_get_force_ms(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_force_ms(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_force_ms(IntPtr context);

		/* use free_format?  default = 0 (disabled) */
		// int CDECL lame_set_free_format(lame_global_flags *, int);
		// int CDECL lame_get_free_format(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_free_format(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_free_format(IntPtr context);

		/* perform ReplayGain analysis?  default = 0 (disabled) */
		// int CDECL lame_set_findReplayGain(lame_global_flags *, int);
		// int CDECL lame_get_findReplayGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_findReplayGain(IntPtr context, [In, MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_findReplayGain(IntPtr context);

		/* decode on the fly. Search for the peak sample. If the ReplayGain
		 * analysis is enabled then perform the analysis on the decoded data
		 * stream. default = 0 (disabled)
		 * NOTE: if this option is set the build-in decoder should not be used */
		// int CDECL lame_set_decode_on_the_fly(lame_global_flags *, int);
		// int CDECL lame_get_decode_on_the_fly(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_decode_on_the_fly(IntPtr context, [In, MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_decode_on_the_fly(IntPtr context);

		/* counters for gapless encoding */
		// int CDECL lame_set_nogap_total(lame_global_flags*, int);
		// int CDECL lame_get_nogap_total(const lame_global_flags*);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_nogap_total(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_nogap_total(IntPtr context);

		// int CDECL lame_set_nogap_currentindex(lame_global_flags* , int);
		// int CDECL lame_get_nogap_currentindex(const lame_global_flags*);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_nogap_currentindex(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_nogap_currentindex(IntPtr context);

		/* set one of brate compression ratio.  default is compression ratio of 11.  */
		// int CDECL lame_set_brate(lame_global_flags *, int);
		// int CDECL lame_get_brate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_brate(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_brate(IntPtr context);

		// int CDECL lame_set_compression_ratio(lame_global_flags *, float);
		// float CDECL lame_get_compression_ratio(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_compression_ratio(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_compression_ratio(IntPtr context);

		//int CDECL lame_set_preset( lame_global_flags*  gfp, int );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_preset(IntPtr context, LAMEPreset value);

		//int CDECL lame_set_asm_optimizations( lame_global_flags*  gfp, int, int );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_asm_optimizations(IntPtr context, ASMOptimizations opt, [MarshalAs(UnmanagedType.Bool)] bool val);

		#endregion

		#region Frame parameters
		/* mark as copyright.  default=0 */
		// int CDECL lame_set_copyright(lame_global_flags *, int);
		// int CDECL lame_get_copyright(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_copyright(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_copyright(IntPtr context);

		/* mark as original.  default=1 */
		// int CDECL lame_set_original(lame_global_flags *, int);
		// int CDECL lame_get_original(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_original(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_original(IntPtr context);

		/* error_protection.  Use 2 bytes from each frame for CRC checksum. default=0 */
		// int CDECL lame_set_error_protection(lame_global_flags *, int);
		// int CDECL lame_get_error_protection(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_error_protection(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_error_protection(IntPtr context);

		/* MP3 'private extension' bit  Meaningless.  default=0 */
		// int CDECL lame_set_extension(lame_global_flags *, int);
		// int CDECL lame_get_extension(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_extension(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_extension(IntPtr context);

		/* enforce strict ISO compliance.  default=0 */
		// int CDECL lame_set_strict_ISO(lame_global_flags *, int);
		// int CDECL lame_get_strict_ISO(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_strict_ISO(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_strict_ISO(IntPtr context);

		#endregion

		#region Quantization/Noise Shaping
		/* disable the bit reservoir. For testing only. default=0 */
		// int CDECL lame_set_disable_reservoir(lame_global_flags *, int);
		// int CDECL lame_get_disable_reservoir(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_disable_reservoir(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_disable_reservoir(IntPtr context);

		/* select a different "best quantization" function. default=0  */
		// int CDECL lame_set_quant_comp(lame_global_flags *, int);
		// int CDECL lame_get_quant_comp(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quant_comp(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quant_comp(IntPtr context);

		// int CDECL lame_set_quant_comp_short(lame_global_flags *, int);
		// int CDECL lame_get_quant_comp_short(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quant_comp_short(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quant_comp_short(IntPtr context);

		// int CDECL lame_set_experimentalX(lame_global_flags *, int); /* compatibility*/
		// int CDECL lame_get_experimentalX(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalX(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalX(IntPtr context);

		/* another experimental option.  for testing only */
		// int CDECL lame_set_experimentalY(lame_global_flags *, int);
		// int CDECL lame_get_experimentalY(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalY(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalY(IntPtr context);

		/* another experimental option.  for testing only */
		// int CDECL lame_set_experimentalZ(lame_global_flags *, int);
		// int CDECL lame_get_experimentalZ(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalZ(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalZ(IntPtr context);

		/* Naoki's psycho acoustic model.  default=0 */
		// int CDECL lame_set_exp_nspsytune(lame_global_flags *, int);
		// int CDECL lame_get_exp_nspsytune(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_exp_nspsytune(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_exp_nspsytune(IntPtr context);

		// void CDECL lame_set_msfix(lame_global_flags *, double);
		// float CDECL lame_get_msfix(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_msfix(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_msfix(IntPtr context);

		#endregion

		#region VBR control
		/* Types of VBR.  default = vbr_off = CBR */
		// int CDECL lame_set_VBR(lame_global_flags *, vbr_mode);
		// vbr_mode CDECL lame_get_VBR(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR(IntPtr context, VBRMode value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern VBRMode lame_get_VBR(IntPtr context);

		/* VBR quality level.  0=highest  9=lowest  */
		// int CDECL lame_set_VBR_q(lame_global_flags *, int);
		// int CDECL lame_get_VBR_q(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_q(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_q(IntPtr context);

		/* VBR quality level.  0=highest  9=lowest, Range [0,...,10[  */
		// int CDECL lame_set_VBR_quality(lame_global_flags *, float);
		// float CDECL lame_get_VBR_quality(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_quality(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_VBR_quality(IntPtr context);

		/* Ignored except for VBR=vbr_abr (ABR mode) */
		// int CDECL lame_set_VBR_mean_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_mean_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_mean_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_mean_bitrate_kbps(IntPtr context);

		// int CDECL lame_set_VBR_min_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_min_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_min_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_min_bitrate_kbps(IntPtr context);

		// int CDECL lame_set_VBR_max_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_max_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_max_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_max_bitrate_kbps(IntPtr context);

		/*
		  1=strictly enforce VBR_min_bitrate.  Normally it will be violated for
		  analog silence
		*/
		// int CDECL lame_set_VBR_hard_min(lame_global_flags *, int);
		// int CDECL lame_get_VBR_hard_min(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_hard_min(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_VBR_hard_min(IntPtr context);

		#endregion

		#region Filtering control
		/* freq in Hz to apply lowpass. Default = 0 = lame chooses.  -1 = disabled */
		// int CDECL lame_set_lowpassfreq(lame_global_flags *, int);
		// int CDECL lame_get_lowpassfreq(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_lowpassfreq(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_lowpassfreq(IntPtr context);

		/* width of transition band, in Hz.  Default = one polyphase filter band */
		// int CDECL lame_set_lowpasswidth(lame_global_flags *, int);
		// int CDECL lame_get_lowpasswidth(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_lowpasswidth(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_lowpasswidth(IntPtr context);

		/* freq in Hz to apply highpass. Default = 0 = lame chooses.  -1 = disabled */
		// int CDECL lame_set_highpassfreq(lame_global_flags *, int);
		// int CDECL lame_get_highpassfreq(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_highpassfreq(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_highpassfreq(IntPtr context);

		/* width of transition band, in Hz.  Default = one polyphase filter band */
		// int CDECL lame_set_highpasswidth(lame_global_flags *, int);
		// int CDECL lame_get_highpasswidth(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_highpasswidth(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_highpasswidth(IntPtr context);

		#endregion

		#region Psychoacoustics and other advanced settings
		/* only use ATH for masking */
		// int CDECL lame_set_ATHonly(lame_global_flags *, int);
		// int CDECL lame_get_ATHonly(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHonly(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_ATHonly(IntPtr context);

		/* only use ATH for short blocks */
		// int CDECL lame_set_ATHshort(lame_global_flags *, int);
		// int CDECL lame_get_ATHshort(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHshort(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_ATHshort(IntPtr context);

		/* disable ATH */
		// int CDECL lame_set_noATH(lame_global_flags *, int);
		// int CDECL lame_get_noATH(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_noATH(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_noATH(IntPtr context);

		/* select ATH formula */
		// int CDECL lame_set_ATHtype(lame_global_flags *, int);
		// int CDECL lame_get_ATHtype(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHtype(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_ATHtype(IntPtr context);

		/* lower ATH by this many db */
		// int CDECL lame_set_ATHlower(lame_global_flags *, float);
		// float CDECL lame_get_ATHlower(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHlower(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_ATHlower(IntPtr context);

		/* select ATH adaptive adjustment type */
		// int CDECL lame_set_athaa_type( lame_global_flags *, int);
		// int CDECL lame_get_athaa_type( const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_athaa_type(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_athaa_type(IntPtr context);

		/* adjust (in dB) the point below which adaptive ATH level adjustment occurs */
		// int CDECL lame_set_athaa_sensitivity( lame_global_flags *, float);
		// float CDECL lame_get_athaa_sensitivity( const lame_global_flags* );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_athaa_sensitivity(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_athaa_sensitivity(IntPtr context);

		/*
		  allow blocktypes to differ between channels?
		  default: 0 for jstereo, 1 for stereo
		*/
		// int CDECL lame_set_allow_diff_short(lame_global_flags *, int);
		// int CDECL lame_get_allow_diff_short(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_allow_diff_short(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_allow_diff_short(IntPtr context);

		/* use temporal masking effect (default = 1) */
		// int CDECL lame_set_useTemporal(lame_global_flags *, int);
		// int CDECL lame_get_useTemporal(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_useTemporal(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_useTemporal(IntPtr context);

		/* use temporal masking effect (default = 1) */
		// int CDECL lame_set_interChRatio(lame_global_flags *, float);
		// float CDECL lame_get_interChRatio(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_interChRatio(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_interChRatio(IntPtr context);

		/* disable short blocks */
		// int CDECL lame_set_no_short_blocks(lame_global_flags *, int);
		// int CDECL lame_get_no_short_blocks(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_no_short_blocks(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_no_short_blocks(IntPtr context);

		/* force short blocks */
		// int CDECL lame_set_force_short_blocks(lame_global_flags *, int);
		// int CDECL lame_get_force_short_blocks(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_force_short_blocks(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_force_short_blocks(IntPtr context);

		/* Input PCM is emphased PCM (for instance from one of the rarely
		   emphased CDs), it is STRONGLY not recommended to use this, because
		   psycho does not take it into account, and last but not least many decoders
		   ignore these bits */
		// int CDECL lame_set_emphasis(lame_global_flags *, int);
		// int CDECL lame_get_emphasis(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_emphasis(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_emphasis(IntPtr context);
		#endregion

		#region Internal state variables, read only
		/* version  0=MPEG-2  1=MPEG-1  (2=MPEG-2.5)     */
		// int CDECL lame_get_version(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern MPEGVersion lame_get_version(IntPtr context);

		/* encoder delay   */
		// int CDECL lame_get_encoder_delay(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_encoder_delay(IntPtr context);

		/*
		  padding appended to the input to make sure decoder can fully decode
		  all input.  Note that this value can only be calculated during the
		  call to lame_encoder_flush().  Before lame_encoder_flush() has
		  been called, the value of encoder_padding = 0.
		*/
		// int CDECL lame_get_encoder_padding(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_encoder_padding(IntPtr context);

		/* size of MPEG frame */
		// int CDECL lame_get_framesize(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_framesize(IntPtr context);

		/* number of PCM samples buffered, but not yet encoded to mp3 data. */
		// int CDECL lame_get_mf_samples_to_encode( const lame_global_flags*  gfp );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_mf_samples_to_encode(IntPtr context);

		/*
		  size (bytes) of mp3 data buffered, but not yet encoded.
		  this is the number of bytes which would be output by a call to
		  lame_encode_flush_nogap.  NOTE: lame_encode_flush() will return
		  more bytes than this because it will encode the reamining buffered
		  PCM samples before flushing the mp3 buffers.
		*/
		// int CDECL lame_get_size_mp3buffer( const lame_global_flags*  gfp );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_size_mp3buffer(IntPtr context);

		/* number of frames encoded so far */
		// int CDECL lame_get_frameNum(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_frameNum(IntPtr context);

		/*
		  lame's estimate of the total number of frames to be encoded
		   only valid if calling program set num_samples
		*/
		// int CDECL lame_get_totalframes(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_totalframes(IntPtr context);

		/* RadioGain value. Multiplied by 10 and rounded to the nearest. */
		// int CDECL lame_get_RadioGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_RadioGain(IntPtr context);

		/* AudiophileGain value. Multipled by 10 and rounded to the nearest. */
		// int CDECL lame_get_AudiophileGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_AudiophileGain(IntPtr context);

		/* the peak sample */
		// float CDECL lame_get_PeakSample(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_PeakSample(IntPtr context);

		/* Gain change required for preventing clipping. The value is correct only if
		   peak sample searching was enabled. If negative then the waveform
		   already does not clip. The value is multiplied by 10 and rounded up. */
		// int CDECL lame_get_noclipGainChange(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_noclipGainChange(IntPtr context);

		/* user-specified scale factor required for preventing clipping. Value is
		   correct only if peak sample searching was enabled and no user-specified
		   scaling was performed. If negative then either the waveform already does
		   not clip or the value cannot be determined */
		// float CDECL lame_get_noclipScale(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_noclipScale(IntPtr context);
		#endregion

		#region Processing
		/*
	 * REQUIRED:
	 * sets more internal configuration based on data provided above.
	 * returns -1 if something failed.
	 */
		// int CDECL lame_init_params(lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_init_params(IntPtr context);

		/*
		 * input pcm data, output (maybe) mp3 frames.
		 * This routine handles all buffering, resampling and filtering for you.
		 *
		 * return code     number of bytes output in mp3buf. Can be 0
		 *                 -1:  mp3buf was too small
		 *                 -2:  malloc() problem
		 *                 -3:  lame_init_params() not called
		 *                 -4:  psycho acoustic problems
		 *
		 * The required mp3buf_size can be computed from num_samples,
		 * samplerate and encoding rate, but here is a worst case estimate:
		 *
		 * mp3buf_size in bytes = 1.25*num_samples + 7200
		 *
		 * I think a tighter bound could be:  (mt, March 2000)
		 * MPEG1:
		 *    num_samples*(bitrate/8)/samplerate + 4*1152*(bitrate/8)/samplerate + 512
		 * MPEG2:
		 *    num_samples*(bitrate/8)/samplerate + 4*576*(bitrate/8)/samplerate + 256
		 *
		 * but test first if you use that!
		 *
		 * set mp3buf_size = 0 and LAME will not check if mp3buf_size is
		 * large enough.
		 *
		 * NOTE:
		 * if gfp->num_channels=2, but gfp->mode = 3 (mono), the L & R channels
		 * will be averaged into the L channel before encoding only the L channel
		 * This will overwrite the data in buffer_l[] and buffer_r[].
		 *
		*/
		// int CDECL lame_encode_buffer (
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const short int     buffer_l [],   /* PCM data for left channel     */
		//		const short int     buffer_r [],   /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * as above, but input has L & R channel data interleaved.
		 * NOTE:
		 * num_samples = number of samples in the L (or R)
		 * channel, not the total number of samples in pcm[]
		 */
		// int CDECL lame_encode_buffer_interleaved(
		//		lame_global_flags*  gfp,           /* global context handlei        */
		//		short int           pcm[],         /* PCM data for left and right
		//											  channel, interleaved          */
		//		int                 num_samples,   /* number of samples per channel, _not_ number of samples in pcm[] */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		int                 mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] pcm,
				int num_samples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'float's.
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * short int, +/- 32768
		 */
		// int CDECL lame_encode_buffer_float(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const float         pcm_l [],      /* PCM data for left channel     */
		//		const float         pcm_r [],      /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'float's.
		 * !! NOTE: !! data must be scaled to +/- 1 full scale
		 */
		// int CDECL lame_encode_buffer_ieee_float(
		//		lame_t          gfp,
		//		const float     pcm_l [],          /* PCM data for left channel     */
		//		const float     pcm_r [],          /* PCM data for right channel    */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_ieee_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		// int CDECL lame_encode_buffer_interleaved_ieee_float(
		//		lame_t          gfp,
		//		const float     pcm[],             /* PCM data for left and right channel, interleaved */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved_ieee_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
			
				float[] pcm,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'double's.
		 * !! NOTE: !! data must be scaled to +/- 1 full scale
		 */
		// int CDECL lame_encode_buffer_ieee_double(
		//		lame_t          gfp,
		//		const double    pcm_l [],          /* PCM data for left channel     */
		//		const double    pcm_r [],          /* PCM data for right channel    */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_ieee_double(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]			
				double[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]
				double[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		// int CDECL lame_encode_buffer_interleaved_ieee_double(
		//		lame_t          gfp,
		//		const double    pcm[],             /* PCM data for left and right channel, interleaved */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved_ieee_double(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]
				double[] pcm,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for long's
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * short int, +/- 32768
		 *
		 * This scaling was a mistake (doesn't allow one to exploit full
		 * precision of type 'long'.  Use lame_encode_buffer_long2() instead.
		 *
		 */
		// int CDECL lame_encode_buffer_long(
		//		lame_global_flags*  gfp,			/* global context handle         */
		//		const long     buffer_l [],			/* PCM data for left channel     */
		//		const long     buffer_r [],			/* PCM data for right channel    */
		//		const int           nsamples,		/* number of samples per channel */
		//		unsigned char*      mp3buf,			/* pointer to encoded MP3 stream */
		//		const int           mp3buf_size );	/* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_long(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* Same as lame_encode_buffer_long(), but with correct scaling.
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * type 'long'.   Data should be in the range:  +/- 2^(8*size(long)-1)
		 *
		 */
		// int CDECL lame_encode_buffer_long2(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const long     buffer_l [],       /* PCM data for left channel     */
		//		const long     buffer_r [],       /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_long2(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for int's
		 * !! NOTE: !! input should be scaled to the maximum range of 'int'
		 * If int is 4 bytes, then the values should range from
		 * +/- 2147483648.
		 *
		 * This routine does not (and cannot, without loosing precision) use
		 * the same scaling as the rest of the lame_encode_buffer() routines.
		 *
		 */
		// int CDECL lame_encode_buffer_int(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const int      buffer_l [],       /* PCM data for left channel     */
		//		const int      buffer_r [],       /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_int(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)]
				int[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)]
				int[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * REQUIRED:
		 * lame_encode_flush will flush the intenal PCM buffers, padding with
		 * 0's to make sure the final frame is complete, and then flush
		 * the internal MP3 buffers, and thus may return a
		 * final few mp3 frames.  'mp3buf' should be at least 7200 bytes long
		 * to hold all possible emitted data.
		 *
		 * will also write id3v1 tags (if any) into the bitstream
		 *
		 * return code = number of bytes output to mp3buf. Can be 0
		 */
		// int CDECL lame_encode_flush(
		//		lame_global_flags *  gfp,    /* global context handle                 */
		//		unsigned char*       mp3buf, /* pointer to encoded MP3 stream         */
		//		int                  size);  /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_flush(IntPtr context,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * OPTIONAL:
		 * lame_encode_flush_nogap will flush the internal mp3 buffers and pad
		 * the last frame with ancillary data so it is a complete mp3 frame.
		 *
		 * 'mp3buf' should be at least 7200 bytes long
		 * to hold all possible emitted data.
		 *
		 * After a call to this routine, the outputed mp3 data is complete, but
		 * you may continue to encode new PCM samples and write future mp3 data
		 * to a different file.  The two mp3 files will play back with no gaps
		 * if they are concatenated together.
		 *
		 * This routine will NOT write id3v1 tags into the bitstream.
		 *
		 * return code = number of bytes output to mp3buf. Can be 0
		 */
		// int CDECL lame_encode_flush_nogap(
		//		lame_global_flags *  gfp,    /* global context handle                 */
		//		unsigned char*       mp3buf, /* pointer to encoded MP3 stream         */
		//		int                  size);  /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_flush_nogap(IntPtr context,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);


		/*
		 * OPTIONAL:
		 * Normally, this is called by lame_init_params().  It writes id3v2 and
		 * Xing headers into the front of the bitstream, and sets frame counters
		 * and bitrate histogram data to 0.  You can also call this after
		 * lame_encode_flush_nogap().
		 */
		//int CDECL lame_init_bitstream(
		//		lame_global_flags *  gfp);    /* global context handle                 */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_init_bitstream(IntPtr context);

		#endregion

		#region Reporting callbacks
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_errorf(IntPtr context, delReportFunction fn);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_debugf(IntPtr context, delReportFunction fn);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_msgf(IntPtr context, delReportFunction fn);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_print_config(IntPtr context);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_print_internals(IntPtr context);


		#endregion

		#region 'printf' support for reporting functions
		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false)]
		internal static extern int _vsnprintf_s(
			[In, Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder str,
			int sizeOfBuffer,
			int count,
			[In, MarshalAs(UnmanagedType.LPStr)] String format,
			[In] IntPtr va_args);

		internal static string printf(string format, IntPtr va_args)
		{
			StringBuilder sb = new StringBuilder(4096);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
			int res = _vsnprintf_s(sb, sb.Capacity, sb.Capacity - 2, format.Replace("\t", "\xFF"), va_args);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			return sb.ToString().Replace("\xFF", "\t");
		}
		#endregion


		#region Decoding
		/// <summary>required call to initialize decoder</summary>
		/// <returns>Decoder context handle</returns>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr hip_decode_init();

		/// <summary>cleanup call to exit decoder</summary>
		/// <param name="decContext">Decoder context</param>
		/// <returns></returns>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode_exit(IntPtr decContext);

		/// <summary>Hip reporting function: error</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_errorf(IntPtr decContext, delReportFunction f);

		/// <summary>Hip reporting function: debug</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_debugf(IntPtr decContext, delReportFunction f);

		/// <summary>Hip reporting function: message</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_msgf(IntPtr decContext, delReportFunction f);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r);

		// Same as hip_decode, and also returns mp3 header data
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode_headers(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data);

		// Same as hip_decode, but returns at most 1 frame
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode1(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r);

		// Same as hip_decode1, but returns at most 1 frame and mp3 header data
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode1_headers(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data);

		// Same as hip_decode1_headers but also returns enc_delay and enc_padding 
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data, out int enc_delay, out int enc_padding);

		#endregion

		#region ID3 tag support
		// utility to obtain alphabetically sorted list of genre names with numbers
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_genre_list(delGenreCallback handler, IntPtr cookie);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_init(IntPtr context);

		// force addition of ID3v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_add_v2(IntPtr context);

		// add only a v1 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_v1_only(IntPtr context);

		// add only a v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_v2_only(IntPtr context);

		// pad version 1 tag with spaces instead of nulls
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_space_v1(IntPtr context);

		// pad version 2 tag with extra 128 bytes
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_pad_v2(IntPtr context);

		// pad version 2 tag with extra n bytes
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_pad(IntPtr context, int n);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_title(IntPtr context, string title);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_artist(IntPtr context, string artist);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_album(IntPtr context, string album);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_year(IntPtr context, string year);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_comment(IntPtr context, string comment);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_comment_utf16(IntPtr context, [MarshalAs(UnmanagedType.LPStr)]string lang, byte[] description, byte[] text);

		// return -1 result if track number is out of ID3v1 range and ignored for ID3v1
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_track(IntPtr context, string track);

		// return non-zero result if genre name or number is invalid
		// result 0: OK
		// result -1: genre number out of range
		// result -2: no valid ID3v1 genre name, mapped to ID3v1 'Other'
		//            but taken as-is for ID3v2 genre tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_genre(IntPtr context, string genre);

		// return non-zero result if field name is invalid
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_fieldvalue(IntPtr context, string value);

		// experimental
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_fieldvalue_utf16(IntPtr context, byte[] value);

		// return non-zero result if image type is invalid
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_albumart(IntPtr context, [In]byte[] image, int size);

		// lame_get_id3v1_tag copies ID3v1 tag into buffer.
		// Function returns number of bytes copied into buffer, or number
		// of bytes required if 'size' is too small.
		// Function fails, if returned value is larger than 'size'
		// NOTE:
		// This function does nothing, if user/LAME disabled ID3v1 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_id3v1_tag(IntPtr context, [In, Out]byte[] buffer, int size);

		// lame_get_id3v2_tag copies ID3v2 tag into buffer.
		// Function returns number of bytes copied into buffer, or number
		// of bytes required if 'size' is too small.
		// Function fails, if returned value is larger than 'size'
		// NOTE:
		// This function does nothing, if user/LAME disabled ID3v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_id3v2_tag(IntPtr context, [In, Out]byte[] buffer, int size);

		// normally lame_init_param writes ID3v2 tags into the audio stream
		// Call lame_set_write_id3tag_automatic(gfp, 0) before lame_init_param 
		// to turn off this behaviour and get ID3v2 tag with above function 
		// write it yourself into your file.
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_set_write_id3tag_automatic(IntPtr context, bool value);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_write_id3tag_automatic(IntPtr context);
		#endregion

#pragma warning restore IDE1006 // Naming Styles
	}
	internal static class NativeMethods64
	{
		const string libname = @"libmp3lame.64.dll";

		static NativeMethods64()
		{
			if (!System.IO.File.Exists(libname))
			{
				try
				{
					System.IO.File.WriteAllBytes(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libname), NAudio.Lame.Properties.Resources.libmp3lame_64);
				}
				catch (Exception ex)
				{
					throw new DllNotFoundException($"Could not load {libname}", ex);
				}
			}
		}

#pragma warning disable IDE1006 // Naming Styles

		public static string BoundDLL { get; } = libname;

		#region Startup/Shutdown

		/*
		 * REQUIRED:
		 * initialize the encoder.  sets default for all encoder parameters,
		 * returns NULL if some malloc()'s failed
		 * otherwise returns pointer to structure needed for all future
		 * API calls.
		 */
		// lame_global_flags * CDECL lame_init(void);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lame_init();

		/*
		 * REQUIRED:
		 * final call to free all remaining buffers
		 */
		// int  CDECL lame_close (lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_close(IntPtr context);


		/*
		 * OPTIONAL:
		 * lame_get_lametag_frame copies the final LAME-tag into 'buffer'.
		 * The function returns the number of bytes copied into buffer, or
		 * the required buffer size, if the provided buffer is too small.
		 * Function failed, if the return value is larger than 'size'!
		 * Make sure lame_encode flush has been called before calling this function.
		 * NOTE:
		 * if VBR  tags are turned off by the user, or turned off by LAME,
		 * this call does nothing and returns 0.
		 * NOTE:
		 * LAME inserted an empty frame in the beginning of mp3 audio data,
		 * which you have to replace by the final LAME-tag frame after encoding.
		 * In case there is no ID3v2 tag, usually this frame will be the very first
		 * data in your mp3 file. If you put some other leading data into your
		 * file, you'll have to do some bookkeeping about where to write this buffer.
		 */
		// int size_t CDECL lame_get_lametag_frame(const lame_global_flags *, unsigned char* buffer, size_t size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern size_t lame_get_lametag_frame(
			IntPtr context,
			[In, Out] byte[] buffer,
			[In] size_t size
			);

		#endregion

		#region LAME information
		/*
		 * OPTIONAL:
		 * get the version number, in a string. of the form:
		 * "3.63 (beta)" or just "3.63".
		 */
		// const char*  CDECL get_lame_version       ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr get_lame_version();

		// const char*  CDECL get_lame_short_version ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_short_version();

		// const char*  CDECL get_lame_very_short_version ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_very_short_version();

		// const char*  CDECL get_psy_version        ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_psy_version();

		// const char*  CDECL get_lame_url           ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_url();

		// const char*  CDECL get_lame_os_bitness    ( void );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern string get_lame_os_bitness();

		// void CDECL get_lame_version_numerical(lame_version_t *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void get_lame_version_numerical([Out]LAMEVersion ver);
		#endregion

		#region Input Stream Description
		/* number of samples.  default = 2^32-1   */
		// int CDECL lame_set_num_samples(lame_global_flags *, unsigned long);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_num_samples(IntPtr context, UInt64 num_samples);
		// unsigned long CDECL lame_get_num_samples(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt64 lame_get_num_samples(IntPtr context);

		/* input sample rate in Hz.  default = 44100hz */
		// int CDECL lame_set_in_samplerate(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_in_samplerate(IntPtr context, int value);
		// int CDECL lame_get_in_samplerate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_in_samplerate(IntPtr context);

		/* number of channels in input stream. default=2  */
		//int CDECL lame_set_num_channels(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_num_channels(IntPtr context, int value);
		//int CDECL lame_get_num_channels(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_num_channels(IntPtr context);

		/*
		  scale the input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		//int CDECL lame_set_scale(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale(IntPtr context, float value);
		//float CDECL lame_get_scale(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale(IntPtr context);

		/*
		  scale the channel 0 (left) input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_scale_left(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale_left(IntPtr context, float value);
		// float CDECL lame_get_scale_left(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale_left(IntPtr context);

		/*
		  scale the channel 1 (right) input by this amount before encoding.  default=1
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_scale_right(lame_global_flags *, float);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_scale_right(IntPtr context, float value);
		// float CDECL lame_get_scale_right(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_scale_right(IntPtr context);

		/*
		  output sample rate in Hz.  default = 0, which means LAME picks best value
		  based on the amount of compression.  MPEG only allows:
		  MPEG1    32, 44.1,   48khz
		  MPEG2    16, 22.05,  24
		  MPEG2.5   8, 11.025, 12
		  (not used by decoding routines)
		*/
		// int CDECL lame_set_out_samplerate(lame_global_flags *, int);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_out_samplerate(IntPtr context, int value);
		// int CDECL lame_get_out_samplerate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_out_samplerate(IntPtr context);

		#endregion

		#region General control parameters

		/* 1=cause LAME to collect data for an MP3 frame analyzer. default=0 */
		// int CDECL lame_set_analysis(lame_global_flags *, int);
		// int CDECL lame_get_analysis(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_analysis(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_analysis(IntPtr context);

		/*
		  1 = write a Xing VBR header frame.
		  default = 1
		  this variable must have been added by a Hungarian notation Windows programmer :-)
		*/
		// int CDECL lame_set_bWriteVbrTag(lame_global_flags *, int);
		// int CDECL lame_get_bWriteVbrTag(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_bWriteVbrTag(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool lame_get_bWriteVbrTag(IntPtr context);

		/* 1=decode only.  use lame/mpglib to convert mp3/ogg to wav.  default=0 */
		// int CDECL lame_set_decode_only(lame_global_flags *, int);
		// int CDECL lame_get_decode_only(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_decode_only(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool lame_get_decode_only(IntPtr context);

		/*
		  internal algorithm selection.  True quality is determined by the bitrate
		  but this variable will effect quality by selecting expensive or cheap algorithms.
		  quality=0..9.  0=best (very slow).  9=worst.
		  recommended:  2     near-best quality, not too slow
						5     good quality, fast
						7     ok quality, really fast
		*/
		// int CDECL lame_set_quality(lame_global_flags *, int);
		// int CDECL lame_get_quality(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quality(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quality(IntPtr context);

		/*
		  mode = 0,1,2,3 = stereo, jstereo, dual channel (not supported), mono
		  default: lame picks based on compression ration and input channels
		*/
		// int CDECL lame_set_mode(lame_global_flags *, MPEG_mode);
		// MPEG_mode CDECL lame_get_mode(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_mode(IntPtr context, MPEGMode value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern MPEGMode lame_get_mode(IntPtr context);

		/*
		  force_ms.  Force M/S for all frames.  For testing only.
		  default = 0 (disabled)
		*/
		// int CDECL lame_set_force_ms(lame_global_flags *, int);
		// int CDECL lame_get_force_ms(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_force_ms(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_force_ms(IntPtr context);

		/* use free_format?  default = 0 (disabled) */
		// int CDECL lame_set_free_format(lame_global_flags *, int);
		// int CDECL lame_get_free_format(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_free_format(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_free_format(IntPtr context);

		/* perform ReplayGain analysis?  default = 0 (disabled) */
		// int CDECL lame_set_findReplayGain(lame_global_flags *, int);
		// int CDECL lame_get_findReplayGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_findReplayGain(IntPtr context, [In, MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_findReplayGain(IntPtr context);

		/* decode on the fly. Search for the peak sample. If the ReplayGain
		 * analysis is enabled then perform the analysis on the decoded data
		 * stream. default = 0 (disabled)
		 * NOTE: if this option is set the build-in decoder should not be used */
		// int CDECL lame_set_decode_on_the_fly(lame_global_flags *, int);
		// int CDECL lame_get_decode_on_the_fly(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_decode_on_the_fly(IntPtr context, [In, MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_decode_on_the_fly(IntPtr context);

		/* counters for gapless encoding */
		// int CDECL lame_set_nogap_total(lame_global_flags*, int);
		// int CDECL lame_get_nogap_total(const lame_global_flags*);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_nogap_total(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_nogap_total(IntPtr context);

		// int CDECL lame_set_nogap_currentindex(lame_global_flags* , int);
		// int CDECL lame_get_nogap_currentindex(const lame_global_flags*);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_nogap_currentindex(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_nogap_currentindex(IntPtr context);

		/* set one of brate compression ratio.  default is compression ratio of 11.  */
		// int CDECL lame_set_brate(lame_global_flags *, int);
		// int CDECL lame_get_brate(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_brate(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_brate(IntPtr context);

		// int CDECL lame_set_compression_ratio(lame_global_flags *, float);
		// float CDECL lame_get_compression_ratio(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_compression_ratio(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_compression_ratio(IntPtr context);

		//int CDECL lame_set_preset( lame_global_flags*  gfp, int );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_preset(IntPtr context, LAMEPreset value);

		//int CDECL lame_set_asm_optimizations( lame_global_flags*  gfp, int, int );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_asm_optimizations(IntPtr context, ASMOptimizations opt, [MarshalAs(UnmanagedType.Bool)] bool val);

		#endregion

		#region Frame parameters
		/* mark as copyright.  default=0 */
		// int CDECL lame_set_copyright(lame_global_flags *, int);
		// int CDECL lame_get_copyright(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_copyright(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_copyright(IntPtr context);

		/* mark as original.  default=1 */
		// int CDECL lame_set_original(lame_global_flags *, int);
		// int CDECL lame_get_original(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_original(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_original(IntPtr context);

		/* error_protection.  Use 2 bytes from each frame for CRC checksum. default=0 */
		// int CDECL lame_set_error_protection(lame_global_flags *, int);
		// int CDECL lame_get_error_protection(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_error_protection(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_error_protection(IntPtr context);

		/* MP3 'private extension' bit  Meaningless.  default=0 */
		// int CDECL lame_set_extension(lame_global_flags *, int);
		// int CDECL lame_get_extension(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_extension(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_extension(IntPtr context);

		/* enforce strict ISO compliance.  default=0 */
		// int CDECL lame_set_strict_ISO(lame_global_flags *, int);
		// int CDECL lame_get_strict_ISO(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_strict_ISO(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_strict_ISO(IntPtr context);

		#endregion

		#region Quantization/Noise Shaping
		/* disable the bit reservoir. For testing only. default=0 */
		// int CDECL lame_set_disable_reservoir(lame_global_flags *, int);
		// int CDECL lame_get_disable_reservoir(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_disable_reservoir(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_disable_reservoir(IntPtr context);

		/* select a different "best quantization" function. default=0  */
		// int CDECL lame_set_quant_comp(lame_global_flags *, int);
		// int CDECL lame_get_quant_comp(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quant_comp(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quant_comp(IntPtr context);

		// int CDECL lame_set_quant_comp_short(lame_global_flags *, int);
		// int CDECL lame_get_quant_comp_short(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_quant_comp_short(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_quant_comp_short(IntPtr context);

		// int CDECL lame_set_experimentalX(lame_global_flags *, int); /* compatibility*/
		// int CDECL lame_get_experimentalX(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalX(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalX(IntPtr context);

		/* another experimental option.  for testing only */
		// int CDECL lame_set_experimentalY(lame_global_flags *, int);
		// int CDECL lame_get_experimentalY(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalY(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalY(IntPtr context);

		/* another experimental option.  for testing only */
		// int CDECL lame_set_experimentalZ(lame_global_flags *, int);
		// int CDECL lame_get_experimentalZ(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_experimentalZ(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_experimentalZ(IntPtr context);

		/* Naoki's psycho acoustic model.  default=0 */
		// int CDECL lame_set_exp_nspsytune(lame_global_flags *, int);
		// int CDECL lame_get_exp_nspsytune(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_exp_nspsytune(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_exp_nspsytune(IntPtr context);

		// void CDECL lame_set_msfix(lame_global_flags *, double);
		// float CDECL lame_get_msfix(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_msfix(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_msfix(IntPtr context);

		#endregion

		#region VBR control
		/* Types of VBR.  default = vbr_off = CBR */
		// int CDECL lame_set_VBR(lame_global_flags *, vbr_mode);
		// vbr_mode CDECL lame_get_VBR(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR(IntPtr context, VBRMode value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern VBRMode lame_get_VBR(IntPtr context);

		/* VBR quality level.  0=highest  9=lowest  */
		// int CDECL lame_set_VBR_q(lame_global_flags *, int);
		// int CDECL lame_get_VBR_q(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_q(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_q(IntPtr context);

		/* VBR quality level.  0=highest  9=lowest, Range [0,...,10[  */
		// int CDECL lame_set_VBR_quality(lame_global_flags *, float);
		// float CDECL lame_get_VBR_quality(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_quality(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_VBR_quality(IntPtr context);

		/* Ignored except for VBR=vbr_abr (ABR mode) */
		// int CDECL lame_set_VBR_mean_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_mean_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_mean_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_mean_bitrate_kbps(IntPtr context);

		// int CDECL lame_set_VBR_min_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_min_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_min_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_min_bitrate_kbps(IntPtr context);

		// int CDECL lame_set_VBR_max_bitrate_kbps(lame_global_flags *, int);
		// int CDECL lame_get_VBR_max_bitrate_kbps(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_max_bitrate_kbps(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_VBR_max_bitrate_kbps(IntPtr context);

		/*
		  1=strictly enforce VBR_min_bitrate.  Normally it will be violated for
		  analog silence
		*/
		// int CDECL lame_set_VBR_hard_min(lame_global_flags *, int);
		// int CDECL lame_get_VBR_hard_min(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_VBR_hard_min(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_VBR_hard_min(IntPtr context);

		#endregion

		#region Filtering control
		/* freq in Hz to apply lowpass. Default = 0 = lame chooses.  -1 = disabled */
		// int CDECL lame_set_lowpassfreq(lame_global_flags *, int);
		// int CDECL lame_get_lowpassfreq(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_lowpassfreq(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_lowpassfreq(IntPtr context);

		/* width of transition band, in Hz.  Default = one polyphase filter band */
		// int CDECL lame_set_lowpasswidth(lame_global_flags *, int);
		// int CDECL lame_get_lowpasswidth(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_lowpasswidth(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_lowpasswidth(IntPtr context);

		/* freq in Hz to apply highpass. Default = 0 = lame chooses.  -1 = disabled */
		// int CDECL lame_set_highpassfreq(lame_global_flags *, int);
		// int CDECL lame_get_highpassfreq(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_highpassfreq(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_highpassfreq(IntPtr context);

		/* width of transition band, in Hz.  Default = one polyphase filter band */
		// int CDECL lame_set_highpasswidth(lame_global_flags *, int);
		// int CDECL lame_get_highpasswidth(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_highpasswidth(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_highpasswidth(IntPtr context);

		#endregion

		#region Psychoacoustics and other advanced settings
		/* only use ATH for masking */
		// int CDECL lame_set_ATHonly(lame_global_flags *, int);
		// int CDECL lame_get_ATHonly(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHonly(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_ATHonly(IntPtr context);

		/* only use ATH for short blocks */
		// int CDECL lame_set_ATHshort(lame_global_flags *, int);
		// int CDECL lame_get_ATHshort(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHshort(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_ATHshort(IntPtr context);

		/* disable ATH */
		// int CDECL lame_set_noATH(lame_global_flags *, int);
		// int CDECL lame_get_noATH(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_noATH(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_noATH(IntPtr context);

		/* select ATH formula */
		// int CDECL lame_set_ATHtype(lame_global_flags *, int);
		// int CDECL lame_get_ATHtype(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHtype(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_ATHtype(IntPtr context);

		/* lower ATH by this many db */
		// int CDECL lame_set_ATHlower(lame_global_flags *, float);
		// float CDECL lame_get_ATHlower(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_ATHlower(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_ATHlower(IntPtr context);

		/* select ATH adaptive adjustment type */
		// int CDECL lame_set_athaa_type( lame_global_flags *, int);
		// int CDECL lame_get_athaa_type( const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_athaa_type(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_athaa_type(IntPtr context);

		/* adjust (in dB) the point below which adaptive ATH level adjustment occurs */
		// int CDECL lame_set_athaa_sensitivity( lame_global_flags *, float);
		// float CDECL lame_get_athaa_sensitivity( const lame_global_flags* );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_athaa_sensitivity(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_athaa_sensitivity(IntPtr context);

		/*
		  allow blocktypes to differ between channels?
		  default: 0 for jstereo, 1 for stereo
		*/
		// int CDECL lame_set_allow_diff_short(lame_global_flags *, int);
		// int CDECL lame_get_allow_diff_short(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_allow_diff_short(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_allow_diff_short(IntPtr context);

		/* use temporal masking effect (default = 1) */
		// int CDECL lame_set_useTemporal(lame_global_flags *, int);
		// int CDECL lame_get_useTemporal(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_useTemporal(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_useTemporal(IntPtr context);

		/* use temporal masking effect (default = 1) */
		// int CDECL lame_set_interChRatio(lame_global_flags *, float);
		// float CDECL lame_get_interChRatio(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_interChRatio(IntPtr context, float value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_interChRatio(IntPtr context);

		/* disable short blocks */
		// int CDECL lame_set_no_short_blocks(lame_global_flags *, int);
		// int CDECL lame_get_no_short_blocks(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_no_short_blocks(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_no_short_blocks(IntPtr context);

		/* force short blocks */
		// int CDECL lame_set_force_short_blocks(lame_global_flags *, int);
		// int CDECL lame_get_force_short_blocks(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_force_short_blocks(IntPtr context, [MarshalAs(UnmanagedType.Bool)] bool value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_force_short_blocks(IntPtr context);

		/* Input PCM is emphased PCM (for instance from one of the rarely
		   emphased CDs), it is STRONGLY not recommended to use this, because
		   psycho does not take it into account, and last but not least many decoders
		   ignore these bits */
		// int CDECL lame_set_emphasis(lame_global_flags *, int);
		// int CDECL lame_get_emphasis(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_emphasis(IntPtr context, int value);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_emphasis(IntPtr context);
		#endregion

		#region Internal state variables, read only
		/* version  0=MPEG-2  1=MPEG-1  (2=MPEG-2.5)     */
		// int CDECL lame_get_version(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern MPEGVersion lame_get_version(IntPtr context);

		/* encoder delay   */
		// int CDECL lame_get_encoder_delay(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_encoder_delay(IntPtr context);

		/*
		  padding appended to the input to make sure decoder can fully decode
		  all input.  Note that this value can only be calculated during the
		  call to lame_encoder_flush().  Before lame_encoder_flush() has
		  been called, the value of encoder_padding = 0.
		*/
		// int CDECL lame_get_encoder_padding(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_encoder_padding(IntPtr context);

		/* size of MPEG frame */
		// int CDECL lame_get_framesize(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_framesize(IntPtr context);

		/* number of PCM samples buffered, but not yet encoded to mp3 data. */
		// int CDECL lame_get_mf_samples_to_encode( const lame_global_flags*  gfp );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_mf_samples_to_encode(IntPtr context);

		/*
		  size (bytes) of mp3 data buffered, but not yet encoded.
		  this is the number of bytes which would be output by a call to
		  lame_encode_flush_nogap.  NOTE: lame_encode_flush() will return
		  more bytes than this because it will encode the reamining buffered
		  PCM samples before flushing the mp3 buffers.
		*/
		// int CDECL lame_get_size_mp3buffer( const lame_global_flags*  gfp );
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_size_mp3buffer(IntPtr context);

		/* number of frames encoded so far */
		// int CDECL lame_get_frameNum(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_frameNum(IntPtr context);

		/*
		  lame's estimate of the total number of frames to be encoded
		   only valid if calling program set num_samples
		*/
		// int CDECL lame_get_totalframes(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_totalframes(IntPtr context);

		/* RadioGain value. Multiplied by 10 and rounded to the nearest. */
		// int CDECL lame_get_RadioGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_RadioGain(IntPtr context);

		/* AudiophileGain value. Multipled by 10 and rounded to the nearest. */
		// int CDECL lame_get_AudiophileGain(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_AudiophileGain(IntPtr context);

		/* the peak sample */
		// float CDECL lame_get_PeakSample(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_PeakSample(IntPtr context);

		/* Gain change required for preventing clipping. The value is correct only if
		   peak sample searching was enabled. If negative then the waveform
		   already does not clip. The value is multiplied by 10 and rounded up. */
		// int CDECL lame_get_noclipGainChange(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_noclipGainChange(IntPtr context);

		/* user-specified scale factor required for preventing clipping. Value is
		   correct only if peak sample searching was enabled and no user-specified
		   scaling was performed. If negative then either the waveform already does
		   not clip or the value cannot be determined */
		// float CDECL lame_get_noclipScale(const lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern float lame_get_noclipScale(IntPtr context);
		#endregion

		#region Processing
		/*
	 * REQUIRED:
	 * sets more internal configuration based on data provided above.
	 * returns -1 if something failed.
	 */
		// int CDECL lame_init_params(lame_global_flags *);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_init_params(IntPtr context);

		/*
		 * input pcm data, output (maybe) mp3 frames.
		 * This routine handles all buffering, resampling and filtering for you.
		 *
		 * return code     number of bytes output in mp3buf. Can be 0
		 *                 -1:  mp3buf was too small
		 *                 -2:  malloc() problem
		 *                 -3:  lame_init_params() not called
		 *                 -4:  psycho acoustic problems
		 *
		 * The required mp3buf_size can be computed from num_samples,
		 * samplerate and encoding rate, but here is a worst case estimate:
		 *
		 * mp3buf_size in bytes = 1.25*num_samples + 7200
		 *
		 * I think a tighter bound could be:  (mt, March 2000)
		 * MPEG1:
		 *    num_samples*(bitrate/8)/samplerate + 4*1152*(bitrate/8)/samplerate + 512
		 * MPEG2:
		 *    num_samples*(bitrate/8)/samplerate + 4*576*(bitrate/8)/samplerate + 256
		 *
		 * but test first if you use that!
		 *
		 * set mp3buf_size = 0 and LAME will not check if mp3buf_size is
		 * large enough.
		 *
		 * NOTE:
		 * if gfp->num_channels=2, but gfp->mode = 3 (mono), the L & R channels
		 * will be averaged into the L channel before encoding only the L channel
		 * This will overwrite the data in buffer_l[] and buffer_r[].
		 *
		*/
		// int CDECL lame_encode_buffer (
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const short int     buffer_l [],   /* PCM data for left channel     */
		//		const short int     buffer_r [],   /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * as above, but input has L & R channel data interleaved.
		 * NOTE:
		 * num_samples = number of samples in the L (or R)
		 * channel, not the total number of samples in pcm[]
		 */
		// int CDECL lame_encode_buffer_interleaved(
		//		lame_global_flags*  gfp,           /* global context handlei        */
		//		short int           pcm[],         /* PCM data for left and right
		//											  channel, interleaved          */
		//		int                 num_samples,   /* number of samples per channel, _not_ number of samples in pcm[] */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		int                 mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)]
				short[] pcm,
				int num_samples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'float's.
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * short int, +/- 32768
		 */
		// int CDECL lame_encode_buffer_float(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const float         pcm_l [],      /* PCM data for left channel     */
		//		const float         pcm_r [],      /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'float's.
		 * !! NOTE: !! data must be scaled to +/- 1 full scale
		 */
		// int CDECL lame_encode_buffer_ieee_float(
		//		lame_t          gfp,
		//		const float     pcm_l [],          /* PCM data for left channel     */
		//		const float     pcm_r [],          /* PCM data for right channel    */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_ieee_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
				float[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		// int CDECL lame_encode_buffer_interleaved_ieee_float(
		//		lame_t          gfp,
		//		const float     pcm[],             /* PCM data for left and right channel, interleaved */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved_ieee_float(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
			
				float[] pcm,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for 'double's.
		 * !! NOTE: !! data must be scaled to +/- 1 full scale
		 */
		// int CDECL lame_encode_buffer_ieee_double(
		//		lame_t          gfp,
		//		const double    pcm_l [],          /* PCM data for left channel     */
		//		const double    pcm_r [],          /* PCM data for right channel    */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_ieee_double(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]			
				double[] pcm_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]
				double[] pcm_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		// int CDECL lame_encode_buffer_interleaved_ieee_double(
		//		lame_t          gfp,
		//		const double    pcm[],             /* PCM data for left and right channel, interleaved */
		//		const int       nsamples,
		//		unsigned char * mp3buf,
		//		const int       mp3buf_size);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_interleaved_ieee_double(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)]
				double[] pcm,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for long's
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * short int, +/- 32768
		 *
		 * This scaling was a mistake (doesn't allow one to exploit full
		 * precision of type 'long'.  Use lame_encode_buffer_long2() instead.
		 *
		 */
		// int CDECL lame_encode_buffer_long(
		//		lame_global_flags*  gfp,			/* global context handle         */
		//		const long     buffer_l [],			/* PCM data for left channel     */
		//		const long     buffer_r [],			/* PCM data for right channel    */
		//		const int           nsamples,		/* number of samples per channel */
		//		unsigned char*      mp3buf,			/* pointer to encoded MP3 stream */
		//		const int           mp3buf_size );	/* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_long(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* Same as lame_encode_buffer_long(), but with correct scaling.
		 * !! NOTE: !! data must still be scaled to be in the same range as
		 * type 'long'.   Data should be in the range:  +/- 2^(8*size(long)-1)
		 *
		 */
		// int CDECL lame_encode_buffer_long2(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const long     buffer_l [],       /* PCM data for left channel     */
		//		const long     buffer_r [],       /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_long2(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)]
				long[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/* as lame_encode_buffer, but for int's
		 * !! NOTE: !! input should be scaled to the maximum range of 'int'
		 * If int is 4 bytes, then the values should range from
		 * +/- 2147483648.
		 *
		 * This routine does not (and cannot, without loosing precision) use
		 * the same scaling as the rest of the lame_encode_buffer() routines.
		 *
		 */
		// int CDECL lame_encode_buffer_int(
		//		lame_global_flags*  gfp,           /* global context handle         */
		//		const int      buffer_l [],       /* PCM data for left channel     */
		//		const int      buffer_r [],       /* PCM data for right channel    */
		//		const int           nsamples,      /* number of samples per channel */
		//		unsigned char*      mp3buf,        /* pointer to encoded MP3 stream */
		//		const int           mp3buf_size ); /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_buffer_int(IntPtr context,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)]
				int[] buffer_l,
			[In]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)]
				int[] buffer_r,
				int nSamples,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * REQUIRED:
		 * lame_encode_flush will flush the intenal PCM buffers, padding with
		 * 0's to make sure the final frame is complete, and then flush
		 * the internal MP3 buffers, and thus may return a
		 * final few mp3 frames.  'mp3buf' should be at least 7200 bytes long
		 * to hold all possible emitted data.
		 *
		 * will also write id3v1 tags (if any) into the bitstream
		 *
		 * return code = number of bytes output to mp3buf. Can be 0
		 */
		// int CDECL lame_encode_flush(
		//		lame_global_flags *  gfp,    /* global context handle                 */
		//		unsigned char*       mp3buf, /* pointer to encoded MP3 stream         */
		//		int                  size);  /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_flush(IntPtr context,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);

		/*
		 * OPTIONAL:
		 * lame_encode_flush_nogap will flush the internal mp3 buffers and pad
		 * the last frame with ancillary data so it is a complete mp3 frame.
		 *
		 * 'mp3buf' should be at least 7200 bytes long
		 * to hold all possible emitted data.
		 *
		 * After a call to this routine, the outputed mp3 data is complete, but
		 * you may continue to encode new PCM samples and write future mp3 data
		 * to a different file.  The two mp3 files will play back with no gaps
		 * if they are concatenated together.
		 *
		 * This routine will NOT write id3v1 tags into the bitstream.
		 *
		 * return code = number of bytes output to mp3buf. Can be 0
		 */
		// int CDECL lame_encode_flush_nogap(
		//		lame_global_flags *  gfp,    /* global context handle                 */
		//		unsigned char*       mp3buf, /* pointer to encoded MP3 stream         */
		//		int                  size);  /* number of valid octets in this stream */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_encode_flush_nogap(IntPtr context,
			[In, Out]//[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
				byte[] mp3buf,
				int mp3buf_size
			);


		/*
		 * OPTIONAL:
		 * Normally, this is called by lame_init_params().  It writes id3v2 and
		 * Xing headers into the front of the bitstream, and sets frame counters
		 * and bitrate histogram data to 0.  You can also call this after
		 * lame_encode_flush_nogap().
		 */
		//int CDECL lame_init_bitstream(
		//		lame_global_flags *  gfp);    /* global context handle                 */
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_init_bitstream(IntPtr context);

		#endregion

		#region Reporting callbacks
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_errorf(IntPtr context, delReportFunction fn);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_debugf(IntPtr context, delReportFunction fn);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_set_msgf(IntPtr context, delReportFunction fn);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_print_config(IntPtr context);
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_print_internals(IntPtr context);


		#endregion

		#region 'printf' support for reporting functions
		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false)]
		internal static extern int _vsnprintf_s(
			[In, Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder str,
			int sizeOfBuffer,
			int count,
			[In, MarshalAs(UnmanagedType.LPStr)] String format,
			[In] IntPtr va_args);

		internal static string printf(string format, IntPtr va_args)
		{
			StringBuilder sb = new StringBuilder(4096);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
			int res = _vsnprintf_s(sb, sb.Capacity, sb.Capacity - 2, format.Replace("\t", "\xFF"), va_args);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			return sb.ToString().Replace("\xFF", "\t");
		}
		#endregion


		#region Decoding
		/// <summary>required call to initialize decoder</summary>
		/// <returns>Decoder context handle</returns>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr hip_decode_init();

		/// <summary>cleanup call to exit decoder</summary>
		/// <param name="decContext">Decoder context</param>
		/// <returns></returns>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode_exit(IntPtr decContext);

		/// <summary>Hip reporting function: error</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_errorf(IntPtr decContext, delReportFunction f);

		/// <summary>Hip reporting function: debug</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_debugf(IntPtr decContext, delReportFunction f);

		/// <summary>Hip reporting function: message</summary>
		/// <param name="decContext">Decoder context</param>
		/// <param name="f">Reporting function</param>
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void hip_set_msgf(IntPtr decContext, delReportFunction f);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r);

		// Same as hip_decode, and also returns mp3 header data
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode_headers(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data);

		// Same as hip_decode, but returns at most 1 frame
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode1(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r);

		// Same as hip_decode1, but returns at most 1 frame and mp3 header data
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode1_headers(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data);

		// Same as hip_decode1_headers but also returns enc_delay and enc_padding 
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int hip_decode(IntPtr decContext, [In] byte[] mp3buf, int len, [In, Out] short[] pcm_l, [In, Out] short[] pcm_r, [Out] mp3data mp3data, out int enc_delay, out int enc_padding);

		#endregion

		#region ID3 tag support
		// utility to obtain alphabetically sorted list of genre names with numbers
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_genre_list(delGenreCallback handler, IntPtr cookie);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_init(IntPtr context);

		// force addition of ID3v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_add_v2(IntPtr context);

		// add only a v1 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_v1_only(IntPtr context);

		// add only a v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_v2_only(IntPtr context);

		// pad version 1 tag with spaces instead of nulls
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_space_v1(IntPtr context);

		// pad version 2 tag with extra 128 bytes
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_pad_v2(IntPtr context);

		// pad version 2 tag with extra n bytes
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_pad(IntPtr context, int n);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_title(IntPtr context, string title);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_artist(IntPtr context, string artist);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_album(IntPtr context, string album);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void id3tag_set_year(IntPtr context, string year);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_comment(IntPtr context, string comment);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_comment_utf16(IntPtr context, [MarshalAs(UnmanagedType.LPStr)]string lang, byte[] description, byte[] text);

		// return -1 result if track number is out of ID3v1 range and ignored for ID3v1
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_track(IntPtr context, string track);

		// return non-zero result if genre name or number is invalid
		// result 0: OK
		// result -1: genre number out of range
		// result -2: no valid ID3v1 genre name, mapped to ID3v1 'Other'
		//            but taken as-is for ID3v2 genre tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_genre(IntPtr context, string genre);

		// return non-zero result if field name is invalid
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_fieldvalue(IntPtr context, string value);

		// experimental
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_fieldvalue_utf16(IntPtr context, byte[] value);

		// return non-zero result if image type is invalid
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int id3tag_set_albumart(IntPtr context, [In]byte[] image, int size);

		// lame_get_id3v1_tag copies ID3v1 tag into buffer.
		// Function returns number of bytes copied into buffer, or number
		// of bytes required if 'size' is too small.
		// Function fails, if returned value is larger than 'size'
		// NOTE:
		// This function does nothing, if user/LAME disabled ID3v1 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_id3v1_tag(IntPtr context, [In, Out]byte[] buffer, int size);

		// lame_get_id3v2_tag copies ID3v2 tag into buffer.
		// Function returns number of bytes copied into buffer, or number
		// of bytes required if 'size' is too small.
		// Function fails, if returned value is larger than 'size'
		// NOTE:
		// This function does nothing, if user/LAME disabled ID3v2 tag
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lame_get_id3v2_tag(IntPtr context, [In, Out]byte[] buffer, int size);

		// normally lame_init_param writes ID3v2 tags into the audio stream
		// Call lame_set_write_id3tag_automatic(gfp, 0) before lame_init_param 
		// to turn off this behaviour and get ID3v2 tag with above function 
		// write it yourself into your file.
		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lame_set_write_id3tag_automatic(IntPtr context, bool value);

		[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool lame_get_write_id3tag_automatic(IntPtr context);
		#endregion

#pragma warning restore IDE1006 // Naming Styles
	}
}
