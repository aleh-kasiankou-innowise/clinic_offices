using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Offices.IntegrationTesting;

internal static class TestHelper
{
    internal static FormFile GenerateSampleFormFile()
    {
        var fileSize = 256;
        var fileName = "photo.png";
        
        var data = new byte[fileSize];
        new Random().NextBytes(data);
        var stream = new MemoryStream(data);

        return new FormFile(stream, 0, fileSize, fileName, fileName);
    }
}