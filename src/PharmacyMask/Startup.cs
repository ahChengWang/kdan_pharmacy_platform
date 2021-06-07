using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PharmacyMask.DomainService;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Repository;

namespace PharmacyMask
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
            services.AddControllers();
            services.AddSingleton<PharmacyRepository>();
            services.AddSingleton<PharmacyDetailRepository>();
            services.AddSingleton<PharmacyProductRepository>();
            services.AddSingleton<PharmacyBalanceRepository>();
            services.AddSingleton<PharmacyBalanceLogRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserTransactionHistoryRepository>();
            services.AddSingleton<UserBalanceRepository>();
            services.AddSingleton<UserBalanceLogRepository>();
            services.AddSingleton<MaskRepository>();
            services.AddSingleton<MaskDetailRepository>();
            services.AddSingleton<PurchaseRepository>();
            services.AddSingleton<PurchaseDetailRepository>();

            services.AddSingleton<SalesManagementDomainService>();
            services.AddSingleton<PharmacyDomainService>();
            services.AddSingleton<MaskService>();
            services.AddSingleton<UserDomainService>();
            services.AddSingleton<ProductDomainService>();
            services.AddSingleton<PurchaseDomainService>();
            services.AddSingleton<BalanceDomainService>();
            services.Add(new ServiceDescriptor(typeof(DbHelperFactory), new DbHelperFactory(Configuration)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
