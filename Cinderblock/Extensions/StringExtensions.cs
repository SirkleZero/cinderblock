using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        //http://www.cambiaresearch.com/c4/bf974b23-484b-41c3-b331-0bd8121d5177/Parsing-Email-Addresses-with-Regular-Expressions.aspx
        /// <summary>
        /// Represents a lenient email regular expression.
        /// </summary>
        private const string EmailPatternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// Represents a strict email regular expression.
        /// </summary>
        private const string EmailPatternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";


        #region Inject

        // See: http://mo.notono.us/2008/07/c-stringinject-format-strings-by-key.html

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching object properties.
        /// <remarks>Uses <see cref="String.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="injectionObject">The object whose properties should be injected in the string</param>
        /// <returns>A version of the formatString string with keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, object injectionObject)
        {
            return formatString.Inject(GetPropertyHash(injectionObject));
        }

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching dictionary entries.
        /// <remarks>Uses <see cref="String.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="dictionary">An <see cref="IDictionary"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with dictionary keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, IDictionary dictionary)
        {
            return formatString.Inject(new Hashtable(dictionary));
        }

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching hashtable entries.
        /// <remarks>Uses <see cref="String.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="attributes">A <see cref="Hashtable"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with hastable keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, Hashtable attributes)
        {
            var result = formatString;
            if (attributes == null || formatString == null)
            {
                return result;
            }

            foreach (string attributeKey in attributes.Keys)
            {
                result = result.InjectSingleValue(attributeKey, attributes[attributeKey]);
            }

            return result;
        }

        /// <summary>
        /// Replaces all instances of a 'key' (e.g. {foo} or {foo:SomeFormat}) in a string with an optionally formatted value, and returns the result.
        /// </summary>
        /// <param name="formatString">The string containing the key; unformatted ({foo}), or formatted ({foo:SomeFormat})</param>
        /// <param name="key">The key name (foo)</param>
        /// <param name="replacementValue">The replacement value; if null is replaced with an empty string</param>
        /// <returns>The input string with any instances of the key replaced with the replacement value</returns>
        public static string InjectSingleValue(this string formatString, string key, object replacementValue)
        {
            var result = formatString;
            //regex replacement of key with value, where the generic key format is:
            var attributeRegex = new Regex("{(" + key + ")(?:}|(?::(.[^}]*)}))");  //for key = foo, matches {foo} and {foo:SomeFormat}

            //loop through matches, since each key may be used more than once (and with a different format string)
            foreach (Match m in attributeRegex.Matches(formatString))
            {
                var replacement = m.ToString();
                if (m.Groups[2].Length > 0) //matched {foo:SomeFormat}
                {
                    //do a double string.Format - first to build the proper format string, and then to format the replacement value
                    var attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", m.Groups[2]);
                    replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
                }
                else //matched {foo}
                {
                    replacement = (replacementValue ?? string.Empty).ToString();
                }
                //perform replacements, one match at a time
                result = result.Replace(m.ToString(), replacement);  //attributeRegex.Replace(result, replacement, 1);
            }

            return result;
        }


        /// <summary>
        /// Creates a HashTable based on current object state.
        /// <remarks>Copied from the MVCToolkit HtmlExtensionUtility class</remarks>
        /// </summary>
        /// <param name="properties">The object from which to get the properties</param>
        /// <returns>A <see cref="Hashtable"/> containing the object instance's property names and their values</returns>
        private static Hashtable GetPropertyHash(object properties)
        {
            Hashtable values = null;
            if (properties != null)
            {
                values = new Hashtable();
                var props = TypeDescriptor.GetProperties(properties);
                foreach (PropertyDescriptor prop in props)
                {
                    values.Add(prop.Name, prop.GetValue(properties));
                }
            }

            return values;
        }

        #endregion

        #region Truncate

        /// <summary>
        /// Substring but OK if shorter
        /// </summary>
        public static string Truncate(this string str, int characterCount)
        {
            return Truncate(str, characterCount, false, false);
        }

        /// <summary>
        /// Substring with elipses but OK if shorter, will take 3 characters off character count if necessary
        /// </summary>
        public static string Truncate(this string str, int characterCount, bool truncateOnWordBoundary)
        {
            return Truncate(str, characterCount, truncateOnWordBoundary, false);
        }

        /// <summary>
        /// Substring with elipses but OK if shorter, will take 3 characters off character count if necessary
        /// tries to land on a space.
        /// </summary>
        public static string Truncate(this string s, int characterCount, bool truncateOnWordBoundary, bool withEllipses)
        {
            if (s.Length <= characterCount)
            {
                return s;
            }

            s = s.Substring(0, characterCount);

            if (truncateOnWordBoundary)
            {
                var wordBoundary = s.LastIndexOfAny(new char[] { ' ', '.', ',', '!', '?' });
                if (wordBoundary > 0)
                {
                    s = s.Substring(0, wordBoundary);
                }
            }

            if (withEllipses)
            {
                s += '…';
            }

            return s;
        }

        #endregion

        /// <summary>
        /// Counts the number of times a string occurs in another string.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <param name="search">The text to search for.</param>
        /// <returns>The number of times the search string occurs in the text.</returns>
        public static int Occurrences(this string text, string search)
        {
            var count = 0;
            var i = 0;

            while ((i = text.IndexOf(search, i, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                i += search.Length;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Determines if the format of the string adheres to that of an internet email address (lenient).
        /// </summary>
        /// <param name="s">The string to validate.</param>
        /// <returns>true if the string is a valid email address; otherwise false.</returns>
        public static bool IsValidEmail(this string s)
        {
            return s.IsValidEmail(EmailValidationType.Lenient);
        }

        /// <summary>
        /// Determines if the format of the string adheres to that of an internet email address.
        /// </summary>
        /// <param name="s">The string to validate.</param>
        /// <param name="type">The type of email validation to use.</param>
        /// <returns>true if the string is a valid email address; otherwise false.</returns>
        public static bool IsValidEmail(this string s, EmailValidationType type)
        {
            if (string.IsNullOrEmpty(s) || s.Length > 254)
            {
                return false;
            }

            Regex regex = null;
            switch (type)
            {
                case EmailValidationType.Lenient:
                    regex = new Regex(StringExtensions.EmailPatternLenient);
                    break;
                case EmailValidationType.Strict:
                    regex = new Regex(StringExtensions.EmailPatternStrict);
                    break;
                default:
                    regex = new Regex(StringExtensions.EmailPatternLenient);
                    break;
            }

            return regex.IsMatch(s);
        }

        /// <summary>
        /// Determines if a string will convert to a numeric value.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true if the value will convert to a numeric value; otherwise false.</returns>
        public static bool IsNumeric(this string s)
        {
            var isNumber = new Regex(@"^\d+$");
            return isNumber.IsMatch(s);
        }

        /// <summary>
        /// Determines if the length of the string falls within a range of acceptable values.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <param name="minLength">The minimum length for the string.</param>
        /// <param name="maxLength">The maximum length for the string.</param>
        /// <returns>true if the length of the string falls within the range specified; otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="minLength"/> is out of range.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="maxLength"/> is out of range.</para>
        /// </exception>
        public static bool LengthInRange(this string s, int minLength, int maxLength)
        {
            if (minLength < 0)
            {
                throw new ArgumentOutOfRangeException("minLength", "minLength cannot have a value less then zero.");
            }
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException("maxLength", "maxLength cannot have a value less then zero.");
            }
            if (minLength > maxLength)
            {
                throw new ArgumentOutOfRangeException("minLength", "minLength cannot have a value that is greater than maxLength.");
            }
            if (s == null)
            {
                return false;
            }

            return s.Length.InRange(minLength, maxLength);
        }

        /// <summary>
        /// Hashes the string based on a common hash name as implemented by the .net Framework.
        /// </summary>
        /// <param name="s">The string to hash.</param>
        /// <param name="hashType">The name of the hash algorithm to use.</param>
        /// <param name="salt">The salt to use when hashing the string.</param>
        /// <returns>A hash of the string.</returns>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="salt"/> is <langword name="null"/>.</para>
        /// </exception>
        public static string Hash(this string s, HashType hashType, byte[] salt)
        {
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }

            var plainTextBuffer = Encoding.UTF8.GetBytes(s);
            var plainTextWithSaltBuffer = new byte[plainTextBuffer.Length + salt.Length];

            // Copy plain text bytes into resulting array.
            for (var i = 0; i < plainTextBuffer.Length; i++)
            {
                plainTextWithSaltBuffer[i] = plainTextBuffer[i];
            }

            // Append salt bytes to the resulting array.
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBuffer[plainTextBuffer.Length + i] = salt[i];
            }

            var hashAlgorithmToUse = string.Empty;
            switch (hashType)
            {
                case HashType.SHA:
                    hashAlgorithmToUse = "SHA";
                    break;
                case HashType.SHA1:
                    hashAlgorithmToUse = "SHA1";
                    break;
                case HashType.SHA256:
                    hashAlgorithmToUse = "SHA256";
                    break;
                case HashType.SHA384:
                    hashAlgorithmToUse = "SHA384";
                    break;
                case HashType.SHA512:
                    hashAlgorithmToUse = "SHA512";
                    break;
                default:
                    hashAlgorithmToUse = "SHA512";
                    break;
            }

            byte[] hashBuffer;
            using (var algorithm = HashAlgorithm.Create(hashAlgorithmToUse))
            {
                hashBuffer = algorithm.ComputeHash(plainTextWithSaltBuffer);
            }

            var builder = new StringBuilder();
            foreach (byte b in hashBuffer)
            {
                builder.Append(b.ToString("x2")); //'X2' for uppercase
            }

            return builder.ToString();
        }

        /// <summary>
        /// Performs base 64 encoding on a string using the ASCII Encoder.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>A base 64 encoded string.</returns>
        public static string Base64Encode(this string s)
        {
            var buffer = Encoding.ASCII.GetBytes(s);
            return Convert.ToBase64String(buffer, Base64FormattingOptions.None);
        }

        /// <summary>
        /// Decodes a base 64 encoded string to an ASCII string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The decoded string.</returns>
        public static string FromBase64String(this string s)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(s));
        }
    }

    /// <summary>
    /// Specifies the different hash algorithms supported by the .net framework.
    /// </summary>
    public enum HashType
    {
        /// <summary>
        /// Specifies the SHA hash algorithm.
        /// </summary>
        SHA,
        /// <summary>
        /// Specifies the SHA1 hash algorithm.
        /// </summary>
        SHA1,
        /// <summary>
        /// Specifies the SHA 256 hash algorithm.
        /// </summary>
        SHA256,
        /// <summary>
        /// Specifies the SHA 384 hash algorithm.
        /// </summary>
        SHA384,
        /// <summary>
        /// Specifies the SHA 512 hash algorithm.
        /// </summary>
        SHA512
    }
}
