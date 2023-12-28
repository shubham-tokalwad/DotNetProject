
using refactor_me.DataAccessLibraray.Interface;
using refactor_me.Models;
using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Xml.Linq;

namespace refactor_me.DataAccessLibraray
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        public async Task CreateOrUpdateOptionAsync(ProductOption option)
        {
            if (option.Id != Guid.Empty)
            {
                option.IsNew = false;
            }
            else
            {
                option.Id = Guid.NewGuid();
                option.IsNew = true;
            }

            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();

                var cmd = option.IsNew
                    ? new SqlCommand($"INSERT INTO productoption (id, productid, name, description) VALUES ('{option.Id}', '{option.ProductId}', '{option.Name}', '{option.Description}')", conn)
                    : new SqlCommand($"UPDATE productoption SET name = '{option.Name}', description = '{option.Description}' WHERE id = '{option.Id}'", conn);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteOptionAsync(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand($"DELETE FROM productoption WHERE id = '{id}'", conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<ProductOption>> GetOptionsAsync(Guid productId)
        {
            string where = productId != Guid.Empty ? $"WHERE productid = '{productId}'" : "";

            List<ProductOption> items = new List<ProductOption>();

            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand($"SELECT id FROM productoption {where}", conn);

                var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    items.Add(await GetOptionAsync(id));
                }
            }

            return items;
        }

        public async Task<ProductOption> GetOptionAsync(Guid id)
        {
            ProductOption productOption = new ProductOption { IsNew = true };

            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand($"SELECT * FROM productoption WHERE id = '{id}'", conn);

                var rdr = await cmd.ExecuteReaderAsync();
                if (await rdr.ReadAsync())
                {
                    productOption.IsNew = false;
                    productOption.Id = Guid.Parse(rdr["Id"].ToString());
                    productOption.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                    productOption.Name = rdr["Name"].ToString();
                    productOption.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                }
            }

            return productOption;
        }

    }
}