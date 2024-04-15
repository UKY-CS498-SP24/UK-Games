using System.Data.SqlTypes;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UK_Games.Infrastructure;
using UK_Games.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace UK_Games.Controllers;

public class GameController : Controller
{
    // GOING TO A GAME PAGE
    
    /*
     *      localhost:3306/game/25
     */
    public IActionResult Index(int id)
    {
        ViewBag.Game = GeneralMethods.GetGame(id);
        return View();
    }

    public class SearchData
    {
        public string pattern = "";
    }

    [HttpPost]
    public IActionResult Search()
    {
        String pattern = Convert.ToString(Request.Form["pattern"]);
        // foreach(Game g in DataUtil.Data.Games)
        //     if(g.Name == pattern)
        //         return RedirectToAction("Index", new { id = g.ID });
        // return RedirectToAction("Index", "Home");
        return RedirectToAction("Index", new { id = ClosestMatch(DataUtil.Data.Games, pattern).ID });
    }

    private Game ClosestMatch(List<Game> games, string pattern)
    {
        int dist = editDistance(pattern, games[0].Name);
        Game g  = games[0];
        int tmp;
        for(int i=1; i<games.Count; ++i)
        {
            tmp = editDistance(pattern, games[i].Name);
            if (tmp < dist)
            {
                dist = tmp;
                g = games[i];
            }
        }
        return g;
    }

    private List<Game> closestMatches(List<Game> games, string pattern, int numMatches = 5)
    {
        if (numMatches<1) numMatches=1;
        List<Tuple<int,Game>> closest = new();
        foreach (Game g in games)
        {
            int dist = editDistance(pattern, g.Name); 
            if (closest.Count < numMatches || dist < closest.Last().Item1)
                closest.Add(new Tuple<int, Game>(dist, g));
            closest.Sort();
            if (closest.Count > numMatches)
                closest.Remove(closest.Last());
        }
        List<Game> r = new();
        foreach(Tuple<int,Game> g in closest)
            r.Add(g.Item2);
        return r;        
    }

    private int min(int a, int b)
    {
        return (a<b) ? a : b;
    }

    private int max(int a, int b)
    {
        return (a>b) ? a : b;
    }

    private int editDistance(string t1, string t2, int padding = 40)
    {
        string s1=t1.ToLower();
        string s2=t2[..min(padding,t2.Length)].ToLower() + new string('*',max(padding-t2.Length,0));
        int n=s1.Length, m=s2.Length;
        int[,] dp = new int[n+1,m+1];
        for(int i=0; i<=n; ++i)
            dp[i,0] =  i;
        for(int j=1; j<=m; ++j)
            dp[0,j] = j;
        for(int i=1;i<=n;++i)
            for(int j=1;j<=m;++j)
                dp[i,j] = (s1[i-1] == s2[j-1]) ? dp[i-1,j-1] : min(dp[i-1,j]+1,min(dp[i,j-1]+1,dp[i-1,j-1]+1));
        return dp[n,m];
    }

}