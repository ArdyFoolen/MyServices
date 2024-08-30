using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Commands
{
    [DerivedType(typeof(PowershellCommand), "Powershell")]
    public class PowershellCommand : NetworkCommand
    {
        public override string Execute()
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "powershell.exe";
            processStartInfo.Arguments = $"-Command \"{Command}\"";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            Output = process.StandardOutput.ReadToEnd().Trim();
            Error = process.StandardError.ReadToEnd().Trim();

            return Output;
        }

        public override string ToString()
        {
            return @$"Command:
  Token:   {SessionToken}
  Command: {Command}
  Output:  {Output}
  Error:   {Error}";
        }
    }
}
