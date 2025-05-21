using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class CryptoManager
{
    private static readonly string ConfigDir = Path.Combine(AppContext.BaseDirectory, "CryptoSoft_config");
    private static readonly string KeyFile = Path.Combine(ConfigDir, "key.bin");



    /// <summary>
    /// Définit la clé d'encryptage (protégée et stockée localement).
    /// </summary>
    public static void SetKey(string password)
    {
        try
        {
            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);

            byte[] keyBytes = Encoding.UTF8.GetBytes(password);

            // Protège la clé texte en clair
            byte[] protectedKey = ProtectedData.Protect(keyBytes, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(KeyFile, protectedKey);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public static byte[] GetKey()
    {
        try
        {
            if (!File.Exists(KeyFile))
                throw new FileNotFoundException("La clé n'a pas encore été définie.");

            byte[] protectedKey = File.ReadAllBytes(KeyFile);
            return ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);
        }
        catch
        {
            throw new Exception();
        }
    }

    public static string GetKeyString()
    {
        try
        {
            byte[] keyBytes = GetKey();
            return Encoding.UTF8.GetString(keyBytes);
        }
        catch
        {
            throw new Exception();
        }
    }



    /// <summary>
    /// Chiffre un fichier avec la clé configurée.
    /// </summary>
    public static void EncryptFile(string filePath)
    {
        try
        {
            byte[] keyBytes = GetKey();

            string outputPath = filePath + ".xor";
            byte[] dataBytes = File.ReadAllBytes(filePath);

            for (int i = 0; i < dataBytes.Length; i++)
            {
                dataBytes[i] ^= keyBytes[i % keyBytes.Length];
            }
            File.WriteAllBytes(outputPath, dataBytes);
        }
        catch
        {
            throw new Exception();
        }
        
    }

    public static bool CompareFile(string fileSource, string fileTarget)
    {
        try
        {
            byte[] keyBytes = GetKey();

            fileTarget = fileTarget + ".xor";
            byte[] dataSourceBytes = File.ReadAllBytes(fileSource);
            byte[] dataTargetBytes = File.ReadAllBytes(fileTarget);

            for (int i = 0; i < dataTargetBytes.Length; i++)
            {
                dataTargetBytes[i] ^= keyBytes[i % keyBytes.Length];
            }
            return dataTargetBytes.SequenceEqual(dataSourceBytes);
        }
        catch
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// Déchiffre un fichier .enc (si besoin pour vérification).
    /// </summary>
    public static void DecryptFile(string encryptedFilePath, string outputFilePath)
    {
        try
        {
            byte[] keyBytes = GetKey();
            byte[] dataFile = File.ReadAllBytes(encryptedFilePath);

            for (int i = 0; i < dataFile.Length; i++)
            {
                dataFile[i] ^= keyBytes[i % keyBytes.Length];
            }
            File.WriteAllBytes(outputFilePath, dataFile);
        }
        catch
        {
            throw new Exception();
        }
    }
}