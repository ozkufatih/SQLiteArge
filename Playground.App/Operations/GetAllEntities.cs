using Playground.App.Operations.Base;
using Playground.App;
using Playground.Common.ResultItems;
using Playground.Utils.ConsoleHelper;

public class GetAllEntities : Operation
{
    public void GetAll(string entityName, bool list = true)
    {
        Console.Clear();
        ConsoleExtensions.ToColorWriteLine($"Main > {entityName} > GetAll", ConsoleColor.Cyan);
        ConsoleExtensions.ToColorWriteLine($"Retrieving all {entityName.ToLower()}s...", ConsoleColor.Cyan);

        switch (entityName)
        {
            case "Portfolio":
                FetchAndDisplayEntities(_portfolioService.GetAllAsync, entityName, list, data => AppMenu.portfolios = data);
                break;
            case "Asset":
                FetchAndDisplayEntities(_assetService.GetAllAsync, entityName, list);
                break;
            case "AssetType":
                FetchAndDisplayEntities(_assetTypeService.GetAllAsync, entityName, list);
                break;
            default:
                ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                break;
        }

        if (list)
        {
            ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
            Console.ReadLine();
        }
    }

    public void GetAll(string entityName, Guid guid)
    {
        ConsoleExtensions.ToColorWriteLine($"Retrieving all {entityName.ToLower()}s...", ConsoleColor.Cyan);

        switch (entityName)
        {
            case "PortfolioAsset":
                FetchAndDisplayEntities(() => _portfolioService.GetAssetsByPortfolioIdAsync(guid), entityName, true);
                break;
            default:
                ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                break;
        }

        ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
        Console.ReadLine();
    }

    private void FetchAndDisplayEntities<T>(Func<Task<ResultItem<List<T>>>> fetchFunc, string entityName, bool list, Action<List<T>> onSuccess = null)
    {
        try
        {
            var result = fetchFunc().Result;
            CheckResult(entityName, result);
            onSuccess?.Invoke(result.Data);

            if (list)
            {
                if (result.Data == null || !result.Data.Any())
                {
                    ConsoleExtensions.ToColorWriteLine($"No {entityName.ToLower()}s found.", ConsoleColor.Yellow);
                }
                else
                {
                    DisplayEntities(entityName, result.Data);
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleExtensions.ToColorWriteLine(ex.Message, ConsoleColor.Red);
            ConsoleExtensions.ToColorWriteLine("Try again!", ConsoleColor.Red);
        }
    }

    private void CheckResult<T>(string entityName, ResultItem<List<T>> result)
    {
        if (result.IsSuccess)
        {
            ConsoleExtensions.ToColorWriteLine($"{entityName}s retrieved successfully.", ConsoleColor.Green);
        }
        else
        {
            throw new ArgumentException($"Failed fetching {entityName}s: {result.Message}");
        }
    }

    private void DisplayEntities<T>(string entityName, IEnumerable<T> entities)
    {
        if (entities != null && entities.Any())
        {
            foreach (var entity in entities)
            {
                ConsoleExtensions.ToColorWriteLine(entity.ToString(), ConsoleColor.Green);
            }
        }
        else
        {
            ConsoleExtensions.ToColorWriteLine($"No {entityName.ToLower()}s found.", ConsoleColor.Yellow);
        }
    }
}