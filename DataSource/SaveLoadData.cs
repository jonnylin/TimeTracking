using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataSource
{
    public class SaveLoadData
    {
        private StorageFolder _currentFolder;

        public bool saveExists { get; set; }

        public StorageFolder CurrentFolder
        {
            get { return _currentFolder; }
            set
            {
                _currentFolder = value;
            }
        }

        public SaveLoadData()
        {
            LoadDocumentsLibrary();
        }

        public void LoadDocumentsLibrary()
        {
            //CurrentFolder = KnownFolders.DocumentsLibrary;
            CurrentFolder = ApplicationData.Current.RoamingFolder;
        }

        public async Task<StorageFile> CreateFile(string subFolderName, string fileName)
        {
            if (CurrentFolder != null)
            {
                var folder = await CurrentFolder.CreateFolderAsync(subFolderName, CreationCollisionOption.OpenIfExists);
                return await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask();
            }
            else
                return null;
        }

        public async Task<bool> GetFile(string subFolderName, string fileName)
        {
            try
            {
                var folder = await CurrentFolder.GetFolderAsync(subFolderName);
                saveExists = await folder.GetFileAsync(fileName) != null;
            }
            catch (Exception)
            {
                saveExists = false;
            }

            return saveExists;
        }

        public async Task<string> LoadFile(string subFolderName, string fileName)
        {
            string _fileContents = "";

            var folder = await CurrentFolder.GetFolderAsync(subFolderName);

            StorageFile selectedFile = await folder.GetFileAsync(fileName);
            _fileContents = await FileIO.ReadTextAsync(selectedFile);

            return _fileContents;
        }
    }
}
