using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace GoogleWaveNotifier
{
	/// <summary>
	/// Simple encryption to avoid storing plaintext usernames/passwords in the config file.
	/// This is not very secure - if someone has a copy of Google Wave Notifier and your config
	/// file, they can work out your password. Your password is only as secure as the config file!
	/// </summary>
	static class Enc
	{
		/// <summary>
		/// Encryption key. This can be changed, but will break backwards compatibility with previously created config files, so be careful.
		/// </summary>
		static readonly byte[] sKey = Convert.FromBase64String("xRYtpojf0oo=");

		static SymmetricAlgorithm key = new DESCryptoServiceProvider { Key = sKey, IV = sKey };

		// Encrypt the string.
		public static string Encrypt(string text)
		{
			// Create a memory stream.
			MemoryStream ms = new MemoryStream();

			// Create a CryptoStream using the memory stream and the 
			// CSP DES key.  
			CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

			// Create a StreamWriter to write a string
			// to the stream.
			StreamWriter sw = new StreamWriter(encStream);

			// Write the plaintext to the stream.
			sw.WriteLine(text);

			// Close the StreamWriter and CryptoStream.
			sw.Close();
			encStream.Close();

			// Get an array of bytes that represents
			// the memory stream.
			byte[] buffer = ms.ToArray();

			// Close the memory stream.
			ms.Close();

			// Return the encrypted byte array.
			return Convert.ToBase64String(buffer);
		}

		// Decrypt the byte array.
		public static string Decrypt(string text)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			try
			{
				// Create a memory stream to the passed buffer.
				MemoryStream ms = new MemoryStream(Convert.FromBase64String(text));

				// Create a CryptoStream using the memory stream and the 
				// CSP DES key. 
				CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

				// Create a StreamReader for reading the stream.
				StreamReader sr = new StreamReader(encStream);

				// Read the stream as a string.
				string val = sr.ReadLine();

				// Close the streams.
				sr.Close();
				encStream.Close();
				ms.Close();

				return val;
			}
			catch
			{
				return null;
			}
		}
	}
}