using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace TestingInfrastructure.STA
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("TestingInfrastructure.STA.STAFactDiscoverer", "TestingInfrastructure")]
    public class STAFactAttribute : FactAttribute
    {
    }
}
