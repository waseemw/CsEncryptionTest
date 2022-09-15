using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EncryptionTest;

public class Encryptor
{
    private readonly byte[] _key;

    public Encryptor(string key)
    {
        _key = Encoding.UTF8.GetBytes(key);
    }

    public string Encrypt<T>(T obj)
    {
        var str = JsonSerializer.Serialize(obj);
        var unencryptedBytes = Encoding.UTF8.GetBytes(str);
        using var aes = Aes.Create();
        var iv = RandomNumberGenerator.GetBytes(16);
        using var encrypt = aes.CreateEncryptor(_key, iv);
        var res = encrypt.TransformFinalBlock(unencryptedBytes, 0, unencryptedBytes.Length);
        return Convert.ToHexString(iv.Concat(res).ToArray());
    }


    public T Decrypt<T>(string str)
    {
        var bytes = Convert.FromHexString(str);
        var iv = bytes[..16];
        using var aes = Aes.Create();
        using var decrypt = aes.CreateDecryptor(_key, iv);
        var res = decrypt.TransformFinalBlock(bytes[16..], 0, bytes.Length-16);
        return JsonSerializer.Deserialize<T>(res)!;
    }
}