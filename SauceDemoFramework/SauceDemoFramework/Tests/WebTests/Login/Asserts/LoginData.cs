using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPracticeDemo.Tests.Tests.Login.Asserts
{
    public class LoginData
    {
        private string username;
        private string password;
        private bool isValid;

        public LoginData(string username, string password, bool isValid)
        {
            this.username = username;
            this.password = password;
            this.isValid = isValid;
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        /// <summary>
        /// Carga una lista de objetos LoginData desde un archivo JSON usando JsonHelper.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo JSON</param>
        /// <returns>Lista de LoginData</returns>
        public static List<LoginData> LoadList(string nombreArchivo)
        {
            return JsonHelper.LoadListFromJson<LoginData>(nombreArchivo);
        }

    }
}
