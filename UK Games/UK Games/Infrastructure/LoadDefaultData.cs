using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class LoadDefaultData
{
    public LoadDefaultData()
    {
        // Create some sample games:
        Game goChickenGo = new Game("Go Chicken Go",
            "<iframe seamless=\"seamless\" allowtransparency=\"true\" allowfullscreen=\"true\" frameborder=\"0\" style=\"width: 100%;height: 100%;border: 0px;\" src=\"https://zv1y2i8p.play.gamezop.com/g/rJ57aMJDcJm\"> </iframe>");

        // Create some sample users:
        User jackson = new User(
            "jphu235",
            "Jackson",
            "Huse",
            DateTime.Parse("09/03/2002"),
            "jphu235@uky.edu",
            "admin0000!"
        );

        // Let's say users played some random games:
        jackson.AddPlayed(goChickenGo);
        jackson.Save();
    }
}