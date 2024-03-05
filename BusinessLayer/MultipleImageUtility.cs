using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MultipleImageUtility
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MultipleImageUtility(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string[] SaveBase64Images(string base64Strings)
        {
            try
            {
                // Split the comma-separated base64 strings
                string[] base64Array = base64Strings.Split('|');

                // Initialize an array to store file names
                string[] savedFileNames = new string[base64Array.Length];

                for (int i = 0; i < base64Array.Length; i++)
                {
                    string base64String = base64Array[i];
                    if (!string.IsNullOrEmpty(base64String))
                    {
                        // Extract Base64 data from the string
                        var match = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)");
                        if (!match.Success)
                        {
                            savedFileNames[i] = null; // Add null for invalid base64 string
                            continue;
                        }

                        string base64Data = match.Groups["data"].Value;

                        // Decode Base64 data
                        byte[] imageBytes = Convert.FromBase64String(base64Data);

                        // Get web root path and generate file path
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string fileName = $"{Guid.NewGuid().ToString()}.jpg"; // Generate unique file name
                        string filePath = Path.Combine(wwwrootPath, "productimages", fileName);

                        // Check if the directory exists, if not create it
                        string directoryPath = Path.GetDirectoryName(filePath)!;
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Save byte array as image file
                        File.WriteAllBytes(filePath, imageBytes);

                        savedFileNames[i] = fileName; // Store file name in array
                    }
                    else
                    {
                        // Handle case where base64 string is empty
                        savedFileNames[i] = null;
                    }
                }

                return savedFileNames; // Return array of file names
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during image saving
                Console.WriteLine($"Error saving images: {ex.Message}");
                return null;
            }
        }
    }
}