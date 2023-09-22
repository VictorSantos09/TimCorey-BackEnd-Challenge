using ChallengeCore.Data;
using ChallengeCore.DTOs;
using ChallengeCore.SQLModels;
using Dapper;

namespace ChallengeCore.Models;
public class ProductDAO : DatabaseConnection
{
    public async Task<IEnumerable<ViewUserProductsSQL>> GetUserProducts(string email)
    {
        List<ViewUserProductsSQL> output = new();
        try
        {
            using (_connection = Connect())
            {
                IEnumerable<ViewUserProductsSQL> result = await _connection.QueryAsync<ViewUserProductsSQL>("SELECT p.PR_NAME, p.PR_PRICE, ups.UP_SOLD_DATE, ups.UP_AMOUNT, ups.UP_TOTAL FROM users_products ups " +
                    "INNER JOIN products p ON p.PR_ID = ups.UP_ID_PRODUCT " +
                    " INNER JOIN users u ON u.US_ID = ups.UP_ID_USER " +
                    " WHERE u.US_EMAIL = @Email", new
                    {
                        Email = email.ToUpper()
                    });
                output.AddRange(result);
            }
            return output;
        }
        catch (Exception)
        {
            throw;
        }
        finally { _connection.Close(); }
    }

    public async Task<BaseDTO> AddBoughtProduct(BuyProductDTO dto)
    {
        try
        {
            using (_connection = Connect())
            {
                ProductSQL? product = await _connection.QueryFirstAsync<ProductSQL>("SELECT * FROM products WHERE PR_ID = @ID", new
                {
                    ID = dto.IdProduct
                });
                if (product is null)
                {
                    return BaseDTO.Invalid("produto não encontrado");
                }

                UserSQL? user = await _connection.QueryFirstAsync<UserSQL>("SELECT US_ID FROM users WHERE US_EMAIL = @Email", new
                {
                    dto.Email
                });
                if (user is null)
                {
                    return BaseDTO.Invalid("usuário não encontrado");
                }

                _ = await _connection.ExecuteAsync("INSERT INTO users_products (UP_ID_PRODUCT, UP_ID_USER, UP_SOLD_DATE, UP_AMOUNT, UP_TOTAL) " +
                    "VALUES (@IdProduct, @IdUser, @SoldDate, @Amount, @Total)", new
                    {
                        dto.IdProduct,
                        IdUser = user.US_ID,
                        SoldDate = DateTime.Now,
                        dto.Amount,
                        Total = product.PR_PRICE * dto.Amount
                    });

                return BaseDTO.Valid("produto comprado");
            }
        }
        catch (Exception)
        {
            return BaseDTO.Invalid("não foi possível comprar o item");
        }
        finally { Disconnect(); }
    }

    public async Task<IEnumerable<ProductDTO>> GetAll()
    {
        try
        {
            List<ProductDTO> output = new();
            using (_connection = Connect())
            {
                IEnumerable<ProductSQL> result = await _connection.QueryAsync<ProductSQL>("SELECT * FROM products");

                foreach (ProductSQL p in result)
                {
                    output.Add(Convert(p));
                }
            }
            return output;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private ProductDTO Convert(ProductSQL sql)
    {
        return new()
        {
            Id = sql.PR_ID,
            Price = sql.PR_PRICE,
            Name = sql.PR_NAME
        };
    }
}