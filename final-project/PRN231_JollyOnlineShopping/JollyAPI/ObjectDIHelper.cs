using JollyAPI.DAO;
using JollyAPI.Services;

namespace JollyAPI
{
    public class ObjectDIHelper
    {
        public static void AddObjectDI(IServiceCollection services)
        {
            // Add DAO to the container.
            services.AddScoped<BlogDAO>();
            services.AddScoped<CartDAO>();
            services.AddScoped<CategoryDAO>();
            services.AddScoped<CommentDAO>();
            services.AddScoped<OrderDAO>();
            services.AddScoped<OrderDetailDAO>();
            services.AddScoped<ProductDAO>();
            services.AddScoped<RatingDAO>();
            services.AddScoped<UserDAO>();
            services.AddScoped<WishListDAO>();
            services.AddScoped<AddressDAO>();




            // Add services to the container.
            services.AddScoped<BlogService>();
            services.AddScoped<CartService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<CommentService>();
            services.AddScoped<OrderService>();
            services.AddScoped<OrderDetailService>();
            services.AddScoped<ProductService>();
            services.AddScoped<RatingService>();
            services.AddScoped<UserService>();
            services.AddScoped<WishListService>();
            services.AddScoped<AddressService>();
        }
    }
}
