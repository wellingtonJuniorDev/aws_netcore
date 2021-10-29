namespace AwsCs.Services.Api.Dto.Response
{
    public class FileUploadResponse
    {
        public FileUploadResponse(string filename)
        {
            Filename = filename;
        }

        /// <summary>
        /// The filename has been generated
        /// </summary>
        /// <example>http://my-bucket.s3.amazonaws.com/my-file.xls</example>
        public string Filename { get; set; }
    }
}
