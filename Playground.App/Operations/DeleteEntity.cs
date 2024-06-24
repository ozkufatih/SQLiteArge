using Playground.App.Operations.Base;
using Playground.Common.ResultItems;
using Playground.Domain.Dtos.Base;
using Playground.Utils.ConsoleHelper;

namespace Playground.App.Operations
{
    public class DeleteEntity : Operation
    {
        public void Delete(string entityName)
        {
            Console.Clear();
            ConsoleExtensions.ToColorWriteLine($"Main > {entityName} > Delete", ConsoleColor.Cyan);
            ConsoleExtensions.ToColorWriteLine($"Deleting a {entityName}...", ConsoleColor.Cyan);

            ConsoleExtensions.ToColorWrite("Enter the Id (Guid) of the entity to delete: ", ConsoleColor.Yellow);
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                ConsoleExtensions.ToColorWriteLine("Invalid Guid format. Please try again.", ConsoleColor.Red);
                return;
            }

            switch (entityName)
            {
                case "Portfolio":
                    DeleteEntityInstance(_portfolioService.DeleteAsync, id, entityName);
                    break;
                case "Asset":
                    DeleteEntityInstance(_assetService.DeleteAsync, id, entityName);
                    break;
                case "AssetType":
                    DeleteEntityInstance(_assetTypeService.DeleteAsync, id, entityName);
                    break;
                default:
                    ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                    break;
            }

            ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
            Console.ReadLine();
        }

        private void DeleteEntityInstance(Func<Guid, Task<ResultItem<BaseDto>>> deleteFunc, Guid id, string entityName)
        {
            try
            {
                var deleteResult = deleteFunc(id).Result;
                CheckDeleteResult(entityName, deleteResult);
            }
            catch (Exception ex)
            {
                ConsoleExtensions.ToColorWriteLine(ex.Message, ConsoleColor.Red);
                ConsoleExtensions.ToColorWriteLine("Try again!", ConsoleColor.Red);
            }
        }

        private void CheckDeleteResult(string entityName, ResultItem<BaseDto> result)
        {
            if (result.IsSuccess)
            {
                ConsoleExtensions.ToColorWriteLine($"{entityName} deleted successfully.", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException($"Failed to delete {entityName}: {result.Message}");
            }
        }
    }
}
