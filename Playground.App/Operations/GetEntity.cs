using Playground.App.Operations.Base;
using Playground.Common.ResultItems;
using Playground.Domain.Dtos.Base;
using Playground.Utils.ConsoleHelper;

namespace Playground.App.Operations
{
    public class GetEntity : Operation
    {
        public void Get(string entityName)
        {
            Console.Clear();
            ConsoleExtensions.ToColorWriteLine($"Main > {entityName} > Get", ConsoleColor.Cyan);
            ConsoleExtensions.ToColorWriteLine($"Getting a {entityName}...", ConsoleColor.Cyan);

            ConsoleExtensions.ToColorWrite("Enter the Id (Guid): ", ConsoleColor.Yellow);
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                ConsoleExtensions.ToColorWriteLine("Invalid Guid format. Please try again.", ConsoleColor.Red);
                return;
            }

            switch (entityName)
            {
                case "Portfolio":
                    FetchEntity(_portfolioService.GetByIdAsync, id, entityName);
                    break;
                case "Asset":
                    FetchEntity(_assetService.GetByIdAsync, id, entityName);
                    break;
                case "AssetType":
                    FetchEntity(_assetTypeService.GetByIdAsync, id, entityName);
                    break;
                default:
                    ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                    break;
            }

            ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
            Console.ReadLine();
        }

        private void FetchEntity<T>(Func<Guid, Task<ResultItem<T>>> fetchFunc, Guid id, string entityName) where T : BaseDto
        {
            try
            {
                var result = fetchFunc(id).Result;
                CheckResult(entityName, result);
                DisplayEntity(result.Data);
            }
            catch (Exception ex)
            {
                ConsoleExtensions.ToColorWriteLine(ex.Message, ConsoleColor.Red);
                ConsoleExtensions.ToColorWriteLine("Try again!", ConsoleColor.Red);
            }
        }

        private void CheckResult<T>(string entityName, ResultItem<T> result)
        {
            if (result.IsSuccess)
            {
                ConsoleExtensions.ToColorWriteLine($"{entityName} retrieved successfully.", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException($"Failed fetching {entityName}: {result.Message}");
            }
        }

        private void DisplayEntity<T>(T entity)
        {
            if (entity != null)
            {
                ConsoleExtensions.ToColorWriteLine(entity.ToString(), ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.ToColorWriteLine("Entity not found.", ConsoleColor.Yellow);
            }
        }
    }
}
