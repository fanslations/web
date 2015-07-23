namespace Thi.Core
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypt some text and return a string suitable for passing in a URL.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <returns></returns>
        string EncryptToString(string textValue);

        /// <summary>
        /// Decrypt string.
        /// </summary>
        /// <param name="encryptedString">The encrypted string.</param>
        /// <returns></returns>
        string DecryptString(string encryptedString);

        /// <summary>
        /// Sets the encryption key.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <returns></returns>
        bool SetEncryptionKey(string secretKey);

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <returns></returns>
        string GetEncryptionKey();
    }
}
