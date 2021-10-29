using AwsCs.Services.Api.Dto.Request;
using AwsCs.Services.Api.Dto.Response;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Services.Interfaces
{
    public interface IStorageService
    {
        /// <summary>
        /// Search all files in the storage and get the filenames
        /// </summary>
        /// <param name="name">The name of storage</param>
        /// <returns>The collection of filenames</returns>
        public Task<IEnumerable<string>> GetFilenamesAsync(string name);

        /// <summary>
        /// Download one file at the storage
        /// </summary>
        /// <param name="name">The name of storage</param>
        /// <param name="filename">The filename in storage</param>
        /// <returns>The fileStream, name and mimeType</returns>
        public Task<FileDownloadResponse> DowloadFileAsync(string name, string filename);

        /// <summary>
        ///  Download all files at the storage
        /// </summary>
        /// <param name="name">The name of storage</param>
        /// <returns>The fileStream, name and mimeType</returns>
        public Task<FileDownloadResponse> DownloadZipFileAsync(string name);

        /// <summary>
        /// Remove file from storage
        /// </summary>
        /// <param name="name">The name of storage</param>
        /// <param name="filename">The name of file</param>
        public Task RemoveFileAsync(string name, string filename);

        /// <summary>
        /// Remove all files from storage
        /// </summary>
        /// <param name="name">The name of storage</param>
        public Task DumpAsync(string name);

        /// <summary>
        /// Upload a file in storage.
        /// When the subfolter is not empty, the file will be uploaded inside it.
        /// </summary>
        /// <param name="fileUploadPost"></param>
        /// <returns></returns>
        public Task<FileUploadResponse> UploadAsyncAsync(FileUploadPost fileUploadPost);
    }
}
