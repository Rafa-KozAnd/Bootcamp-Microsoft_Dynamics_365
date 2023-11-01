using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PluginsTreinamento
{
    public class MinhaPrimeiraAction : IPlugin
    {        
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            trace.Trace("Minha Primeira Action executada com sucesso e criando Lead no Dataverse!");

            Entity entLead = new Entity("lead");
            entLead["subject"] = "Lead criado via Action";
            entLead["firstname"] = "Primeiro Nome";
            entLead["lastname"] = "Lastname Lead";
            entLead["mobilephone"] = "920220720";
            entLead["ownerid"] = new EntityReference("systemuser", context.UserId);
            Guid guidLead = serviceAdmin.Create(entLead);
            trace.Trace("Lead criado: " + guidLead);
        }
    }
}
