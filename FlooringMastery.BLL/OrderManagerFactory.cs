using FlooringMastery.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public static class OrderManagerFactory
    {
        public static OrderManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch(mode)
            {
                case "Test":
                    return new OrderManager(new TestOrderRepository(
                        @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Test\"));
                case "Prod":
                    return new OrderManager(new ProdOrderRepository(
                        @"C:\GithubRepos\Portfolio\FlooringMastery\FlooringMastery.UI\Data\Prod\"));
                default:
                    throw new Exception("Mode value in app config is not");
            }
        }
    }
}
