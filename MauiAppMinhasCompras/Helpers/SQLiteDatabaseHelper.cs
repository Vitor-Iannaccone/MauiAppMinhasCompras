using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produtos>().Wait();
        }

        public Task<int> insert(Produtos p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produtos>> update(Produtos p)
        {
            string sql = "UPDATE Produtos SET Descricao = ?, Quantidade = ?, Preco = ? WHERE Id = ?";

            return _conn.QueryAsync<Produtos>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        public Task<int> delete(int id)
        {
            return _conn.Table<Produtos>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produtos>> getAll()
        {
            return _conn.Table<Produtos>().ToListAsync();

        }

        public Task<List<Produtos>> Search(string q)
        {
            string sql = "SELECT * From Produtos WHERE Descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produtos>(sql);


        }

    }
}