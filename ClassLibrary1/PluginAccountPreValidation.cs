using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace MeuPrimeiroProjeto
{
    public class PluginAccountPreValidation : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity entidadeContexto = null;
            if (context.InputParameters.Contains("Target"))
            {
                entidadeContexto = (Entity)context.InputParameters["Target"];

                trace.Trace("Entidade do Contexto: " + entidadeContexto.Attributes.Count);

                if (entidadeContexto == null)
                {
                    return;
                }

                if (!entidadeContexto.Contains("telephone1"))
                {
                    throw new InvalidPluginExecutionException("Campo Telefone principal é obrigatório!");
                }
            }
        }
    }
}
