using AwsCs.Services.Api.Dto.Request;
using AwsCs.Services.Api.Dto.Response;
using AwsCs.Services.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        private readonly IStorageService _storageService;

        public S3Controller(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Get all names of storage files
        /// </summary>
        /// <param name="bucketname">The name of storage</param>
        /// <returns>The collection of filenames</returns>
        /// <response code="200">The filenames collection</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<string>> GetAsync([FromQuery] string bucketname)
        {
            return await _storageService
                .GetFilenamesAsync(bucketname);
        }

        /// <summary>
        /// Download a specific file at the storage
        /// </summary>
        /// <param name="bucketname">The name of storage</param>
        /// <param name="filename">The filename in the storage</param>
        /// <returns>The downloaded file</returns>
        /// <response code="200">The fileStream result</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpGet("download")]
        [ProducesResponseType(typeof(ZipFile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadAsync([FromQuery] string bucketname, string filename)
        {
            var downloadResponse = await
                _storageService.DowloadFileAsync(bucketname, filename);

            return DownloadFileResponse(downloadResponse);
        }

        /// <summary>
        /// Download all files at the storage as zip
        /// </summary>
        /// <param name="bucketname">The name of storage</param>
        /// <returns>The zip file</returns>
        /// <response code="200">The zip file with all storage files</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpGet("zip")]
        [ProducesResponseType(typeof(ZipFile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadAllFilesAsync([FromQuery] string bucketname)
        {
            var downloadResponse = await
                _storageService.DownloadZipFileAsync(bucketname);

            return DownloadFileResponse(downloadResponse);
        }

        private IActionResult DownloadFileResponse(FileDownloadResponse downloadResponse)
        {
            return File(
                downloadResponse.File,
                downloadResponse.MimeType,
                downloadResponse.Name);
        }

        /// <summary>
        /// Remove file from storage
        /// </summary>
        /// <param name="bucketname">The name of storage</param>
        /// <param name="filename">The filename</param>
        /// <response code="200">The file has been removed</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync([FromQuery] string bucketname, string filename)
        {
            await _storageService
                .RemoveFileAsync(bucketname, filename);
        }

        /// <summary>
        /// Remove all files from storage
        /// </summary>
        /// <param name="bucketname">The name of storage</param>
        /// <response code="200">The storage has been cleaned</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpDelete("dump")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DumpAsync([FromQuery] string bucketname)
        {
            await _storageService.DumpAsync(bucketname);
        }

        /// <summary>
        /// Upload a new file to storage
        /// </summary>
        /// <param name="fileUploadPost">A file, bucketname, and subfolder to upload</param>
        /// <returns>The uri of new file uploaded</returns>
        /// <response code="200">The storage has been cleaned</response>
        /// <response code="400">Invalid params data</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromForm] FileUploadPost fileUploadPost)
        {
            var response = await _storageService
                .UploadAsyncAsync(fileUploadPost);

            return Created(response.Filename, response);
        }
    }
}
