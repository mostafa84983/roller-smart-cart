using Microsoft.AspNetCore.Identity;
using SmartCart.Domain.Models;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Data
{
public static class DatabaseSeeder
{
    public static async Task SeedAsync(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        // Seed Roles
        string[] roleNames = { "User", "Admin" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
            }
        }

        // Seed Users
        var users = new List<User>
        {
            new User
            {
                UserName = "mostafa.ali@Rollereg.com",
                Email = "mostafa.ali@Rollereg.com",
                FirstName = "Mostafa",
                LastName = "Ali",
                Gender = "Male",
                BirthDate = new DateTime(2001, 3, 10),
                EmailConfirmed = true
            },
            new User
            {
                UserName = "heba.elsayed@gmail.com",
                Email = "hebaabobaa00@gmail.com",
                FirstName = "Heba",
                LastName = "Elsayed",
                Gender = "Female",
                BirthDate = new DateTime(2001, 6, 11),
                EmailConfirmed = true
            },
            new User
            {
                UserName = "menna.mabrouk@gmail.com",
                Email = "mmaly569@gmail.com",
                FirstName = "Menna",
                LastName = "Mabrouk",
                Gender = "Female",
                BirthDate = new DateTime(2000, 11, 9),
                EmailConfirmed = true
            }
        };

        foreach (var user in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                // Updated passwords to have only one # at the end
                string password = user.Email == "mostafa.ali@Rollereg.com" ? "AdminSecure2025#" : "Password2025#";
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (user.Email == "mostafa.ali@Rollereg.com")
                        await userManager.AddToRoleAsync(user, "Admin");
                    else
                        await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        // Seed Categories
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                    new Category
                {
                    CategoryName = "Beverages",
                    CategoryImage = "beverages.jpg",
                    IsOffer = false
                },
                new Category
                {
                    CategoryName = "Snacks",
                    CategoryImage = "snacks.jpg",
                    IsOffer = false
                },
                new Category
                {
                    CategoryName = "Bakery & Cakes",
                    CategoryImage = "bakery_cakes.jpg",
                    IsOffer = false
                },
                new Category
                {
                    CategoryName = "Chocolate & Candy",
                    CategoryImage = "chocolate_candy.jpg",
                    IsOffer = false
                },
                new Category
                {
                    CategoryName = "Canned Goods",
                    CategoryImage = "canned_goods.jpg",
                    IsOffer = false
                },
                new Category
                {
                    CategoryName = "Dairy",
                    CategoryImage = "dairy.jpg",
                    IsOffer = true
                }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // Seed Products
        if (!context.Products.Any())
        {
            var categories = context.Categories.ToList();
            var products = new List<Product>
            {
                new Product
                {
                    ProductName = "Alarousa Black Tea",
                    ProductCode = 1001,
                    ProductWeight = 100.00m,
                    Quantity = 100,
                    ProductPrice = 25.00m,
                    ProductImage = "alarousa_tea.jpg",
                    ProductDescription = "Alarousa Black Tea - 100g pack",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Dasani_Water",
                    ProductCode = 1003,
                    ProductWeight = 575.00m,
                    Quantity = 100,
                    ProductPrice = 7.00m,
                    ProductImage = "Dasani_Water.jpg",
                    ProductDescription = "Dasani_Water 575ml",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Sprite Can",
                    ProductCode = 1008,
                    ProductWeight = 0.33m,
                    Quantity = 100,
                    ProductPrice = 18.00m,
                    ProductImage = "sprite_can.jpg",
                    ProductDescription = "Sprite Can - 330ml",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Juhayna_Guava Juice",
                    ProductCode = 1012,
                    ProductWeight = 220.00m,
                    Quantity = 100,
                    ProductPrice = 12.00m,
                    ProductImage = "Juhayna_Guava.jpg",
                    ProductDescription = "Juhayna_Guava Juice 220ml",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Pepsi Can",
                    ProductCode = 1013,
                    ProductWeight = 0.33m,
                    Quantity = 100,
                    ProductPrice = 18.00m,
                    ProductImage = "pepsi_can.jpg",
                    ProductDescription = "Pepsi Can - 330ml",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Schweppes Can",
                    ProductCode = 1014,
                    ProductWeight = 0.33m,
                    Quantity = 100,
                    ProductPrice = 18.00m,
                    ProductImage = "schweppes_can.jpg",
                    ProductDescription = "Schweppes Can - 330ml",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                // Snacks (CategoryId: 2)
                new Product
                {
                    ProductName = "Chipsy Shatta & Lemon",
                    ProductCode = 1002,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "chipsy_shatta_lemon.jpg",
                    ProductDescription = "Chipsy Shatta & Lemon Flavor - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Doritos",
                    ProductCode = 1004,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "doritos.jpg",
                    ProductDescription = "Doritos Chips - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Halawa_Elbawady",
                    ProductCode = 1009,
                    ProductWeight = 250m,
                    Quantity = 100,
                    ProductPrice = 57.00m,
                    ProductImage = "Halawa_Elbawady.jpg",
                    ProductDescription = "Halawa_Elbawady 250g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Chipsy Marinated Cheese",
                    ProductCode = 1010,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "chipsy_marinated_cheese.jpg",
                    ProductDescription = "Chipsy Marinated Cheese Flavor - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Jaguar Stix Cheeseburger",
                    ProductCode = 1011,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m, 
                    ProductImage = "jaguar_stix_cheeseburger.jpg",
                    ProductDescription = "Jaguar Stix Cheeseburger Flavor - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Lays Chips",
                    ProductCode = 1016,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 25.00m,
                    ProductImage = "lays_chips.jpg",
                    ProductDescription = "Lays Chips - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Chipsy Kebab",
                    ProductCode = 1018,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "chipsy_kebab.jpg",
                    ProductDescription = "Chipsy Kebab Flavor - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Snacks").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                    // Bakery & Cakes (CategoryId: 3)
                new Product
                {
                    ProductName = "HOHOs",
                    ProductCode = 1005,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "hohos.jpg",
                    ProductDescription = "HOHOs Cakes - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Bakery & Cakes").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "HOHOs Mix",
                    ProductCode = 1006,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "hohos_mix.jpg",
                    ProductDescription = "HOHOs Mix Cakes - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Bakery & Cakes").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Molto",
                    ProductCode = 1007,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 10.00m,
                    ProductImage = "molto.jpg",
                    ProductDescription = "Molto Cakes - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Bakery & Cakes").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                // Chocolate & Candy (CategoryId: 6)
                new Product
                {
                    ProductName = "Twix Bar",
                    ProductCode = 1015,
                    ProductWeight = 0.05m,
                    Quantity = 100,
                    ProductPrice = 35.65m,
                    ProductImage = "twix_bar.jpg",
                    ProductDescription = "Twix Bar - 50g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Chocolate & Candy").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                // Canned Goods (CategoryId: 9)
                new Product
                {
                    ProductName = "Americana Red Kidney Beans",
                    ProductCode = 1020,
                    ProductWeight = 0.40m,
                    Quantity = 100,
                    ProductPrice = 38.95m,
                    ProductImage = "americana_red_kidney_beans.jpg",
                    ProductDescription = "Americana Red Kidney Beans - 400g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Canned Goods").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Dulfin tune",
                    ProductCode = 2004,
                    ProductWeight = 200.0m,
                    Quantity = 100,
                    ProductPrice = 85.0m,
                    ProductImage = "tune200.jpg",
                    ProductDescription = "Dulfin tune- 200g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Canned Goods").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                new Product
                {
                    ProductName = "Dulfin tune",
                    ProductCode = 2005,
                    ProductWeight = 140.0m,
                    Quantity = 100,
                    ProductPrice = 70.0m,
                    ProductImage = "tune140.jpg",
                    ProductDescription = "Dulfin tune- 140g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Canned Goods").CategoryId,
                    IsOffer = false,
                    OfferPercentage = 0m
                },
                // Dairy (CategoryId: 10)
                new Product
                {
                    ProductName = "Danone Mixed Berries Greek Yogurt",
                    ProductCode = 1024,
                    ProductWeight = 0.12m,
                    Quantity = 100,
                    ProductPrice = 25.00m,
                    ProductImage = "danone_mixed_berries_yogurt.jpg",
                    ProductDescription = "Danone Mixed Berries Greek Yogurt - 120g",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Dairy").CategoryId,
                    IsOffer = true,
                    OfferPercentage = 10m
                },
                new Product
                {
                    ProductName = "Bekhero Full Cream Milk",
                    ProductCode = 1017,
                    ProductWeight = 0.5m, // Updated to 500ml
                    Quantity= 100,
                    ProductPrice = 21.75m, // Updated based on Amazon link
                    ProductImage = "bekhero_full_cream_milk.jpg",
                    ProductDescription = "Bekhero Full Cream Milk - 500ml, rich in vitamins and minerals with a creamy taste",
                    IsAvaiable = true,
                    CategoryId = categories.First(c => c.CategoryName == "Dairy").CategoryId,
                    IsOffer = true,
                    OfferPercentage = 10m
                }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Seed Orders and OrderProducts (Added order for Menna Mabrouk)
        if (!context.Orders.Any())
        {
            var products = context.Products.ToList();

            // Order for Heba Elsayed
            var userHeba = await userManager.FindByEmailAsync("heba.elsayed@gmail.com");
            if (await userManager.IsInRoleAsync(userHeba, "User"))
            {
                var orderHeba = new Order
                {
                    OrderPrice = 0m, // Will be calculated
                    OrderNumber = "ORD-001",
                    CreationDate = DateTime.UtcNow,
                    OrderDiscount = 0m,
                    UserId = userHeba.Id,
                    OrderProducts = new List<OrderProduct>
                    {
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "Chipsy Kebab").ProductId,
                            Quantity = 2
                        },
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "HOHOs").ProductId,
                            Quantity = 1
                        },
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "Sprite Can").ProductId,
                            Quantity = 5
                        }
                    }
                };

                // Calculate OrderPrice for Heba
                decimal totalPriceHeba = 0m;
                foreach (var orderProduct in orderHeba.OrderProducts)
                {
                    var product = products.First(p => p.ProductId == orderProduct.ProductId);
                    var price = product.IsOffer ? product.ProductPrice * (1 - product.OfferPercentage / 100) : product.ProductPrice;
                    totalPriceHeba += price * orderProduct.Quantity;
                }
                orderHeba.OrderPrice = totalPriceHeba;

                await context.Orders.AddAsync(orderHeba);
            }

            // Order for Menna Mabrouk
            var userMenna = await userManager.FindByEmailAsync("menna.mabrouk@gmail.com");
            if (await userManager.IsInRoleAsync(userMenna, "User"))
            {
                var orderMenna = new Order
                {
                    OrderPrice = 0m, // Will be calculated
                    OrderNumber = "ORD-002", // Different order number
                    CreationDate = DateTime.UtcNow,
                    OrderDiscount = 0m,
                    UserId = userMenna.Id,
                    OrderProducts = new List<OrderProduct>
                    {
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "Molto").ProductId,
                            Quantity = 1
                        },
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "Chipsy Marinated Cheese").ProductId,
                            Quantity = 2
                        },
                        new OrderProduct
                        {
                            ProductId = products.First(p => p.ProductName == "Doritos").ProductId,
                            Quantity = 3
                        }
                    }
                };

                // Calculate OrderPrice for Menna
                decimal totalPriceMenna = 0m;
                foreach (var orderProduct in orderMenna.OrderProducts)
                {
                    var product = products.First(p => p.ProductId == orderProduct.ProductId);
                    var price = product.IsOffer ? product.ProductPrice * (1 - product.OfferPercentage / 100) : product.ProductPrice;
                    totalPriceMenna += price * orderProduct.Quantity;
                }
                orderMenna.OrderPrice = totalPriceMenna;

                await context.Orders.AddAsync(orderMenna);
            }

            await context.SaveChangesAsync();
        }
    }
}
}