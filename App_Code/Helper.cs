using System;
using System.Data;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace SVAMITVA.App_Code
{
    public class Helper
    {
        public static readonly byte[] salt = Encoding.ASCII.GetBytes("Svamitva.Password Version: 7.0.6.168");

        public string SetPassword(string Password, string Username)
        {
            string pass = Encrypt(Password, Username);
            return pass;
        }
        public string GetPassword(string Password, string Username)
        {
            string pass = Decrypt(Password, Username);
            return pass;
        }
        public static string Encrypt(string textToEncrypt, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            //Anything to process?
            if (textToEncrypt == null || textToEncrypt == "") return "";

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedText, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            //Anything to process?
            if (encryptedText == null || encryptedText == "") return "";

            byte[] descryptedBytes;
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
            }
            return Encoding.UTF8.GetString(descryptedBytes);
        }
        public static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }
        public static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            var iterations = 1000;
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt, iterations,HashAlgorithmName.SHA256);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #region FetchRegions

        public DataTable FetchRegions(string selection_type, string selection_Parameter)
        {

            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["FetchRegion"].ToString());
            request.Method = "POST";
            string postData = "{'selection_type': '" + selection_type + "', 'selection_Parameter': '" + selection_Parameter + "'}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("x-access-token", ConfigurationManager.AppSettings["AccessToken"].ToString());

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string content = reader.ReadToEnd();

            // Clean up the streams.  
            reader.Close();
            dataStream.Close();
            response.Close();


            //Define response table structure
            DataTable dtData = new DataTable();
            dtData.Columns.Add("locationID", typeof(string));
            dtData.Columns.Add("location_name", typeof(string));
            dtData.Columns.Add("location_name_p", typeof(string));
            dtData.Columns.Add("location_CLR_ID", typeof(string));

            //Parse Response JSON
            JavaScriptSerializer _Deserializer = new JavaScriptSerializer();
            RegionObject _JsonObject = _Deserializer.Deserialize<RegionObject>(content);

            //Handle individual response code
            switch (_JsonObject.response)
            {
                case "1":
                    #region Fill response table with data
                    foreach (var item in _JsonObject.data)
                    {
                        dtData.Rows.Add(item.locationID, HttpUtility.UrlDecode(item.location_name), HttpUtility.UrlDecode(item.location_name_p), item.location_CLR_ID);
                    }
                    #endregion
                    break;
                case "0":
                    //Handle Issue as per document
                    break;
                case "-1":
                    //Handle Issue as per document
                    break;
                case "-2":
                    //Handle Issue as per document
                    break;
            }
            return dtData;
        }


        public DataTable RegionFetch_TehsilVillages(int selection_Parameter)
        {

            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["FetchRegionTehsilVillages"].ToString());
            request.Method = "POST";
            string postData = "{'tehsilID': '" + selection_Parameter.ToString() + "'}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("x-access-token", ConfigurationManager.AppSettings["AccessToken"].ToString());

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string content = reader.ReadToEnd();

            // Clean up the streams.  
            reader.Close();
            dataStream.Close();
            response.Close();


            //Define response table structure
            DataTable dtData = new DataTable();
            dtData.Columns.Add("locationID", typeof(string));
            dtData.Columns.Add("location_name", typeof(string));
            dtData.Columns.Add("location_name_p", typeof(string));
            dtData.Columns.Add("location_CLR_ID", typeof(string));
            //Parse Response JSON
            JavaScriptSerializer _Deserializer = new JavaScriptSerializer();
            RegionObject _JsonObject = _Deserializer.Deserialize<RegionObject>(content);

            //Handle individual response code
            switch (_JsonObject.response)
            {
                case "1":
                    #region Fill response table with data
                    foreach (var item in _JsonObject.data)
                    {
                        dtData.Rows.Add(item.locationID, HttpUtility.UrlDecode(item.location_name), HttpUtility.UrlDecode(item.location_name_p));
                    }
                    #endregion
                    break;
                case "0":
                    //Handle Issue as per document
                    break;
                case "-1":
                    //Handle Issue as per document
                    break;
                case "-2":
                    //Handle Issue as per document
                    break;
            }
            return dtData;
        }

        #endregion

              
        public class RegionObject
        {
            public string response { get; set; }
            public List<Data> data { get; set; }
            public string reason { get; set; }
            public string sys_message { get; set; }
        }
        public class Data
        {
            public string locationID { get; set; }
            public string location_name { get; set; }
            public string location_name_p { get; set; }

            public string location_CLR_ID { get; set; }
        }
        public class PatwarKanungo
        {
            public string response { get; set; }
            public List<PatwarKanungoData> data { get; set; }
            public string reason { get; set; }
            public string sys_message { get; set; }
        }
        public class PatwarKanungoData
        {
            public string patwarID { get; set; }
            public string patwar_name { get; set; }
            public string patwar_name_p { get; set; }
            public string KanungoID { get; set; }
            public string Kanungo_name { get; set; }
            public string Kanungo_name_p { get; set; }
        }
        public class JamabandiObject
        {
            public string response { get; set; }
            public List<JamabandiData> data { get; set; }
            public string reason { get; set; }
            public string sys_message { get; set; }
        }
        public class JamabandiData
        {
            public string HadbastNo { get; set; }
            public string JamabandiDaur { get; set; }
        }
        
       

    }
}

