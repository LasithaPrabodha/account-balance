using Entities.DTOs;
using System.Collections.Generic;

namespace WebApi.UnitTests
{
    public class TestDataGenerator
    {

        public static IEnumerable<object[]> GetRegisterData()
        {
            return new List<object[]>
            {
                new object[] { "Admin", "Admin" , "Admin@#123", "Admin@#123", "admin@gmail.com", Roles.Administrator},
                new object[] { "Lasitha", "Prabodha" , "Lasitha@#123","Lasitha@#123", "lasitha@gmail.com",Roles.Viewer},
            };

        }
        public static IEnumerable<object[]> GetRegisterDataWithInvalidPassword()
        {
            return new List<object[]>
            {
                new object[] { "Admin", "Admin" , "admin@#123", "admin@#123", "admin@gmail.com", Roles.Administrator},
                new object[] { "Lasitha", "Prabodha" , "Lasitha123", "Lasitha123", "lasitha@gmail.com",Roles.Viewer},
                new object[] { "Prabodha", "Prabodha" , "Lasitha","Lasitha", "lasitha@gmail.com",Roles.Viewer},
            };

        }
        public static IEnumerable<object[]> GetRegisterDataDuplicates()
        {
            return new List<object[]>
            {
                new object[] { "Lasitha", "Prabodha" , "Lasitha@#123", "Lasitha@#123", "lasitha@gmail.com",Roles.Viewer},
            };

        }
    }
}
