using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.WebTests.Checkout.Asserts
{
        public class CheckoutTestData
        {
            public string TestCase { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PostalCode { get; set; }
            public bool IsValid { get; set; }
            public string ExpectedError { get; set; }
        }
}