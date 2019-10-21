using Com.WIC.Encoder.Enums;
using Com.WIC.Encoder.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.WIC.Encoder
{
	public class Encoder
	{
		private readonly string _ffmpegBinPath;
		private readonly string _outputPath;
		public Encoder(string ffmpegBinPath)
		{
			_ffmpegBinPath = ffmpegBinPath ?? throw new ArgumentException(nameof(ffmpegBinPath));
			_outputPath = Path.GetTempPath();
		}

		public async Task<int> ConvertAsync(FileTypeEnum targetFormat, params AudioFile[] audioFiles)
		{
			var tcs = new TaskCompletionSource<int>();
			Process proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = Path.Combine(_ffmpegBinPath, "ffmepg.exe"),
					// https://stackoverflow.com/questions/41346234/can-ffmpeg-encode-multiple-files-at-once?rq=1
					Arguments = $"-i {string.Join(" -i ", audioFiles.Select(x => x.LocalPath + x.FileName))} -map {string.Join(" -map ", audioFiles.Select(x => _outputPath + x.FileName))}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};

			proc.Exited += (sender, args) =>
			{
				tcs.SetResult(proc.ExitCode);
				proc.Dispose();
			};
			proc.Start();

			return await tcs.Task;
		}

		public async Task<int> JoinAsync(FileTypeEnum targetFormat, params AudioFile[] audioFiles)
		{
			var tcs = new TaskCompletionSource<int>();

			Process proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = Path.Combine(_ffmpegBinPath, "ffmepg.exe"),
					Arguments = $"-i \"concat:{string.Join("|", audioFiles.Select(x => x.LocalPath + x.FileName))}\" -c copy {_outputPath}.{Enum.GetName(typeof(FileTypeEnum), targetFormat).ToLowerInvariant()}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};

			//proc.Start();
			//while (!proc.StandardOutput.EndOfStream)
			//{
			//	string line = proc.StandardOutput.ReadLine();
			//}

			proc.Exited += (sender, args) =>
			{
				tcs.SetResult(proc.ExitCode);
				proc.Dispose();
			};
			proc.Start();

			return await tcs.Task;
		}
	}
}
