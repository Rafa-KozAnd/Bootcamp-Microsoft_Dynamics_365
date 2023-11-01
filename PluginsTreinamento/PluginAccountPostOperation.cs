using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PluginsTreinamento
{
    public class PluginAccountPostOperation : IPlugin
    {
		// método requerido para execução do Plugin recebendo como parâmetro os dados do provedor de serviço
		public void Execute(IServiceProvider serviceProvider)
		{
			try //tentativa de execução
			{
				// Variável contendo o contexto da execução
				IPluginExecutionContext context =
					(IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

				// Variável contendo o Service Factory da Organização
				IOrganizationServiceFactory serviceFactory =
					(IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

				// Variável contendo o Service Admin que estabele os serviços de conexão com o Dataverse
				IOrganizationService serviceAdmin = serviceFactory.CreateOrganizationService(null);

				// Variável do Trace que armazena informações de LOG
				ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

				// Verifica se contém dados para o destino e se corresponde a uma Entity
				if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
				{
					// Variável do tipo Entity herdando a entidade do contexto
					Entity entidadeContexto = (Entity)context.InputParameters["Target"];

					if (!entidadeContexto.Contains("websiteurl")) // verifica se o atributo websiteurl não está presente no contexto
					{
						throw new InvalidPluginExecutionException("Campo websiteurl é obrigatório!"); // exibe Exception de Erro
					}

					// Variável para nova entidade TASK vazia
					var Task = new Entity("task");

					// atribuição dos atributos para novo registro da entidade TASK
					Task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
					Task.Attributes["regardingobjectid"] = new EntityReference("account", context.PrimaryEntityId);
					Task.Attributes["subject"] = "Visite nosso site: " + entidadeContexto["websiteurl"];
					Task.Attributes["description"] = "TASK criada via Plugin Post Operation";

					serviceAdmin.Create(Task); // executa o método Create para entidade TASK
				}
			}
			catch (InvalidPluginExecutionException ex) // em caso de falha
			{
				throw new InvalidPluginExecutionException("Erro ocorrido: " + ex.Message); // exibe a Exception
			}
		}
	}
}
