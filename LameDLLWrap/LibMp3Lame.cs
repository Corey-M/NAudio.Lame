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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

#if X64
using size_t = System.UInt64;
#else
using size_t = System.UInt32;
#endif

namespace LameDLLWrap
{
	/// <summary>Delegate for receiving output messages</summary>
	/// <param name="text">Text to output</param>
	public delegate void ReportFunction(string text);

	/// <summary>Delegate for receiving ID3 genres from dll.</summary>
	/// <param name="index">Genre Index.</param>
	/// <param name="genre">Genre Name.</param>
	internal delegate void GenreCallback(int index, string genre);

	/// <summary>LAME interface class</summary>
	public class LibMp3Lame : IDisposable
	{
		/// <summary>Constructor</summary>
		public LibMp3Lame()
		{
			context = NativeMethods.lame_init();
			InitReportFunctions();
		}

		/// <summary>Destructor</summary>
		~LibMp3Lame()
		{
			Dispose(true);
		}

		/// <summary>Dispose of object</summary>
		public void Dispose()
		{
			Dispose(false);
		}

		/// <summary>Clean up object, closing LAME context if present</summary>
		/// <param name="final">True if called from destructor, else false</param>
		protected virtual void Dispose(bool final)
		{
			if (context != IntPtr.Zero)
			{
				NativeMethods.lame_close(context);
				context = IntPtr.Zero;
			}
		}

		#region LAME context handle
		private IntPtr context = IntPtr.Zero;
		#endregion

		#region DLL version data
		/// <summary>Lame Version</summary>
		public static string LameVersion => Marshal.PtrToStringAnsi(NativeMethods.get_lame_version());

		/// <summary>Lame Short Version</summary>
		public static string LameShortVersion { get { return NativeMethods.get_lame_short_version(); } }
		/// <summary>Lame Very Short Version</summary>
		public static string LameVeryShortVersion { get { return NativeMethods.get_lame_very_short_version(); } }
		/// <summary>Lame Psychoacoustic Version</summary>
		public static string LamePsychoacousticVersion { get { return NativeMethods.get_psy_version(); } }
		/// <summary>Lame URL</summary>
		public static string LameURL { get { return NativeMethods.get_lame_url(); } }
		/// <summary>Lame library bit width - 32 or 64 bit</summary>
		public static string LameOSBitness { get { return NativeMethods.get_lame_os_bitness(); } }

		/// <summary>Get LAME version information</summary>
		/// <returns>LAME version structure</returns>
		public static LAMEVersion GetLameVersion()
		{
			LAMEVersion ver = new LAMEVersion();
			NativeMethods.get_lame_version_numerical(ver);
			return ver;
		}
		#endregion

		#region Properties
		delegate int setFunc<T>(IntPtr p, T val);

		// wrapper function to simplify calling lame_set_* entry points
		void Setter<T>(setFunc<T> f, T value, string name = null)
		{
			int res = f(context, value);
			if (res != 0)
			{
				if (string.IsNullOrEmpty(name))
					name = f.Method.Name;
				throw new Exception(string.Format("libmp3lame: {0}({1}) returned error code: {2}", name, value, res));
			}
		}

		#region Input Stream Description
		/// <summary>Number of samples (optional)</summary>
		public UInt64 NumSamples
		{
			get { return NativeMethods.lame_get_num_samples(context); }
			set { Setter(NativeMethods.lame_set_num_samples, value); }
		}
		/// <summary>Input sample rate</summary>
		public int InputSampleRate
		{
			get { return NativeMethods.lame_get_in_samplerate(context); }
			set { Setter(NativeMethods.lame_set_in_samplerate, value); }
		}
		/// <summary>Number of channels</summary>
		public int NumChannels
		{
			get { return NativeMethods.lame_get_num_channels(context); }
			set { Setter(NativeMethods.lame_set_num_channels, value); }
		}
		/// <summary>Global amplification factor</summary>
		public float Scale
		{
			get { return NativeMethods.lame_get_scale(context); }
			set { Setter(NativeMethods.lame_set_scale, value); }
		}
		/// <summary>Left channel amplification</summary>
		public float ScaleLeft
		{
			get { return NativeMethods.lame_get_scale_left(context); }
			set { Setter(NativeMethods.lame_set_scale_left, value); }
		}
		/// <summary>Right channel amplification</summary>
		public float ScaleRight
		{
			get { return NativeMethods.lame_get_scale_right(context); }
			set { Setter(NativeMethods.lame_set_scale_right, value); }
		}
		/// <summary>Output sample rate</summary>
		public int OutputSampleRate
		{
			get { return NativeMethods.lame_get_out_samplerate(context); }
			set { Setter(NativeMethods.lame_set_out_samplerate, value); }
		}
		#endregion

		#region General Control Parameters
		/// <summary>Enable analysis</summary>
		public bool Analysis
		{
			get { return NativeMethods.lame_get_analysis(context); }
			set { Setter(NativeMethods.lame_set_analysis, value); }
		}
		/// <summary>Write VBR tag to MP3 file</summary>
		public bool WriteVBRTag
		{
			get { return NativeMethods.lame_get_bWriteVbrTag(context); }
			set { Setter(NativeMethods.lame_set_bWriteVbrTag, value); }
		}
		/// <summary></summary>
		public bool DecodeOnly
		{
			get { return NativeMethods.lame_get_decode_only(context); }
			set { Setter(NativeMethods.lame_set_decode_only, value); }
		}
		/// <summary>Encoding quality</summary>
		public int Quality
		{
			get { return NativeMethods.lame_get_quality(context); }
			set { Setter(NativeMethods.lame_set_quality, value); }
		}
		/// <summary>Specify MPEG channel mode, or use best guess if false</summary>
		public MPEGMode Mode
		{
			get { return NativeMethods.lame_get_mode(context); }
			set { Setter(NativeMethods.lame_set_mode, value); }
		}
		/// <summary>Force M/S mode</summary>
		public bool ForceMS
		{
			get { return NativeMethods.lame_get_force_ms(context); }
			set { Setter(NativeMethods.lame_set_force_ms, value); }
		}
		/// <summary>Use free format</summary>
		public bool UseFreeFormat
		{
			get { return NativeMethods.lame_get_free_format(context); }
			set { Setter(NativeMethods.lame_set_free_format, value); }
		}
		/// <summary>Perform replay gain analysis</summary>
		public bool FindReplayGain
		{
			get { return NativeMethods.lame_get_findReplayGain(context); }
			set { Setter(NativeMethods.lame_set_findReplayGain, value); }
		}
		/// <summary>Decode on the fly.  Search for the peak sample.  If the ReplayGain analysis is enabled then perform the analysis on the decoded data stream.</summary>
		public bool DecodeOnTheFly
		{
			get { return NativeMethods.lame_get_decode_on_the_fly(context); }
			set { Setter(NativeMethods.lame_set_decode_on_the_fly, value); }
		}
		/// <summary>Counters for gapless encoding</summary>
		public int NoGapTotal
		{
			get { return NativeMethods.lame_get_nogap_total(context); }
			set { Setter(NativeMethods.lame_set_nogap_total, value); }
		}
		/// <summary>Counters for gapless encoding</summary>
		public int NoGapCurrentIndex
		{
			get { return NativeMethods.lame_get_nogap_currentindex(context); }
			set { Setter(NativeMethods.lame_set_nogap_currentindex, value); }
		}
		/// <summary>Output bitrate</summary>
		public int BitRate
		{
			get { return NativeMethods.lame_get_brate(context); }
			set { Setter(NativeMethods.lame_set_brate, value); }
		}
		/// <summary>Output compression ratio</summary>
		public float CompressionRatio
		{
			get { return NativeMethods.lame_get_compression_ratio(context); }
			set { Setter(NativeMethods.lame_set_compression_ratio, value); }
		}

		/// <summary>Set compression preset</summary>
		public bool SetPreset(int preset)
		{
			int res = NativeMethods.lame_set_preset(context, (LAMEPreset)preset);
			return res == 0;
		}

		/// <summary>Enable/Disable optimizations</summary>
		public bool SetOptimization(ASMOptimizations opt, bool enabled)
		{
			int res = NativeMethods.lame_set_asm_optimizations(context, opt, enabled);
			return res == 0;
		}
		#endregion

		#region Frame parameters
		/// <summary>Set output Copyright flag</summary>
		public bool Copyright
		{
			get { return NativeMethods.lame_get_copyright(context); }
			set { Setter(NativeMethods.lame_set_copyright, value); }
		}
		/// <summary>Set output Original flag</summary>
		public bool Original
		{
			get { return NativeMethods.lame_get_original(context); }
			set { Setter(NativeMethods.lame_set_original, value); }
		}
		/// <summary>Set error protection.  Uses 2 bytes from each frame for CRC checksum</summary>
		public bool ErrorProtection
		{
			get { return NativeMethods.lame_get_error_protection(context); }
			set { Setter(NativeMethods.lame_set_error_protection, value); }
		}
		/// <summary>MP3 'private extension' bit.  Meaningless.</summary>
		public bool Extension
		{
			get { return NativeMethods.lame_get_extension(context); }
			set { Setter(NativeMethods.lame_set_extension, value); }
		}
		/// <summary>Enforce strict ISO compliance.</summary>
		public bool StrictISO
		{
			get { return NativeMethods.lame_get_strict_ISO(context); }
			set { Setter(NativeMethods.lame_set_strict_ISO, value); }
		}
		#endregion

		#region Quantization/Noise Shaping
		/// <summary>Disable the bit reservoir.</summary>
		public bool DisableReservoir { get { return NativeMethods.lame_get_disable_reservoir(context); } set { Setter(NativeMethods.lame_set_disable_reservoir, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int QuantComp { get { return NativeMethods.lame_get_quant_comp(context); } set { Setter(NativeMethods.lame_set_quant_comp, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int QuantCompShort { get { return NativeMethods.lame_get_quant_comp_short(context); } set { Setter(NativeMethods.lame_set_quant_comp_short, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int ExperimentalX { get { return NativeMethods.lame_get_experimentalX(context); } set { Setter(NativeMethods.lame_set_experimentalX, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int ExperimentalY { get { return NativeMethods.lame_get_experimentalY(context); } set { Setter(NativeMethods.lame_set_experimentalY, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int ExperimentalZ { get { return NativeMethods.lame_get_experimentalZ(context); } set { Setter(NativeMethods.lame_set_experimentalZ, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int ExperimentalNSPsyTune { get { return NativeMethods.lame_get_exp_nspsytune(context); } set { Setter(NativeMethods.lame_set_exp_nspsytune, value); } }
		/// <summary>Set a different "best quantization" function</summary>
		public int MSFix { get { return NativeMethods.lame_get_msfix(context); } set { Setter(NativeMethods.lame_set_msfix, value); } }
		#endregion

		#region VBR Control
		/// <summary>Set VBR mode</summary>
		public VBRMode VBR { get { return NativeMethods.lame_get_VBR(context); } set { Setter(NativeMethods.lame_set_VBR, value); } }
		/// <summary>VBR quality level.  0 = highest, 9 = lowest.</summary>
		public int VBRQualityLevel { get { return NativeMethods.lame_get_VBR_q(context); } set { Setter(NativeMethods.lame_set_VBR_q, value); } }
		/// <summary>VBR quality level.  0 = highest, 9 = lowest</summary>
		public float VBRQuality { get { return NativeMethods.lame_get_VBR_quality(context); } set { Setter(NativeMethods.lame_set_VBR_quality, value); } }

		/// <summary>ABR average bitrate</summary>
		public int VBRMeanBitrateKbps { get { return NativeMethods.lame_get_VBR_mean_bitrate_kbps(context); } set { Setter(NativeMethods.lame_set_VBR_mean_bitrate_kbps, value); } }
		/// <summary>ABR minimum bitrate</summary>
		public int VBRMinBitrateKbps { get { return NativeMethods.lame_get_VBR_min_bitrate_kbps(context); } set { Setter(NativeMethods.lame_set_VBR_min_bitrate_kbps, value); } }
		/// <summary>ABR maximum bitrate</summary>
		public int VBRMaxBitrateKbps { get { return NativeMethods.lame_get_VBR_max_bitrate_kbps(context); } set { Setter(NativeMethods.lame_set_VBR_max_bitrate_kbps, value); } }

		/// <summary>Strictly enforce minimum bitrate.  Normall it will be violated for analog silence.</summary>
		public bool VBRHardMin { get { return NativeMethods.lame_get_VBR_hard_min(context); } set { Setter(NativeMethods.lame_set_VBR_hard_min, value); } }
		#endregion

		#region Filtering control
#pragma warning disable 1591
		public int LowPassFreq { get { return NativeMethods.lame_get_lowpassfreq(context); } set { Setter(NativeMethods.lame_set_lowpassfreq, value); } }
		public int LowPassWidth { get { return NativeMethods.lame_get_lowpasswidth(context); } set { Setter(NativeMethods.lame_set_lowpasswidth, value); } }
		public int HighPassFreq { get { return NativeMethods.lame_get_highpassfreq(context); } set { Setter(NativeMethods.lame_set_highpassfreq, value); } }
		public int HighPassWidth { get { return NativeMethods.lame_get_highpasswidth(context); } set { Setter(NativeMethods.lame_set_highpasswidth, value); } }
#pragma warning restore 1591
		#endregion

		#region Internal state variables, read only
#pragma warning disable 1591
		public MPEGVersion Version { get { return NativeMethods.lame_get_version(context); } }
		public int EncoderDelay { get { return NativeMethods.lame_get_encoder_delay(context); } }
		public int EncoderPadding { get { return NativeMethods.lame_get_encoder_padding(context); } }
		public int MFSamplesToEncode { get { return NativeMethods.lame_get_mf_samples_to_encode(context); } }
		public int MP3BufferSize { get { return NativeMethods.lame_get_size_mp3buffer(context); } }
		public int FrameNumber { get { return NativeMethods.lame_get_frameNum(context); } }
		public int TotalFrames { get { return NativeMethods.lame_get_totalframes(context); } }
		public int RadioGain { get { return NativeMethods.lame_get_RadioGain(context); } }
		public int AudiophileGain { get { return NativeMethods.lame_get_AudiophileGain(context); } }
		public float PeakSample { get { return NativeMethods.lame_get_PeakSample(context); } }
		public int NoClipGainChange { get { return NativeMethods.lame_get_noclipGainChange(context); } }
		public float NoClipScale { get { return NativeMethods.lame_get_noclipScale(context); } }
#pragma warning restore 1591
		#endregion

		#endregion

		#region Methods
		/// <summary>Initialize encoder with parameters</summary>
		/// <returns>Success/fail</returns>
		public bool InitParams()
		{
			if (context == IntPtr.Zero)
				throw new InvalidOperationException("InitParams called without initializing context");
			return CheckResult(NativeMethods.lame_init_params(context));
		}

		/// <summary>Write 16-bit integer PCM samples to encoder</summary>
		/// <param name="samples">PCM sample data.  Interleaved for stereo.</param>
		/// <param name="nSamples">Number of valid samples.</param>
		/// <param name="output">Buffer to write encoded data to</param>
		/// <param name="outputSize">Size of buffer.</param>
		/// <param name="mono">True if mono, false if stereo.</param>
		/// <returns>Number of bytes of encoded data written to output buffer.</returns>
		public int Write(short[] samples, int nSamples, byte[] output, int outputSize, bool mono)
		{
			int rc;
			if (mono)
				rc = NativeMethods.lame_encode_buffer(context, samples, samples, nSamples, output, outputSize);
			else
				rc = NativeMethods.lame_encode_buffer_interleaved(context, samples, nSamples / 2, output, outputSize);

			return rc;
		}

		// float
		/// <summary>Write 32-bit floating point PCM samples to encoder</summary>
		/// <param name="samples">PCM sample data.  Interleaved for stereo.</param>
		/// <param name="nSamples">Number of valid samples.</param>
		/// <param name="output">Buffer to write encoded data to</param>
		/// <param name="outputSize">Size of buffer.</param>
		/// <param name="mono">True if mono, false if stereo.</param>
		/// <returns>Number of bytes of encoded data written to output buffer.</returns>
		public int Write(float[] samples, int nSamples, byte[] output, int outputSize, bool mono)
		{
			int rc;
			if (mono)
				rc = NativeMethods.lame_encode_buffer_ieee_float(context, samples, samples, nSamples, output, outputSize);
			else
				rc = NativeMethods.lame_encode_buffer_interleaved_ieee_float(context, samples, nSamples / 2, output, outputSize);
			return rc;
		}

		/// <summary>Flush encoder output</summary>
		/// <param name="output">Buffer to write encoded data to</param>
		/// <param name="outputSize">Size of buffer.</param>
		/// <returns>Number of bytes of encoded data written to output buffer.</returns>
		public int Flush(byte[] output, int outputSize)
		{
			int res = NativeMethods.lame_encode_flush(context, output, outputSize);
			return Math.Max(0, res);
		}

		/// <summary>Get the LAME VBR frame content</summary>
		/// <returns>Byte array with VBR frame contents or null on error.</returns>
		public byte[] GetLAMETagFrame()
		{
			byte[] buffer = new byte[1];
			size_t frameSize = NativeMethods.lame_get_lametag_frame(context, buffer, 0);
			if (frameSize == 0)
				return null;
			buffer = new byte[(int)frameSize];
			size_t res = NativeMethods.lame_get_lametag_frame(context, buffer, frameSize);
			if (res != frameSize)
				return null;
			return buffer;
		}

		#endregion

		/// <summary>Print out LAME configuration to standard output, or to registered output function</summary>
		public void PrintConfig() { NativeMethods.lame_print_config(context); }
		/// <summary>Print out LAME internals to standard output, or to registered output function</summary>
		public void PrintInternals() { NativeMethods.lame_print_internals(context); }

		#region Reporting function support

		private ReportFunction rptError = null;
		private ReportFunction rptDebug = null;
		private ReportFunction rptMsg = null;

		private void ErrorProxy(string format, IntPtr va_args)
		{
			string text = NativeMethods.printf(format, va_args);
			rptError?.Invoke(text);
		}

		private void DebugProxy(string format, IntPtr va_args)
		{
			string text = NativeMethods.printf(format, va_args);
			rptDebug?.Invoke(text);
		}

		private void MessageProxy(string format, IntPtr va_args)
		{
			string text = NativeMethods.printf(format, va_args);
			rptMsg?.Invoke(text);
		}

		private void InitReportFunctions()
		{
			NativeMethods.lame_set_errorf(context, ErrorProxy);
			NativeMethods.lame_set_debugf(context, DebugProxy);
			NativeMethods.lame_set_msgf(context, MessageProxy);
		}

		/// <summary>Set reporting function for error output from LAME library</summary>
		/// <param name="fn">Reporting function</param>
		public void SetErrorFunc(ReportFunction fn)
		{
			rptError = fn;
		}

		/// <summary>Set reporting function for debug output from LAME library</summary>
		/// <param name="fn">Reporting function</param>
		public void SetDebugFunc(ReportFunction fn)
		{
			rptDebug = fn;
		}

		/// <summary>Set reporting function for message output from LAME library</summary>
		/// <param name="fn">Reporting function</param>
		public void SetMsgFunc(ReportFunction fn)
		{
			rptMsg = fn;
		}
		#endregion

		#region ID3 tag support
		private static GenreCallback id3GenreCallback = null;

		private static void ID3Genre_proxy(int index, string genre, IntPtr cookie)
		{
			id3GenreCallback?.Invoke(index, genre);
		}

#if false // disabled for now, call ID3GenreList method instead
		public void SetID3GenreCallback(GenreCallback fn)
		{
			id3GenreCallback = fn;
		}
#endif

		/// <summary>Utility to obtain a list of genre names with numbers</summary>
		/// <returns>Dictionary containing genres</returns>
		public static Dictionary<int, string> ID3GenreList()
		{
			Dictionary<int, string> res = new Dictionary<int, string>();
			GenreCallback cbsave = id3GenreCallback;
			id3GenreCallback = (idx, gen) => res[idx] = gen;

			NativeMethods.id3tag_genre_list(ID3Genre_proxy, IntPtr.Zero);
			id3GenreCallback = null;
			id3GenreCallback = cbsave;
			return res;
		}

		/// <summary>Initialize ID3 tag on context, clearing any existing ID3 information</summary>
		public void ID3Init()
		{
			NativeMethods.id3tag_init(context);
		}

		/// <summary>force addition of ID3v2 tag</summary>
		public void ID3AddV2()
		{
			NativeMethods.id3tag_add_v2(context);
		}

		/// <summary>add only a version 1 tag</summary>
		public void ID3V1Only()
		{
			NativeMethods.id3tag_v1_only(context);
		}

		/// <summary>add only a version 2 tag</summary>
		public void ID3V2Only()
		{
			NativeMethods.id3tag_v2_only(context);
		}

		/// <summary>pad version 1 tag with spaces instead of nulls</summary>
		public void ID3SpaceV1()
		{
			NativeMethods.id3tag_space_v1(context);
		}

		/// <summary>pad version 2 tag with extra 128 bytes</summary>
		public void ID3PadV2()
		{
			NativeMethods.id3tag_pad_v2(context);
		}

		/// <summary>pad version 2 tag with extra <paramref name="nBytes"/> bytes</summary>
		/// <param name="nBytes">Number of bytes to pad</param>
		public void ID3SetPad(int nBytes)
		{
			NativeMethods.id3tag_set_pad(context, nBytes);
		}

		/// <summary>Set ID3 title</summary>
		/// <param name="title">Value to set</param>
		public void ID3SetTitle(string title)
		{
			NativeMethods.id3tag_set_title(context, title);
		}

		public void ID3SetArtist(string artist)
		{
			NativeMethods.id3tag_set_artist(context, artist);
		}

		public void ID3SetAlbum(string album)
		{
			NativeMethods.id3tag_set_album(context, album);
		}

		/// <summary>Set year</summary>
		/// <param name="year">Year value to set, as string</param>
		public void ID3SetYear(string year)
		{
			NativeMethods.id3tag_set_year(context, year);
		}

		/// <summary>Set year</summary>
		/// <param name="year">Year value to set, as integer</param>
		public void ID3SetYear(int year)
		{
			NativeMethods.id3tag_set_year(context, year.ToString());
		}

		public bool ID3SetComment(string comment)
		{
			if (Encoding.UTF8.GetByteCount(comment) == comment.Length)
				return CheckResult(NativeMethods.id3tag_set_comment(context, comment));

			// Comment is Unicode.  Encode as UCS2 with BOM and terminator.
			byte[] data = UCS2.GetBytes(comment);
			return CheckResult(NativeMethods.id3tag_set_comment_utf16(context, "zxx", (byte[])null, data));
		}

		public bool ID3SetTrack(string track)
		{
			return CheckResult(NativeMethods.id3tag_set_track(context, track));
		}

		public bool ID3SetGenre(string genre)
		{
			return CheckResult(NativeMethods.id3tag_set_genre(context, genre));
		}

		public int ID3SetGenre(int genreIndex)
		{
			return NativeMethods.id3tag_set_genre(context, genreIndex.ToString());
		}

		public bool ID3SetFieldValue(string value)
		{
			if (Encoding.UTF8.GetByteCount(value) == value.Length)
				return CheckResult(NativeMethods.id3tag_set_fieldvalue(context, value));

			// Value is Unicode.  Encode as UCS2 with BOM and terminator.
			byte[] data = UCS2.GetBytes(value);
			return CheckResult(NativeMethods.id3tag_set_fieldvalue_utf16(context, data));
		}

		/// <summary>Set albumart of ID3 tag</summary>
		/// <param name="image">raw image file data</param>
		/// <returns>True if successful, else false</returns>
		/// <remarks>Supported formats: JPG, PNG, GIF.
		/// Max image size: 128KB</remarks>
		public bool ID3SetAlbumArt(byte[] image)
		{
			return CheckResult(NativeMethods.id3tag_set_albumart(context, image, image.Length));
		}

		public byte[] ID3GetID3v1Tag()
		{
			int len = NativeMethods.lame_get_id3v1_tag(context, new byte[] { }, 0);
			if (len < 1)
				return null;
			byte[] res = new byte[len];
			int rc = NativeMethods.lame_get_id3v1_tag(context, res, len);
			if (rc != len)
				return null;
			return res;
		}

		public byte[] ID3GetID3v2Tag()
		{
			int len = NativeMethods.lame_get_id3v2_tag(context, new byte[] { }, 0);
			if (len < 1)
				return null;
			byte[] res = new byte[len];
			int rc = NativeMethods.lame_get_id3v2_tag(context, res, len);
			if (rc != len)
				return null;
			return res;
		}

		public bool ID3WriteTagAutomatic
		{
			get { return NativeMethods.lame_get_write_id3tag_automatic(context); }
			set { NativeMethods.lame_set_write_id3tag_automatic(context, value); }
		}
		#endregion

		#region Result value handling
		private int _lastLameError = 0;

		/// <summary>
		/// Last result value from calling a number of different DLL entry points.
		/// </summary>
		public int LastLameError => _lastLameError;

		/// <summary>
		/// Check success lf LAME call returning BOOL, saving copy to <paramref name="save"/>.
		/// </summary>
		/// <param name="result">Result value to test.</param>
		/// <param name="save">Output int to copy result value to.</param>
		/// <returns>True if result is 0 (LAME_NOERROR), else false.</returns>
		private bool CheckSuccess(int result, out int save)
		{
			save = result;
			return result == 0;
		}

		/// <summary>
		/// Check success of LAME call returning BOOL, updating <see cref="LastLameError"/> property.
		/// </summary>
		/// <param name="result">Return value to test.</param>
		/// <returns>True if result is 0 (LAME_NOERROR), else false.</returns>
		private bool CheckResult(int result)
			=> CheckSuccess(result, out _lastLameError);
		#endregion
	}
}
