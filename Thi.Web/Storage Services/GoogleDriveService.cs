using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v2.Data.File;

namespace Thi.Web
{
    public class GoogleDriveService
    {
        public static DriveService GetDriveService()
        {
            //Scopes for use with the Google Drive API
            string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var keyFile = HttpContext.Current.Server.MapPath("/assets/secret/GoogleServiceCredential.p12");
            var certificate = new X509Certificate2(keyFile, "notasecret", X509KeyStorageFlags.Exportable);

            // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer("455221808943-hn57uegaiiloob18cld7jeoh9s46ulsa@developer.gserviceaccount.com")
                {
                    Scopes = scopes
                }.FromCertificate(certificate));

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Fanslations",
            });

            return service;
        }

        //// <summary>
        /// Create a new Directory.
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// </summary>
        /// <param name="_service">a Valid authenticated DriveService</param>
        /// <param name="_title">The title of the file. Used to identify file or folder name.</param>
        /// <param name="_description">A short description of the file.</param>
        /// <param name="_parent">Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.</param>
        /// <returns></returns>
        public static File createDirectory(DriveService _service, string _title, string _description, string _parent)
        {

            File NewDirectory = null;

            // Create metaData for a new Directory
            File body = new File();
            body.Title = _title;
            body.Description = _description;
            body.MimeType = "application/vnd.google-apps.folder";
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = _parent } };
            try
            {
                FilesResource.InsertRequest request = _service.Files.Insert(body);
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return NewDirectory;
        }

        // tries to figure out the mime type of the file.
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Uploads a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// </summary>
        /// <param name="_service">a Valid authenticated DriveService</param>
        /// <param name="_uploadFile">path to the file to upload</param>
        /// <param name="_parent">Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.</param>
        /// <returns>If upload succeeded returns the File resource of the uploaded file 
        ///          If the upload fails returns null</returns>
        public static string uploadFile(DriveService _service, Stream stream, string fileName, string path)
        {
            File body = new File();
            body.Title = fileName;
            body.Description = "";
            body.MimeType = GetMimeType(fileName);
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = Folder(_service, path) } };

            var request = _service.Files.Insert(body, stream, body.MimeType);
            request.Upload();
            var file = request.ResponseBody;
            return file.Id;
        }

        public static Permission SetOwner(DriveService _service, File file)
        {
            Permission newPermission = new Permission();
            newPermission.Value = "fanslations@gmail.com";
            newPermission.Type = "user";
            newPermission.Role = "owner";
            return _service.Permissions.Insert(newPermission, file.Id).Execute();
        }
        public static string Folder(DriveService _service, string path)
        {
            var currentFolderID = "root";

            var folders = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var folder in folders)
            {
                var fileQuery = _service.Files.List();
                fileQuery.Q = string.Format(@"'{0}' in parents and title = '{1}' and mimeType = 'application/vnd.google-apps.folder'", currentFolderID, folder);

                var fileList = fileQuery.Execute();

                if (fileList.Items.Count == 0)
                {
                    File body = new File();
                    body.Title = folder;
                    body.Description = "";
                    body.MimeType = "application/vnd.google-apps.folder";
                    body.Parents = new List<ParentReference>() { new ParentReference() { Id = currentFolderID } };

                    File file = _service.Files.Insert(body).Execute();

                    // set owner if it a folder in root
                    if (file.Parents.First().IsRoot.GetValueOrDefault())
                    {
                        SetOwner(_service, file);
                    }

                    currentFolderID = file.Id;
                }
                else
                {
                    currentFolderID = fileList.Items.First().Id;
                }
            }
            return currentFolderID;
        }

        /// <summary>
        /// Insert a new permission.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to insert permission for.</param>
        /// <param name="value">
        /// User or group e-mail address, domain name or null for "default" type.
        /// </param>
        /// <param name="type">The value "user", "group", "domain" or "default".</param>
        /// <param name="role">The value "owner", "writer" or "reader".</param>
        /// <returns>The inserted permission, null is returned if an API error occurred</returns>
        public static Permission InsertPermission(DriveService service, String fileId, String value,
            String type, String role)
        {
            Permission newPermission = new Permission();
            newPermission.Value = value;
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return service.Permissions.Insert(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }
    }
}
