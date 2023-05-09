using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebForm.Models
{
        public class CartItem
        {
            public phone shopping { get; set; }

            public int shop_quantity { get; set; }

            public P_Order order { get; set; }
        }

        public class Cart
        {
            List<CartItem> items = new List<CartItem>();

            public IEnumerable<CartItem> Items
            {
                get { return items; }

            }

            public void add(phone _phone, int _quantity = 1)
            {
                var item = items.FirstOrDefault(s => s.shopping.id.Equals( _phone.id) );
                if(item == null)
                {
                    items.Add(new CartItem
                    {
                        shopping = _phone,
                        shop_quantity = _quantity
                    }) ;
                }
                else
                {
                    item.shop_quantity += _quantity;
                }

            }

            public void Update_Quantity_Shopping(string id, int _quantity)
            {
                var item = items.Find(s => s.shopping.id.Equals( id));
                if(item != null)
                {
                    item.shop_quantity= _quantity; 
                }
            }

            public void update_status(int id, int change, string description)
            {

            string connectionString = "initial catalog = final; data source = MSI\\SQLEXPRESS; integrated security = true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE P_Order SET d_status = @Status,descrip = @des WHERE id = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", change);
                    command.Parameters.AddWithValue("@des", description);
                    command.Parameters.AddWithValue("@ProductId", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    // Xử lý số hàng bị ảnh hưởng (rowsAffected) sau khi cập nhật
                }
            }

        }

            public int total()
            {
                var total = items.Sum(s => s.shopping.price * s.shop_quantity);
                return (int)total;
            }

            public void remove_cart(string id)
            {
                items.RemoveAll(s=>s.shopping.id.Equals(id));
            }

            public int Total_Quantity_in_Cart()
            {
                return items.Sum(s => s.shop_quantity);
            }

            public void ClearCart()
            {
                items.Clear();
            }
        }
}