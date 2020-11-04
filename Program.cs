using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ADPC
{
    public static class Program
    {
        /// <summary>
        /// It prevents os going to sleep(power saving mode).
        /// Must be called AllowSleepOrPowerSaving before program is shutdown.
        /// </summary>
        public static void PreventSleepOrPowerSaving()
        {
            SetThreadExecutionState(ExeState.Continous | ExeState.System | ExeState.Display);
        }

        /// <summary>
        /// Must be called AllowSleepOrPowerSaving before program is shutdown.
        /// </summary>
        public static void AllowSleepOrPowerSaving()
        {
            SetThreadExecutionState(ExeState.Continous);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExeState SetThreadExecutionState(ExeState flags);

        [FlagsAttribute]
        private enum ExeState : uint
        {
            AwayMode = 0x00000040,
            Continous = 0x80000000,
            Display = 0x00000002,
            System = 0x00000001
        }
    }
}
