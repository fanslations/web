using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Thi.Core
{

    /// <summary>
    /// Cryptography class
    /// </summary>
    [Serializable]
    public class EncryptionService : IEncryptionService
    {
        #region Singleton
        static readonly EncryptionService Service = new EncryptionService();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static EncryptionService Instance
        {
            get
            {
                return Service;
            }
        }
        /// <summary>
        /// Private constructor so it can only be use through Instance.
        /// </summary>
        private EncryptionService() { }
        #endregion Singleton

        #region Fields

        private const string REGISTRY_LOCATION = "Software\\Thi";
        private const string QUICK_KEY = "0123456789qwerty";
        //key has been moved to registry for security
        //private readonly byte[] _key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private readonly UTF8Encoding utfEncoder = new UTF8Encoding();
        private readonly byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 2, 112, 79, 32, 114, 156 };

        #endregion Fields

        #region Methods

        #region Commonly used methods

        public string QuickEncrypt(string plainText)
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(plainText);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = utfEncoder.GetBytes(QUICK_KEY),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return BytesToHex(resultArray);
        }

        public string QuickDecrypt(string encryptedText)
        {
            byte[] inputArray = HexToBytes(encryptedText);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = utfEncoder.GetBytes(QUICK_KEY),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Encrypt some text and return a string suitable for passing in a URL.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <returns></returns>
        public string EncryptToString(string textValue)
        {
            return BytesToHex(Encrypt(textValue.Trim()));
        }


        /// <summary>
        /// Decrypt string.
        /// </summary>
        /// <param name="encryptedString">The encrypted string.</param>
        /// <returns></returns>
        public string DecryptString(string encryptedString)
        {
            return Decrypt(HexToBytes(encryptedString.Trim()));
        }

        /// <summary>
        /// Sets the encryption key.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <returns></returns>
        public bool SetEncryptionKey(string secretKey)
        {
            secretKey = secretKey.Trim().Replace(" ", string.Empty).Replace("-", string.Empty);
            // key size 16=>128, 24=>192 and 32=>256 bits
            if (secretKey.Length == 16 || secretKey.Length == 24 || secretKey.Length == 32)
            {
                using (RegistryKey registryLtc = Registry.CurrentUser.CreateSubKey(REGISTRY_LOCATION))
                {
                    byte[] protectedSecretKey = Protect(this.utfEncoder.GetBytes(secretKey));
                    registryLtc.SetValue("BCS", protectedSecretKey, RegistryValueKind.Binary);
                    registryLtc.Close();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <returns></returns>
        public string GetEncryptionKey()
        {
            return this.utfEncoder.GetString(GetKey());
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Encrypt some text and return an encrypted byte array.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <returns></returns>
        private byte[] Encrypt(string textValue)
        {
            //Translates our text value into a byte array.
            Byte[] bytes = new UTF8Encoding().GetBytes(textValue);

            //Used to stream the data in and out of the CryptoStream.
            var memoryStream = new MemoryStream();

            /*
             * We will have to write the unencrypted bytes to the stream,
             * then read the encrypted result back from the stream.
             */

            #region Write the decrypted value to the encryption stream

            //This is our encryption method
            ICryptoTransform encryptorTransform = new RijndaelManaged().CreateEncryptor(GetKey(), vector);
            var cs = new CryptoStream(memoryStream, encryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            #endregion

            #region Read encrypted value back out of the stream

            memoryStream.Position = 0;
            var encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);

            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// <summary>
        /// Decryption when working with byte arrays.
        /// </summary>
        /// <param name="encryptedValue">The encrypted value.</param>
        /// <returns></returns>
        private string Decrypt(byte[] encryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            var encryptedStream = new MemoryStream();
            ICryptoTransform decryptorTransform = new RijndaelManaged().CreateDecryptor(GetKey(), this.vector);
            var decryptStream = new CryptoStream(encryptedStream, decryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
            decryptStream.FlushFinalBlock();

            #endregion

            #region Read the decrypted value from the stream.

            encryptedStream.Position = 0;
            var decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            #endregion

            return utfEncoder.GetString(decryptedBytes);
        }

        /// <summary>
        /// Strings to byte array.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        /// <remarks>
        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        ///      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        ///      return encoding.GetBytes(str);
        /// However, this results in character values that cannot be passed in a URL.  So, instead, I just
        /// lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        /// </remarks>
        private byte[] StringToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StringToByteArray");

            var byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                byte val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            } while (i < str.Length);
            return byteArr;
        }


        /// <summary>
        /// Bytes the array to string.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <returns></returns>
        /// <remarks>
        /// Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        ///     System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        ///     return enc.GetString(byteArr);   
        /// </remarks>
        private string ByteArrayToString(byte[] byteArr)
        {
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                byte val = byteArr[i];
                if (val < 10)
                    tempStr += "00" + val;
                else if (val < 100)
                    tempStr += "0" + val;
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
        /// <summary>
        /// Convert bytes to Hex string.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        /// <returns>Hex string</returns>
        private string BytesToHex(byte[] bytes)
        {
            var c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }

        /// <summary>
        /// Convert Hex string to bytes.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>Array of bytes.</returns>
        private byte[] HexToBytes(string hexString)
        {
            if (hexString.Length == 0 || hexString.Length % 2 != 0)
            {
                return new byte[0];
            }

            var buffer = new byte[hexString.Length / 2];
            char c;
            for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
            {
                // Convert first half of byte
                c = hexString[sx];
                buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

                // Convert second half of byte
                c = hexString[++sx];
                buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
            }

            return buffer;
        }

        /// <summary>
        /// Gets the key to use for encrypt/decrypt.
        /// </summary>
        /// <returns>
        /// Key
        /// </returns>
        private byte[] GetKey()
        {
            // Get the encrypted key from windows registry
            using (RegistryKey registryLtc = Registry.CurrentUser.OpenSubKey(REGISTRY_LOCATION))
            {
                var data = registryLtc.GetValue("BCS") as byte[];
                registryLtc.Close();

                byte[] decryptedKey = Unprotect(data);
                return decryptedKey;
            }
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private byte[] Protect(byte[] data)
        {
            // Create byte array for additional entropy when using Protect method.
            byte[] aditionalEntropy = { 9, 8, 7, 6, 5 };

            // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
            //  only by the same current user.
            return ProtectedData.Protect(data, aditionalEntropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Unprotects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private byte[] Unprotect(byte[] data)
        {
            // Create byte array for additional entropy when using Protect method.
            byte[] aditionalEntropy = { 9, 8, 7, 6, 5 };

            //Decrypt the data using DataProtectionScope.CurrentUser.
            return ProtectedData.Unprotect(data, aditionalEntropy, DataProtectionScope.CurrentUser);
        }

        #endregion

        #endregion Methods
    }
}

