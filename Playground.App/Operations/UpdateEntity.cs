using Playground.App.Operations.Base;
using Playground.Common.ResultItems;
using Playground.Domain.Dtos;
using Playground.Domain.Dtos.Base;
using Playground.Utils.ConsoleHelper;
using System.Reflection;

namespace Playground.App.Operations
{
    public class UpdateEntity : Operation
    {
        public void Update(string entityName)
        {
            Console.Clear();
            ConsoleExtensions.ToColorWriteLine($"Main > {entityName} > Update", ConsoleColor.Cyan);
            ConsoleExtensions.ToColorWriteLine($"Updating an existing {entityName}...", ConsoleColor.Cyan);

            ConsoleExtensions.ToColorWrite("Enter the Id (Guid) of the entity to update: ", ConsoleColor.Yellow);
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                ConsoleExtensions.ToColorWriteLine("Invalid Guid format. Please try again.", ConsoleColor.Red);
                return;
            }

            switch (entityName)
            {
                case "Portfolio":
                    UpdateEntityInstance<UpdatePortfolioDto>(_portfolioService.GetByIdAsync, _portfolioService.UpdateAsync, id, entityName);
                    break;
                case "Asset":
                    UpdateEntityInstance<UpdateAssetDto>(_assetService.GetByIdAsync, _assetService.UpdateAsync, id, entityName);
                    break;
                case "AssetType":
                    UpdateEntityInstance<UpdateAssetTypeDto>(_assetTypeService.GetByIdAsync, _assetTypeService.UpdateAsync, id, entityName);
                    break;
                default:
                    ConsoleExtensions.ToColorWriteLine("Invalid entity name!", ConsoleColor.Red);
                    break;
            }

            ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
            Console.ReadLine();
        }

        private void UpdateEntityInstance<T>(Func<Guid, Task<ResultItem<BaseDto>>> getFunc, Func<T, Task<ResultItem<BaseDto>>> updateFunc, Guid id, string entityName) where T : BaseDto, new()
        {
            try
            {
                var fetchResult = getFunc(id).Result;
                CheckResult(entityName, fetchResult);

                var old = fetchResult.Data as GetPortfolioDto;

                var entity = new T { Guid = id };
                FillEntityProperties(entity, old);

                var updateResult = updateFunc(entity).Result;
                CheckUpdateResult(entityName, updateResult);
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
                ConsoleExtensions.ToColorWriteLine($"{entityName} fetched successfully.", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException($"Failed fetching {entityName}: {result.Message}");
            }
        }

        private void CheckUpdateResult(string entityName, ResultItem<BaseDto> result)
        {
            if (result.IsSuccess)
            {
                ConsoleExtensions.ToColorWriteLine($"{entityName} updated successfully. The Id is {result.Data.Guid}", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException($"Failed to update {entityName}: {result.Message}");
            }
        }

        private void FillEntityProperties<T1, T2>(T1 entity, T2 oldEntity)
        {
            foreach (var property in typeof(T1).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                var currentValue = typeof(T2).GetProperty(property.Name)?.GetValue(oldEntity);

                string value;
                do
                {
                    ConsoleExtensions.ToColorWrite($"Enter new value for {property.Name} (current value: {currentValue ?? "N/A"}): ", ConsoleColor.Yellow);
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
                    // Convert input value to the property's type and set it
                    property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
                }
            }
        }
    }
}
