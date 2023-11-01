using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace MeuPrimeiroProjeto
{
    internal class DataModel
    {
        public void FetchXML(CrmServiceClient crmService)
        {
            string query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' />
                                <attribute name='telephone1' />
                                <attribute name='accountid' />
                                <attribute name='emailaddress1' />
                                <order attribute='name' descending='false' />
                              </entity>
                            </fetch>";

            EntityCollection colecao = crmService.RetrieveMultiple(new FetchExpression(query));
            foreach (var item in colecao.Entities)
            {
                Console.WriteLine(item["name"]);
                if (item.Attributes.Contains("telephone1"))
                {
                    Console.WriteLine(item["telephone1"]);
                }
            }
            Console.ReadLine();
        }

        public void Create(CrmServiceClient crmService)
        {
            for (int i = 0; i < 2; i++)
            {
                Guid newRecord = new Guid();
                Entity newEntity = new Entity("account");
                newEntity.Attributes.Add("name", "Treinamento " + DateTime.Now.ToString());
                newEntity.Attributes.Add("telephone1", "11985326471");
                newEntity.Attributes.Add("emailaddress1", "contato@provedor.com");

                newRecord = crmService.Create(newEntity);

                if (newRecord != Guid.Empty)
                    Console.WriteLine("Id do Registro criado : " + newRecord);
                else
                    Console.WriteLine("newRecord não criado!");
            }
            Console.ReadLine();
        }

        public void UpdateEntity(CrmServiceClient crmService, Guid guidAddcount)
        {
            Entity upEntity = new Entity("account", guidAddcount);
            upEntity["name"] = "Registro Alterado em Treinamento";
            upEntity["telephone1"] = "11985263417";
            upEntity["emailaddress1"] = "contato@meuprovedor.com";
            crmService.Update(upEntity);
            Console.WriteLine("UPdate finalizado!");
            Console.ReadKey();
        }

        public void UpdateMoney(CrmServiceClient crmService)
        {
            Entity upEntity = new Entity("curso_cursosdisponiveis", new Guid("6b818328-ead3-ec11-a7b5-002248373452"));
            upEntity["curso_valordocurso"] = null;            
            crmService.Update(upEntity);
            Console.WriteLine("UPdate finalizado!");
            Console.ReadKey();
        }

        public void DeleteEntity(CrmServiceClient crmService, Guid guidAccount)
        {
            Entity entityDelete = crmService.Retrieve("account", guidAccount, new ColumnSet("name"));

            Console.WriteLine("==================================================================================");
            Console.WriteLine("Confirma a exclusão da conta: " + entityDelete["name"] + " ? (SIM/NAO)");
            var confirm = Console.ReadLine();

            if (confirm == "SIM" || confirm == "sim")
            {
                if (entityDelete.Attributes.Count > 0)
                {
                    crmService.Delete("account", guidAccount);
                    Console.WriteLine("Conta excluída!");
                    Console.WriteLine("==================================================================================");
                    Console.WriteLine("pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}
