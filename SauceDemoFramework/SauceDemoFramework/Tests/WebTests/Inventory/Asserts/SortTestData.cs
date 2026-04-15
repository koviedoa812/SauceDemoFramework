using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.WebTests.Inventory.Asserts
{
    public class SortTestData
    {
        public string TestCase { get; set; }
        public string SortOption { get; set; }
        public string SortValue { get; set; }
        public bool IsAscending { get; set; }
    }
}
