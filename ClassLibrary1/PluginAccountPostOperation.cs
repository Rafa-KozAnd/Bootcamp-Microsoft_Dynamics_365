using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace MeuPrimeiroProjeto
{
    public class PluginAccountPostOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

                ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {                    
                    Entity entity = (Entity)context.InputParameters["Target"];

                    var Task = new Entity("task");
                    Task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
                    Task.Attributes["regardingobjectid"] = new EntityReference("account", context.PrimaryEntityId);
                    Task.Attributes["subject"] = "Visite nosso site: " + entity["websiteurl"];
                    Task.Attributes["description"] = "TASK criada via Plugin Post Operation";

                    serviceAdmin.Create(Task);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException("Erro ocorrido: " + ex.Message);
            }
        }
    }
}
