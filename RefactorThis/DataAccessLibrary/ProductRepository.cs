using refactor_me.DataAccessLibraray.Interface;
using refactor_me.Models;
using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace refactor_me.DataAccessLibraray
{
    public class ProductRepository: IProductRepository
    {

        public async Task<IEnumerable<Product>> LoadProductsAsync(string name)
        {
            if(!String.IsNullOrEmpty(name))
                name = $"where lower(name) like '%{name.ToLower()}%'";

            List<Product>  items = new List<Product>();
            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand($"select id from product {name}", conn))
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        items.Add(await GetProductByIdAsync(id));
                    }
                }
            }
            return items;
        }

        public async Task<Product> GetProductByIdAsync(Guid? id)
        {
            Product product = new Product { IsNew = true };

            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand($"select * from product where id = '{id}'", conn))
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if (await rdr.ReadAsync())
                    {
                        product.IsNew = false;
                        product.Id = Guid.Parse(rdr["Id"].ToString());
                        product.Name = rdr["Name"].ToString();
                        product.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                        product.Price = decimal.Parse(rdr["Price"].ToString());
                        product.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                    }
                }
            }

            return product;
        }

        public async Task SaveOrUpdateProductAsync(Product product)
        {
            if (product.Id != null && product.Id != Guid.Empty)
            {
                product.IsNew = false;
            }
            else
            {
                product.Id = Guid.NewGuid();
                product.IsNew = true;
            }

            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();
                using (var cmd = product.IsNew ?
                    new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})", conn) :
                    new SqlCommand($"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'", conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteProductAsync(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand($"delete from product where id = '{id}'", conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


    }
}