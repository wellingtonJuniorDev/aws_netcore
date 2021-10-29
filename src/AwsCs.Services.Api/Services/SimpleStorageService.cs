using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsCs.Services.Api.Dto.Request;
using AwsCs.Services.Api.Dto.Response;
using AwsCs.Services.Api.Extensions;
using AwsCs.Services.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Services
{
    public class SimpleStorageService : IStorageService
    {
        private AmazonS3Client GetS3Client()
        {
            /*
             * This is only for dev purposes
             * For production environments follow the recommended practices: 
             * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
             */
            var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            return new AmazonS3Client(key, secret, RegionEndpoint.USEast1);
        }

        public async Task<IEnumerable<string>> GetFilenamesAsync(string name)
        {
            using var client = GetS3Client();

            var request = new ListObjectsV2Request { BucketName = name };

            var response = await client.ListObjectsV2Async(request);

            return response.S3Objects.Select(s => s.Key);
        }

        public async Task<FileDownloadResponse> DowloadFileAsync(string name, string filename)
        {
            using var client = GetS3Client();
            var request = new GetObjectRequest
            {
                BucketName = name,
                Key = filename
            };

            using var objectResponse = await client.GetObjectAsync(request);
            using var responseStream = objectResponse.ResponseStream;
            var stream = new MemoryStream();
            await responseStream.CopyToAsync(stream);
            stream.Position = 0;

            return new FileDownloadResponse(stream, filename);
        }

        public async Task<FileDownloadResponse> DownloadZipFileAsync(string name)
        {
            CheckDirectories();
            await DownloadAllFilesAsync(name);

            ZipFile.CreateFromDirectory(
                $"{AppContext.BaseDirectory}\\Files", 
                $"{AppContext.BaseDirectory}\\Zip\\temp.zip");
            
            var stream = new FileStream($"{AppContext.BaseDirectory}\\Zip\\temp.zip", FileMode.Open);
            var filename = $"{name}_{DateTime.Now:dd-MM-yyyy_HHmmss}.zip";

            return new FileDownloadResponse(stream, filename);
        }

        private void CheckDirectories()
        {
            if(Directory.Exists($"{AppContext.BaseDirectory}\\Files"))
            {
                Directory.Delete($"{AppContext.BaseDirectory}\\Files", true);
            }

            if (Directory.Exists($"{AppContext.BaseDirectory}\\Zip"))
            {
                Directory.Delete($"{AppContext.BaseDirectory}\\Zip", true);
            }

            Directory.CreateDirectory($"{AppContext.BaseDirectory}\\Files");
            Directory.CreateDirectory($"{AppContext.BaseDirectory}\\Zip");
        }

        private async Task DownloadAllFilesAsync(string name)
        {
            using var client = GetS3Client();

            var fileTransfer = new TransferUtility(client);
            var request = new ListObjectsV2Request { BucketName = name };

            var response = await client.ListObjectsV2Async(request);
            foreach (S3Object entry in response.S3Objects)
            {
                if (entry.Key.HasExtension())
                {
                    await fileTransfer.DownloadAsync(
                       $"{AppContext.BaseDirectory}\\Files\\{RemoveIllegalFileNameChars(entry.Key)}", name, entry.Key);
                }                
            }
        }
        //TODO: segregar responsabilidade de manusear arquivos
        private string RemoveIllegalFileNameChars(string input, string replacement = "_")
        {
            var regexSearch = (new string(Path.GetInvalidFileNameChars()) + 
                               new string(Path.GetInvalidPathChars()))
                              .Replace("/","");

            var regex = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return regex.Replace(input, replacement);
        }

        public async Task RemoveFileAsync(string name, string filename)
        {
            using var client = GetS3Client();
            var fileTransferUtility = new TransferUtility(client);

            await fileTransferUtility.S3Client
                .DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = name,
                    Key = filename
                });
        }

        public async Task DumpAsync(string name)
        {
            var files = await GetFilenamesAsync(name);

            using var client = GetS3Client();
            var fileTransferUtility = new TransferUtility(client);

            var request = new DeleteObjectsRequest
            {
                BucketName = name
            };

            foreach (var file in files) request.AddKey(file);

            await fileTransferUtility.S3Client
                .DeleteObjectsAsync(request);
        }

        public async Task<FileUploadResponse> UploadAsyncAsync
            (FileUploadPost fileUploadPost)
        {
            var fileStream = fileUploadPost.OpenFileStream();
            var objectRequest = new PutObjectRequest
            {
                BucketName = fileUploadPost.Bucketname,
                CannedACL = S3CannedACL.PublicRead,
                ContentType = fileUploadPost.GetContentType(),
                InputStream = fileStream,
                Key = fileUploadPost.GetPathUpload()
            };
            
            using var client = GetS3Client();
            await client.PutObjectAsync(objectRequest);

            //TODO: remover valor hard coded
            var newFile = string.Format("http://{0}.s3.amazonaws.com/{1}", 
                    objectRequest.BucketName, 
                    objectRequest.Key);

            return new FileUploadResponse(newFile);
        }
    }
}
