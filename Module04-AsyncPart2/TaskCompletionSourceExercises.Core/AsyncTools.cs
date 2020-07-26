using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
    public class AsyncTools
    {
        public static Task<string> RunProgramAsync(string path, string args = null)
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(path, args)
                {
                    RedirectStandardOutput = true, 
                    RedirectStandardError = true
                }
            };

            process.Exited += (sender, eventArgs) =>
            {
                var senderProcess = sender as Process;
                if (senderProcess?.ExitCode != 0)
                    tcs.SetException(new Exception(senderProcess?.StandardError.ReadToEnd()));
                else
                    tcs.SetResult(senderProcess?.StandardOutput.ReadToEnd());

                senderProcess?.Dispose();
            };
            process.Start();

            return tcs.Task;
        }
    }
}
