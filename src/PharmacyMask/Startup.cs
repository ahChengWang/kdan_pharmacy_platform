using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PharmacyMask.DomainService;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Repository;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;

namespace PharmacyMask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(g =>
            {
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "Pharmacy.API.Platform.xml");

                g.IncludeXmlComments(xmlPath);
            });
            services.AddControllers();
            services.AddSingleton<IPharmacyRepository, PharmacyRepository>();
            services.AddSingleton<IPharmacyDetailRepository, PharmacyDetailRepository>();
            services.AddSingleton<IPharmacyProductRepository, PharmacyProductRepository>();
            services.AddSingleton<IPharmacyBalanceRepository, PharmacyBalanceRepository>();
            services.AddSingleton<IPharmacyBalanceLogRepository, PharmacyBalanceLogRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserTransactionHistoryRepository, UserTransactionHistoryRepository>();
            services.AddSingleton<IUserBalanceRepository, UserBalanceRepository>();
            services.AddSingleton<IUserBalanceLogRepository, UserBalanceLogRepository>();
            services.AddSingleton<IMaskRepository, MaskRepository>();
            services.AddSingleton<IMaskDetailRepository, MaskDetailRepository>();
            services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
            services.AddSingleton<IPurchaseDetailRepository, PurchaseDetailRepository>();

            services.AddSingleton<ISalesManagementDomainService, SalesManagementDomainService>();
            services.AddSingleton<IPharmacyDomainService, PharmacyDomainService>();
            services.AddSingleton<IMaskService, MaskService>();
            services.AddSingleton<IUserDomainService, UserDomainService>();
            services.AddSingleton<IProductDomainService, ProductDomainService>();
            services.AddSingleton<IPurchaseDomainService, PurchaseDomainService>();
            services.AddSingleton<IBalanceDomainService, BalanceDomainService>();

            services.Add(new ServiceDescriptor(typeof(DbHelperFactory), new DbHelperFactory(Configuration)));
        }

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
            });
        }
    }
}
