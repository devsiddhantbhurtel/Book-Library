using BookLibrarySystem.Models;

namespace BookLibrarySystem.Services
{
    public interface IBookPublisher
    {
        Task<bool> PublishBookAsync(Book book);
        Task<bool> UnpublishBookAsync(int bookId);
        Task<bool> UpdatePublicationStatusAsync(int bookId, bool isPublished);
        Task<IEnumerable<Book>> GetPublishedBooksAsync();
        Task<IEnumerable<Book>> GetUnpublishedBooksAsync();
    }
}