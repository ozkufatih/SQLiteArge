using Playground.Common.ResultItems;
using Playground.Domain.Dtos;

namespace Playground.Services.Services.Base
{
    public interface IBatchOperationsService<TResult> where TResult : class
    {
        /// <summary>
        /// Retrieves all records from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with the result of the retrieval.</returns>
        Task<TResult> GetAllAsync();
    }
}
