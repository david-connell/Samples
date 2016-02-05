using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTestNUnit.CodeInfrastructure
{
    static class SharedAsserts
    {
        public static NUnit.Framework.Constraints.EqualConstraint IsEqualToCalibrationCompany
        {
            get
            {
                return Is.EqualTo("Windows7-PC").Or.EqualTo("TQC");
            }
        }

        public static NUnit.Framework.Constraints.EqualConstraint IsEqualToCalibrationUser
        {
            get
            {
                return Is.EqualTo("Windows7").Or.EqualTo("Raoul");
            }
        }

    }
}
