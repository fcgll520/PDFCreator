﻿using pdfforge.PDFCreator.Core.DirectConversion;
using pdfforge.PDFCreator.Core.StartupInterface;
using System.Threading.Tasks;

namespace pdfforge.PDFCreator.Core.Startup.AppStarts
{
    public abstract class MaybePipedStart : AppStartBase
    {
        private readonly IMaybePipedApplicationStarter _maybePipedApplicationStarter;

        protected MaybePipedStart(IMaybePipedApplicationStarter maybePipedApplicationStarter)
            : base(maybePipedApplicationStarter.StartupConditions)
        {
            _maybePipedApplicationStarter = maybePipedApplicationStarter;
        }

        private AppStartParameters _appStartParameters;

        public AppStartParameters AppStartParameters
        {
            get => _appStartParameters ?? (_appStartParameters = new AppStartParameters());
            protected internal set => _appStartParameters = value;
        }

        public override async Task<ExitCode> Run()
        {
            var success = await _maybePipedApplicationStarter.SendMessageOrStartApplication(ComposePipeMessage, StartApplication, AppStartParameters.ManagePrintJobs);
            if (!success)
                return ExitCode.ErrorWhileManagingPrintJobs;

            return ExitCode.Ok;
        }

        protected abstract string ComposePipeMessage();

        protected abstract bool StartApplication();
    }
}
