namespace Inventory_App
{
    class Program
    {
        static List<MeatItem> _meatItems = new List<MeatItem>(); // List for meat items
        static List<Produce> _produceList = new List<Produce>(); // List for produce items
        public static Random RandomBarcode = new Random();
        static List<CartItem> _shoppingCart = new List<CartItem>(); // Shopping cart

        public static string GenerateBarcodeId() // Method for random number generation
        {
            return RandomBarcode.Next(100000, 999999).ToString(); // Limits the random number to 6 digits
        }

        static void Main(string[] args) // Main method
        {
            InitializeProduceInventory(); // Initialize produce inventory
            InitializeMeatInventory(); // Initialize meats inventory

            bool running = true;
            while (running)
            {
                Console.Clear(); // Clear the previous screen
                Console.WriteLine("========= Welcome to QuiqChange Console ===========");
                Console.WriteLine("------------ What would you like to do? -----------");
                Console.WriteLine("1. Shop Butcher Products");
                Console.WriteLine("2. Shop Produce Section");
                Console.WriteLine("3. View Shopping Cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Exit the Store");

                // Main menu options
                switch (Console.ReadLine())
                {
                    case "1":
                        ShopMeat();
                        break;
                    case "2":
                        ShopProduce();
                        break;
                    case "3":
                        ViewShoppingCart(); // View shopping cart
                        break;
                    case "4":
                        Checkout(); // Checkout process
                        running = false;
                        break;
                    case "5":
                        running = false; // Exit
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public class MeatItem //Glory
        {
            public string Name { get; }
            public string Description { get; }
            public decimal Stock { get; private set; }
            public decimal Price { get; }

            public MeatItem(string name, string description, decimal stock, decimal price)
            {
                Name = name;
                Description = description;
                Stock = stock;
                Price = price;
            }

            public bool PurchaseItem(decimal quantity)
            {
                if (quantity <= Stock)
                {
                    Stock -= quantity; // Deduct from stock
                    return true; // Purchase successful
                }
                return false; // Not enough stock
            }
        }

        public class Produce //Dan
        {
            public string Name { get; }
            public string Description { get; }
            public decimal Stock { get; private set; }
            public decimal Price { get; }
            public decimal AverageWeight { get; } // Average weight of one item

            public Produce(string name, string description, decimal stock, decimal price, decimal averageWeight)
            {
                Name = name;
                Description = description;
                Stock = stock;
                Price = price;
                AverageWeight = averageWeight; // Set average weight for calculations
            }

            public bool PurchaseItem(decimal weight)
            {
                if (weight <= Stock)
                {
                    Stock -= weight; // Deduct from stock
                    return true; // Purchase successful
                }
                return false; // Not enough stock
            }
        }

        public class CartItem //Juan
        {
            public MeatItem MeatItem { get; }
            public Produce ProduceItem { get; }
            public decimal Quantity { get; }
            public string Barcode { get; }
            public string Description { get; }
            public bool IsProduce { get; }

            // Constructor for MeatItem - Juan , Paul
            public CartItem(MeatItem meatItem, decimal quantity, string barcode, string description)
            {
                MeatItem = meatItem;
                Quantity = quantity;
                Barcode = barcode;
                Description = description;
                IsProduce = false; // Indicates this is a meat item
            }

            // Constructor for Produce - Juan, Paul
            public CartItem(Produce produceItem, decimal quantity, string barcode, string description)
            {
                ProduceItem = produceItem;
                Quantity = quantity;
                Barcode = barcode;
                Description = description;
                IsProduce = true; // Indicates this is a produce item
            }
        }

        // Initialize meat inventory
        static void InitializeMeatInventory() 
        {
            _meatItems.Add(new MeatItem("Chicken Breasts", "Free run chicken meat", 20m, 5.99m));
            _meatItems.Add(new MeatItem("Ground Beef", "100% grass-fed ground beef", 15m, 8.99m));
            _meatItems.Add(new MeatItem("Pork Chops", "Organic pork chops", 10m, 7.49m));
            _meatItems.Add(new MeatItem("Salmon Fillets", "Wild-caught salmon", 8m, 12.99m));
            _meatItems.Add(new MeatItem("Turkey Breast", "Lean turkey breast", 25m, 6.99m));
        }

        // Initialize produce inventory
        static void InitializeProduceInventory() 
        {
            _produceList.Add(new Produce("Apple", "Fresh and crispy red apples.", 20m, 2.99m, 0.25m));
            _produceList.Add(new Produce("Banana", "Sweet and ripe bananas.", 15m, 1.290m, 0.2m));
            _produceList.Add(new Produce("Orange", "Juicy and tangy oranges.", 8m, 2.49m, 0.3m));
            _produceList.Add(new Produce("Strawberry", "Sweet strawberries.", 20m, 4.25m, 0.02m));
            _produceList.Add(new Produce("Grapes", "Juicy green grapes.", 12m, 4.99m, 0.01m));
        }

        public static decimal GetValidQuantity(string itemName, decimal availableStock) 
        {
            decimal quantity = 0;
            while (true)
            {
                Console.WriteLine($"How much {itemName}, in lbs, would you like to purchase?");
                if (decimal.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                {
                    if (quantity <= availableStock) 
                    {
                        return quantity;
                    }
                    else
                    {
                        Console.WriteLine($"We have {availableStock} lbs of {itemName} in stock. Please enter a new amount.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid quantity.");
                }
            }
        }

        // Shop Meat Method  - Glory
        static void ShopMeat()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Meat Items ===");
                for (int i = 0; i < _meatItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_meatItems[i].Name} - {_meatItems[i].Description} - ${_meatItems[i].Price}/lb"); //added description and price per lb to list displayed  - Juan

                }

                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select a meat item (1-5) or 0 to return: ");

                if (decimal.TryParse(Console.ReadLine(), out decimal choice) && choice >= 0 &&
                    choice <= _meatItems.Count)
                {
                    if (choice == 0) break; // Go back to main menu

                    MeatItem selectedItem = _meatItems[(int)choice - 1]; 
                    decimal quantity = GetValidQuantity(selectedItem.Name, selectedItem.Stock);

                    if (selectedItem.PurchaseItem(quantity))
                    {
                        string barcode = GenerateBarcodeId(); // Generate barcode for the item
                        CartItem cartItem = new CartItem(selectedItem, quantity, barcode,
                            $"Purchased {quantity} lbs of {selectedItem.Name}.");
                        _shoppingCart.Add(cartItem);
                        Console.WriteLine($"Successfully added {quantity} lbs of {selectedItem.Name} to your cart.\n" +
                                          $"Total price for this item: ${selectedItem.Price * quantity:F2}\nBarcode: {barcode}");
                        Console.ReadLine(); // Wait for user input to continue
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please try again."); // For invalid meat items
                }
            }
        }
        
        // Shop Produce Method - Dan
        static void ShopProduce()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Produce Items ===");
        for (int i = 0; i < _produceList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_produceList[i].Name} - {_produceList[i].Description} - ${_produceList[i].Price}/lb");
        }

        Console.WriteLine("0. Return to Main Menu");
        Console.Write("Select a produce item (1-5) or 0 to return: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _produceList.Count)
        {
            Produce selectedProduce = _produceList[choice - 1];

            Console.WriteLine($"How many {selectedProduce.Name}(s) would you like to purchase?");
            if (int.TryParse(Console.ReadLine(), out int itemQuantity) && itemQuantity > 0)
            {
                decimal totalWeight = itemQuantity * selectedProduce.AverageWeight;

                if (totalWeight <= selectedProduce.Stock)
                {
                    if (selectedProduce.PurchaseItem(totalWeight))
                    {
                        string barcode = GenerateBarcodeId();
                        CartItem cartItem = new CartItem(selectedProduce, totalWeight, barcode,
                            $"Purchased {itemQuantity} {selectedProduce.Name}(s) weighing {totalWeight} lbs.");
                        _shoppingCart.Add(cartItem);

                        Console.WriteLine(
                            $"Successfully added {itemQuantity} {selectedProduce.Name}(s) to your cart.\n" +
                            $"Total price for this item: ${selectedProduce.Price * totalWeight:F2}\n");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine($"Not enough stock. We only have {selectedProduce.Stock} lbs available.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid quantity.");
            }
        }
        else if (choice == 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid selection. Please try again.");
        }
    }
}
        // View Shopping Cart - Paul
        static void ViewShoppingCart()
        {
            Console.Clear();
            Console.WriteLine("=== Shopping Cart ===");

            if (_shoppingCart.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
            }
            else
            {
                decimal totalCost = 0; // Variable to hold the total cost
                foreach (CartItem item in _shoppingCart)
                {
                    decimal itemCost = item.IsProduce
                        ? item.Quantity * item.ProduceItem.Price
                        : item.Quantity * item.MeatItem.Price;

                    totalCost += itemCost; // Add item cost to total
                    if (!item.IsProduce)
                    {
                        Console.WriteLine($"{item.Quantity} lbs of {item.MeatItem.Name}, " +
                                          $"Total Price: ${itemCost:F2}, Barcode: {item.Barcode}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Quantity} lbs of {item.ProduceItem.Name}, " +
                                          $"Total Price: ${itemCost:F2}, Barcode: {item.Barcode}");
                    }
                }

                // Display total cost of all items in the cart
                Console.WriteLine($"Total Cost of All Items: ${totalCost:F2}");
            }
            Console.ReadLine(); // Wait for user input to return to menu
        }
                    
        // Checkout Method - Juan
        static void Checkout()
        {
            Console.Clear();
            Console.WriteLine("=== Checkout ===");

            if (_shoppingCart.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            decimal totalCost = 0;
            foreach (CartItem item in _shoppingCart)
            {
                totalCost += item.IsProduce
                    ? item.Quantity * item.ProduceItem.Price
                    : item.Quantity * item.MeatItem.Price;
            }

            Console.WriteLine($"Your total cost is: ${totalCost:F2}");
            Console.WriteLine("Thank you for shopping with us!");
        }
    }
}
