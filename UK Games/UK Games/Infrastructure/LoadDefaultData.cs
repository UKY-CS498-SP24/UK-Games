using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class LoadDefaultData
{
    public LoadDefaultData()
    {
        // Create some sample games:
        Game hiddenObj = new Game("Tokyo Hidden Objects",
            "<script src=\"https://cdn.htmlgames.com/embed.js?game=TokyoHiddenObjects&amp;bgcolor=white\"></script>");
        hiddenObj.RefURL = "https://www.htmlgames.com/html5-games-for-your-site/";
        hiddenObj.ImagePath = "~/img/games/tokyohiddenobjects500300.webp";
        hiddenObj.Save();

        Game solitaire = new Game("Solitaire Collection", "<script src=\"https://cdn.htmlgames.com/embed.js?game=SolitaireCollection&amp;bgcolor=white\"></script>");
        solitaire.RefURL = "https://www.htmlgames.com/html5-games-for-your-site/";
        solitaire.ImagePath = "~/img/games/solitairecollection500300.webp";
        solitaire.Save();
        
        Game pinball = new Game("Pinball Breakout", "<script src=\"https://cdn.htmlgames.com/embed.js?game=PinballBreakout&amp;bgcolor=white\"></script>");
        pinball.RefURL = "https://www.htmlgames.com/html5-games-for-your-site/";
        pinball.ImagePath = "~/img/games/pinballbreakout500300.webp";
        pinball.Save();
        
        Game mahjong = new Game("Mahjong 3D Connect", "<script src=\"https://cdn.htmlgames.com/embed.js?game=Mahjong3DConnect&amp;bgcolor=white\"></script>");
        mahjong.RefURL = "https://www.htmlgames.com/html5-games-for-your-site/";
        mahjong.ImagePath = "~/img/games/mahjong3dconnect500300.webp";
        mahjong.Save();
        
        Game billiards = new Game("2048 Billiards", "<script src=\"https://cdn.htmlgames.com/embed.js?game=2048Billiards&amp;bgcolor=white\"></script>");
        billiards.RefURL = "https://www.htmlgames.com/html5-games-for-your-site/";
        billiards.ImagePath = "~/img/games/2048billiards500300.webp";
        billiards.Save();
        
        // Create some sample users:
        User jackson = new User(
            "jphu235",
            "Jackson",
            "Huse",
            DateTime.Parse("09/03/2002"),
            "jphu235@uky.edu",
            "admin0000!"
        );
        
        User nathan = new User(
            "nldu227",
            "Nathan",
            "Duncan",
            DateTime.Parse("01/01/2001"),
            "nldu227@uky.edu",
            "admin0000!"
        );
        
        User muhammad = new User(
            "mhal236",
            "Muhammad",
            "Ali",
            DateTime.Parse("06/30/2002"),
            "mhal236@uky.edu",
            "admin0000!"
        );

        // Let's say users played some random games:
        jackson.AddPlayed(hiddenObj.ID);
        jackson.AddPlayed(solitaire.ID);
        jackson.AddPlayed(pinball.ID);
        jackson.AddPlayed(mahjong.ID);
        jackson.AddPlayed(billiards.ID);
        jackson.Save();

        nathan.AddPlayed(hiddenObj.ID);
        nathan.AddPlayed(solitaire.ID);
        nathan.AddPlayed(pinball.ID);
        nathan.Save();

        muhammad.AddPlayed(solitaire.ID);
        muhammad.Save();
    }
}