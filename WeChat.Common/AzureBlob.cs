using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Common
{
    public class AzureBlob
    {
        private string _conn { get; set; }
        private string _containerName { get; set; }

        private string _blobEndpoint { get; set; }

        private bool IsUseLocal = false;

        private string FileLocalPath = string.Empty;

        private CloudBlobContainer container;
        public AzureBlob(string conn, string containerName, string blobEndpoint)
        {
            bool tempV = false;
            if(bool.TryParse(CloudConfigHelper.GetSetting("UseLocalBlob"),out tempV))
            {
                IsUseLocal = tempV;
            }

            if (!IsUseLocal)
            {
                this._conn = conn;
                this._containerName = containerName;
                this._blobEndpoint = blobEndpoint.TrimEnd('/');
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._conn);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
                container = blobClient.GetContainerReference(containerName);
                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });
            }
            else
            {
                this._conn = conn.TrimEnd('/').TrimEnd('\\');
                this._containerName = containerName;
                this._blobEndpoint = blobEndpoint.TrimEnd('/').TrimEnd('\\');

                FileLocalPath = string.Format(@"{0}\{1}\", this._conn, this._containerName);

            }

        }

        public string SaveFile(string fileName, byte[] fileContent)
        {
            if (!IsUseLocal)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.UploadFromByteArray(fileContent, 0, fileContent.Length);
                return string.Format("{0}/{1}/{2}", this._blobEndpoint, this._containerName, fileName);
            }
            else
            {
                string targetfileName = string.Format(@"{0}\{1}", FileLocalPath, fileName);
                File.WriteAllBytes(targetfileName, fileContent);
                return string.Format("{0}/{1}/{2}", this._blobEndpoint, this._containerName, fileName);
            }
        }

        public string[] ListFile()
        {
            if (!IsUseLocal)
            {
                List<string> Result = new List<string>();
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        Result.Add(blob.Uri.ToString());
                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {

                    }
                    else if (item.GetType() == typeof(CloudBlobDirectory))
                    {

                    }
                }

                return Result.ToArray();
            }
            else
            { 
                return new string[0];
            }
        }
    }
}
