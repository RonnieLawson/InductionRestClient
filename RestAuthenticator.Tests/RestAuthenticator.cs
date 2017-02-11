using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InductionClient.RestAuthenticator.Tests
{
    [TestFixture]
    public class RestAuthenticatorTests
    {
        [TestFixture]
        public class GivenValidCredentials
        {
            [OneTimeSetUp]
            public void WhenCreatingTheAuthenticator()
            {
                RestAuthenticator authenticator = new RestAuthenticator();
            }
        }
    }
}
