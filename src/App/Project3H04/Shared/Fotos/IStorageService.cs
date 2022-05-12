using System;
using System.Threading.Tasks;

namespace Project3H04.Shared.Fotos {
    public interface IStorageService {
        string StorageBaseUri { get; }
        Uri CreateUploadUri(string filename);
        Task DeleteImage(string filename);
    }
}
