using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace BibliotecaBackend.Tests
{
    public class BibliotecaTests
    {
        [Fact]
        public async Task GetObras()
        {
            await using var app = new BibliotecaApplication();

            var client = app.CreateClient();
            var obras = await client.GetFromJsonAsync<List<Obra>>("/obras");

            Assert.Empty(obras);
        }

        [Fact]
        public async Task PostObras()
        {
            await using var app = new BibliotecaApplication();

            var client = app.CreateClient();
            var response = await client.PostAsJsonAsync("/obras", new Obra
            {
                id = 1,
                titulo = "Harry Potter",
                editora = "Rocco",
                foto = "'https://i.imgur.com/UH3IPXw.jpg",
                autores = new string[] { "JK Rowling" }
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var obras = await client.GetFromJsonAsync<List<Obra>>("/obras");

            var obra = Assert.Single(obras);
            Assert.Equal("Harry Potter", obra.titulo);
        }

        [Fact]
        public async Task DeleteObras()
        {
            await using var app = new BibliotecaApplication();

            var client = app.CreateClient();
            var response = await client.PostAsJsonAsync("/obras", new Obra
            {
                id = 1,
                titulo = "Harry Potter",
                editora = "Rocco",
                foto = "'https://i.imgur.com/UH3IPXw.jpg",
                autores = new string[] { "JK Rowling" }
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var obras = await client.GetFromJsonAsync<List<Obra>>("/obras");

            var obra = Assert.Single(obras);
            Assert.Equal("Harry Potter", obra.titulo);

            response = await client.DeleteAsync($"/obras/{obra.id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await client.GetAsync($"/obras/{obra.id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    class BibliotecaApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<BibliotecaDB>));

                services.AddDbContext<BibliotecaDB>(options =>
                    options.UseInMemoryDatabase("Testing", root));
            });

            return base.CreateHost(builder);
        }
    }
}