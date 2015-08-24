﻿using System;
using System.Runtime.Remoting.Services;
using SteamKit2.GC.Dota.Internal;
using STEAMNERD.Modules;

namespace STEAMNERD
{
    class Program
    {
        static void Main(string[] args)
        {
            string user, password;
            FancyLogIn(out user, out password);

            var steamNerd = new SteamNerd(user, password);
            steamNerd.AddModule(new LingT(steamNerd));
            steamNerd.AddModule(new DiceRoll(steamNerd));
            steamNerd.AddModule(new PersistentChat(steamNerd));
            steamNerd.AddModule(new Mingag(steamNerd));
            steamNerd.AddModule(new TrollSlayer(steamNerd));

            steamNerd.Connect();
        }

        static void FancyLogIn(out string user, out string password)
        {
            // Get the username
            Console.Write("Username: ");
            user = Console.ReadLine();

            // Get the password
            Console.Write("Password: ");
            var key = Console.ReadKey(true);
            password = "";

            while (key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                key = Console.ReadKey(true);
            }

            Console.WriteLine();
        }
    }
}