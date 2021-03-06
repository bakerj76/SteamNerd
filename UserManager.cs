﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace SteamNerd
{
    public class UserManager
    {
        private SteamNerd _steamNerd;
        // Where the admin file path is located.
        private string _adminListPath;
        private List<SteamID> _admins;
        private Dictionary<SteamID, User> _users;


        /// <summary>
        /// Creates an Admin Manager.
        /// </summary>
        /// <param name="adminListPath">The file that contains the admin list.</param>
        public UserManager(SteamNerd steamNerd, string adminListPath)
        {
            _steamNerd = steamNerd;
            _adminListPath = adminListPath;
            LoadAdminFile();

            _users = new Dictionary<SteamID, User>();
            _admins = new List<SteamID>();
        }

        /// <summary>
        /// Adds an admin and gives them permissions.
        /// </summary>
        /// <param name="user">The user to make admin.</param>
        public void AddAdmin(User user)
        {
            var steamID = user.SteamID;

            user.IsAdmin = true;
            _admins.Add(steamID);
            SaveAdmin(steamID);
        }

        /// <summary>
        /// Saves a new admin.
        /// </summary>
        private void SaveAdmin(SteamID steamID)
        {
            using (var file = new StreamWriter(_adminListPath))
            {
                file.WriteLine(steamID.Render());
                file.Flush();
            }
        }

        /// <summary>
        /// Loads the list of admins.
        /// </summary>
        private void LoadAdminFile()
        {
            // Open the file or create it if it doesn't exist.
            using (var file = new StreamReader(
                new FileStream(_adminListPath, FileMode.OpenOrCreate)
            ))
            {
                // Read the file and add each SteamID.
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();

                    var steamID = new SteamID(line);
                    _admins.Add(steamID);
                }
            }
        }

        /// <summary>
        /// Checks the admin list to see if this user is an admin.
        /// </summary>
        /// <returns>True if the user is an admin, false otherwise.</returns>
        private bool IsAdmin(User user)
        {
            return _admins.Contains(user.SteamID);
        }

        /// <summary>
        /// Adds a user to the manager.
        /// </summary>
        /// <param name="steamID">The user's SteamID.</param>
        /// <param name="name">The user's name.</param>
        /// <param name="personaState">The user's persona state.</param>
        /// <returns></returns>
        public User AddUser(SteamID steamID, string name, EPersonaState personaState)
        {
            Console.WriteLine("Adding user {0}", name);
            var user = new User(_steamNerd, steamID, name, personaState);
            user.IsAdmin = IsAdmin(user);
            _users[user.SteamID] = user;

            return user;
        }

        /// <summary>
        /// Gets the User from a SteamID.
        /// </summary>
        /// <param name="steamID">The user's SteamID.</param>
        /// <returns>The user's information.</returns>
        public User GetUser(SteamID steamID)
        {
            return _users.ContainsKey(steamID) ? _users[steamID] : null;
        }

        /// <summary>
        /// Checks if the user exists.
        /// </summary>
        /// <param name="steamID">The steamID of the user</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        public bool UserExists(SteamID steamID)
        {
            return _users.ContainsKey(steamID);
        }
    }
}
