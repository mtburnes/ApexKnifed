using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Plugin.Commands;
using PRoCon.Core.Players;
using PRoCon.Core.Players.Items;
using PRoCon.Core.Battlemap;
using PRoCon.Core.Maps;
using PRoCon.Core.HttpServer;

namespace ProConEvents {
    public class ApexKnifed : ProConPluginAPI, IPRoConPluginInterface {
        private const String PluginVersion = "1.0";

        public enum GameVersion {
            BF4
        };

        //bools
        private bool m_isPluginEnabled;

        public ApexKnifed() {
            

            this.m_isPluginEnabled = false;
        }

        public string GetPluginName() {
            return "ApexKnifed";
        }

        public string GetPluginVersion() {
            return PluginVersion;
        }

        public string GetPluginAuthor() {
            return "mtburnes";
        }

        public string GetPluginWebsite() {
            return "https://github.com/mtburnes/ApexKnifed";
        }

        public string GetPluginDescription() {
            return @"
<h2>Description</h2>
    <p>Creates custom 'player got knifed' messages.</p>
		";
        }

        public void OnPluginEnable() {
            this.ExecuteCommand("procon.protected.pluginconsole.write", "^bApexKnifed ^2Enabled!");

            this.m_isPluginEnabled = true;
        }

        public void OnPluginDisable() {
            this.ExecuteCommand("procon.protected.pluginconsole.write", "^bApexKnifed ^1Disabled!" );

            this.m_isPluginEnabled = false;
        }

        public void OnPluginLoaded(String strHostName, String strPort, String strPRoConVersion) {
            Log.Debug("Entering OnPluginLoaded", 7);
            try {
                //Set the server IP
                _serverInfo.ServerIP = strHostName + ":" + strPort;
                //Register all events
                RegisterEvents(GetType().Name,
                    "OnListPlayers",
                    "OnPlayerKilled",
                    );
            } catch (Exception e) {
                Log.Exception("FATAL ERROR on plugin load.", e);
            }
            Log.Debug("Exiting OnPluginLoaded", 7);
        }

        public List<CPluginVariable> GetDisplayPluginVariables() {

            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
			lstReturn.Add(new CPluginVariable("ApexKnifed|Enabled", typeof(bool), ApexKnifedEnable));

            return lstReturn;
        }

        public void SetPluginVariable(string strVariable, string strValue) {
			if (Regex.Match(strVariable, @"Enabled").Success)
            {
				bool tmpApexKnifedEnable = false;
                Boolean.TryParse(strValue, out tmpApexKnifedEnable);
				ApexKnifedEnable = tmpApexKnifedEnable;
            }
			else if (Regex.Match(strVariable, @"Delay").Success)
            {
                int tmpTime = 15;
                int.TryParse(strValue, out tmpTime);
                ApexKnifedDelay = tmpTime;
            }
        }

        //Quick methods to do annoying things
        
        private void ConsoleWrite(string str)
		{
			ExecuteCommand("procon.protected.pluginconsole.write", str);
		}

        private void Announce(string Message)
		{
			if (GetState() == GState.BetweenRounds) return;
			ExecuteCommand("procon.protected.send", "admin.yell", Message, AnnounceDisplayLength.ToString(), "all");
			LogChat(Message);
		}
        private void LogChat(string Message)
		{
			ExecuteCommand("procon.protected.chat.write", Message);
		}

        /*
        This function is used as some kind of interrupt that is ran whenever someone is killed. 
        A kill on the server will trigger an event, which runs this function. OnPlayerKilled's sole parameter (AFAIK)
        is a `Kill`, which has info regarding a particular player kill event.
        */
        public override void OnPlayerKilled(Kill kill) {
            Log.Debug("Entering OnPlayerKilled", 7);
            try {
                //If the plugin is not enabled and running just return
                if (!_pluginEnabled || !_threadsReady || !_firstPlayerListComplete) {
                    return;
            }
            AdKatsSubscribedPlayer killer;
            AdKatsSubscribedPlayer victim;
            if (kill.Killer != null && !String.IsNullOrEmpty(kill.Killer.SoldierName)) {
                    if (!_playerDictionary.TryGetValue(kill.Killer.SoldierName, out killer)) {
                        Log.Error("Unable to fetch killer " + kill.Killer.SoldierName + " on kill.");
                        return;
                    }
                } else {
                    return;
            }

            if (kill.Victim != null && !String.IsNullOrEmpty(kill.Victim.SoldierName)) {
                    if (!_playerDictionary.TryGetValue(kill.Victim.SoldierName, out victim)) {
                        Log.Error("Unable to fetch victim " + kill.Victim.SoldierName + " on kill.");
                        return;
                    }
                } else {
                    return;
            }
            //If a player gets knifed...
            if (kill.DamageType == Melee){
                if(kill.Headshot == false){
                    Log.Debug($"{kill.Killer} just melee'd {kill.Victim}.", 7);
                    
                }
                else{
                    Log.Debug($"{kill.Killer} just melee'd {kill.Victim}. With a headshot?", 7);
                }
            }
            if (kill.DamageType == SniperRifle && kill.Headshot && kill.Distance >= 1000){
                Log.Debug($"{kill.Killer} just popped {kill.Victim}'s head off with a {kill.Distance}m shot!", 7);
            }
        }
    }
}