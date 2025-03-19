﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Drive.v3.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planify_BackEnd.Services.GoogleDrive
{
    public class GoogleDriveService
    {
        private readonly DriveService? _driveService; // Cho phép null nếu không có credentials
        private readonly string _folderId;
        private readonly bool _isInitialized = false;

        public GoogleDriveService(IConfiguration configuration)
        {
            _folderId = configuration["GoogleDrive:FolderId"] ?? "root";
            string credentialsPath = configuration["GoogleDrive:CredentialsPath"];

            if (string.IsNullOrEmpty(credentialsPath) || !System.IO.File.Exists(credentialsPath))
            {
                Console.WriteLine("⚠️ Google Drive credentials.json not found. Google Drive service is disabled.");
                return; // Không throw lỗi, chỉ log cảnh báo
            }

            try
            {
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    var credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
                    _driveService = new DriveService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Planify"
                    });
                    _isInitialized = true; // Đánh dấu đã khởi tạo thành công
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Google Drive initialization failed: {ex.Message}");
            }
        }

        public async Task<string?> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            if (!_isInitialized || _driveService == null)
            {
                Console.WriteLine("🚨 Google Drive Service is not initialized. Upload failed.");
                return null;
            }

            try
            {
                if (fileStream == null || fileStream.Length == 0)
                {
                    throw new ArgumentException("File stream cannot be empty.");
                }

                string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = uniqueFileName,
                    Parents = new List<string> { _folderId }
                };

                var request = _driveService.Files.Create(fileMetadata, fileStream, contentType);
                request.Fields = "id";
                var uploadProgress = await request.UploadAsync();

                if (uploadProgress.Status == UploadStatus.Failed)
                {
                    throw new Exception($"Upload failed: {uploadProgress.Exception?.Message}");
                }

                var file = request.ResponseBody;
                if (file == null || string.IsNullOrEmpty(file.Id))
                {
                    throw new Exception("Upload failed: No file ID returned.");
                }

                // **Gán quyền public**
                var permission = new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };
                await _driveService.Permissions.Create(permission, file.Id).ExecuteAsync();

                return $"https://drive.google.com/uc?id={file.Id}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Google Drive Upload Error: {ex.Message}");
                return null;
            }
        }
    }
}
