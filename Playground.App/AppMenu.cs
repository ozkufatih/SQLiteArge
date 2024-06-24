namespace Playground.App
{
    using Playground.App.Operations;
    using Playground.Domain.Dtos;
    using Playground.Utils.ConsoleHelper;
    using System;
    using System.Collections.Generic;

    public class AppMenu
    {
        private Dictionary<int, Action> mainMenuActions;
        private Dictionary<int, string> mainMenuOptions;

        private readonly CreateEntity createEntity = new();
        private readonly GetAllEntities getAllEntities = new();
        private readonly GetEntity getEntity = new();
        private readonly UpdateEntity updateEntity = new();
        private readonly DeleteEntity deleteEntity = new();

        public static List<GetPortfolioDto> portfolios = null;
        public static GetPortfolioDto selectedPortfolio = null;

        public AppMenu()
        {
            mainMenuOptions = new Dictionary<int, string>
            {
                {1, "Portfolio"},
                {2, "Asset"},
                {3, "AssetType"},
                {4, "Change Portfolio"},
                {5, "View Portfolio Assets"}
            };

            mainMenuActions = new Dictionary<int, Action>
            {
                {1, () => ShowEntityMenu("Portfolio")},
                {2, () => ShowEntityMenu("Asset")},
                {3, () => ShowEntityMenu("AssetType")},
                {4, ChangePortfolio},
                {5, ViewPortfolioAssets}
            };
        }

        // Display Main Menu

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();

                CheckInitialPortfolio();

                ConsoleExtensions.ToColorWriteLine("Main", ConsoleColor.Cyan);
                ConsoleExtensions.ToColorWriteLine($"Selected Portfolio: {selectedPortfolio.Name}", ConsoleColor.Cyan);

                foreach (var option in mainMenuOptions)
                {
                    ConsoleExtensions.ToColorWriteLine($"{option.Key}. {option.Value}", ConsoleColor.Green);
                }

                ConsoleExtensions.ToColorWriteLine("Enter the number of the entity or type '0' to quit:", ConsoleColor.Yellow);

                var key = Console.ReadLine();
                if (key == "0") break;

                if (int.TryParse(key.ToString(), out int choice) && mainMenuActions.ContainsKey(choice))
                {
                    mainMenuActions[choice].Invoke();
                }
                else
                {
                    InvalidChoice();
                }

                Console.Clear();
            }
        }

        // Display Entity Menu

        private void ShowEntityMenu(string entityName)
        {
            var actions = new Dictionary<int, Action>
            {
                {1, () => createEntity.Create(entityName)},
                {2, () => updateEntity.Update(entityName)},
                {3, () => getEntity.Get(entityName)},
                {4, () => getAllEntities.GetAll(entityName)},
                {5, () => deleteEntity.Delete(entityName)}
            };

            var actionNames = new Dictionary<int, string>
            {
                {1, "Create"},
                {2, "Update"},
                {3, "Get"},
                {4, "GetAll"},
                {5, "Delete"},
                {6, "Go Back"}
            };

            while (true)
            {
                Console.Clear();

                ConsoleExtensions.ToColorWriteLine($"Main > {entityName}", ConsoleColor.Cyan);
                ConsoleExtensions.ToColorWriteLine($"{entityName} Menu", ConsoleColor.Cyan);

                foreach (var action in actionNames)
                {
                    ConsoleExtensions.ToColorWriteLine($"{action.Key}. {action.Value}", ConsoleColor.Green);
                }

                ConsoleExtensions.ToColorWriteLine("Enter the number of the action:", ConsoleColor.Yellow);

                var key = Console.ReadLine();
                if (key == actionNames.Keys.Count.ToString()) break;

                if (int.TryParse(key.ToString(), out int choice) && actions.ContainsKey(choice))
                {
                    actions[choice].Invoke();
                }
                else
                {
                    InvalidChoice();
                }
            }
        }

        // Check if there are any portfolios and if not, create one

        private void CheckInitialPortfolio()
        {
            if (portfolios == null)
            {
                getAllEntities.GetAll("Portfolio", list: false);
            }

            if (portfolios.Count == 0)
            {
                ConsoleExtensions.ToColorWriteLine($"No portfolios found.", ConsoleColor.Yellow);
                ConsoleExtensions.ToColorWriteLine("You need to create a portfolio.", ConsoleColor.Red);
                ConsoleExtensions.ToColorWriteLine("Press Enter to continue...", ConsoleColor.Gray);
                Console.ReadLine();
                createEntity.Create("Portfolio"); // Redirect user to create a portfolio
                getAllEntities.GetAll("Portfolio", list: false);
                Console.Clear();
            }

            if (selectedPortfolio == null && portfolios.Count > 0)
            {
                ChangePortfolio();
            }
        }

        // Change Portfolio

        private void ChangePortfolio()
        {
            int selectedPortfolioIndex = -1;

            do
            {
                getAllEntities.GetAll("Portfolio", list: false);
                Console.Clear();
                ConsoleExtensions.ToColorWriteLine("Portfolios:", ConsoleColor.Cyan);
                for (int i = 0; i < portfolios.Count; i++)
                {
                    ConsoleExtensions.ToColorWriteLine($"{i + 1}. {portfolios[i].Name}", ConsoleColor.Green);
                }
                ConsoleExtensions.ToColorWriteLine("Select a Portfolio:", ConsoleColor.Yellow);

                var selectedKey = Console.ReadLine();
                if (int.TryParse(selectedKey, out int index) && index > 0 && index <= portfolios.Count)
                {
                    selectedPortfolioIndex = index - 1;
                }
                else
                {
                    InvalidChoice();
                }
            }
            while (selectedPortfolioIndex < 0 || selectedPortfolioIndex >= portfolios.Count);

            selectedPortfolio = portfolios[selectedPortfolioIndex];
            Console.Clear();
        }

        // View Portfolio Assets

        private void ViewPortfolioAssets()
        {
            Console.Clear();

            ConsoleExtensions.ToColorWriteLine($"Assets in Portfolio: {selectedPortfolio.Name}", ConsoleColor.Cyan);

            // Fetch and display assets for the selected portfolio
            getAllEntities.GetAll("PortfolioAsset", selectedPortfolio.Guid);
        }


        // Invalid Choice

        private void InvalidChoice()
        {
            Console.Clear();
            ConsoleExtensions.ToColorWriteLine("Invalid choice. Press Enter to try again.", ConsoleColor.Red);
            Console.ReadLine();
        }
    }
}
