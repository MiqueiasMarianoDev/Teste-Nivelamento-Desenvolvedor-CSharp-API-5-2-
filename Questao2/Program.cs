using Newtonsoft.Json.Linq;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoalsAsync(teamName, year).Result;

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoalsAsync(teamName, year).Result;

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoalsAsync(string team, int year)
    {

        int totalGoals = 0;
        int currentPage = 1;
        int totalPages;

        do
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
            string responseBody = await client.GetStringAsync(url);
            JObject response = JObject.Parse(responseBody);

            totalPages = (int)response["total_pages"];
            var matches = response["data"];

            foreach (var match in matches)
            {
                if ((string)match["team1"] == team)
                {
                    totalGoals += int.Parse((string)match["team1goals"]);
                }
            }

            currentPage++;
        } while (currentPage <= totalPages);

        currentPage = 1;
        do
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}";
            string responseBody =  await client.GetStringAsync(url);
            JObject response = JObject.Parse(responseBody);

            totalPages = (int)response["total_pages"];
            var matches = response["data"];

            foreach (var match in matches)
            {
                if ((string)match["team2"] == team)
                {
                    totalGoals += int.Parse((string)match["team2goals"]);
                }
            }

            currentPage++;
        } while (currentPage <= totalPages);

        return totalGoals;
    }

}