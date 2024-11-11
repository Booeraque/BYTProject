using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class PersistenceManager
{
    public static void SaveExtent<T>(List<T> extent, string filename)
    {
        try
        {
            string directory = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Console.WriteLine($"Created directory: {directory}");
            }

            using (StreamWriter file = File.CreateText(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                using (XmlTextWriter writer = new XmlTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    xmlSerializer.Serialize(writer, extent);
                }
            }
            Console.WriteLine($"{typeof(T).Name} data saved successfully to {filename}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving {typeof(T).Name} data to {filename}: {ex.Message}");
        }
    }

    public static List<T> LoadExtent<T>(string filename)
    {
        Console.WriteLine($"Attempting to load {typeof(T).Name} data from {filename}...");

        try
        {
            if (File.Exists(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                using (XmlTextReader reader = new XmlTextReader(filename))
                {
                    var extent = (List<T>)xmlSerializer.Deserialize(reader);
                    Console.WriteLine($"Successfully loaded {extent.Count} items of {typeof(T).Name} data.");
                    return extent;
                }
            }
            else
            {
                Console.WriteLine($"{typeof(T).Name} file not found at {filename}. Returning empty list.");
                return new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading {typeof(T).Name} data from {filename}: {ex.Message}");
            return new List<T>();
        }
    }
}
