using APIEntidades.Application.Contracts;
using APIEntidades.Controllers;
using APIEntidades.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesTest
{
    public class GamesTesting
    {
        private readonly Mock<IUsuarioAppService> _mockUsuarioAppService;
        private readonly Mock<IVideoJuegosAppService> _mockVideoJuegosAppService;
        private readonly EntidadesController _controller;

        public GamesTesting()
        {
            // Crear un mock del servicio IVideoGameService
            _mockUsuarioAppService = new Mock<IUsuarioAppService>();
            _mockVideoJuegosAppService = new Mock<IVideoJuegosAppService>();

            // Inyectar el mock en el controlador
            _controller = new EntidadesController(_mockUsuarioAppService.Object, _mockVideoJuegosAppService.Object);
        }

        [Fact]
        public void GetStatus()
        {
            // Arrange: configurar el mock para devolver una lista de videojuegos
            var gamesList = new List<VideoJuegosDto>
            {
                new() { Nombre = "Game 1", Compania = "Compania 1", Ano = 1, Precio = 1, Puntaje = 1, FechaActualizacion = null, Usuario = null },
                new() { Nombre = "Game 2", Compania = "Compania 2", Ano = 2, Precio = 2, Puntaje = 2, FechaActualizacion = null, Usuario = null }
            };

            var mockVideoGames = new ResponseDto<IEnumerable<VideoJuegosDto>>
            {
                Success = true,
                Data = gamesList
            };

            _mockVideoJuegosAppService.Setup(service => service.Get()).Returns(mockVideoGames);

            // Act: llamar al método del controlador
            var result = _controller.GetGames();

            // Assert: verificar el resultado
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetStatusGamesPaginate()
        {
            // Arrange: configurar el mock para devolver una lista de videojuegos
            var gamesList = new List<VideoJuegosDto>
            {
                new() { Nombre = "Game 1", Compania = "Compania 1", Ano = 1, Precio = 1, Puntaje = 1, FechaActualizacion = null, Usuario = null },
                new() { Nombre = "Game 2", Compania = "Compania 1", Ano = 2, Precio = 2, Puntaje = 2, FechaActualizacion = null, Usuario = null }
            };

            var param = new FiltroVideoJuegoDto
            {
                Page = 1,
                Nombre = "Game",
                Compania = "Compania 1",
                Ano = 2024
            };

            var mockVideoGames = new ResponseDto<IEnumerable<VideoJuegosDto>>
            {
                Success = true,
                Data = gamesList
            };

            _mockVideoJuegosAppService.Setup(service => service.GetGamesPaginate(param)).Returns(mockVideoGames);

            // Act: llamar al método del controlador
            var result = _controller.GetGamesPaginate(param);

            // Assert: verificar el resultado
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetStatusGameById()
        {
            // Arrange: configurar el mock para devolver videojuego
            var id = "d768ec7d-2d64-43b2-b32c-9ba861b4e8a2";
            Guid guid = new(id);
            var game = new VideoJuegosDto { Nombre = "Game 1", Compania = "Compania 1", Ano = 1, Precio = 1, Puntaje = 1, FechaActualizacion = null, Usuario = null };

            var mockVideoGames = new ResponseDto<VideoJuegosDto>
            {
                Success = true,
                Data = game
            };

            _mockVideoJuegosAppService.Setup(service => service.GetById(guid)).Returns(mockVideoGames);

            // Act: llamar al método del controlador
            var result = _controller.GetGamesById(guid);

            // Assert: verificar el resultado
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
