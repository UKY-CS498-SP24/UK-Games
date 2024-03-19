﻿using Microsoft.AspNetCore.Mvc;

namespace UK_Games.Controllers;

public class GameController : Controller
{
    // GOING TO A GAME PAGE
    
    /*
     *      localhost:3306/game/25
     */
    public IActionResult Index(int id)
    {
        ViewBag.GameID = id;
        return View();
    }

    public IActionResult SolitaireCollection()
    {
        return View();
    }

    public IActionResult PinballBreakout()
    {
        return View();
    }

    public IActionResult Mahjong3DConnect()
    {
        return View();
    }

    public IActionResult _2048Billiards()
    {
        return View();
    }

}