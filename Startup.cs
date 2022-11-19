using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Bike_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Cors設定
        readonly string CorsPolicy = "_CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Cors設定
            services.AddCors(Options =>
            {
                Options.AddPolicy(CorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        //builder.AllowAnyOrigin();
                    });
            });
            #endregion

            #region JWT配置
            services.AddSingleton<JwtHelpers>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),

                        // 通常不太需要驗證 Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true,

                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" 應該從 IConfiguration 取得
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:SignKey")))
                    };
                });
            #endregion

            #region 註冊Swagger服務
            //services.AddSwaggerDocument(settings =>
            //{
            //    settings.AddSecurity("輸入身份認證Token", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme()
            //    {
            //        Description = "JWT認證 請輸入Bearer {token}",
            //        Name = "Authorization",
            //        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
            //        Type = NSwag.OpenApiSecuritySchemeType.ApiKey
            //    });

            //    settings.OperationProcessors.Add(
            //        new AspNetCoreOperationSecurityScopeProcessor("JWT Token"));
            //});

            // Add OpenAPI v3 document
            services.AddOpenApiDocument(config =>
            {
                config.Title = "腳踏車庫存系統API";

                // 這個 OpenApiSecurityScheme 物件請勿加上 Name 與 In 屬性，否則產生出來的 OpenAPI Spec 格式會有錯誤！
                var apiScheme = new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT", // for documentation purposes (OpenAPI only)
                    Description = "Copy JWT Token into the value field: {token}"
                };

                // 這裡會同時註冊 SecurityDefinition (.components.securitySchemes) 與 SecurityRequirement (.security)
                // 針對特定有套用 [Authorize] 的 API 才出現鎖頭
                config.AddSecurity("Bearer", apiScheme);

                //全體API顯示鎖頭
                //config.AddSecurity("Bearer", Enumerable.Empty<string>(), apiScheme);

                // 這段是為了將 "Bearer" 加入到 OpenAPI Spec 裡 Operator 的 security (Security requirements) 中
                // 這裡的 new AspNetCoreOperationSecurityScopeProcessor() 預設安全名稱就是 Bearer，因此不用特別設定。
                // 但記得 config.AddSecurity() 的第一個參數一定要設定成 Bearer 才正確。
                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());


            }); //Register the Swagger services
            

            // Add Swagger v2 document
            // services.AddSwaggerDocument();

            #endregion
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

            // Cors設定
            app.UseCors(CorsPolicy);

            //啟用認證中介軟體，要寫在授權 UseAuthorization()的前面
            app.UseAuthentication(); //先驗證

            app.UseAuthorization();  //再授權

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();  // 啟動 OpenAPI 文件
            app.UseSwaggerUi3(); // 啟動 Swagger UI

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
