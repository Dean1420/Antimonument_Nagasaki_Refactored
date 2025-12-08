
// C# program to add elements to the hashtable
using System;
using System.Collections.Generic;

// streams
using System.IO;

// unity
using UnityEngine;

namespace LocalStorageOperations
{
    public static class TextFile
    {
        public static Dictionary<string, string> LoadLinesByKeyValue(string relativePath, string separator)
        {
            // construct absolute path starting from Assets/
            string fullPath = Path.Combine(Application.dataPath, relativePath);
            if (!File.Exists(fullPath))
            {
                Debug.Log("LOCAL STORAGE >>> file not found at: " + fullPath);
                return null;
            }

            // initialize return value
            Dictionary<string, string> data = new Dictionary<string, string>();

            try
            {
                // read all lines
                string[] lines = File.ReadAllLines(fullPath);

                // parse key value pairs for each line
                foreach (string line in lines)
                {
                    // skip empty
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(separator, StringSplitOptions.None);

                    if (parts.Length != 2)
                    {
                    Debug.LogWarning($"LOCAL STORAGE >>> invalid line format: " + line);
                    continue;
                    }
                    
                    // remove white space
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    // add if key does not exist
                    if (data.ContainsKey(key))
                    {
                        Debug.LogWarning($"LOCAL STORAGE >>> duplicate key found: " + key);
                        continue;
                    }

                    data.Add(key, value);
                    Debug.Log($"LOCAL STORAGE >>> key: " + key + "\t\tvalue: " + value);

                }

                Debug.Log($"LOCAL STORAGE >>> Loaded {data.Count} entries from {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"LOCAL STORAGE >>> error reading file:" + e.Message);
                return null;
            }

            return data;
        }
    }
}