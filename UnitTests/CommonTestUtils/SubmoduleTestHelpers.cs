﻿using System.Threading;
using GitCommands;
using GitCommands.Submodules;

namespace CommonTestUtils
{
    public class SubmoduleTestHelpers
    {
        public static SubmoduleInfoResult UpdateSubmoduleStatusAndWaitForResult(ISubmoduleStatusProvider provider, GitModule module, bool updateStatus = false)
        {
            SubmoduleInfoResult result = null;
            SemaphoreSlim onUpdateCompleteSignal = new SemaphoreSlim(0, 1);

            provider.StatusUpdating += Provider_StatusUpdating;
            provider.StatusUpdated += Provider_StatusUpdated;

            provider.UpdateSubmodulesStatus(
                updateStatus: updateStatus,
                workingDirectory: module.WorkingDir,
                noBranchText: string.Empty);

            onUpdateCompleteSignal.Wait();

            provider.StatusUpdating -= Provider_StatusUpdating;
            provider.StatusUpdated -= Provider_StatusUpdated;

            return result;

            void Provider_StatusUpdating(object sender, System.EventArgs e)
            {
            }

            void Provider_StatusUpdated(object sender, SubmoduleStatusEventArgs e)
            {
                result = e.Info;
                onUpdateCompleteSignal.Release();
            }
        }
    }
}
