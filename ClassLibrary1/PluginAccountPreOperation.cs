using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace MeuPrimeiroProjeto
{
    public class PluginAccountPreOperation : IPlugin
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

                    if (entity.LogicalName == "account")
                    {
                        if (entity.Attributes.Contains("telephone1"))
                        {                            
                            var phone1 = entity["telephone1"].ToString();
                            
                            string FetchContact = @"<?xml version='1.0'?>" +
                                "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>" +
                                "<entity name='contact'>" +
                                "<attribute name='fullname'/>" +
                                "<attribute name='telephone1'/>" +
                                "<attribute name='contactid'/>" +
                                "<order descending='false' attribute='fullname'/>" +
                                "<filter type='and'>" +
                                "<condition attribute='telephone1' value='" + phone1 + "' operator='eq'/>" +
                                "</filter>" +
                                "</entity>" +
                                "</fetch>";

                            trace.Trace("FetchContact: " + FetchContact);

                            var primarycontact = serviceAdmin.RetrieveMultiple(new FetchExpression(FetchContact));
                            if (primarycontact.Entities.Count > 0)
                            {                                
                                foreach (var entityContact in primarycontact.Entities)
                                {                                    
                                    entity["primarycontactid"] = new EntityReference("contact", entityContact.Id);
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidPluginExecutionException ex)
            {                
                throw new InvalidPluginExecutionException("Erro ocorrido: " + ex.Message);
            }
        }

    }
}
