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

        // Cors�]�w
        readonly string CorsPolicy = "_CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Cors�]�w
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

            #region JWT�t�m
            services.AddSingleton<JwtHelpers>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
                    options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // �z�L�o���ŧi�A�N�i�H�q "sub" ���Ȩó]�w�� User.Identity.Name
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // �z�L�o���ŧi�A�N�i�H�q "roles" ���ȡA�åi�� [Authorize] �P�_����
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // �@��ڭ̳��|���� Issuer
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),

                        // �q�`���ӻݭn���� Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

                        // �@��ڭ̳��|���� Token �����Ĵ���
                        ValidateLifetime = true,

                        // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" ���ӱq IConfiguration ���o
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:SignKey")))
                    };
                });
            #endregion

            #region ���USwagger�A��
            //services.AddSwaggerDocument(settings =>
            //{
            //    settings.AddSecurity("��J�����{��Token", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme()
            //    {
            //        Description = "JWT�{�� �п�JBearer {token}",
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
                config.Title = "�}�񨮮w�s�t��API";

                // �o�� OpenApiSecurityScheme ����Фť[�W Name �P In �ݩʡA�_�h���ͥX�Ӫ� OpenAPI Spec �榡�|�����~�I
                var apiScheme = new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT", // for documentation purposes (OpenAPI only)
                    Description = "Copy JWT Token into the value field: {token}"
                };

                // �o�̷|�P�ɵ��U SecurityDefinition (.components.securitySchemes) �P SecurityRequirement (.security)
                // �w��S�w���M�� [Authorize] �� API �~�X�{���Y
                config.AddSecurity("Bearer", apiScheme);

                //����API������Y
                //config.AddSecurity("Bearer", Enumerable.Empty<string>(), apiScheme);

                // �o�q�O���F�N "Bearer" �[�J�� OpenAPI Spec �� Operator �� security (Security requirements) ��
                // �o�̪� new AspNetCoreOperationSecurityScopeProcessor() �w�]�w���W�ٴN�O Bearer�A�]�����ίS�O�]�w�C
                // ���O�o config.AddSecurity() ���Ĥ@�ӰѼƤ@�w�n�]�w�� Bearer �~���T�C
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

            // Cors�]�w
            app.UseCors(CorsPolicy);

            //�ҥλ{�Ҥ����n��A�n�g�b���v UseAuthorization()���e��
            app.UseAuthentication(); //������

            app.UseAuthorization();  //�A���v

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();  // �Ұ� OpenAPI ���
            app.UseSwaggerUi3(); // �Ұ� Swagger UI

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
