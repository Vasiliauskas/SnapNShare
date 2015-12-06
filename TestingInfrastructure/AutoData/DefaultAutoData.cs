using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingInfrastructure.AutoData
{
    public class DefaultAutoDataAttribute : AutoDataAttribute
    {
        public DefaultAutoDataAttribute()
            : base(new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
