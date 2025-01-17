using Supabase;
using Supabase.Gotrue;
using Supabase.Storage;
using Supabase.Storage.Interfaces;
using Client = Supabase.Client;

namespace Gig.Platform.Core.Services
{
    public class SupabaseStorageService
    {
        private readonly Client _client;
        private readonly string _bucketName;
        private readonly string _projectUrl;

        public SupabaseStorageService(string url, string apiKey, string bucketName)
        {
            _client = new Client(url, apiKey);
            _bucketName = bucketName;
            _projectUrl = url;
        }

        public async Task InitializeAsync()
        {
            await _client.InitializeAsync();
        }

        public async Task<string> UploadFileAsync(string fileName, byte[] fileBytes)
        {
            var storage = _client.Storage;
            var bucket = storage.From(_bucketName);

            if (CheckIfFileExists(fileName, bucket).Result)
            {
                await bucket.Remove(new List<string> { fileName });
            }

            var uploadUrl = await bucket.Upload(fileBytes, fileName);

            if (string.IsNullOrEmpty(uploadUrl))
                throw new Exception("Upload failed: Unable to get file URL.");

            return GetPublicUrl(fileName);
        }

        private string GetPublicUrl(string fileName)
        {
            return $"{_projectUrl}/storage/v1/object/public/{_bucketName}/{fileName}";
        }

        private async Task<bool> CheckIfFileExists(string fileName, IStorageFileApi<FileObject> bucket)
        {
            var existingFiles = await bucket.List();
            return existingFiles.Any(f => f.Name == fileName);
        }
    }
}
