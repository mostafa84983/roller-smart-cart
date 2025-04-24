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
                    Email = "heba.elsayed@gmail.com",
                    FirstName = "Heba",
                    LastName = "Elsayed",
                    Gender = "Female",
                    BirthDate = new DateTime(2001, 6, 11),
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "menna.mabrouk@gmail.com",
                    Email = "menna.mabrouk@gmail.com",
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
                        CategoryName = "Fruits and Vegetables",
                        CategoryImage = "fruits_vegetables.jpg",
                        IsOffer = false
                    },
                    new Category
                    {
                        CategoryName = "Dairy",
                        CategoryImage = "dairy.jpg",
                        IsOffer = true
                    },
                    new Category
                    {
                        CategoryName = "Beverages",
                        CategoryImage = "beverages.jpg",
                        IsOffer = false
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
                        ProductName = "Egyptian Tomatoes",
                        ProductCode = 1001,
                        ProductWeight = 1.0m, // 1 kg
                        ProductPrice = 15.00m, // EGP
                        ProductImage = "tomatoes.jpg",
                        ProductDescription = "Fresh tomatoes from local farms",
                        IsAvaiable = true,
                        CategoryId = categories.First(c => c.CategoryName == "Fruits and Vegetables").CategoryId,
                        IsOffer = false,
                        OfferPercentage = 0m
                    },
                    new Product
                    {
                        ProductName = "Feta Cheese",
                        ProductCode = 2001,
                        ProductWeight = 0.5m, // 500g
                        ProductPrice = 40.00m, // EGP
                        ProductImage = "feta_cheese.jpg",
                        ProductDescription = "Traditional Egyptian feta cheese",
                        IsAvaiable = true,
                        CategoryId = categories.First(c => c.CategoryName == "Dairy").CategoryId,
                        IsOffer = true,
                        OfferPercentage = 15m // 15% discount
                    },
                    new Product
                    {
                        ProductName = "V Cola",
                        ProductCode = 3001,
                        ProductWeight = 0.3m, // 300 ml = 0.3 kg
                        ProductPrice = 10.00m, // EGP
                        ProductImage = "v_super_soda.jpg",
                        ProductDescription = "Super Soda Cola Sparkling Drink - 300 ml",
                        IsAvaiable = true,
                        CategoryId = categories.First(c => c.CategoryName == "Beverages").CategoryId,
                        IsOffer = false,
                        OfferPercentage = 0m
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
                                ProductId = products.First(p => p.ProductName == "Egyptian Tomatoes").ProductId,
                                Quantity = 2 // 2 kg
                            },
                            new OrderProduct
                            {
                                ProductId = products.First(p => p.ProductName == "Feta Cheese").ProductId,
                                Quantity = 1 // 500g
                            },
                            new OrderProduct
                            {
                                ProductId = products.First(p => p.ProductName == "V Cola").ProductId,
                                Quantity = 5 // 5 cans
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
                                ProductId = products.First(p => p.ProductName == "Egyptian Tomatoes").ProductId,
                                Quantity = 1 // 1 kg (different quantity)
                            },
                            new OrderProduct
                            {
                                ProductId = products.First(p => p.ProductName == "Feta Cheese").ProductId,
                                Quantity = 2 // 1 kg (different quantity)
                            },
                            new OrderProduct
                            {
                                ProductId = products.First(p => p.ProductName == "V Cola").ProductId,
                                Quantity = 3 // 3 cans (different quantity)
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