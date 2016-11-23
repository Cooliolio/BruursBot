using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Net;
using HtmlAgilityPack;
namespace DiscordBruursBot
{
    class BruursBot
    {
        DiscordClient discord;
        CommandService commands;
        public BruursBot()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();


            

            RegisterHelpCommand();
            RegisterMatchesCommand();
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjM3MjU5MDE3NTM2Mjc0NDMy.CxZQWw.vUOikUGHLNsUlUjS59g6WzaysVc", TokenType.Bot);
            });
        }

        private void RegisterHelpCommand()
        {
            commands.CreateCommand("help")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Work In Progress");
                });
        }

        private void RegisterMatchesCommand()
        {
            commands.CreateCommand("csgo")
                .Do(async (e) =>
                {
                    String currentTime = DateTime.Now.ToString();
                    String matches = "", whenMatch, teamsMatch;

                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument document = web.Load("https://csgolounge.com/");

                    HtmlNode[] when = document.DocumentNode.SelectNodes("//div[@class='whenm']").ToArray();
                    HtmlNode[] teams = document.DocumentNode.SelectNodes("//div[@class='matchleft']").ToArray();
                
                    for (int i = 0; i < 5; i++)
                        {
                            whenMatch = when[i].InnerText.Trim() + "\n";
                            teamsMatch = Regex.Replace(teams[i].InnerText.Replace(System.Environment.NewLine, " "), @"\s+", " ") + "\n";

                            matches += whenMatch + teamsMatch + "\n"; 
                        }
                    
                    Console.WriteLine("Matches got requested by: " + e.User.Name.ToString());
                    await e.Channel.SendMessage("```Requested at:\n" + currentTime + "\n\n" + matches + "```");
                });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
