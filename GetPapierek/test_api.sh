#!/bin/bash

BASE_URL="http://localhost:5241/api" # Adjust port if necessary

echo "\n=========================="
echo "Testing GetPapierek API..."
echo "==========================\n"

# --- Books ---
echo "[1] GET /api/Books - Get all books"
curl -s -X GET "$BASE_URL/Books" -H "Content-Type: application/json" | jq

echo -e "\n[2] GET /api/Books/1 - Get book by ID 1"
curl -s -X GET "$BASE_URL/Books/1" -H "Content-Type: application/json" | jq

echo -e "\n[3] POST /api/Books - Add a new book (RAW OUTPUT)"
curl -s -X POST "$BASE_URL/Books" -H "Content-Type: application/json" -d '{
  "title": "New Test Book",
  "author": "Test Author",
  "publicationYear": 2025,
  "categoryId": 2,
  "category": { "id": 2, "name": "Science" },
  "shelf": "T1"
}'

echo -e "\n[4] PUT /api/Books/1 - Update book with ID 1"
curl -s -X PUT "$BASE_URL/Books/1" -H "Content-Type: application/json" -d '{
  "bookId": 1,
  "title": "Pan Tadeusz (Updated Edition)",
  "author": "Adam Mickiewicz",
  "publicationYear": 1834,
  "categoryId": 1,
  "category": { "id": 1, "name": "Novel" },
  "shelf": "A1-Updated"
}' | jq

# Example for DELETE (commented out for safety)
# echo "\n[5] DELETE /api/Books/1 - Delete book with ID 1 (CAUTION)"
# curl -s -X DELETE "$BASE_URL/Books/1" | jq

echo -e "\n[6] GET /api/Books/search?query=Test - Search for books with 'Test'"
curl -s -X GET "$BASE_URL/Books/search?query=Test" -H "Content-Type: application/json" | jq

# --- Categories ---
echo -e "\n[7] GET /api/Category - Get all categories"
curl -s -X GET "$BASE_URL/Category" -H "Content-Type: application/json" | jq

# --- Rentals ---
echo -e "\n[8] POST /api/Rental - Create a new rental (User 1, Book 1) (RAW OUTPUT)"
curl -s -X POST "$BASE_URL/Rental" -H "Content-Type: application/json" -d '{
  "userId": 1,
  "user": {
    "userId": 1,
    "firstName": "Jan",
    "lastName": "Kowalski",
    "email": "jan.kowalski@example.com",
    "password": "password123",
    "name": "Jan Kowalski"
  },
  "bookId": 1,
  "book": {
    "bookId": 1,
    "title": "Pan Tadeusz (Updated Edition)",
    "author": "Adam Mickiewicz",
    "shelf": "A1-Updated",
    "publicationYear": 1834,
    "categoryId": 1,
    "category": { "id": 1, "name": "Novel" }
  },
  "rentalDate": "2025-05-25T00:00:00Z"
}'

echo -e "\n[9] GET /api/Rental - Get all rentals"
curl -s -X GET "$BASE_URL/Rental" -H "Content-Type: application/json" | jq

echo -e "\n[10] POST /api/Rental/1/return - Mark rental ID 1 as returned"
curl -s -X POST "$BASE_URL/Rental/1/return" -H "Content-Type: application/json" | jq

# --- Search ---
echo -e "\n[11] GET /api/Search?query=Pan - Simple search for 'Pan'"
curl -s -X GET "$BASE_URL/Search?query=Pan" -H "Content-Type: application/json" | jq

echo -e "\n[12] GET /api/Search/advanced?title=Pan&author=Adam - Advanced search"
curl -s -X GET "$BASE_URL/Search/advanced?title=Pan&author=Adam&categoryId=1&yearFrom=1800&yearTo=1900" -H "Content-Type: application/json" | jq

echo -e "\n=========================="
echo "API Testing Finished."
echo "NOTE: For POST/PUT/DELETE operations that create/modify specific resources,"
echo "you might need to adjust IDs based on the current state of your database."
echo "The DELETE book example is commented out to prevent accidental data loss."
echo "You would typically add a book, then use its returned ID to update it, then delete it."

