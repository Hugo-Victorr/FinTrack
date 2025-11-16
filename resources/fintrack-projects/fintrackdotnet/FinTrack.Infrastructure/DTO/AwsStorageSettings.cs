namespace FinTrack.Infrastructure.DTO;

public class AwsStorageSettings
{
    public string BucketName { get; set; } = null!;
    
    public string CloudFrontDomain { get; set; } = null!;
    public string CloudFrontKeyPairId { get; set; } = null!;
    public string CloudFrontPrivateKeyPath { get; set; } = null!;
}