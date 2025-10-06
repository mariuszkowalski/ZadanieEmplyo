using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.InvalidationHandlers.Interfaces;


namespace Services.Core.DataOperation.InvalidationHandlers
{
    public class CacheInvalidationHandler
    {
        private readonly IVacationReportService _vacationReportService;
        private readonly ITeamReportService _teamReportService;

        public CacheInvalidationHandler(IEventBus eventBus, IVacationReportService vacationReportService, ITeamReportService teamReportService)
        {
            _vacationReportService = vacationReportService;
            _teamReportService = teamReportService;

            eventBus.Subscribe<VacationAdded>(Handle);
            eventBus.Subscribe<VacationUpdated>(Handle);
            eventBus.Subscribe<VacationDeleted>(Handle);
            eventBus.Subscribe<TeamAdded>(Handle);
            eventBus.Subscribe<TeamUpdated>(Handle);
            eventBus.Subscribe<TeamDeleted>(Handle);
            eventBus.Subscribe<EmployeeAdded>(Handle);
            eventBus.Subscribe<EmployeeUpdated>(Handle);
            eventBus.Subscribe<EmployeeDeleted>(Handle);
        }

        private void Handle(VacationAdded e) => InvalidateRerpotCaches();
        private void Handle(VacationUpdated e) => InvalidateRerpotCaches();
        private void Handle(VacationDeleted e) => InvalidateRerpotCaches();
        private void Handle(TeamAdded e) => InvalidateRerpotCaches();
        private void Handle(TeamUpdated e) => InvalidateRerpotCaches();
        private void Handle(TeamDeleted e) => InvalidateRerpotCaches();
        private void Handle(EmployeeAdded e) => InvalidateRerpotCaches();
        private void Handle(EmployeeUpdated e) => InvalidateRerpotCaches();
        private void Handle(EmployeeDeleted e) => InvalidateRerpotCaches();

        private void InvalidateRerpotCaches()
        {
            _vacationReportService.InvalidateVacationUsageCache();
            _teamReportService.InvalidateVacationUsageCache();
        }
    }


}
