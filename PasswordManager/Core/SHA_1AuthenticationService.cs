using Password_Manager.Core;
using System;
using System.Collections.Generic;

namespace Password_Manager.Core
{
    public class SHA_1AuthenticationService : IAuthenticationService
    {
        public string HashPassword(string masterPassword)
        {
            byte[] StringToByteArray(string input)
            {
                byte[] result = new byte[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    result[i] = (byte)input[i];
                }
                return result;
            }

            byte[] UInt64ToByteArray(ulong value)
            {
                byte[] result = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    result[7 - i] = (byte)(value >> (i * 8));
                }
                return result;
            }

            uint LeftRotate(uint value, int bits)
            {
                return (value << bits) | (value >> (32 - bits));
            }

            string UIntToHexString(uint value)
            {
                char[] hex = new char[8];
                for (int i = 0; i < 8; i++)
                {
                    uint nibble = (value >> (28 - i * 4)) & 0xF;
                    hex[i] = (char)(nibble < 10 ? '0' + nibble : 'a' + (nibble - 10));
                }
                return new string(hex);
            }

            byte[] message = StringToByteArray(masterPassword);
            ulong originalLength = (ulong)message.Length * 8; 

            List<byte> paddedMessage = new List<byte>(message);
            paddedMessage.Add(0x80); 

            while ((paddedMessage.Count * 8) % 512 != 448)
            {
                paddedMessage.Add(0x00); 
            }

            byte[] lengthBytes = UInt64ToByteArray(originalLength);
            paddedMessage.AddRange(lengthBytes);

            uint h0 = 0x67452301;
            uint h1 = 0xEFCDAB89;
            uint h2 = 0x98BADCFE;
            uint h3 = 0x10325476;
            uint h4 = 0xC3D2E1F0;

            for (int i = 0; i < paddedMessage.Count; i += 64)
            {
                uint[] words = new uint[80];

                for (int j = 0; j < 16; j++)
                {
                    words[j] = (uint)(paddedMessage[i + j * 4] << 24 |
                                      paddedMessage[i + j * 4 + 1] << 16 |
                                      paddedMessage[i + j * 4 + 2] << 8 |
                                      paddedMessage[i + j * 4 + 3]);
                }

                for (int j = 16; j < 80; j++)
                {
                    words[j] = LeftRotate(words[j - 3] ^ words[j - 8] ^ words[j - 14] ^ words[j - 16], 1);
                }

                uint a = h0, b = h1, c = h2, d = h3, e = h4;

                for (int j = 0; j < 80; j++)
                {
                    uint f, k;
                    if (j < 20)
                    {
                        f = (b & c) | (~b & d);
                        k = 0x5A827999;
                    }
                    else if (j < 40)
                    {
                        f = b ^ c ^ d;
                        k = 0x6ED9EBA1;
                    }
                    else if (j < 60)
                    {
                        f = (b & c) | (b & d) | (c & d);
                        k = 0x8F1BBCDC;
                    }
                    else
                    {
                        f = b ^ c ^ d;
                        k = 0xCA62C1D6;
                    }

                    uint temp = LeftRotate(a, 5) + f + e + k + words[j];
                    e = d;
                    d = c;
                    c = LeftRotate(b, 30);
                    b = a;
                    a = temp;
                }

                h0 += a;
                h1 += b;
                h2 += c;
                h3 += d;
                h4 += e;
            }

            return UIntToHexString(h0) + UIntToHexString(h1) + UIntToHexString(h2) + UIntToHexString(h3) + UIntToHexString(h4);
        }

        public bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return HashPassword(enteredPassword) == storedPassword;
        }
    }
}
