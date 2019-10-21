using Com.WIC.Common.Interfaces;
using Com.WIC.Encoder.Enums;
using Com.WIC.Encoder.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Com.WIC.Encoder
{
	public class EncoderService : IEncoderService
    {
		private readonly string _ffmpegBinPath;
		private readonly string _outputFileName;
        private readonly Regex _fileExtensionRegex;
		public EncoderService(string ffmpegBinPath)
		{
			_ffmpegBinPath = ffmpegBinPath ?? throw new ArgumentException(nameof(ffmpegBinPath));
			_outputFileName = Path.GetTempPath();
            _fileExtensionRegex = new Regex(@"\.([a-z]{1,3})$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
		}

		public async Task<int> ConvertAsync(FileTypeEnum targetFormat, params AudioFile[] audioFiles)
		{
            if (audioFiles == null)
                throw new Exception("No aduio files provided for conversion");
            var newExtension = Enum.GetName(typeof(FileTypeEnum), targetFormat).ToLowerInvariant();
            var ffmpegArgs = $"-i {string.Join(" -i ", audioFiles.Select(x => Path.Combine(x.LocalPath, x.FileName)))} {string.Join("", audioFiles.Select((x, i) => $" -map {i} {Path.Combine(x.LocalPath, _fileExtensionRegex.Replace(x.FileName, "." + newExtension))}"))}";
            var tcs = new TaskCompletionSource<int>();
			Process proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = _ffmpegBinPath,
					// https://stackoverflow.com/questions/41346234/can-ffmpeg-encode-multiple-files-at-once?rq=1
					Arguments = ffmpegArgs,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};
            proc.ErrorDataReceived += (sender, args) =>
            {
                var msg = string.Empty;
                while (!proc.StandardOutput.EndOfStream)
                {
                    msg += proc.StandardOutput.ReadLine();
                }
                tcs.SetException(new Exception(msg));
                tcs.SetResult(-2);
                proc.Dispose();
            };
            proc.OutputDataReceived += (sender, args) =>
            {
                var msg = string.Empty;
                while (!proc.StandardOutput.EndOfStream)
                {
                    msg += proc.StandardOutput.ReadLine();
                }
                tcs.SetException(new Exception(msg));
                tcs.SetResult(-1);
                proc.Dispose();
            };
			proc.Exited += (sender, args) =>
			{
				tcs.SetResult(proc.ExitCode);
				proc.Dispose();
			};
            proc.Disposed += (sender, args) =>
            {
                tcs.SetResult(-3);
            };
            proc.Start();

			return await tcs.Task;
		}

        /// <summary>
        /// Join two or more audio files together. Note: the files must be of the same type and bitrate.
        /// </summary>
        /// <param name="outputFileName">The resulting joined audio</param>
        /// <param name="audioFiles">Input audio files</param>
        /// <returns></returns>
		public async Task<int> JoinAsync(AudioFile outputFile, params AudioFile[] audioFiles)
		{
            if (outputFile == null)
                throw new Exception("The output filename can not be empty");
            if (audioFiles.Length < 2)
                throw new Exception("You must provide at least two audio files to join");
            //var targetFormatSearch = _fileExtensionRegex.Matches(outputFileName);
            //FileTypeEnum targetFormat;
            ////if (targetFormatSearch.Count < 1 || targetFormatSearch[0].Groups.Count < 2)
            ////    throw new Exception("Output file name has indeterminable file extension");
            //if (!Enum.TryParse<FileTypeEnum>(targetFormatSearch?[0]?.Groups?[1]?.Value, out targetFormat))
            //    throw new Exception("Output file name has unrecognized file extension");
            if (audioFiles.Where(x => x.AudioType != outputFile.AudioType).Any())
                throw new Exception("Not all files provided for joining are of the same file type.");
            var ffmpegVars = $"-i \"concat:{string.Join("|", audioFiles.Select(x => Path.Combine(x.LocalPath, x.FileName)))}\" -c copy {Path.Combine(outputFile.LocalPath, outputFile.FileName)}";
            var tcs = new TaskCompletionSource<int>();

			Process proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = _ffmpegBinPath,
					Arguments = ffmpegVars,
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

            proc.ErrorDataReceived += (sender, args) =>
            {
                var msg = string.Empty;
                while (!proc.StandardOutput.EndOfStream)
                {
                    msg += proc.StandardOutput.ReadLine();
                }
                tcs.SetException(new Exception(msg));
                tcs.SetResult(-2);
                proc.Dispose();
            };
            proc.OutputDataReceived += (sender, args) =>
            {
                var msg = string.Empty;
                while (!proc.StandardOutput.EndOfStream)
                {
                    msg += proc.StandardOutput.ReadLine();
                }
                tcs.SetException(new Exception(msg));
                tcs.SetResult(-1);
                proc.Dispose();
            };
            proc.Exited += (sender, args) =>
            {
                tcs.SetResult(proc.ExitCode);
                proc.Dispose();
            };
            proc.Disposed += (sender, args) =>
            {
                tcs.SetResult(-3);
            };
            proc.Start();

            return await tcs.Task;
		}
	}
}
