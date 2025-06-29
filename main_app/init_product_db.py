# init_product_db.py

import sqlite3

# Name of the SQLite DB file
DB_NAME = "products.db"

# Product data to insert
products = [
    {
        "id": "3",
        "name": "Dasany_Water",
        "price": 10.00,         # example price
        "expected_weight": 600,
        "tolerance": 25,
        "barcode": "1234567890123"
    },
    {
        "id": "0",
        "name": "Arousa_Tea",
        "price": 5.50,          # example price
        "expected_weight": 40,
        "tolerance": 10,
        "barcode": "9876543210987"
    }
]

def init_db():
    conn = sqlite3.connect(DB_NAME)
    cursor = conn.cursor()

    cursor.execute("""
    CREATE TABLE IF NOT EXISTS products (
        id TEXT PRIMARY KEY,
        name TEXT NOT NULL,
        price REAL NOT NULL,
        expected_weight INTEGER NOT NULL,
        tolerance INTEGER NOT NULL,
        barcode TEXT UNIQUE
    )
    """)

    # Clear existing entries
    cursor.execute("DELETE FROM products")

    # Insert products
    for p in products:
        cursor.execute("""
        INSERT INTO products (id, name, price, expected_weight, tolerance, barcode)
        VALUES (?, ?, ?, ?, ?, ?)
        """, (p["id"], p["name"], p["price"], p["expected_weight"], p["tolerance"], p["barcode"]))

    conn.commit()
    conn.close()
    print(f"✅ Database '{DB_NAME}' initialized with {len(products)} products.")

if __name__ == "__main__":
    init_db()

