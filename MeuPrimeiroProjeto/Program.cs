using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuPrimeiroProjeto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var crmService = new Conexao().ObterConexao();
            DataModel model = new DataModel();
            model.UpdateMoney(crmService);           
        }                              
    }
}
