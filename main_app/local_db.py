import sqlite3

DB_PATH = "products.db"

class LocalProductDB:
    def __init__(self, db_path=DB_PATH):
        self.conn = sqlite3.connect(db_path)
        self.conn.row_factory = sqlite3.Row
        self.cursor = self.conn.cursor()

    def get_product_by_name(self, name):
        query = "SELECT * FROM products WHERE name = ?"
        self.cursor.execute(query, (name,))
        row = self.cursor.fetchone()
        return dict(row) if row else None

    def close(self):
        self.conn.close()

