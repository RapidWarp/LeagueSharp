
#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace Laugh_and_Kill
{
    internal class Program
    {

        private static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Laugh", "Laugh", true);
            Config.AddSubMenu(new Menu("On/Off", "On/Off"));
            Config.SubMenu("On/Off")
                .AddItem(new MenuItem("LaughSpam Toggle", "Toggleable Laugh"))
                .SetValue(new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle));
            Config.SubMenu("On/Off").AddItem(new MenuItem("Type", "Spam Laughing??")).SetValue(true);

            Config.AddToMainMenu();

            Game.OnGameNotifyEvent += Game_OnGameNotifyEvent;

            Game.PrintChat("Laugh on Kill loaded by RapidWarp, credits to The Kush Style for emote spammer");
        }

        private static void SPAM()
        {
            if ((Config.Item("Type").GetValue<Slider>().Value) != 1) return;
            Packet.C2S.Emote.Encoded(new Packet.C2S.Emote.Struct(2)).Send();
            Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(Game.CursorPos.X, Game.CursorPos.Y)).Send();
        }

        private static void Game_OnGameNotifyEvent(GameNotifyEventArgs args)
        {
            if (args.NetworkId != ObjectManager.Player.NetworkId)
            {
                return;
            }

            switch (args.EventId)
            {
                case GameEventId.OnChampionKill:
                {
                    if (!Config.Item("EmoteToggable").GetValue<KeyBind>().Active) return;
                    SPAM();
                    break;
                }
            }
        }
    }
}