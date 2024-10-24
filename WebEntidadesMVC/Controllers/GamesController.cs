﻿using Microsoft.AspNetCore.Mvc;
using WebEntidadesMVC.Models;
using WebEntidadesMVC.Utilities;

namespace WebEntidadesMVC.Controllers
{
    public class GamesController() : Controller
    {
        public async Task<IActionResult> GetPages(int Pagina = 1)
        {
            GetServices getServices = new();

            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            var gameFilter = await getServices.GetGamesAsync(Pagina, token!);

            if (gameFilter.Count() == 0)
                return RedirectToAction("Index", "Home");

            int totalPag = gameFilter.FirstOrDefault()?.TotalPaginas ?? 0;
            var modeloPaginacion = new PaginacionViewModel<VideojuegosViewModel>
            {
                Elementos = gameFilter.ToList(),
                PaginaActual = Pagina,
                TotalPaginas = totalPag,
                TamanoPagina = 5
            };

            ViewBag.AccessToken = token;

            return View(modeloPaginacion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameViewModel createGameViewModel)
        {
            GetServices getServices = new();

            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            var response = await getServices.CreateGamesAsync(createGameViewModel.Nombre!, createGameViewModel.Compania!, createGameViewModel.Ano, createGameViewModel.Precio, createGameViewModel.Puntaje, token!);

            if (!response)
                return View(createGameViewModel);

            TempData["SuccessMessage"] = "¡El registro fue satisfactorio!";

            return RedirectToAction("GetPages");
        }

        public IActionResult GetArchiveCsv()
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");
            ViewBag.AccessToken = token;

            return View();
        }
    }
}