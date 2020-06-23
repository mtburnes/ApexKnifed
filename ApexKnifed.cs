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
    }
}