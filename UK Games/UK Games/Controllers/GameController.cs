using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UK_Games.Infrastructure;
using UK_Games.Models;


namespace UK_Games.Controllers;

public class GameController : Controller
{
    // GOING TO A GAME PAGE
    
    /*
     *      localhost:3306/game/25
     */
    public IActionResult Index(int id)
    {
        Game? selectedGame = GeneralMethods.GetGame(id);
        ViewBag.Game = selectedGame;
        GeneralMethods.GetLoggedInUser(HttpContext.Session)?.AddPlayed(selectedGame);
        return View();
    }

    [HttpPost]
    public IActionResult Search()
    {
        string pattern = Convert.ToString(Request.Form["pattern"]);
        Game? selectedGame = ClosestMatch(DataUtil.Data.Games.ToList(), pattern);
        ViewBag.Game = selectedGame;
        GeneralMethods.GetLoggedInUser(HttpContext.Session)?.AddPlayed(selectedGame);
        return View("Index");
    }

    private Game? ClosestMatch(List<Game> games, string pattern)
    {
        int maxLen = games.Max(g => g.Name.Length);
        return games.MinBy(g => editDistance(pattern, g.Name, maxLen));
    }

    private List<Game> closestMatches(List<Game> games, string pattern, int numMatches = 5)
    {
        int maxLen = games.Max(g => g.Name.Length);
        if (numMatches<1) numMatches=1;
        List<Tuple<int,Game>> closest = new();
        foreach (Game g in games)
        {
            int dist = editDistance(pattern, g.Name, maxLen); 
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

    private int editDistance(string s1, string s2, int padding = 40)
    {
        int n=s1.Length, m=s2.Length;
        int[,] dp = new int[n+1,m+1];
        for(int i=0; i<=n; ++i)
            dp[i,0] =  i;
        for(int j=1; j<=m; ++j)
            dp[0,j] = j;
        for(int i=1;i<=n;++i)
            for(int j=1;j<=m;++j)
                dp[i,j] = (char.ToLower(s1[i-1]) == char.ToLower(s2[j-1])) ? dp[i-1,j-1] : Math.Min(dp[i-1,j]+1,Math.Min(dp[i,j-1]+1,dp[i-1,j-1]+1));
        return dp[n,m] + Math.Max(padding-s2.Length,0);
    }

}