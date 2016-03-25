using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTestNUnit
{
    [SetUpFixture]
    public class SetupFixture
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Console.WriteLine("Config Log4Net");
            log4net.Config.XmlConfigurator.Configure();
        }

        [TearDown]
	    public void RunAfterAnyTests()
	    {
	        // ...
	    }

    }
}
