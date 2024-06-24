using Playground.App.Operations.Base;
using Playground.Common.ResultItems;
using Playground.Domain.Dtos;
using Playground.Domain.Dtos.Base;
using Playground.Utils.ConsoleHelper;
using System.Reflection;

public class CreateEntity : Operation
{
    public void Create(string entityName)
    {
        Console.Clear();
        ConsoleExtensions.ToColorWriteLine($"Main > {entityName} > Create", ConsoleColor.Cyan);
        ConsoleExtensions.ToColorWriteLine($"Creating a new {entityName}...", ConsoleColor.Green);

        switch (entityName)
        {
            case "Portfolio":
                CreateEntityInstance(new CreatePortfolioDto(), entityName, _portfolioService.CreateAsync);
                break;
            case "Asset":
                CreateEntityInstance(new CreateAssetDto(), entityName, _assetService.CreateAsync);
                break;
            case "AssetType":
                CreateEntityInstance(new CreateAssetTypeDto(), entityName, _assetTypeService.CreateAsync);
                break;
            default:
                ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                break;
        }

        ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
        Console.ReadLine();
    }

    private void CreateEntityInstance<T>(T entity, string entityName, Func<T, Task<ResultItem<BaseDto>>> createFunc)
    {
        try
        {
            FillEntityProperties(entity);
            var result = createFunc(entity).Result;
            CheckResult(entityName, result);
        }
        catch (Exception ex)
        {
            ConsoleExtensions.ToColorWriteLine(ex.Message, ConsoleColor.Red);
            ConsoleExtensions.ToColorWriteLine("Try again!", ConsoleColor.Red);
        }
    }

    private void CheckResult(string entityName, ResultItem<BaseDto> result)
    {
        if (result.IsSuccess)
        {
            ConsoleExtensions.ToColorWriteLine($"{entityName} created successfully. The Id is {result.Data.Guid}", ConsoleColor.Green);
        }
        else
        {
            throw new ArgumentException($"Failed to create {entityName}: {result.Message}");
        }
    }

    private void FillEntityProperties<T>(T entity)
    {
        foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            string value;
            do
            {
                ConsoleExtensions.ToColorWrite($"Enter {property.Name}: ", ConsoleColor.Yellow);
                value = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(value));

            if (property.PropertyType == typeof(Guid))
            {
                if (Guid.TryParse(value, out Guid guidValue))
                {
                    property.SetValue(entity, guidValue);
                }
                else
                {
                    throw new ArgumentException("Invalid Guid format. Please try again.");
                }
            }
            else
            {
                property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
            }
        }
    }
}