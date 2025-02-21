using System.Configuration;


namespace AIIcsoftAPI.HelperClasses
{
    public class DBConfig
    {
        public string GetConnectionString()
        {
            ////for KI
            //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftledger;port=3306;password=sa1234;";

            ////testdb for testing purpose
            //string ConStr = "server=127.0.0.1;user=root;database=testicsoftledger;port=3306;password=sa1234;";

            ////for newgen
            // string ConStr = "server=127.0.0.1;user=root;database=kiicsoftledger;port=3306;password=sa1234;";

            //ConStr = "Data Source=SQLPC\\SQL22;Initial Catalog=ngicsoftledger;Persist Security Info=True;User ID=sa;Password=sa1234;TrustServerCertificate=true";

            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            string ConStr = configurationRoot.GetConnectionString("LedgerConnection").ToString();
            return ConStr;
        }

        public string GetMainConnectionString()
        {
            ////for KI
            //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftmain;port=3306;password=sa1234;";

            ////testdb for testing purpose
            //string ConStr = "server=127.0.0.1;user=root;database=testicsoftmain;port=3306;password=sa1234;";

            ////for newgen
            // string ConStr = "server=127.0.0.1;user=root;database=kiicsoftmain;port=3306;password=sa1234;";

            //string ConStr = "Data Source=SQLPC\\SQL22;Initial Catalog=ngicsoftmain;Persist Security Info=True;User ID=sa;Password=sa1234;TrustServerCertificate=true";


            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            string ConStr = configurationRoot.GetConnectionString("IcsoftConnection").ToString();
            return ConStr;
        }

        public string GetReportConnectionString()
        {
            ////for KI
            //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftreport;port=3306;password=sa1234;";

            ////testdb for testing purpose
            //string ConStr = "server=127.0.0.1;user=root;database=testicsoftreport;port=3306;password=sa1234;";

            ////for newgen
            //string ConStr = "server=127.0.0.1;user=root;database=kiicsoftreport;port=3306;password=sa1234;";
            //ConStr = "Data Source=SQLPC\\SQL22;Initial Catalog=ngicsoftreport;Persist Security Info=True;User ID=sa;Password=sa1234;TrustServerCertificate=true";

            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            string ConStr = configurationRoot.GetConnectionString("ReportConnection").ToString();
            return ConStr;
        }


        public string GetBizsoftConnectionString()
        {
            ////for KI
            //string ConStr = "server=127.0.0.1;user=root;database=ngbizsoft;port=3306;password=sa1234;";

            ////testdb for testing purpose
            //string ConStr = "server=127.0.0.1;user=root;database=testbizsoft;port=3306;password=sa1234;";

            ////for newgen
            // string ConStr = "server=127.0.0.1;user=root;database=kibizsoft;port=3306;password=sa1234;";

            //ConStr = "Data Source=SQLPC\\SQL22;Initial Catalog=ngbizsoft;Persist Security Info=True;User ID=sa;Password=sa1234;TrustServerCertificate=true";

            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            string ConStr = configurationRoot.GetConnectionString("BizsoftConnection").ToString();
            return ConStr;
        }

//icosoftdatabase falt database added by sdj
public string GetFlatDatabaseConnectionString()
        {
            ////for KI
            //string ConStr = "server=127.0.0.1;user=root;database=ngbizsoft;port=3306;password=sa1234;";

            ////testdb for testing purpose
            //string ConStr = "server=127.0.0.1;user=root;database=testbizsoft;port=3306;password=sa1234;";

            ////for newgen
            // string ConStr = "server=127.0.0.1;user=root;database=kibizsoft;port=3306;password=sa1234;";

            //ConStr = "Data Source=SQLPC\\SQL22;Initial Catalog=ngbizsoft;Persist Security Info=True;User ID=sa;Password=sa1234;TrustServerCertificate=true";

            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            string ConStr = configurationRoot.GetConnectionString("IcsoftFlatConnection").ToString();
            return ConStr;
        }



        //public string GetConnectionString()
        //{
        //    ////for KI
        //    string ConStr = "server=127.0.0.1;user=root;database=kiicsoftledger;port=3306;password=sa1234;";

        //    ////testdb for testing purpose
        //    //string ConStr = "server=127.0.0.1;user=root;database=testicsoftledger;port=3306;password=sa1234;";

        //    ////for newgen
        //    //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftledger;port=3306;password=sa1234;";
        //    return ConStr;
        //}

        //public string GetMainConnectionString()
        //{
        //    ////for KI
        //    string ConStr = "server=127.0.0.1;user=root;database=kiicsoftmain;port=3306;password=sa1234;";

        //    ////testdb for testing purpose
        //    //string ConStr = "server=127.0.0.1;user=root;database=testicsoftmain;port=3306;password=sa1234;";

        //    ////for newgen
        //    //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftmain;port=3306;password=sa1234;";

        //    return ConStr;
        //}

        //public string GetReportConnectionString()
        //{
        //    ////for KI
        //    string ConStr = "server=127.0.0.1;user=root;database=kiicsoftreport;port=3306;password=sa1234;";

        //    ////testdb for testing purpose
        //    //string ConStr = "server=127.0.0.1;user=root;database=testicsoftreport;port=3306;password=sa1234;";

        //    ////for newgen
        //    //string ConStr = "server=127.0.0.1;user=root;database=ngicsoftreport;port=3306;password=sa1234;";

        //    return ConStr;
        //}


        //public string GetBizsoftConnectionString()
        //{
        //    ////for KI
        //    string ConStr = "server=127.0.0.1;user=root;database=kibizsoft;port=3306;password=sa1234;";

        //    ////testdb for testing purpose
        //    //string ConStr = "server=127.0.0.1;user=root;database=testbizsoft;port=3306;password=sa1234;";

        //    ////for newgen
        //    //string ConStr = "server=127.0.0.1;user=root;database=ngbizsoft;port=3306;password=sa1234;";
        //    return ConStr;
        //}

    }

}
