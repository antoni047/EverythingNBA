namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;

    public interface IImageInfoWriterService
    {
        Task<int> WriteToDbAsync(string imageURL,/* string imageThumbnailURL,*/ string publicId, long imageLenght);
    }
}