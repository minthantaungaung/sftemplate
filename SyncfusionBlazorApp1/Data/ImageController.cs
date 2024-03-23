using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace SyncfusionBlazorApp1.Data
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		[HttpPost("[action]")]
        public async Task Save(IList<IFormFile> UploadFiles)
		{
			try
			{
				string blobUrl = "";
				foreach (var file in UploadFiles)
				{
					const string accountName = "sitodev";
					const string key = "StZmbVEwJyjQVShmtCLv+lTsz0hqRu/+TQKEAm+aKNHHwX9cP3WrdVu5myJkilN0DkaH0dDZYfid+AStoOIwwA==";

					var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, key), true);

					var blobClient = storageAccount.CreateCloudBlobClient();
					var container = blobClient.GetContainerReference("fileupload"); // Provide your container name
					await container.CreateIfNotExistsAsync();
					await container.SetPermissionsAsync(new BlobContainerPermissions()
					{
						PublicAccess = BlobContainerPublicAccessType.Blob
					});

					var blob = container.GetBlockBlobReference(file.FileName);
					using (var stream = file.OpenReadStream())
					{
						await blob.UploadFromStreamAsync(stream);
					}
					blobUrl = blob.Uri.ToString();
				}
				Response.StatusCode = 200;
			}
			catch (Exception e)
			{
				Response.Clear();
				Response.StatusCode = 204;
				Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
				Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
			}
		}
	}
}
