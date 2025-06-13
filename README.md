# capstone project of my coursera Microsoft Back-End course

## Part 1
create initial app:
* two classes Order and InventoryItem
* DBContext using EFCore and SQLite3
* Define the one-to-many relation between the two classes
* test and populate the database

## Part 2
* API controllers for Order and Inventory
* async method for all endpoints

## Part 3
* authorization using ASP.NET Identity
* also role-based access on specific endpoints

## Part 4
* logging middleware
* in-cache memory for Order + Inventory endpoints

## Part 5
* final review using Copilot
* centralised seeding user + role
* eliminated redundancy basecode in user + inventory controllers

# API Endpoints

## Auth
* POST /api/Auth/register, register a user
* POST /api/Auth/login, login in order to get the access token

## Inventory (admin only)
* GET /api/Inventory, get all inventory items
* POST /api/Inventory, submit an inventory item
* DELETE /api/Inventory/<id>, remove inventory item by Id

## Order
* GET /api/Order, get all orders
* POST /api/Order, submit a new order
* GET /api/Order/<id>, get a specific order by Id
* DELETE /api/Order/<id>, delete a specific order by Id
