using ShopManager.Clients;
using ShopManager.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopManager
{
    public static class ClientFactory
    {
        public static IClient GenorateClient(int input)
        {
            IClient client = null;
            switch (input)
            {
                case 1:
                    client = new ItemClient();
                    break;

                case 2:
                    client = new MaterialClient();
                    break;

                case 3:
                    client = new AnalysisClient();
                    break;
            }

            return client;
        }
    }
}