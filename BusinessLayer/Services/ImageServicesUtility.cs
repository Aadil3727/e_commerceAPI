using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace DataFirstApproach.Utility.Image
{
    public class ImageServicesUtility
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageServicesUtility(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string SaveBase64Image(string base64String)
        {
            try
            {
                if (!string.IsNullOrEmpty(base64String))
                {
                    // Extract Base64 data from the string
                    var match = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)");
                    if (!match.Success)
                    {
                        throw new ArgumentException("Invalid Base64 string format");
                    }

                    string base64Data = match.Groups["data"].Value;

                    // Decode Base64 data
                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    // Get web root path and generate file path
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string fileName = $"{Guid.NewGuid().ToString()}.jpg"; // Generate unique file name
                    string filePath = Path.Combine(wwwrootPath, "images", fileName);

                    // Check if the directory exists, if not create it
                    string directoryPath = Path.GetDirectoryName(filePath)!;
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save byte array as image file
                    File.WriteAllBytes(filePath, imageBytes);

                    return fileName;
                }
                else
                {
                    // Handle case where base64 string is empty
                    return null!;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during image saving
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null!;
            }
        }
    }
}