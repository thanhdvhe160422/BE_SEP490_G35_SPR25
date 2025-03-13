using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Drive.v3.Data;

namespace Planify_BackEnd.Services.GoogleDrive
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _folderId;

        public GoogleDriveService(IConfiguration configuration)
        {
            _folderId = configuration["GoogleDrive:FolderId"] ?? "root";

            GoogleCredential credential;
            using (var stream = new FileStream(configuration["GoogleDrive:CredentialsPath"], FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
            }

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Planify"
            });
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                // Đảm bảo tên file là unique (tránh ghi đè)
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
                    throw new Exception($"Upload failed: {uploadProgress.Exception.Message}");
                }

                var file = request.ResponseBody;

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
                Console.WriteLine($"Google Drive Upload Error: {ex.Message}");
                return null; // Hoặc throw lỗi tuỳ vào cách bạn xử lý
            }
        }
    }
}
