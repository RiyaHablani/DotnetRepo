using BillingService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(BillingDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Transactions.AnyAsync() || await context.Invoices.AnyAsync() || await context.Expenditures.AnyAsync())
            {
                return; // Data already seeded
            }

            var now = DateTime.UtcNow;

            // Seed Transactions
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    PatientId = 1,
                    TransactionType = "Payment",
                    Amount = 150.00m,
                    Description = "Consultation fee payment",
                    PaymentMethod = "Credit Card",
                    ReferenceNumber = "TXN001",
                    TransactionDate = now.AddDays(-30),
                    CreatedAt = now.AddDays(-30),
                    UpdatedAt = now.AddDays(-30),
                    IsDeleted = false
                },
                new Transaction
                {
                    PatientId = 1,
                    TransactionType = "Payment",
                    Amount = 75.50m,
                    Description = "Lab test payment",
                    PaymentMethod = "Cash",
                    ReferenceNumber = "TXN002",
                    TransactionDate = now.AddDays(-25),
                    CreatedAt = now.AddDays(-25),
                    UpdatedAt = now.AddDays(-25),
                    IsDeleted = false
                },
                new Transaction
                {
                    PatientId = 2,
                    TransactionType = "Payment",
                    Amount = 200.00m,
                    Description = "Emergency room visit",
                    PaymentMethod = "Insurance",
                    ReferenceNumber = "TXN003",
                    TransactionDate = now.AddDays(-20),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-20),
                    IsDeleted = false
                },
                new Transaction
                {
                    PatientId = 2,
                    TransactionType = "Refund",
                    Amount = 25.00m,
                    Description = "Overpayment refund",
                    PaymentMethod = "Credit Card",
                    ReferenceNumber = "TXN004",
                    TransactionDate = now.AddDays(-15),
                    CreatedAt = now.AddDays(-15),
                    UpdatedAt = now.AddDays(-15),
                    IsDeleted = false
                },
                new Transaction
                {
                    PatientId = 3,
                    TransactionType = "Payment",
                    Amount = 300.00m,
                    Description = "Surgery payment",
                    PaymentMethod = "Bank Transfer",
                    ReferenceNumber = "TXN005",
                    TransactionDate = now.AddDays(-10),
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-10),
                    IsDeleted = false
                },
                new Transaction
                {
                    PatientId = 4,
                    TransactionType = "Adjustment",
                    Amount = -50.00m,
                    Description = "Insurance adjustment",
                    PaymentMethod = "Insurance",
                    ReferenceNumber = "TXN006",
                    TransactionDate = now.AddDays(-5),
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-5),
                    IsDeleted = false
                }
            };

            await context.Transactions.AddRangeAsync(transactions);

            // Seed Invoices
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    PatientId = 1,
                    InvoiceNumber = "INV-2024-001",
                    TotalAmount = 500.00m,
                    PaidAmount = 225.50m,
                    BalanceAmount = 274.50m,
                    Status = "Pending",
                    Description = "Consultation and lab tests",
                    InvoiceDate = now.AddDays(-30),
                    DueDate = now.AddDays(-15),
                    CreatedAt = now.AddDays(-30),
                    UpdatedAt = now.AddDays(-30),
                    IsDeleted = false
                },
                new Invoice
                {
                    PatientId = 2,
                    InvoiceNumber = "INV-2024-002",
                    TotalAmount = 750.00m,
                    PaidAmount = 750.00m,
                    BalanceAmount = 0.00m,
                    Status = "Paid",
                    Description = "Emergency room treatment",
                    InvoiceDate = now.AddDays(-20),
                    DueDate = now.AddDays(-5),
                    PaidDate = now.AddDays(-18),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-18),
                    IsDeleted = false
                },
                new Invoice
                {
                    PatientId = 3,
                    InvoiceNumber = "INV-2024-003",
                    TotalAmount = 1200.00m,
                    PaidAmount = 300.00m,
                    BalanceAmount = 900.00m,
                    Status = "Overdue",
                    Description = "Surgery and post-operative care",
                    InvoiceDate = now.AddDays(-10),
                    DueDate = now.AddDays(5),
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-10),
                    IsDeleted = false
                },
                new Invoice
                {
                    PatientId = 4,
                    InvoiceNumber = "INV-2024-004",
                    TotalAmount = 200.00m,
                    PaidAmount = 0.00m,
                    BalanceAmount = 200.00m,
                    Status = "Pending",
                    Description = "Routine checkup",
                    InvoiceDate = now.AddDays(-5),
                    DueDate = now.AddDays(10),
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-5),
                    IsDeleted = false
                },
                new Invoice
                {
                    PatientId = 5,
                    InvoiceNumber = "INV-2024-005",
                    TotalAmount = 150.00m,
                    PaidAmount = 150.00m,
                    BalanceAmount = 0.00m,
                    Status = "Paid",
                    Description = "Pharmacy prescription",
                    InvoiceDate = now.AddDays(-3),
                    DueDate = now.AddDays(12),
                    PaidDate = now.AddDays(-2),
                    CreatedAt = now.AddDays(-3),
                    UpdatedAt = now.AddDays(-2),
                    IsDeleted = false
                }
            };

            await context.Invoices.AddRangeAsync(invoices);

            // Seed Expenditures
            var expenditures = new List<Expenditure>
            {
                new Expenditure
                {
                    Category = "Equipment",
                    Description = "New X-ray machine purchase",
                    Amount = 50000.00m,
                    Vendor = "Medical Equipment Co.",
                    ReferenceNumber = "EQ001",
                    ExpenditureDate = now.AddDays(-60),
                    CreatedAt = now.AddDays(-60),
                    UpdatedAt = now.AddDays(-60),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Supplies",
                    Description = "Medical supplies and consumables",
                    Amount = 2500.00m,
                    Vendor = "MedSupply Inc.",
                    ReferenceNumber = "SUP001",
                    ExpenditureDate = now.AddDays(-45),
                    CreatedAt = now.AddDays(-45),
                    UpdatedAt = now.AddDays(-45),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Utilities",
                    Description = "Monthly electricity bill",
                    Amount = 1200.00m,
                    Vendor = "Power Company",
                    ReferenceNumber = "UTL001",
                    ExpenditureDate = now.AddDays(-30),
                    CreatedAt = now.AddDays(-30),
                    UpdatedAt = now.AddDays(-30),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Maintenance",
                    Description = "HVAC system maintenance",
                    Amount = 800.00m,
                    Vendor = "HVAC Services",
                    ReferenceNumber = "MNT001",
                    ExpenditureDate = now.AddDays(-25),
                    CreatedAt = now.AddDays(-25),
                    UpdatedAt = now.AddDays(-25),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Supplies",
                    Description = "Pharmaceutical supplies",
                    Amount = 1500.00m,
                    Vendor = "Pharma Distributors",
                    ReferenceNumber = "SUP002",
                    ExpenditureDate = now.AddDays(-20),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-20),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Utilities",
                    Description = "Water and sewage bill",
                    Amount = 300.00m,
                    Vendor = "Water Authority",
                    ReferenceNumber = "UTL002",
                    ExpenditureDate = now.AddDays(-15),
                    CreatedAt = now.AddDays(-15),
                    UpdatedAt = now.AddDays(-15),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Equipment",
                    Description = "Computer system upgrade",
                    Amount = 5000.00m,
                    Vendor = "IT Solutions",
                    ReferenceNumber = "EQ002",
                    ExpenditureDate = now.AddDays(-10),
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-10),
                    IsDeleted = false
                },
                new Expenditure
                {
                    Category = "Supplies",
                    Description = "Cleaning supplies and equipment",
                    Amount = 400.00m,
                    Vendor = "CleanPro Services",
                    ReferenceNumber = "SUP003",
                    ExpenditureDate = now.AddDays(-5),
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-5),
                    IsDeleted = false
                }
            };

            await context.Expenditures.AddRangeAsync(expenditures);

            // Save all changes
            await context.SaveChangesAsync();
        }
    }
}
