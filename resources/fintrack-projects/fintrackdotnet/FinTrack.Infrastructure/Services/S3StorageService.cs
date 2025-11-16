using Amazon.CloudFront;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using FinTrack.Infrastructure.DTO;
using FinTrack.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FinTrack.Infrastructure.Services;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsStorageSettings _settings;

    public S3StorageService(IAmazonS3 s3Client, IOptions<AwsStorageSettings> settings)
    {
        _s3Client = s3Client;
        _settings = settings.Value;
    }

    public string GenerateDownloadUrl(string key, TimeSpan expiresIn)
    {
        var cdnUrl = $"https://{_settings.CloudFrontDomain}/{key}";

        var privateKey = File.OpenText(_settings.CloudFrontPrivateKeyPath);

        try
        {
            var url = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                cdnUrl,
                privateKey,
                _settings.CloudFrontKeyPairId,
                DateTime.UtcNow.Add(expiresIn)
            );
            return url;
        }
        catch (AmazonCloudFrontException ex)
        {
            throw new Exception($"Error generating presigned URL: {ex.Message}");
        }
    }

    public async Task<string> GenerateUploadUrl(string key, TimeSpan expiresIn)
    {
        var req = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiresIn)
        };

        try
        {
            var url = await _s3Client.GetPreSignedURLAsync(req);
            return url;
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Error generating presigned URL: {ex.Message}");
        }
    }
}