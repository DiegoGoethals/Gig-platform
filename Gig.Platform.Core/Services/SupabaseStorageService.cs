using Supabase;
using Supabase.Storage;
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

            var uploadUrl = await bucket.Upload(fileBytes, fileName);

            if (string.IsNullOrEmpty(uploadUrl))
                throw new Exception("Upload failed: Unable to get file URL.");

            return GetPublicUrl(fileName);
        }

        public string GetPublicUrl(string fileName)
        {
            return $"{_projectUrl}/storage/v1/object/public/{_bucketName}/{fileName}";
        }
    }
}
