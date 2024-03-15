using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class PackagePanel : UIPanel
    {
        public override void OnOpened()
        {
            base.OnOpened();

            Player.Local.DisableInput();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            if (Player.Local != null)
            {
                Player.Local.EnableInput();
            }
        }
    }
}