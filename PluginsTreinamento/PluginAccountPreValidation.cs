using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PluginsTreinamento
{
    public class PluginAccountPreValidation : IPlugin
    {
		// método requerido para execução do Plugin recebendo como parâmetro os dados do provedor de serviço
		public void Execute(IServiceProvider serviceProvider)
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

			// Variável do tipo Entity vazia
			Entity entidadeContexto = null;

			if (context.InputParameters.Contains("Target")) // Verifica se contém dados para o destino
			{
				entidadeContexto = (Entity)context.InputParameters["Target"]; // atribui o contexto da entidade para a variável

				trace.Trace("Entidade do Contexto: " + entidadeContexto.Attributes.Count); // armazena informações de LOG

				if (entidadeContexto == null) //verifica se a entidade do contexto está vazia
				{
					return; // caso verdadeira retorna sem nada executar
				}

				if (!entidadeContexto.Contains("telephone1")) // verifica se o atributo telephone1 não está presente no contexto
				{
					throw new InvalidPluginExecutionException("Campo Telefone principal é obrigatório!!"); // exibe Exception de Erro
				}
			}
		}
	}
}
