using MangaShopApi.Models;


namespace MangaShopApi.Services
{
    public interface IMangaService
    {
        Task<List<Manga>> GetAsync();
        Task<Manga?> GetAsync(string id);
        Task CreateAsync(Manga newManga);
        Task UpdateAsync(string id, Manga updatedManga);
        Task RemoveAsync(string id);
    }
}