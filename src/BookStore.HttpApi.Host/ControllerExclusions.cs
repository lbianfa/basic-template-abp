using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Pages.Abp.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace BookStore
{
    public class ControllerExclusions : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApiDefinitionController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApplicationConfigurationController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpTenantController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApplicationLocalizationController));
        }
    }
}
