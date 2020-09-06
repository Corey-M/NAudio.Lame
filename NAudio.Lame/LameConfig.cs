using NAudio.Wave;
using System;

namespace NAudio.Lame
{
	/// <summary>
	/// Holds configuration for the LAME engine, applied when the encoder instance is initialised.
	/// </summary>
	public class LameConfig
	{
		#region Quality
		private LAMEPreset? _preset = null;
		/// <summary>Compression preset, clears <see cref="BitRate"/> if set, defaults to STANDARD</summary>
		public LAMEPreset? Preset
		{
			get => _preset;
			set
			{
				_preset = value;
				if (value != null)
					_bitrate = null;
			}
		}

		private int? _bitrate = null;
		/// <summary>Compression bitrate, clears <see cref="Preset"/> if set.</summary>
		public int? BitRate
		{
			get => _bitrate;
			set
			{
				_bitrate = value;
				if (value != null)
					_preset = null;
			}
		}
		
		/// <summary>Select output sampling frequency. If not specified, LAME will automatically resample the input when using high compression ratios.</summary>
		public int? OutputSampleRate { get; set; }
		#endregion

		#region Variable Bitrate (VBR/ABR) Control
		/// <summary>Mode to use for VBR.</summary>
		public VBRMode? VBR { get; set; }

		/// <summary>Target bitrate when <see cref="VBR"/> is set to <see cref="VBRMode.ABR"/></summary>
		public int? ABRRateKbps { get; set; }

		/// <summary>Minimum bitrate in kbps</summary>
		public int? VBRMinimumRateKbps { get; set; }

		/// <summary>Maximum bitrate in kbps</summary>
		public int? VBRMaximumRateKbps { get; set; }

		/// <summary>If true then <see cref="VBRMinimumRateKbps"/> is a hard limit, otherwise analog silence will produce lower bitrate frames.</summary>
		public bool? VBREnforceMinimum { get; set; }

		private int? _vbrquality;
		/// <summary>VBR algoritm quality, 0 (highest) to 9 (lowest)</summary>
		public int? VBRQuality
		{
			get => _vbrquality;
			set
			{
				if (value == null)
					_vbrquality = null;
				else
					_vbrquality = Math.Max(0, Math.Min(9, value.Value));
			}
		}
		#endregion

		#region Input settings
		/// <summary>Global amplification factor.</summary>
		public float? Scale { get; set; }

		/// <summary>Left channel amplification.</summary>
		public float? ScaleLeft { get; set; }

		/// <summary>Right channel amplification.</summary>
		public float? ScaleRight { get; set; }
		#endregion

		#region Filtering
		/// <summary>Frequency in Hz to apply low-pass filter.  -1 to disable, 0 to auto-select</summary>
		public int? LowPassFreq { get; set; }

		/// <summary>Width of transition band in Hz.</summary>
		public int? LowPassWidth { get; set; }

		/// <summary>Frequency in Hz to apply high-pass filter. -1 to disable, 0 to auto-select.</summary>
		public int? HighPassFreq { get; set; }

		/// <summary>Width of transition band in Hz.</summary>
		public int? HighPassWidth { get; set; }
		#endregion

		#region General Control
		/// <summary>Enable analysis.</summary>
		public bool? Analysis { get; set; }

		/// <summary>WRite VBR tag to MP3 file.</summary>
		public bool? WriteVBRTag { get; set; }

		/// <summary>Specify MPEG channel mode, use best guess if false/unset.</summary>
		public MPEGMode? Mode { get; set; }

		/// <summary>Force M/S mode.</summary>
		public bool? ForceMS { get; set; }

		/// <summary>Use free format.</summary>
		public bool? UseFreeFormat { get; set; }
		#endregion

		#region Frame Parameters
		/// <summary>Set output Copyright flag.</summary>
		public bool? Copyright { get; set; }

		/// <summary>Set output Original flag.</summary>
		public bool? Original { get; set; }

		/// <summary>Set error protection.  Uses 2 bytes from each fram for CRC checksum.</summary>
		public bool? ErrorProtection { get; set; }

		/// <summary>Enforce strict ISO compliance.</summary>
		public bool? StrictISO { get; set; }
		#endregion

		#region ID3
		/// <summary>ID3 tag data, to be added after configuration.</summary>
		public ID3TagData ID3 { get; set; }
		#endregion

		#region DLL initialisation
		/// <summary>Create <see cref="LibMp3Lame"/> and configure it.</summary>
		/// <returns></returns>
		public LameDLLWrap.LibMp3Lame ConfigureDLL(WaveFormat format)
		{
			var result = new LameDLLWrap.LibMp3Lame
			{
				// Input settings
				InputSampleRate = format.SampleRate,
				NumChannels = format.Channels,
			};

			// Set quality
			if (_bitrate != null)
			{
				result.BitRate = _bitrate.Value;
			}
			else
			{
				if (_preset >= LAMEPreset.V9 && _preset <= LAMEPreset.V0 && result.VBR == LameDLLWrap.VBRMode.Off && VBR == null)
				{
					result.VBR = LameDLLWrap.VBRMode.Default;
				}
				result.SetPreset((int)(_preset ?? LAMEPreset.STANDARD));
			}

			if (OutputSampleRate != null) result.OutputSampleRate = OutputSampleRate.Value;

			// VBR configuration
			if (VBR != null)
			{
				result.VBR = (LameDLLWrap.VBRMode)VBR.Value;
				if (VBR == VBRMode.ABR && ABRRateKbps != null) result.VBRMeanBitrateKbps = ABRRateKbps.Value;
				if (VBRMinimumRateKbps != null) result.VBRMinBitrateKbps = VBRMinimumRateKbps.Value;
				if (VBRMaximumRateKbps != null) result.VBRMaxBitrateKbps = VBRMaximumRateKbps.Value;
				if (VBREnforceMinimum != null) result.VBRHardMin = VBREnforceMinimum.Value;
				if (VBRQuality != null) result.VBRQualityLevel = VBRQuality.Value;
			}

			// Scaling
			if (Scale != null) result.Scale = Scale.Value;
			if (ScaleLeft != null) result.ScaleLeft = ScaleLeft.Value;
			if (ScaleRight != null) result.ScaleRight = ScaleRight.Value;

			// Filtering
			if (LowPassFreq != null) result.LowPassFreq = LowPassFreq.Value;
			if (LowPassWidth != null) result.LowPassWidth = LowPassWidth.Value;
			if (HighPassFreq != null) result.HighPassFreq = HighPassFreq.Value;
			if (HighPassWidth != null) result.HighPassWidth = HighPassWidth.Value;

			// General Control
			if (Analysis != null) result.Analysis = Analysis.Value;
			if (WriteVBRTag != null) result.WriteVBRTag = WriteVBRTag.Value;
			if (Mode != null) result.Mode = (LameDLLWrap.MPEGMode)Mode.Value;
			if (ForceMS != null) result.ForceMS = ForceMS.Value;
			if (UseFreeFormat != null) result.UseFreeFormat = UseFreeFormat.Value;
			
			// Frame Parameters
			if (Copyright != null) result.Copyright = Copyright.Value;
			if (Original != null) result.Original = Original.Value;
			if (ErrorProtection != null) result.ErrorProtection = ErrorProtection.Value;
			if (StrictISO != null) result.StrictISO = StrictISO.Value;

			return result;
		}
		#endregion
	}
}
