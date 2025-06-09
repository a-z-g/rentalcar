# 🚀 Rental Car Backend – README

This project implements a **backend** for managing rental car bookings.
It includes:

✅ Logic for registering **car pickups** and **returns**  
✅ Calculation of **total rental price** according to different car categories (Small, Combi, Truck)  
✅ A **console app** to test the logic for all categories  
✅ Uses **.NET Aspire** to orchestrate the app and MongoDB

---

## 💡 Project Overview

This project handles:

- **Registering a rental car pickup** (booking number, registration number, social security number, and car category).
- **Registering a rental car return** (booking number, return time, and odometer reading).
- **Calculating the total rental price** after the car is returned using category-specific formulas.
- The system is designed to be **flexible** to support more car categories in the future.

---

## ⚠️ Assumptions and Notes

- **There’s always a car available** when a pickup is registered.  
- **Each car can only be rented by one customer at a time**.  
- **Each booking has a unique booking number**.  
- The **social security number** is stored directly in the first version, but in a real-world scenario, it would be handled in compliance with **GDPR** – typically in a separate customer database.  
- **Minimum charge is for one day** – even if the actual rental time is shorter.  
- **Base day rental and base kilometer price** are provided as **inputs** when calculating the total price and are not stored in the database.

---

## 🟢 Why Aspire?

I chose to use **.NET Aspire** to orchestrate the application because it makes it easy to:

✅ Spin up **MongoDB and other services** locally for development and testing  
✅ Run the API and console app together as a single cohesive environment  
✅ Use **environment variables** for shared configuration (like MongoDB connection string), reducing hard-coded config

This mirrors real-world production environments and simplifies the local developer experience.

---

## 🟢 How to Run

1️⃣ Go to the **.AppHost** project folder:  
```bash
cd RentalCar.AppHost
```

2️⃣ Start the **Aspire environment** (this also starts the console app and MongoDB):  
```bash
dotnet run
```

3️⃣ The console app will automatically:  
- Register pickups and returns for all 3 categories  
- Calculate prices  
- Print the **expected and actual prices** to the console

Example output:
```
--- Testing category: Small ---
Booking number: B15
Calculated price: 200
Expected price: 200
✅ Price for Small matches expected value.

--- Testing category: Combi ---
Booking number: B16
Calculated price: 760
Expected price: 760
✅ Price for Combi matches expected value.

--- Testing category: Truck ---
Booking number: B17
Calculated price: 1050
Expected price: 1050
✅ Price for Truck matches expected value.

All categories tested
```

---

## 🔧 Additional Notes

- If you want to **run the console app separately** (outside Aspire), you’ll need to manually set the MongoDB connection string:  
```bash
export ConnectionStrings__mongodb="mongodb://localhost:27017/rentalcar"
dotnet run --project RentalCar.ConsoleApp
```

- The logic for calculating the price is tested for **2 days** and **500km** driven for each car category.
- The console app was created for fast testing and validation of the core logic. As an improvement, I would replace it with a proper testing framework (like xUnit) and expand the test scenarios to cover more edge cases and integrations.
