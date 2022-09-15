using System.Diagnostics;
using EncryptionTest;

var sw = new Stopwatch();
sw.Start();
var encrypt = new Encryptor("testtestesstestsetsetsetsettseet");
var tasks = new List<Task>();
for (var i = 0; i < 100_000; i++) tasks.Add(Test());
await Task.WhenAll(tasks);
sw.Stop();
Console.WriteLine(sw.Elapsed.TotalMilliseconds);
await Task.Delay(TimeSpan.FromSeconds(1000));

async Task Test()
{
    var str = encrypt.Encrypt(new Whatever { A = "hmm", B = "test", C = 123 });

    var decrypted = encrypt.Decrypt<Whatever>(str);
    await Task.Delay(TimeSpan.FromSeconds(10));
    _ = decrypted.C;
}


internal class Whatever
{
    public string A { get; set; } = null!;
    public string B { get; set; } = null!;
    public int C { get; set; }
}