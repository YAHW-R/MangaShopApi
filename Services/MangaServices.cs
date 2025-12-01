using MangaShopApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MangaShopApi.Services
{
    public class MangaService : IMangaService
    {
        private readonly IMongoCollection<Manga> _mangas;

        public MangaService(IOptions<MangaStorageDbSettings> mangaStorageDbSettings)
        {
            var connectionString = mangaStorageDbSettings.Value.StringConnection;

            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            var mongoDatabase = client.GetDatabase(
                            mangaStorageDbSettings.Value.DatabaseName);

            _mangas = mongoDatabase.GetCollection<Manga>(
                mangaStorageDbSettings.Value.CollectionName);

        }


        // Obtener todos
        public async Task<List<Manga>> GetAsync() =>
            await _mangas.Find(_ => true).ToListAsync();

        // Obtener uno por ID
        public async Task<Manga?> GetAsync(string id) =>
            await _mangas.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Crear nuevo manga
        public async Task CreateAsync(Manga newManga) =>
            await _mangas.InsertOneAsync(newManga);

        // Actualizar
        public async Task UpdateAsync(string id, Manga updatedManga) =>
            await _mangas.ReplaceOneAsync(x => x.Id == updatedManga.Id, updatedManga);

        // Borrar
        public async Task RemoveAsync(string id) =>
            await _mangas.DeleteOneAsync(x => x.Id == id);
    }
}