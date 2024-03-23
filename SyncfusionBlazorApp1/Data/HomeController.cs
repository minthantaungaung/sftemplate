using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Blazor.ImageEditor;

namespace SyncfusionBlazorApp1.Data
{

	[ApiController]
	public class HomeController : ControllerBase
	{
		private const string CONNECTIONSTRING = "DefaultEndpointsProtocol=https;AccountName=sitoblob;AccountKey=DfjG5fpdqa5OEzKMAiIINL8CkEuNdL4Dm+z3HHq51bJ2GhJkAoCH/x7q1b16PSM5Tpf4NMhwDPue+ASt4iSaeg==;EndpointSuffix=core.windows.net";

		[HttpPost("[action]")]
		[Route("api/Home/Save")]
		public void Save(IList<IFormFile> UploadFiles)
		{
			try
			{
				foreach (var file in UploadFiles)
				{
					if (UploadFiles != null)
					{
						using (var ms = new MemoryStream())
						{
							file.CopyTo(ms);
						
						// Create a memory stream from the edited file

						// Save to Azure
						string containerName = "sito/rte";

						var blobServiceClient = new BlobServiceClient(CONNECTIONSTRING);
						var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
						var blobClient = containerClient.GetBlobClient("rte/" + "file");

						// Upload the MemoryStream to Azure Blob Storage
						var rslt = blobClient.UploadAsync(ms, true).Result;

						ms.Close();

						var pathToImage = "https://sitoblob.blob.core.windows.net/sito/profileImages/";
						}
					}
				}
			}
			catch (Exception e)
			{
				Response.Clear();
				Response.ContentType = "application/json; charset=utf-8";
				Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
			}
		}
	}

}
