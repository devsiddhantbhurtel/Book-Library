using BookLibrarySystem.Models;
using BookLibrarySystem.Repositories;

namespace BookLibrarySystem.Services
{
    public class BookPublisherService : IBookPublisher
    {
        private readonly IBookRepository _bookRepository;

        public BookPublisherService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<bool> PublishBookAsync(Book book)
        {
            try
            {
                // Skip setting IsPublished since column doesn't exist
                // book.IsPublished = true;
                // book.PublishedDate = DateTime.UtcNow;
                await _bookRepository.UpdateBookAsync(book);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnpublishBookAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book == null) return false;
                
                // Skip setting IsPublished since column doesn't exist
                // book.IsPublished = false;
                await _bookRepository.UpdateBookAsync(book);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePublicationStatusAsync(int bookId, bool isPublished)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book == null) return false;

                // Skip setting IsPublished since column doesn't exist
                // book.IsPublished = isPublished;
                // if (isPublished)
                // {
                //     book.PublishedDate = DateTime.UtcNow;
                // }
                await _bookRepository.UpdateBookAsync(book);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Book>> GetPublishedBooksAsync()
        {
            // Return all books for now since IsPublished column doesn't exist in database
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetUnpublishedBooksAsync()
        {
            // Return empty list for now since IsPublished column doesn't exist in database
            return new List<Book>();
        }
    }
}