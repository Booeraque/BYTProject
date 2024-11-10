using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class PersistenceManager
{
    // Generic method to save extents (lists of objects) to XML using XmlTextWriter
    public static void SaveExtent<T>(List<T> extent, string filename)
    {
        try
        {
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a StreamWriter for the given path
            using (StreamWriter file = File.CreateText(filename))
            {
                // Create an XmlSerializer instance to serialize List<T>
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

                // Use XmlTextWriter for writing the XML content
                using (XmlTextWriter writer = new XmlTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented; // Optional: Makes the XML more readable
                    xmlSerializer.Serialize(writer, extent);  // Serialize the list to the XML file
                }
            }
            Console.WriteLine($"{typeof(T).Name} data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving {typeof(T).Name} data: {ex.Message}");
        }
    }

    // Generic method to load extents (lists of objects) from XML using XmlTextReader
    public static List<T> LoadExtent<T>(string filename)
    {
        try
        {
            if (File.Exists(filename))
            {
                // Create an XmlSerializer instance to deserialize List<T>
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

                // Use XmlTextReader for reading the XML content
                using (XmlTextReader reader = new XmlTextReader(filename))
                {
                    return (List<T>)xmlSerializer.Deserialize(reader);  // Deserialize the XML file into the list
                }
            }
            else
            {
                Console.WriteLine($"No {typeof(T).Name} file found. Starting fresh.");
                return new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading {typeof(T).Name} data: {ex.Message}");
            return new List<T>();
        }
    }
}
