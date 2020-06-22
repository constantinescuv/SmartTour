using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTour.Business;
using SmartTour.Business.Funct;
using SmartTour.DataAccess;

namespace SmartTour.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UsersRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("Myconnection")));
            services.AddDbContext<PostsRepository>(options => options.UseSqlServer(Configuration.GetConnectionString("Myconnection")));
            services.AddTransient<UsersRepository>();
            services.AddTransient<PostsRepository>();
            services.AddSingleton<ITourService, TourService>();
            services.AddSingleton<IGetTour, GetTour>();
            services.AddTransient<IRegister, Register>();
            services.AddTransient<ILogin, Login>();
            services.AddTransient<IEdit, Edit>();
            services.AddTransient<IIncrement, Increment>();
            services.AddTransient<IRefreshProfile, RefreshProfile>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAddPost, AddPost>();
            services.AddTransient<IGetPosts, GetPosts>();
            services.AddTransient<IFeedService, FeedService>();
            services.AddSingleton<IExploreService, ExploreService>();
            services.AddSingleton<IGetExploreList, GetExploreList>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
